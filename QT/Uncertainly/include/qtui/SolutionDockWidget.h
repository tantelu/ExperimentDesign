#pragma once
#pragma execution_character_set("utf-8")
#include <QWidget>
#include <qdockwidget.h>
#include "qtui/QModelTreeWidget.h"
#include <QMenu>

class SolutionDockWidget :
	public QDockWidget
{
	Q_OBJECT
public:

	SolutionDockWidget(QWidget* parent = nullptr);

	QModelTreeWidget* treeWidget() { return dynamic_cast<QModelTreeWidget*>(widget()); }

public Q_SLOTS:
	
};

