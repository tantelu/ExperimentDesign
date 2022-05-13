#include "qtui/QWorkWidget.h"

void QWorkWidget::InitComponent()
{
	scroll = new QScrollArea(this);

	center = new QWidget(this);

	add = new QPushButton(this);
	add->setText("添加工作流");

	emptyWorkunit = new QSpacerItem(40, 20, QSizePolicy::Expanding, QSizePolicy::Expanding);

	QGridLayout* workTabLayout = new QGridLayout(this);
	workTabLayout->setContentsMargins(0, 10, 0, 0);
	workTabLayout->setSpacing(10);
	workTabLayout->setColumnStretch(0, 8);
	workTabLayout->setColumnStretch(1, 2);
	workTabLayout->addWidget(add, 0, 1, 1, 1);
	workTabLayout->addWidget(scroll, 1, 0, 1, 2);

	unitlistLayout = new QVBoxLayout(center);
	unitlistLayout->setSpacing(0);
	unitlistLayout->setContentsMargins(0, 0, 0, 0);
	unitlistLayout->addItem(emptyWorkunit);

	scroll->setWidget(center);
	scroll->setSizePolicy(QSizePolicy::Expanding, QSizePolicy::Expanding);
	scroll->setWidgetResizable(true);
	connect(add, &QPushButton::clicked, this, &QWorkWidget::AddWorkUnit);
}

void QWorkWidget::AddWorkUnit()
{
	auto row = unitlistLayout->count();
	QWorkUnitWidget* unit = new QWorkUnitWidget(row, center);
	unitlistLayout->insertWidget(row - 1, unit);
	connect(unit, &QWorkUnitWidget::UpUnit, this, &QWorkWidget::Up);
	connect(unit, &QWorkUnitWidget::DownUnit, this, &QWorkWidget::Down);
}

void QWorkWidget::Up(QWidget* cur)
{
	auto index = unitlistLayout->indexOf(cur);
	if (index > 0) {
		unitlistLayout->removeWidget(cur);
		unitlistLayout->insertWidget(index - 1, cur);
	}
}

void QWorkWidget::Down(QWidget* cur)
{
	auto index = unitlistLayout->indexOf(cur);
	auto lastindex = unitlistLayout->count() - 1;
	if (index < lastindex - 1) {
		unitlistLayout->removeWidget(cur);
		unitlistLayout->insertWidget(index + 1, cur);
	}
}

QWorkWidget::QWorkWidget(QWidget* parent)
{
	InitComponent();
}
