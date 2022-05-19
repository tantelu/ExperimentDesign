#include "scenes/FacieGeode.h"

FacieGeode::FacieGeode() {
	geometry = new Geometry;
	normals = new osg::Vec3Array;
	draws = new osg::DrawElementsUInt(GL_QUADS);
	material = new osg::Material;
	material->setSpecular(osg::Material::FRONT, osg::Vec4d(0.7f, 0.7f, 0.7f, 1.0f));
	material->setShininess(osg::Material::FRONT, 100);
	material->setColorMode(osg::Material::AMBIENT);
	geometry->addPrimitiveSet(draws);
	geometry->setNormalArray(normals);
	geometry->setNormalBinding((osg::Geometry::AttributeBinding)(osg::Geometry::AttributeBinding::BIND_OVERALL | osg::Geometry::AttributeBinding::BIND_PER_PRIMITIVE_SET));
	addDrawable(geometry);
	getOrCreateStateSet()->setAttributeAndModes(material.get(), osg::StateAttribute::ON);
	getOrCreateStateSet()->setMode(GL_BLEND, osg::StateAttribute::ON);//Í¸Ã÷
	getOrCreateStateSet()->setMode(GL_DEPTH_TEST, osg::StateAttribute::ON);//Éî¶È²âÊÔ
}

void FacieGeode::setColor(const Vec4& color)
{
	material->setAmbient(osg::Material::FRONT, color);
	material->setDiffuse(osg::Material::FRONT, color);
}

void FacieGeode::addDrawElementsAndNormal(const GLuint& value1, const GLuint& value2, const GLuint& value3, const GLuint& value4, const osg::Vec3& normal)
{
	draws->push_back(value1);
	draws->push_back(value2);
	draws->push_back(value3);
	draws->push_back(value4);
	normals->push_back(normal);
}

void FacieGeode::clear() 
{
	draws->clear();
	normals->clear();
}
