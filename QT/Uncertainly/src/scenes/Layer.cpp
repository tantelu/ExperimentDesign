#include "scenes\Layer.h"
#include <scenes/OsgScene.h>

void Layer::initBase()
{
	for (auto facie : allFacies) {
		auto temp = make_shared<FacieGeode>();
		basegeos.insert({ facie,temp });
	}
	for (auto kv : basegeos)
	{
		kv.second->setFacieVertexArray(this->vecArray.get());
		kv.second->setColor(defaultColors.find(kv.first)->second);
		basesw->addChild(kv.second.get(), false);
	}
	int vicount = getModel()->getIcount() + 1;
	int vjcount = getModel()->getJcount() + 1;
	int vijcount = vicount * vjcount;
	for (size_t k = 0; k < getModel()->getKcount(); k++)
	{
		for (size_t j = 0; j < getModel()->getJcount(); j++)
		{
			for (size_t i = 0; i < getModel()->getIcount(); i++)
			{
				auto facie = getModel()->getValue(i, j, k);
				auto findgeo = basegeos.find(facie);
				if (findgeo != basegeos.end()) {
					int vi = k * vijcount + j * vicount + i;
					auto curgeo = findgeo->second.get();
					if (k == 0)
					{   //底
						curgeo->addDrawElementsAndNormal(vi, vi + 1, vi + vicount + 1, vi + vicount, osg::Vec3(0.f, 0.f, -1.f));
					}
					if (j == 0)
					{	//前
						curgeo->addDrawElementsAndNormal(vi, vi + 1, vi + vijcount + 1, vi + vijcount, osg::Vec3(0.f, -1.f, 0.f));
					}
					if (i == 0)
					{	//左
						curgeo->addDrawElementsAndNormal(vi, vi + vicount, vi + vicount + vijcount, vi + vijcount, osg::Vec3(-1.f, 0.f, 0.f));
					}
					if (k == getModel()->getKcount() - 1)
					{	//上
						curgeo->addDrawElementsAndNormal(vi + vijcount, vi + vijcount + 1, vi + vijcount + vicount + 1, vi + vicount + vijcount, osg::Vec3(0.f, 0.f, 1.f));
					}
					if (i == getModel()->getIcount() - 1)
					{	//右
						curgeo->addDrawElementsAndNormal(vi + 1, vi + vicount + 1, vi + vijcount + vicount + 1, vi + vijcount + 1, osg::Vec3(0.f, 1.f, 0.f));
					}
					if (j == getModel()->getJcount() - 1)
					{	//后
						curgeo->addDrawElementsAndNormal(vi + vicount, vi + vicount + 1, vi + vijcount + vicount + 1, vi + vijcount + vicount, osg::Vec3(0.f, 1.f, 0.f));
					}
				}
			}
		}
	}
}

Layer::Layer(const string& url) :Layer()
{
	this->url = url;
	model = make_shared<GslibModel<int>>(url);
	vecArray = new osg::Vec3Array;
	int vicount = getModel()->getIcount() + 1;
	int vjcount = getModel()->getJcount() + 1;
	int vijcount = vicount * vjcount;
	int vkcount = getModel()->getKcount() + 1;
	vecArray->reserve(vicount * vjcount * vkcount);
	for (size_t k = 0; k <= getModel()->getKcount(); k++)
	{
		for (size_t j = 0; j <= getModel()->getJcount(); j++)
		{
			for (size_t i = 0; i <= getModel()->getIcount(); i++)
			{
				vecArray->push_back(osg::Vec3d(i, j, k));
			}
		}
	}
	for (size_t i = 0; i < getModel()->size(); i++)
	{
		allFacies.insert(getModel()->getValue(i));
	}

	defaultColors = { {-99, osg::Vec4(0, 0, 1, 1.f)},{0,osg::Vec4(0, 0, 1, 1.f)}, {1,osg::Vec4(0, 1.0, 0, 1.f)} ,{2,osg::Vec4(1.0, 0, 0, 1.f)} };
}

