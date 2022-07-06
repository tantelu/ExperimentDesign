#include "scenes\OsgScene.h"
#include <osgDB/ReadFile>//�������ļ���ͷ�ļ�

int OsgScene::addLayer(const string& url)
{
	auto pointer = std::make_shared<DiscreteLayer>(url);
	int nextindex = 1;
	if (layers.size() > 0) {
		nextindex = layers.rbegin()->first + 1;
	}
	pointer.get()->setSecens(this);
	pointer.get()->setVisibleFilter(pointer.get()->allFacies);
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