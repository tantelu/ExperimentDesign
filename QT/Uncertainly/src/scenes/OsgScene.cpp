#include "scenes\OsgScene.h"
#include <osgDB/ReadFile>//包含读文件的头文件

int OsgScene::addLayer(const string& url)
{
	auto pointer = std::make_shared<Layer>(url);
	int nextindex = 1;
	if (layers.size() > 0) {
		nextindex = layers.rbegin()->first + 1;
	}
	pointer.get()->setSecens(this);
	layers.insert(std::make_pair(nextindex, pointer));
	return nextindex;
}

Layer* OsgScene::GetLayer(int index)
{
	auto f = layers.find(index);
	if (f != layers.end()) {
		return ((*f).second).get();
	}
	else {
		return nullptr;
	}
}