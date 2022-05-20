#pragma once
#pragma execution_character_set("utf-8")  
#include "layer/DiscreteLayer.h"
#include <osg/Group>

class OsgScene
{
private:
	osg::ref_ptr <osg::Group> root;

	osg::ref_ptr<osg::Node> node;

	map<int, shared_ptr<DiscreteLayer>> layers;

public:
	OsgScene() { root = new osg::Group; getRoot()->setName("Root"); }

	int addLayer(const string& url);

	DiscreteLayer* GetLayer(int index);

	osg::Group* getRoot() { return root.get(); }
};

