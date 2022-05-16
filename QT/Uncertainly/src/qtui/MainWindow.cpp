#include "qtui/MainWindow.h"
#include "qtui/Uncertainly.h"
#include "qtui/osg/ModelShow.h"
#include <QtWidgets>

MainWindow::MainWindow()
{
	setGeometry(200,100,1000, 800);
	QMenuBar* mBar = this->menuBar();
	QMenu* menu = mBar->addMenu("�ļ�");
	QAction* action1 = menu->addAction("��ȷ���Է���");
	QAction* action2 = menu->addAction("��ʾ����");

	connect(action2, &QAction::triggered, this, [=]() {
		if (this->centralWidget() == NULL) {
			ModelShow* osg = new ModelShow(this);
			this->setCentralWidget(osg);
			action2->setText("�رճ���");
		}
		else {
			this->setCentralWidget(NULL);
			action2->setText("��ʾ����");
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
