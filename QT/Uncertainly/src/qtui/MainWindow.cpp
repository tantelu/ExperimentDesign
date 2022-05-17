#include "qtui/MainWindow.h"
#include "qtui/Uncertainly.h"
#include "qtui/osg/OsgBaseWidget.h"

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
			QString fileName = QFileDialog::getOpenFileName(this, tr("�ļ��Ի���"), "F:", tr("ģ���ļ�(*gslib *txt)"));
			if (QFile::exists(fileName))
			{
				auto root = solu->treeWidget()->topLevelItem(0);
				QTreeWidgetItem* childItem1 = new QTreeWidgetItem();
				childItem1->setText(0, "�ڵ�1");
				childItem1->setCheckState(0, Qt::CheckState::Unchecked);
				GslibModel<int>* model = new GslibModel<int>(fileName.toStdString(), 204, 366, 40);
				DiscreteLayer* layer = new DiscreteLayer(model);
				QVariant data(QVariant::Type::UserType);
				data.setValue(layer);
				childItem1->setData(0, 0, data);
				root->addChild(childItem1);
				OsgBaseWidget* secen = dynamic_cast<OsgBaseWidget*>(centralWidget());
				if (secen != nullptr) {
					secen->addDiscreteLayer(layer);
				}
			}
		});

	//connect(solu->treeWidget(), &QTreeWidget::itemClicked, this, [=](QTreeWidgetItem* item, int column)
	//	{
	//		bool check = item->checkState(column) == Qt::CheckState::Checked;
	//		DiscreteLayer* layer = (DiscreteLayer*)((item->data(column, 0)).data());
	//		layer->setVisible(check);
	//	});
}

void MainWindow::openSecens()
{
}

void MainWindow::openUncertainly()
{
}
