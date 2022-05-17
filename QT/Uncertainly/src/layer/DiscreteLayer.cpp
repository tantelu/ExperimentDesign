#include "layer/DiscreteLayer.h"
#include <osg/Material>
#include <osgUtil/SmoothingVisitor>

DiscreteLayer::DiscreteLayer(const GslibModel<int>* model)
{
	sw = new osg::Switch;
	this->inmodel = model;
	vecArray = new osg::Vec3Array;
	int vicount = inmodel->getIcount() + 1;
	int vjcount = inmodel->getJcount() + 1;
	int vijcount = vicount * vjcount;
	int vkcount = inmodel->getKcount() + 1;
	vecArray->reserve(vicount * vjcount * vkcount);
	for (size_t k = 0; k <= inmodel->getKcount(); k++)
	{
		for (size_t j = 0; j <= inmodel->getJcount(); j++)
		{
			for (size_t i = 0; i <= inmodel->getIcount(); i++)
			{
				vecArray->push_back(osg::Vec3d(i, j, k));
			}
		}
	}
	colorS = { {-99, osg::Vec4(0, 0, 1, 1.f)},{0,osg::Vec4(0, 0, 1, 1.f)}, {1,osg::Vec4(0, 1.0, 0, 1.f)} ,{2,osg::Vec4(1.0, 0, 0, 1.f)} };
}

void DiscreteLayer::setVisible(bool checked)
{
	if (checked) {
		sw->setAllChildrenOn();
	}
	else {
		sw->setAllChildrenOff();
	}
}

void DiscreteLayer::setVisibleFilter(set<int>& filter)
{
	sw.get()->removeChildren(0, sw.get()->getNumChildren());
	for (auto fit = filter.begin(); fit != filter.end(); fit++)
	{
		auto fcolor = colorS.find(*fit);
		osg::ref_ptr<osg::Geometry> geometry = new osg::Geometry;

		osg::ref_ptr<osg::Vec3Array> n = new osg::Vec3Array;
		osg::ref_ptr<osg::DrawElementsUInt> drawElemUIntPt = new osg::DrawElementsUInt(GL_QUADS);
		osg::ref_ptr<osg::Material> material = new osg::Material;
		material->setAmbient(osg::Material::FRONT, (*fcolor).second);
		material->setDiffuse(osg::Material::FRONT, (*fcolor).second);
		material->setSpecular(osg::Material::FRONT, (*fcolor).second);
		material->setShininess(osg::Material::FRONT, 60.0);
		material->setColorMode(osg::Material::AMBIENT);
		osg::DrawElementsUInt* drawElemUInt = drawElemUIntPt.get();
		int vicount = inmodel->getIcount() + 1;
		int vjcount = inmodel->getJcount() + 1;
		int vijcount = vicount * vjcount;
		for (size_t k = 0; k < inmodel->getKcount(); k++)
		{
			for (size_t j = 0; j < inmodel->getJcount(); j++)
			{
				for (size_t i = 0; i < inmodel->getIcount(); i++)
				{
					if (inmodel->getValue(i, j, k) == *fit) {

						int vi = k * vijcount + j * vicount + i;
						if (k == 0 || filter.find(inmodel->getValue(i, j, k - 1)) == filter.end())
						{   //底
							drawElemUInt->push_back(vi);
							drawElemUInt->push_back(vi + 1);
							drawElemUInt->push_back(vi + vicount + 1);
							drawElemUInt->push_back(vi + vicount);
							n->push_back(osg::Vec3(0.f, 0.f, -1.f));
						}
						if (j == 0 || filter.find(inmodel->getValue(i, j - 1, k)) == filter.end())
						{	//前
							drawElemUInt->push_back(vi);
							drawElemUInt->push_back(vi + 1);
							drawElemUInt->push_back(vi + vijcount + 1);
							drawElemUInt->push_back(vi + vijcount);
							n->push_back(osg::Vec3(0.f, -1.f, 0.f));
						}
						if (i == 0 || filter.find(inmodel->getValue(i - 1, j, k)) == filter.end())
						{	//左
							drawElemUInt->push_back(vi);
							drawElemUInt->push_back(vi + vicount);
							drawElemUInt->push_back(vi + vicount + vijcount);
							drawElemUInt->push_back(vi + vijcount);
							n->push_back(osg::Vec3(-1.f, 0.f, 0.f));
						}
						if (k == inmodel->getKcount() - 1 || filter.find(inmodel->getValue(i, j, k + 1)) == filter.end())
						{	//上
							drawElemUInt->push_back(vi + vijcount);
							drawElemUInt->push_back(vi + vijcount + 1);
							drawElemUInt->push_back(vi + vijcount + vicount + 1);
							drawElemUInt->push_back(vi + vicount + vijcount);
							n->push_back(osg::Vec3(0.f, 0.f, 1.f));
						}
						if (i == inmodel->getIcount() - 1 || filter.find(inmodel->getValue(i + 1, j, k)) == filter.end())
						{	//右
							drawElemUInt->push_back(vi + 1);
							drawElemUInt->push_back(vi + vicount + 1);
							drawElemUInt->push_back(vi + vijcount + vicount + 1);
							drawElemUInt->push_back(vi + vijcount + 1);
							n->push_back(osg::Vec3(0.f, 1.f, 0.f));
						}
						if (j == inmodel->getJcount() - 1 || filter.find(inmodel->getValue(i, j + 1, k)) == filter.end())
						{	//后
							drawElemUInt->push_back(vi + vicount);
							drawElemUInt->push_back(vi + vicount + 1);
							drawElemUInt->push_back(vi + vijcount + vicount + 1);
							drawElemUInt->push_back(vi + vijcount + vicount);
							n->push_back(osg::Vec3(0.f, 1.f, 0.f));
						}
					}
				}
			}
		}
		geometry->setVertexArray(vecArray);
		geometry->addPrimitiveSet(drawElemUInt);
		osg::ref_ptr<osg::Geode> geode = new osg::Geode;
		geometry->setNormalArray(n.get());
		auto bind = (osg::Geometry::AttributeBinding::BIND_OVERALL | osg::Geometry::AttributeBinding::BIND_PER_PRIMITIVE_SET);
		geometry->setNormalBinding((osg::Geometry::AttributeBinding)bind);
		geode->addDrawable(geometry);
		geode->getOrCreateStateSet()->setAttributeAndModes(material.get(), osg::StateAttribute::ON);
		geode->getOrCreateStateSet()->setMode(GL_BLEND, osg::StateAttribute::ON);//透明
		geode->getOrCreateStateSet()->setMode(GL_DEPTH_TEST, osg::StateAttribute::ON);//深度测试
		sw.get()->addChild(geode, false);
	}
	sw.get()->setAllChildrenOn();
}

osg::Switch* DiscreteLayer::getSwitch()
{
	return sw;
}
