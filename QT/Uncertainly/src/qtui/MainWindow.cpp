#include "qtui/MainWindow.h"
#include "qtui/Uncertainly.h"
#include "qtui/osg/OsgBaseWidget.h"
#include <layer/TreeNodeData.h>

MainWindow::MainWindow()
{
	setGeometry(200, 100, 1000, 800);
	auto center = new QWidget();
	setCentralWidget(center);

	solu = new SolutionDockWidget(this);
	this->addDockWidget(Qt::DockWidgetArea::LeftDockWidgetArea, solu);

	QMenuBar* mBar = this->menuBar();
	mainMenu = mBar->addMenu("文件");
	QAction* action1 = mainMenu->addAction("不确定性分析");

	auto toolbar = this->addToolBar("工具栏");
	toolbar->setMinimumHeight(50);
	QAction* showsecen = toolbar->addAction("显示场景");

	connect(showsecen, &QAction::triggered, this, [=]() {
		OsgBaseWidget* secen = dynamic_cast<OsgBaseWidget*>(centralWidget());
		if (secen == nullptr) {
			OsgBaseWidget* s = new OsgBaseWidget(this);
			setCentralWidget(s);
			s->setGeometry(centralWidget()->geometry());
			s->setAttribute(Qt::WidgetAttribute::WA_DeleteOnClose);
			showsecen->setText("关闭场景");
		}
		else {
			auto center = new QWidget();
			center->setGeometry(secen->geometry());
			setCentralWidget(center);
			showsecen->setText("打开场景");
		}
		});
}

void MainWindow::openSecens()
{
}

void MainWindow::openUncertainly()
{
}
