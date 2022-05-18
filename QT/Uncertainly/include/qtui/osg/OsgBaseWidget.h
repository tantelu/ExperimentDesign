#pragma once
#pragma execution_character_set("utf-8")
#ifndef OSGCONTAINER_H
#define OSGCONTAINER_H

#include <QOpenGLWidget>
#include <osgQOpenGL/osgQOpenGLWidget>
#include <osgViewer/Viewer>
#include "OsgScene.h"
#include <osgGA/MultiTouchTrackballManipulator>
#include <osgGA/StateSetManipulator>
#include <osgViewer/ViewerEventHandlers>

class OsgBaseWidget : public osgQOpenGLWidget {
	Q_OBJECT
public:
	OsgBaseWidget(QWidget* parent = 0);
	~OsgBaseWidget() 
	{

	}

	osg::Group* getRoot() {
		return root.get();
	}
private:
	shared_ptr<OsgScene> scene;
	osg::ref_ptr<osg::Group> root; 
	osg::ref_ptr<osgGA::TrackballManipulator> tm;
	osg::ref_ptr<osgViewer::StatsHandler> sh;
	osg::ref_ptr<osgViewer::ThreadingHandler> th;
	osg::ref_ptr<osgViewer::HelpHandler> hp;
	osg::ref_ptr<osgGA::StateSetManipulator> sm;
public:
	OsgScene* getScene()
	{
		if (scene.get() == nullptr) {
			scene = make_shared<OsgScene>();
			scene.get()->setRoot(getRoot());
		}
		return scene.get();
	}
private Q_SLOTS:
	void initOsg();
};
#endif // OSGCONTAINER_H


