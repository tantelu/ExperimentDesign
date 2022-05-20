#include "qtui\QModelTreeWidget.h"
#include "qtui/osg/OsgBaseWidget.h"
#include "QFileDialog"
#include <layer/TreeNodeData.h>

QMenu* QModelTreeWidget::getRootMenu()
{
	if (rootMenu == nullptr) {
		rootMenu = new QMenu(this);
		auto ac1 = rootMenu->addAction("Ԥ��");
		auto ac2 = rootMenu->addAction("���ģ��");
		auto ac3 = rootMenu->addAction("ɾ��");
		connect(ac1, &QAction::triggered, this, [this](bool checked)
			{ 
				emit previewScenes(this); 
			});
		connect(ac2, SIGNAL(triggered(bool)), this, SLOT(addModel()));
		connect(ac3, SIGNAL(triggered(bool)), this, SLOT(delModel()));
	}
	return rootMenu;
}

QMenu* QModelTreeWidget::getBlankMenu()
{
	if (blankMenu == nullptr) {
		blankMenu = new QMenu(this);
		auto ac1 = blankMenu->addAction("�½�Ŀ¼");
		connect(ac1, SIGNAL(triggered(bool)), this, SLOT(addRoot()));
	}
	return blankMenu;
}

QMenu* QModelTreeWidget::getItemMenu()
{
	if (itemMenu == nullptr) {
		itemMenu = new QMenu(this);
		auto ac1 = itemMenu->addAction("������ͨ��");
		auto ac2 = itemMenu->addAction("ɾ��");
		connect(ac1, SIGNAL(triggered(bool)), this, SLOT(calConnectVolumn()));
		connect(ac2, SIGNAL(triggered(bool)), this, SLOT(delModel()));
	}
	return itemMenu;
}

QModelTreeWidget::QModelTreeWidget(QWidget* parent) :QTreeWidget(parent)
{
	setHeaderHidden(true);
	setContextMenuPolicy(Qt::CustomContextMenu);
	this->connect(this, SIGNAL(customContextMenuRequested(QPoint)),
		this, SLOT(showTreeRightMenu(QPoint)));
	//connect(this, SIGNAL(itemClicked(QTreeWidgetItem*, int)), this, SLOT(itemClick(QTreeWidgetItem*, int)));
}

void QModelTreeWidget::showTreeRightMenu(QPoint pos)
{
	//����pos�ж�����һ�λ�������ĸ��ڵ�root�����Ǻ��ӽڵ�child�����߶�����
	//����һ���λ�������Ľڵ㣬��item�Ƕ�Ӧ�Ľڵ���Ϣ������ΪNULL
	QTreeWidgetItem* item = itemAt(pos);//�ؼ�
	if (item)
	{
		if (item->parent()) //�и�
		{
			getItemMenu()->move(cursor().pos());
			getItemMenu()->show();
		}
		else { //���ڵ�˵�
			getRootMenu()->move(cursor().pos());
			getRootMenu()->show();
		}
	}
	else
	{
		if (topLevelItemCount() == 0) {
			getBlankMenu()->move(cursor().pos());
			getBlankMenu()->show();
		}
	}
}

void QModelTreeWidget::addRoot()
{
	QTreeWidgetItem* item = new QTreeWidgetItem(this);
	item->setText(0, tr("�������"));
}

void QModelTreeWidget::addModel()
{
	if (currentItem()) {
		QString fileName = QFileDialog::getOpenFileName(this, tr("�ļ��Ի���"), "F:", tr("ģ���ļ�(*osgurl)"));
		if (QFile::exists(fileName))
		{
			QTreeWidgetItem* item = new QTreeWidgetItem(currentItem());
			//item->setFlags(Qt::ItemIsSelectable | Qt::ItemIsUserCheckable | Qt::ItemIsEnabled | Qt::ItemIsAutoTristate);
			item->setCheckState(0, Qt::CheckState::Unchecked);
			item->setText(0, "�½ڵ�");
			TreeNodeData nodeData(fileName, -1);
			item->setData(0, Qt::UserRole, QVariant::fromValue(nodeData));
		}
	}
}

void QModelTreeWidget::delModel()
{
	if (currentItem()->parent()) {
		currentItem()->parent()->removeChild(currentItem());
	}
	else {
		this->takeTopLevelItem(indexOfTopLevelItem(currentItem()));
	}
}

void QModelTreeWidget::calConnectVolumn()
{

}


