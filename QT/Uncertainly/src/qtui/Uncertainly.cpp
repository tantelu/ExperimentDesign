#include "qtui/Uncertainly.h"
#include "qmessagebox.h"

void Uncertainly::InitComponent()
{
	resize(1005, 627);
	setMaximumSize(QSize(16777215, 16777215));
	newproject = new QCheckBox(this);
	newproject->setText("新建");
	newproject->setFixedSize(100, 30);

	editExit = new QCheckBox(this);
	editExit->setText("编辑现有");

	projectName = new QTextEdit(this);
	projectName->setFixedHeight(30);
	QSizePolicy sizePolicy1(QSizePolicy::Preferred, QSizePolicy::Minimum);
	projectName->setSizePolicy(sizePolicy1);

	hasProject = new QComboBox(this);

	tab = new QTabWidget(this);
	workTab = new QWorkWidget();
	variableTab = new QTableWidget(4, 3);
	uncertainTab = new QWidget();

	tab->addTab(workTab, "工作流");
	tab->addTab(variableTab, "变量");
	tab->addTab(uncertainTab, "不确定性");
	tab->tabBar()->setShape(QTabBar::TriangularNorth);
	tab->tabBar()->setStyleSheet("QTabBar::tab{min-height: 30px; min-width: 80px;}");

	//布局最后一行
	lastWidget = new QWidget(this);
	ok = new QPushButton(lastWidget);
	ok->setText("运行");
	save = new QPushButton(lastWidget);
	save->setText("保存工作流");
	cancel = new QPushButton(lastWidget);
	cancel->setText("取消");
	lastRowSpace = new QSpacerItem(40, 20, QSizePolicy::Expanding, QSizePolicy::Minimum);
	lastlayout = new QGridLayout(lastWidget);
	lastlayout->addWidget(ok, 0, 2, 1, 1);
	lastlayout->addWidget(save, 0, 0, 1, 1);
	lastlayout->addWidget(cancel, 0, 3, 1, 1);
	lastlayout->addItem(lastRowSpace, 0, 1, 1, 1);

	layout1 = new QGridLayout(this);
	layout1->setSpacing(15);
	layout1->setContentsMargins(20, 20, 20, 0);
	layout1->setObjectName(QString::fromUtf8("gridLayout"));
	layout1->addWidget(newproject, 0, 0);
	layout1->addWidget(editExit, 1, 0);
	layout1->addWidget(projectName, 0, 1);
	layout1->addWidget(hasProject, 1, 1);
	layout1->addWidget(tab, 2, 0, 1, 2);
	layout1->addWidget(lastWidget, 3, 0, 1, 2);

	this->setLayout(layout1);
}

void Uncertainly::SetTable()
{
	/*variableTab->setHorizontalHeaderLabels(QStringList() << "教程" << "网址" << "状态");
	variableTab->setItem(0, 0, new QTableWidgetItem("C语言教程"));
	variableTab->setItem(0, 1, new QTableWidgetItem("http://c.biancheng.net/c/"));
	variableTab->setItem(0, 2, new QTableWidgetItem("已更新完毕"));
	variableTab->setItem(1, 0, new QTableWidgetItem("Qt教程"));
	variableTab->setItem(1, 1, new QTableWidgetItem("http://c.biancheng.net/qt/"));
	variableTab->setItem(1, 2, new QTableWidgetItem("正在更新"));
	variableTab->setItem(2, 0, new QTableWidgetItem("C++教程"));
	variableTab->setItem(2, 1, new QTableWidgetItem("http://c.biancheng.net/cplus/"));
	variableTab->setItem(2, 2, new QTableWidgetItem("已更新完毕"));*/
}

Uncertainly::Uncertainly(QWidget* parent)
	: QWidget(parent)
{
	InitComponent();
}
