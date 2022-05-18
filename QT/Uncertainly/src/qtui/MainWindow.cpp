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
	QAction* addmodel = toolbar->addAction("���ģ��");
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

	connect(addmodel, &QAction::triggered, this,
		[=]()
		{
			QString fileName = QFileDialog::getOpenFileName(this, tr("�ļ��Ի���"), "F:", tr("ģ���ļ�(*osgurl)"));
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
