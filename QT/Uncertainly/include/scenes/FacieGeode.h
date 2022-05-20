#pragma once
#pragma execution_character_set("utf-8")
//#include <osg/Node>
//#include <osg/Group>
#include <osgViewer/Viewer>
#include <osg/Material>
#include <string>

using namespace std;
using namespace osg;

class FacieGeode : public osg::Geode
{
private:
	osg::ref_ptr<osg::Geometry> geometry;
	osg::ref_ptr<osg::Vec3Array> normals;
	osg::ref_ptr<osg::DrawElementsUInt> draws;
	osg::ref_ptr<osg::Material> material;
public:
	FacieGeode();

	void setColor(const Vec4& color);

	void clear();

	void setFacieVertexArray(Array* array) { geometry.get()->setVertexArray(array); }

	void addDrawElementsUInt(const GLuint& value);

	void addDrawElementsUInts(const vector<GLuint>& values);

	void addNormalArray(const osg::Vec3Array& normal);

	void addDrawElementsAndNormal(const GLuint& value1, const GLuint& value2, const GLuint& value3, const GLuint& value4, const osg::Vec3& normal);
};