FacieGeode* Layer::getGeodeByFacie(int facie)
{
	auto res = facieGeos.find(facie);
	if (res != facieGeos.end()) {
		return res->second.get();
	}
	else {
		return nullptr;
	}
}

void Layer::setVisibleFilter(const set<int>& filter)
{
	if (filter.size() == 0) {
		closeVisibleFilter();
		return;
	}
	bool neadchange = false;
	if (filter.size() == facieGeos.size())
	{
		for (auto kv : facieGeos) {
			if (filter.find(kv.first) == filter.end()) {
				neadchange = true;
				break;
			}
		}
	}
	else {
		neadchange = true;
	}
	if (neadchange) {
		facieGeos.clear();
		for (auto facie : filter) {
			auto temp = make_shared<FacieGeode>();
			facieGeos.insert({ facie,temp });
		}
		for (auto kv : facieGeos)
		{
			kv.second->setFacieVertexArray(this->vecArray.get());
			faciesw->addChild(kv.second.get(), false);
		}
		int vicount = getModel()->getIcount() + 1;
		int vjcount = getModel()->getJcount() + 1;
		int vijcount = vicount * vjcount;
		for (size_t k = 0; k < getModel()->getKcount(); k++)
		{
			for (size_t j = 0; j < getModel()->getJcount(); j++)
			{
				for (size_t i = 0; i < getModel()->getIcount(); i++)
				{
					auto facie = getModel()->getValue(i, j, k);
					auto findgeo = facieGeos.find(facie);
					if (findgeo != facieGeos.end()) {
						int vi = k * vijcount + j * vicount + i;
						auto curgeo = findgeo->second.get();
						if (k == 0 || filter.find(getModel()->getValue(i, j, k - 1)) == filter.end())
						{   //底
							curgeo->addDrawElementsAndNormal(vi, vi + 1, vi + vicount + 1, vi + vicount, osg::Vec3(0.f, 0.f, -1.f));
						}
						if (j == 0 || filter.find(getModel()->getValue(i, j - 1, k)) == filter.end())
						{	//前
							curgeo->addDrawElementsAndNormal(vi, vi + 1, vi + vijcount + 1, vi + vijcount, osg::Vec3(0.f, -1.f, 0.f));
						}
						if (i == 0 || filter.find(getModel()->getValue(i - 1, j, k)) == filter.end())
						{	//左
							curgeo->addDrawElementsAndNormal(vi, vi + vicount, vi + vicount + vijcount, vi + vijcount, osg::Vec3(-1.f, 0.f, 0.f));
						}
						if (k == getModel()->getKcount() - 1 || filter.find(getModel()->getValue(i, j, k + 1)) == filter.end())
						{	//上
							curgeo->addDrawElementsAndNormal(vi + vijcount, vi + vijcount + 1, vi + vijcount + vicount + 1, vi + vicount + vijcount, osg::Vec3(0.f, 0.f, 1.f));
						}
						if (i == getModel()->getIcount() - 1 || filter.find(getModel()->getValue(i + 1, j, k)) == filter.end())
						{	//右
							curgeo->addDrawElementsAndNormal(vi + 1, vi + vicount + 1, vi + vijcount + vicount + 1, vi + vijcount + 1, osg::Vec3(0.f, 1.f, 0.f));
						}
						if (j == getModel()->getJcount() - 1 || filter.find(getModel()->getValue(i, j + 1, k)) == filter.end())
						{	//后
							curgeo->addDrawElementsAndNormal(vi + vicount, vi + vicount + 1, vi + vijcount + vicount + 1, vi + vijcount + vicount, osg::Vec3(0.f, 1.f, 0.f));
						}
					}
				}
			}
		}
	}
}

void Layer::setVisible(bool checked)
{
	if (checked) {
		if (basegeos.size() == 0) {
			initBase();
		}
		basesw->setAllChildrenOn();
	}
	else {
		basesw->setAllChildrenOff();
	}
}

void Layer::setSecens(OsgScene* secen)
{
	secen->getRoot()->addChild(basesw.get()); secen->getRoot()->addChild(faciesw.get());
}
