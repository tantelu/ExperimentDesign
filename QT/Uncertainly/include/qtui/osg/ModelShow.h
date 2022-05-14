#pragma once
#pragma execution_character_set("utf-8")
#include <qmainwindow.h>
#include "OsgContainer.h"
#include <QtWidgets>

class ModelShow :
	public QWidget
{
	Q_OBJECT
	QPushButton* button1;
	QPushButton* button2;
	QGridLayout* unitlistLayout;
	OsgContainer* osgViewer;  //���ǵ���ǰ����Ŷ��

public:
	ModelShow(QWidget* parent = Q_NULLPTR);
};

