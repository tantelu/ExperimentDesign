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

void OsgBaseWidget::addTestModel()
{
	auto root = getRoot();
	size_t ij = 100;
	auto z = rand() % 100;

	osg::ref_ptr<osg::Geometry> geometry = new osg::Geometry;
	osg::Vec4dArray* color = new osg::Vec4dArray;
	osg::Vec3Array* vecArray = new osg::Vec3Array;
	for (size_t i = 0; i < ij; i++)
	{
		for (size_t j = 0; j < ij; j++)
		{
			vecArray->push_back(osg::Vec3d(i, j, z));
			color->push_back(osg::Vec4((float)rand() / (float)RAND_MAX, (float)rand() / (float)RAND_MAX, (float)rand() / (float)RAND_MAX, 1.f));
		}
	}
	osg::ref_ptr<osg::DrawElementsUInt> drawElemUInt = new osg::DrawElementsUInt(GL_TRIANGLES);
	for (size_t i = 0; i < ij - 1; i++)
	{
		for (size_t j = 0; j < ij - 1; j++)
		{
			size_t index = i + j * ij;
			drawElemUInt->push_back(index);
			drawElemUInt->push_back(index + ij);
			drawElemUInt->push_back(index + ij + 1);

			drawElemUInt->push_back(index);
			drawElemUInt->push_back(index + 1);
			drawElemUInt->push_back(index + ij + 1);
		}
	}
	geometry->setVertexArray(vecArray);
	geometry->setColorArray(color);
	color->setBinding(osg::Array::BIND_PER_VERTEX);
	geometry->addPrimitiveSet(drawElemUInt);
	osg::ref_ptr < osg::Geode> geode = new osg::Geode;
	geode->addDrawable(geometry);
	root->addChild(geode);
}


