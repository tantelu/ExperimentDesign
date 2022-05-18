#pragma once
#pragma execution_character_set("utf-8")  
#include <map>
#include <osg/Node>
#include <osg/Group>
#include <osgViewer/Viewer>
#include "gslib/model.h"
#include "QDebug"
using namespace std;

class DiscreteLayer
{
private:
	string url;
	shared_ptr<GslibModel<int>> model;
	osg::ref_ptr<osg::Switch> sw;
	osg::ref_ptr<osg::Vec3Array> vecArray;
	map<int, osg::Vec4> colorS;
	set<int> allFacies;
public:
	DiscreteLayer(const string& url);

	void setVisible(bool checked);

	void setVisibleFilter(set<int>& filter);

	void showConnect();

	void closeVisibleFilter() { setVisibleFilter(allFacies); }

	osg::Switch* getSwitch();

	const GslibModel<int>* getModel() { return model.get(); }

	void print(QString str) { qDebug() << str << model.use_count() << endl; }
};
