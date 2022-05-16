#include "layer/DiscreteLayer.h"

DiscreteLayer::DiscreteLayer(const GslibModel<int>& model)
{
	osg::ref_ptr<osg::Switch> _switch = new osg::Switch;
	sw = _switch.get();
	map<int, int> sortfacie;
	for (size_t i = 0; i < model.size(); i++)
	{
		auto f = model.getValue(i);
		auto it = sortfacie.find(f);
		if (it != sortfacie.end()) {
			(*it).second += 1;
		}
		else {
			sortfacie.insert(make_pair(f, 1));
		}
	}
	osg::ref_ptr<osg::Vec3Array> ptVecArray = new osg::Vec3Array;
	osg::Vec3Array* vecArray = ptVecArray.get();
	int vicount = model.getIcount() + 1;
	int vjcount = model.getJcount() + 1;
	int vijcount = vicount * vjcount;
	int vkcount = model.getKcount() + 1;
	vecArray->reserve(vicount * vjcount * vkcount);
	for (size_t k = 0; k <= model.getKcount(); k++)
	{
		for (size_t j = 0; j <= model.getJcount(); j++)
		{
			for (size_t i = 0; i <= model.getIcount(); i++)
			{
				vecArray->push_back(osg::Vec3d(i, j, k));
			}
		}
	}
	osg::Vec4dArray* colorS = new osg::Vec4dArray;
	colorS->push_back(osg::Vec4(0, 0, 1, 1.f));
	colorS->push_back(osg::Vec4(0, 1.0, 0, 1.f));
	colorS->push_back(osg::Vec4(1.0, 0, 0, 1.f));
	for (auto fit = sortfacie.begin(); fit != sortfacie.end(); fit++)
	{
		osg::ref_ptr<osg::Geometry> geometry = new osg::Geometry;
		osg::Vec4dArray* color = new osg::Vec4dArray;
		color->push_back(colorS->at((*fit).first));
		osg::ref_ptr<osg::DrawElementsUInt> drawElemUIntPt = new osg::DrawElementsUInt(GL_QUADS);
		osg::DrawElementsUInt* drawElemUInt = drawElemUIntPt.get();
		drawElemUInt->reserve(24 * (*fit).second);
		for (size_t k = 0; k < model.getKcount(); k++)
		{
			for (size_t j = 0; j < model.getJcount(); j++)
			{
				for (size_t i = 0; i < model.getIcount(); i++)
				{
					int modelindex = k * model.getIcount() * model.getJcount() + j * model.getIcount() + i;
					auto f = model.getValue(modelindex);
					if (f == (*fit).first) {

						int vi = k * vijcount + j * vicount + i;
						//µ×
						drawElemUInt->push_back(vi);
						drawElemUInt->push_back(vi + 1);
						drawElemUInt->push_back(vi + vicount + 1);
						drawElemUInt->push_back(vi + vicount);
						//Ç°
						drawElemUInt->push_back(vi);
						drawElemUInt->push_back(vi + 1);
						drawElemUInt->push_back(vi + vijcount + 1);
						drawElemUInt->push_back(vi + vijcount);
						//×ó
						drawElemUInt->push_back(vi);
						drawElemUInt->push_back(vi + vicount);
						drawElemUInt->push_back(vi + vicount + vijcount);
						drawElemUInt->push_back(vi + vijcount);
						//ÉÏ
						drawElemUInt->push_back(vi + vijcount);
						drawElemUInt->push_back(vi + vijcount + 1);
						drawElemUInt->push_back(vi + vijcount + vicount + 1);
						drawElemUInt->push_back(vi + vicount + vijcount);
						//ÓÒ
						drawElemUInt->push_back(vi + 1);
						drawElemUInt->push_back(vi + vicount + 1);
						drawElemUInt->push_back(vi + vijcount + vicount + 1);
						drawElemUInt->push_back(vi + vijcount + 1);
						//ºó
						drawElemUInt->push_back(vi + vicount);
						drawElemUInt->push_back(vi + vicount + 1);
						drawElemUInt->push_back(vi + vijcount + vicount + 1);
						drawElemUInt->push_back(vi + vijcount + vicount);
					}
				}
			}
		}
		geometry->setVertexArray(vecArray);
		geometry->addPrimitiveSet(drawElemUInt);
		osg::ref_ptr<osg::Geode> geode = new osg::Geode;
		geometry->setColorArray(color);
		color->setBinding(osg::Array::BIND_OVERALL);
		geode->addDrawable(geometry);
		facies.insert(make_pair((*fit).first, geode.get()));
	}
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
	for (auto it = facies.begin(); it != facies.end(); it++)
	{
		if (filter.find((*it).first) != filter.end()) {
			sw->setChildValue((*it).second, true);
		}
		else {
			sw->setChildValue((*it).second, false);
		}
	}
}

osg::Geode* DiscreteLayer::getGeode(const int& facie)
{
	auto it = facies.find(facie);
	if (it != facies.end()) {
		return (*it).second;
	}
	else {
		return nullptr;
	}
}

osg::Switch* DiscreteLayer::getSwitch()
{
	return sw;
}

vector<int> DiscreteLayer::getFacies()
{
	std::vector<int> r;
	r.reserve(facies.size());
	for (const auto& kvp : facies)
	{
		r.push_back(kvp.first);
	}
	return r;
}
