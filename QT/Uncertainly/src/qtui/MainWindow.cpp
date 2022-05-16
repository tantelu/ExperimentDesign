#include "qtui/MainWindow.h"
#include "qtui/Uncertainly.h"
#include "qtui/osg/ModelShow.h"
#include <QtWidgets>

MainWindow::MainWindow()
{
	setGeometry(200,100,1000, 800);
	QMenuBar* mBar = this->menuBar();
	QMenu* menu = mBar->addMenu("文件");
	QAction* action1 = menu->addAction("不确定性分析");
	QAction* action2 = menu->addAction("显示场景");

	connect(action2, &QAction::triggered, this, [=]() {
		if (this->centralWidget() == NULL) {
			ModelShow* osg = new ModelShow(this);
			this->setCentralWidget(osg);
			action2->setText("关闭场景");
		}
		else {
			this->setCentralWidget(NULL);
			action2->setText("显示场景");
		}
		});

	connect(action1, &QAction::triggered, this, [=]() {
		Uncertainly* uncertain = new Uncertainly();
		uncertain->setWindowModality(Qt::ApplicationModal);
		uncertain->setAttribute(Qt::WidgetAttribute::WA_DeleteOnClose);
		uncertain->show();
		});
}

void MainWindow::openSecens()
{
}

void MainWindow::openUncertainly()
{
}
