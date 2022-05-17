#include "qtui\SolutionDockWidget.h"
#include <QtWidgets>

SolutionDockWidget::SolutionDockWidget(QWidget* parent) :QDockWidget(parent)
{
	auto soluTree = new QTreeWidget(this);
	QTreeWidgetItem* topItem = new QTreeWidgetItem();
	topItem->setText(0, "解决方案");
	soluTree->addTopLevelItem(topItem);
	setWidget(soluTree);
}
