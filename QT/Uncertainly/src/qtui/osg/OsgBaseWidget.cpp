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

void OsgBaseWidget::addDiscreteLayer(DiscreteLayer* layer)
{
	auto root = getRoot();
	auto _switch = layer->getSwitch();
	root->addChild(_switch);
	_switch->setAllChildrenOn();
}

void OsgBaseWidget::addTestModel()
{
	GslibModel<int> model("C:\\Users\\24249\\Desktop\\channel.gslib", 500, 500, 1);
	DiscreteLayer* layer = new DiscreteLayer(model);
	addDiscreteLayer(layer);

	/*auto root = getRoot();
	size_t ij = 100;
	auto z = rand() % 100;

	osg::Vec4dArray* colorS = new osg::Vec4dArray;
	colorS->push_back(osg::Vec4(0, 0, 1, 1.f));
	colorS->push_back(osg::Vec4(0, 1.0, 0, 1.f));
	colorS->push_back(osg::Vec4(1.0, 0, 0, 1.f));

	osg::ref_ptr<osg::Geometry> geometry = new osg::Geometry;
	osg::Vec4dArray* color = new osg::Vec4dArray;
	osg::Vec3Array* vecArray = new osg::Vec3Array;
	osg::ref_ptr<osg::DrawElementsUInt> drawElemUInt = new osg::DrawElementsUInt(GL_QUADS);
	for (size_t i = 0; i < ij; i++)
	{
		for (size_t j = 0; j < ij; j++)
		{
			int cpos = int(2.999999 * (float)rand() / (float)RAND_MAX);
			for (size_t index = 0; index < 4; index++)
			{
				auto start = vecArray->size();
				vecArray->push_back(osg::Vec3d(i, j, z));
				vecArray->push_back(osg::Vec3d(i + 1, j, z));
				vecArray->push_back(osg::Vec3d(i + 1, j + 1, z));
				vecArray->push_back(osg::Vec3d(i, j + 1, z));
				color->push_back(colorS->at(cpos));
				color->push_back(colorS->at(cpos));
				color->push_back(colorS->at(cpos));
				color->push_back(colorS->at(cpos));
				drawElemUInt->push_back(start);
				drawElemUInt->push_back(start + 1);
				drawElemUInt->push_back(start + 2);
				drawElemUInt->push_back(start + 3);
			}
		}
	}
	geometry->setVertexArray(vecArray);
	geometry->setColorArray(color);
	color->setBinding(osg::Array::BIND_PER_VERTEX);
	geometry->addPrimitiveSet(drawElemUInt);
	osg::ref_ptr<osg::Geode> geode = new osg::Geode;
	geode->addDrawable(geometry);
	root->addChild(geode);*/
}