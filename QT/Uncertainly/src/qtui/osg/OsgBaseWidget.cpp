#include "qtui/osg/OsgBaseWidget.h"
#include <osgGA/MultiTouchTrackballManipulator>
#include <osgGA/StateSetManipulator>
#include <osgViewer/ViewerEventHandlers>
#include <QApplication>
#include <osg/ShapeDrawable>

OsgBaseWidget::OsgBaseWidget(QWidget* parent)
	: osgQOpenGLWidget(parent) {
	setMouseTracking(true);
	setFocusPolicy(Qt::StrongFocus);
	connect(this, &osgQOpenGLWidget::initialized, this, &OsgBaseWidget::initOsg);
}

OsgBaseWidget::~OsgBaseWidget()
{

}

void OsgBaseWidget::initOsg() {
	osg::ref_ptr<osg::Group> root = new osg::Group;
	root->setName("Root");
	auto viewer = getOsgViewer();
	viewer->setSceneData(root);
	osg::ref_ptr<osgGA::TrackballManipulator> manipulator = new osgGA::TrackballManipulator;
	viewer->setCameraManipulator(manipulator);
	viewer->addEventHandler(new osgViewer::StatsHandler);
	viewer->addEventHandler(new osgViewer::ThreadingHandler());
	viewer->addEventHandler(new osgViewer::HelpHandler);
	viewer->addEventHandler(new osgGA::StateSetManipulator(getOsgViewer()->getCamera()->getOrCreateStateSet()));
	viewer->getCamera()->setProjectionMatrixAsPerspective(30.0f, width() / height(), 1.0f, 10000.0f);
	root->getOrCreateStateSet()->setMode(GL_LIGHTING, osg::StateAttribute::ON);
	root->getOrCreateStateSet()->setMode(GL_DEPTH_TEST, osg::StateAttribute::ON);
	viewer->realize();
	startTimer(10);
}

void OsgBaseWidget::addCylinder() {
	for (size_t i = 0; i < 5; i++)
	{
		auto x = rand() % 10;
		auto y = rand() % 10;
		auto z = rand() % 10;
		if (x > 5)
		{
			osg::ref_ptr<osg::Cylinder> cylinder = new osg::Cylinder(osg::Vec3(x, y, z), 0.25f, 0.5f);
			osg::ref_ptr<osg::ShapeDrawable> sd = new osg::ShapeDrawable(cylinder);
			sd->setColor(osg::Vec4(0.8f, 0.5f, 0.2f, 1.f));
			osg::ref_ptr < osg::Geode> geode = new osg::Geode;
			geode->addDrawable(sd);
			getRoot()->addChild(geode);
		}
		else {
			osg::ref_ptr<osg::Sphere> sphere = new osg::Sphere(osg::Vec3(x, y, z), 0.5f);
			osg::ref_ptr<osg::ShapeDrawable> sd = new osg::ShapeDrawable(sphere);
			sd->setColor(osg::Vec4(0.8f, 0.5f, 0.8f, 1.f));
			osg::ref_ptr < osg::Geode> geode = new osg::Geode;
			geode->addDrawable(sd);
			getRoot()->addChild(geode);
		}
	}
}


