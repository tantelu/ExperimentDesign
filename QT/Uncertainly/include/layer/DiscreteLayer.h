#pragma once
#include <unordered_map>
#include <osg/Node>
#include <osg/Group>
#include <osgViewer/Viewer>
#include "gslib/model.h"
using namespace std;

class DiscreteLayer
{
private:
	osg::Switch* sw;
	map<int, osg::Geode*> facies;

public:
	DiscreteLayer(const GslibModel<int>& model);

	DiscreteLayer(const DiscreteLayer& other) = delete;

	void setVisible(bool checked);

	void setVisibleFilter(set<int>& filter);

	osg::Geode* getGeode(const int& facie);

	osg::Switch* getSwitch();

	vector<int> getFacies();
};

