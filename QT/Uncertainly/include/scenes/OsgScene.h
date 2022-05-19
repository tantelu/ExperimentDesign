#pragma once
#pragma execution_character_set("utf-8")  
#include "scenes/Layer.h"

class OsgScene
{
private:
	osg::ref_ptr <osg::Group> root;

	osg::ref_ptr<osg::Node> node;

	map<int, shared_ptr<Layer>> layers;

public:
	OsgScene() { root = new Group; getRoot()->setName("Root"); }

	int addLayer(const string& url);

	Layer* GetLayer(int index);

	Group* getRoot() { return root.get(); }
};

