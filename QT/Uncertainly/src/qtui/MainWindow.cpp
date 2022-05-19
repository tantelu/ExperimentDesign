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
	mainMenu = mBar->addMenu("�ļ�");
	QAction* action1 = mainMenu->addAction("��ȷ���Է���");

	auto toolbar = this->addToolBar("������");
	toolbar->setMinimumHeight(50);
	QAction* showsecen = toolbar->addAction("��ʾ����");

	connect(showsecen, &QAction::triggered, this, [=]() {
		OsgBaseWidget* secen = dynamic_cast<OsgBaseWidget*>(centralWidget());
		if (secen == nullptr) {
			OsgBaseWidget* s = new OsgBaseWidget(this);
			setCentralWidget(s);
			s->setGeometry(centralWidget()->geometry());
			s->setAttribute(Qt::WidgetAttribute::WA_DeleteOnClose);
			showsecen->setText("�رճ���");
		}
		else {
			auto center = new QWidget();
			center->setGeometry(secen->geometry());
			setCentralWidget(center);
			showsecen->setText("�򿪳���");
		}
		});
}

void MainWindow::openSecens()
{
}

void MainWindow::openUncertainly()
{
}
