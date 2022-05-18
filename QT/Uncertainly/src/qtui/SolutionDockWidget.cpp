#include "qtui\SolutionDockWidget.h"
#include "qtui\QModelTreeWidget.h"
#include <QtWidgets>

SolutionDockWidget::SolutionDockWidget(QWidget* parent) :QDockWidget(parent)
{
	auto soluTree = new QModelTreeWidget(this);
	setWidget(soluTree);
}
