#pragma once
#pragma execution_character_set("utf-8")
#include <qdockwidget.h>
#include <QTreeWidget>

class SolutionDockWidget :
	public QDockWidget
{
public:
	SolutionDockWidget(QWidget* parent = nullptr);

	QTreeWidget* treeWidget() { return dynamic_cast<QTreeWidget*>(widget()); }
};

