#include "qtui/osg/OsgBaseWidget.h"
#include <QApplication>
#include <osg/ShapeDrawable>

OsgBaseWidget::OsgBaseWidget(QWidget* parent)
	: osgQOpenGLWidget(parent) {
	setMouseTracking(true);
	setFocusPolicy(Qt::StrongFocus);
	connect(this, &osgQOpenGLWidget::initialized, this, &OsgBaseWidget::initOsg);
	this->setAttribute(Qt::WidgetAttribute::WA_DeleteOnClose);
}

void OsgBaseWidget::initOsg() {
	root = new osg::Group;
	getRoot()->setName("Root");
	auto viewer = getOsgViewer();
	viewer->setSceneData(root);
	tm = new osgGA::TrackballManipulator();
	sh = new osgViewer::StatsHandler();
	th = new osgViewer::ThreadingHandler();
	hp = new osgViewer::HelpHandler();
	sm = new osgGA::StateSetManipulator(getOsgViewer()->getCamera()->getOrCreateStateSet());
	viewer->setCameraManipulator(tm.get());
	viewer->addEventHandler(sh.get());
	viewer->addEventHandler(th.get());
	viewer->addEventHandler(hp.get());
	viewer->addEventHandler(sm.get());
	viewer->getCamera()->setProjectionMatrixAsPerspective(30.0f, width() / height(), 1.0f, 10000.0f);
	getRoot()->getOrCreateStateSet()->setMode(GL_LIGHTING, osg::StateAttribute::ON);
	getRoot()->getOrCreateStateSet()->setMode(GL_DEPTH_TEST, osg::StateAttribute::ON);
	viewer->realize();
	startTimer(10);
}
