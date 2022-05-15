#include "qtui/osg/ModelShow.h"
#include "qtui/osg/OsgBaseWidget.h"

ModelShow::ModelShow(QWidget* parent)
{
	osgViewer = new OsgBaseWidget(this);

	button1 = new QPushButton(this);
	button1->setText("���");

	button2 = new QPushButton(this);
	button2->setText("���");

	unitlistLayout = new QGridLayout(this);
	unitlistLayout->setSpacing(0);
	unitlistLayout->setContentsMargins(0, 0, 0, 0);
	unitlistLayout->addWidget(button1, 0, 0, 1, 1);
	unitlistLayout->addWidget(button2, 0, 1, 1, 1);
	unitlistLayout->addWidget(osgViewer, 1, 0, 1, 2);

	connect(button1, &QPushButton::clicked, osgViewer, &OsgBaseWidget::addCylinder);
	connect(button2, &QPushButton::clicked, osgViewer, [=](){ osgViewer->getRoot()->removeChildren(0, osgViewer->getRoot()->getNumChildren()); });
}
