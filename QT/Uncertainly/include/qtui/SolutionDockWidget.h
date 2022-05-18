#pragma once
#pragma execution_character_set("utf-8")

#include <QWidget>
#include <qdockwidget.h>
#include <QTreeWidget>
#include <QMenu>

class SolutionDockWidget :
	public QDockWidget
{
	Q_OBJECT
public:

	SolutionDockWidget(QWidget* parent = nullptr);

	QTreeWidget* treeWidget() { return dynamic_cast<QTreeWidget*>(widget()); }

public Q_SLOTS:
	
};

