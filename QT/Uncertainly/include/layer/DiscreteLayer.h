#pragma once
#include <unordered_map>
#include <osg/Node>
#include <osg/Group>
#include <osgViewer/Viewer>
#include "gslib/model.h"
#include "QVariant"
using namespace std;

class DiscreteLayer
{
private:
	const GslibModel<int>* inmodel;
	osg::ref_ptr<osg::Switch> sw;
	osg::ref_ptr<osg::Vec3Array> vecArray;
	map<int, osg::Vec4> colorS;
public:
	DiscreteLayer(const GslibModel<int>* model);

	DiscreteLayer(const DiscreteLayer& other) = delete;

	void setVisible(bool checked);

	void setVisibleFilter(set<int>& filter);

	osg::Switch* getSwitch();
};
Q_DECLARE_METATYPE(DiscreteLayer)
