#include "qtui/MainWindow.h"
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
	showsecen = toolbar->addAction("�򿪳���");

	connect(showsecen, &QAction::triggered, this, [=]() {this->openAndCloseSecens((showsecen->text() == "�򿪳���")); });
	auto tree = solu->treeWidget();
	connect(tree, &QModelTreeWidget::previewScenes, this, &MainWindow::openSecens);
	connect(tree, &QModelTreeWidget::itemClicked, this, &MainWindow::treeItemClick);
}

void MainWindow::openSecens(QModelTreeWidget* tree)
{
	openAndCloseSecens(true);
}

void MainWindow::openAndCloseSecens(bool open)
{
	OsgBaseWidget* secen = dynamic_cast<OsgBaseWidget*>(centralWidget());
	if (secen == nullptr) {
		if (open) {
			OsgBaseWidget* s = new OsgBaseWidget(this);
			connect(s, SIGNAL(OsgBaseWidget::destroyed), this, SLOT(secenclosed));
			setCentralWidget(s);
			s->setAttribute(Qt::WidgetAttribute::WA_DeleteOnClose);
			showsecen->setText("�رճ���");
		}
	}
	else {
		if (!open) {
			auto center = new QWidget();
			center->setGeometry(secen->geometry());
			setCentralWidget(center);
			showsecen->setText("�򿪳���");
		}
	}
}

void MainWindow::openUncertainly()
{
}

void MainWindow::treeItemClick(QTreeWidgetItem* item, int index)
{
	OsgBaseWidget* secen = dynamic_cast<OsgBaseWidget*>(centralWidget());
	if (secen != nullptr)
	{
		if (index == 0 && item != nullptr) {
			auto data = item->data(0, Qt::UserRole).value<TreeNodeData>();
			if (item->checkState(0) == Qt::Checked) {
				if (data.getLayerIndex() == -1) {
					int newi = secen->getScene()->addLayer(data.getUrl().toStdString());
					data.updateLayerIndex(newi);
				}
				auto layer = secen->getScene()->GetLayer(data.getLayerIndex());
				layer->setVisible(true);
			}
			else {
				if (data.getLayerIndex() >= 0) {
					auto layer = secen->getScene()->GetLayer(data.getLayerIndex());
					layer->setVisible(false);
				}
			}
			item->setData(0, Qt::UserRole, QVariant::fromValue(data));
		}
	}
}

void MainWindow::secenclosed()
{
	
}
