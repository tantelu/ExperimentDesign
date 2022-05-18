#include "qtui\osg\OsgScene.h"
#include <osgDB/ReadFile>//包含读文件的头文件

int OsgScene::addDiscreteLayer(const string& url)
{
	auto pointer = std::make_shared<DiscreteLayer>(url);
	int nextindex = 1;
	if (layers.size() > 0) {
		nextindex = layers.rbegin()->first + 1;
	}
	pointer.get()->closeVisibleFilter();
	root->addChild(pointer.get()->getSwitch());
	layers.insert(std::make_pair(nextindex, pointer));
	return nextindex;
}

DiscreteLayer* OsgScene::GetLayer(int index)
{
	auto f = layers.find(index);
	if (f != layers.end()) {
		return ((*f).second).get();
	}
	else {
		return nullptr;
	}
}

void OsgScene::setRoot(osg::Group* root)
{
	this->root = root;
}
