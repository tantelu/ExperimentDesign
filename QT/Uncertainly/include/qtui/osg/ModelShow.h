#pragma once
#pragma execution_character_set("utf-8")
#include <qmainwindow.h>
#include "OsgBaseWidget.h"
#include <QtWidgets>

class ModelShow :
	public QWidget
{
	Q_OBJECT
	QPushButton* button1;
	QPushButton* button2;
	QPushButton* button3;
	QGridLayout* unitlistLayout;
	OsgBaseWidget* osgViewer;  //���ǵ���ǰ����Ŷ��

public:
	ModelShow(QWidget* parent = Q_NULLPTR);
};

