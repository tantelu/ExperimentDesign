#include "qtui/QWorkUnitWidget.h"
#include "qtui/QWorkWidget.h"

void QWorkUnitWidget::InitComponent()
{
	//resize(1000, 50);
	setFixedHeight(55);
	label = new QLabel(this);
	label->setMinimumSize(20, 32);
	QSizePolicy labelPolicy(QSizePolicy::Minimum, QSizePolicy::Preferred);
	label->setSizePolicy(labelPolicy);

	vars = new QLineEdit(this);
	vars->setMinimumWidth(100);
	QSizePolicy varsPolicy(QSizePolicy::Minimum, QSizePolicy::Preferred);
	vars->setSizePolicy(varsPolicy);

	param = new QPushButton(this);
	param->setFixedSize(32, 32);
	QIcon paramIcon;
	paramIcon.addFile(tr(":/Uncertainly/image/Setting32.png"));
	param->setIcon(paramIcon);

	del = new QPushButton(this);
	del->setObjectName("É¾³ý");
	del->setFixedSize(32, 32);
	QIcon clearIcon;
	clearIcon.addFile(tr(":/Uncertainly/image/Delete32.png"));
	del->setIcon(clearIcon);

	up = new QPushButton(this);
	up->setFixedSize(32, 32);
	QIcon upIcon;
	upIcon.addFile(tr(":/Uncertainly/image/Up32.png"));
	up->setIcon(upIcon);

	down = new QPushButton(this);
	down->setFixedSize(32, 32);
	QIcon dowmIcon;
	dowmIcon.addFile(tr(":/Uncertainly/image/Down32.png"));
	down->setIcon(dowmIcon);

	description = new QLineEdit(this);
	description->setMinimumWidth(200);
	QSizePolicy descriptionPolicy(QSizePolicy::Minimum, QSizePolicy::Preferred);
	description->setSizePolicy(descriptionPolicy);

	empty = new QSpacerItem(100, 32, QSizePolicy::Expanding, QSizePolicy::Preferred);

	horizon = new QHBoxLayout(this);
	horizon->addWidget(label);
	horizon->addWidget(vars);
	horizon->addWidget(param);
	horizon->addWidget(description);
	horizon->addWidget(del);
	horizon->addWidget(up);
	horizon->addWidget(down);
	horizon->addItem(empty);

	connect(del, &QPushButton::clicked, this, &QWorkUnitWidget::deleteLater);
	connect(up, &QPushButton::clicked, [=]() {emit UpUnit(this); });
	connect(down, &QPushButton::clicked, [=]() {emit DownUnit(this); });
}

QWorkUnitWidget::QWorkUnitWidget(QWidget* parent) :QWidget(parent)
{
	InitComponent();
}

QWorkUnitWidget::QWorkUnitWidget(int index, QWidget* parent)
{
	InitComponent();
	this->label->setText(QString::number(index));
}
