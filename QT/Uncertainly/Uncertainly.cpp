#include "Uncertainly.h"
#include"qmessagebox.h"

void Uncertainly::InitComponent()
{
	resize(1005, 627);
	setMaximumSize(QSize(16777215, 16777215));
	center = new QWidget(this);
	setCentralWidget(center);//MainWindom本身有自己的布局,不能直接setLayout,MainWindom需要通过设置CentralWidget来布局
	newproject = new QCheckBox(center);
	newproject->setText("新建");
	newproject->setFixedSize(100, 30);

	editExit = new QCheckBox(center);
	editExit->setText("编辑现有");

	projectName = new QTextEdit(center);
	projectName->setFixedHeight(30);
	QSizePolicy sizePolicy1(QSizePolicy::Preferred, QSizePolicy::Minimum);
	projectName->setSizePolicy(sizePolicy1);

	hasProject = new QComboBox(center);

	tab = new QTabWidget(center);

	lastWidget = new QWidget(center);

	ok = new QPushButton(lastWidget);
	ok->setText("运行");

	lastlayout = new QGridLayout(lastWidget);
	lastlayout->addWidget(ok, 0, 1, 1, 1);
	horizonok = new QSpacerItem(40, 20, QSizePolicy::Expanding, QSizePolicy::Minimum);
	lastlayout->addItem(horizonok, 0, 0, 1, 1);

	layout1 = new QGridLayout(center);
	layout1->setSpacing(6);
	layout1->setContentsMargins(11, 11, 11, 11);
	layout1->setObjectName(QString::fromUtf8("gridLayout"));
	layout1->addWidget(newproject, 0, 0);
	layout1->addWidget(editExit, 1, 0);
	layout1->addWidget(projectName, 0, 1);
	layout1->addWidget(hasProject, 1, 1);
	layout1->addWidget(tab, 2, 0, 1, 2);
	layout1->addWidget(lastWidget, 3, 0, 1, 2);

	center->setLayout(layout1);
}

Uncertainly::Uncertainly(QWidget* parent)
	: QMainWindow(parent)
{
	InitComponent();
}

void Uncertainly::Response()
{
	QMessageBox::information(this, "提示", "信息已收到", QMessageBox::Yes);
}
