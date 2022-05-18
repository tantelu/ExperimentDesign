#pragma once
#pragma execution_character_set("utf-8")  

#include <layer/DiscreteLayer.h>

class OsgScene
{
private:
	osg::Group* root;

	osg::ref_ptr<osg::Node> node;

	map<int, shared_ptr<DiscreteLayer>> layers;

public:
	int addDiscreteLayer(const string& url);

	DiscreteLayer* GetLayer(int index);

	void setRoot(osg::Group* root);
};

