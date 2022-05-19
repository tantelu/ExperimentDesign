#pragma once
#include <memory>
#include <map>
#include <set>
#include <scenes/FacieGeode.h>
#include <gslib/model.h>
using namespace std;

class OsgScene;
class Layer
{
private:
	string url;
	shared_ptr<GslibModel<int>> model;
	osg::ref_ptr<osg::Switch> faciesw;
	osg::ref_ptr<osg::Vec3Array> vecArray;
	map<int, osg::Vec4> defaultColors;
	set<int> allFacies;
	map<int, shared_ptr<FacieGeode>> facieGeos;
	map<int, shared_ptr<FacieGeode>> basegeos;
	osg::ref_ptr<osg::Switch> basesw;

	void initBase();
public:
	Layer() { basesw = new Switch; faciesw = new Switch; }

	Layer(const string& url);

	FacieGeode* getGeodeByFacie(int facie);

	void setVisibleFilter(const set<int>& filter);

	void closeVisibleFilter() { faciesw->setAllChildrenOff(); }

	void setVisible(bool checked);

	void setSecens(OsgScene* secen);

	const GslibModel<int>* getModel() { return model.get(); }
};

