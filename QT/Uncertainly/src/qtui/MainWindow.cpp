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
	QAction* addmodel = toolbar->addAction("添加模型");
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

	connect(addmodel, &QAction::triggered, this,
		[=]()
		{
			QString fileName = QFileDialog::getOpenFileName(this, tr("文件对话框！"), "F:", tr("模型文件(*osgurl)"));
			if (QFile::exists(fileName))
			{
				auto root = solu->treeWidget()->topLevelItem(0);
				QTreeWidgetItem* childItem1 = new QTreeWidgetItem();
				childItem1->setText(0, "Facie");
				childItem1->setCheckState(0, Qt::CheckState::Checked);
				OsgBaseWidget* ctrl = dynamic_cast<OsgBaseWidget*>(centralWidget());
				if (ctrl != nullptr) {
					int index = ctrl->getScene()->addDiscreteLayer(fileName.toStdString());
					TreeNodeData nodeData(fileName, index);
					childItem1->setData(0, 0, QVariant::fromValue(nodeData));
					root->addChild(childItem1);
				}
			}
		});

	connect(solu->treeWidget(), &QTreeWidget::itemClicked, this, [=](QTreeWidgetItem* item, int column)
		{
			bool check = item->checkState(column) == Qt::CheckState::Checked;
			auto data = item->data(column, 0).value<TreeNodeData>();
			OsgBaseWidget* ctrl = dynamic_cast<OsgBaseWidget*>(centralWidget());
			if (ctrl != nullptr && !data.getUrl().isEmpty()) {
				auto layer = ctrl->getScene()->GetLayer(data.getLayerIndex());
				if (layer != nullptr) {
					layer->setVisible(check);
				}
				else {
					int index = ctrl->getScene()->addDiscreteLayer(data.getUrl().toStdString());
					TreeNodeData nodeData(data.getUrl(), index);
					item->setData(0, 0, QVariant::fromValue(nodeData));
				}
			}
		});

	connect(solu->treeWidget(), &QTreeWidget::itemDoubleClicked, this, [=](QTreeWidgetItem* item, int column)
		{
			bool check = item->checkState(column) == Qt::CheckState::Checked;
			auto data = item->data(column, 0).value<TreeNodeData>();
			OsgBaseWidget* ctrl = dynamic_cast<OsgBaseWidget*>(centralWidget());
			if (ctrl != nullptr && !data.getUrl().isEmpty()) {
				auto layer = ctrl->getScene()->GetLayer(data.getLayerIndex());
				if (layer != nullptr) {
					if (qApp->mouseButtons() == Qt::RightButton) {
						layer->showConnect();
					}
					else if(qApp->mouseButtons() == Qt::LeftButton){
						layer->closeVisibleFilter();
					}
				}
			}
		});
}

void MainWindow::openSecens()
{
}

void MainWindow::openUncertainly()
{
}
