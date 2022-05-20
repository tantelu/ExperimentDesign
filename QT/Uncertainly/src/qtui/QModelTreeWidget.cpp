#include "qtui\QModelTreeWidget.h"
#include "qtui/osg/OsgBaseWidget.h"
#include "QFileDialog"
#include <layer/TreeNodeData.h>

QMenu* QModelTreeWidget::getRootMenu()
{
	if (rootMenu == nullptr) {
		rootMenu = new QMenu(this);
		auto ac1 = rootMenu->addAction("预览");
		auto ac2 = rootMenu->addAction("添加模型");
		auto ac3 = rootMenu->addAction("删除");
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
		auto ac1 = blankMenu->addAction("新建目录");
		connect(ac1, SIGNAL(triggered(bool)), this, SLOT(addRoot()));
	}
	return blankMenu;
}

QMenu* QModelTreeWidget::getItemMenu()
{
	if (itemMenu == nullptr) {
		itemMenu = new QMenu(this);
		auto ac1 = itemMenu->addAction("计算连通体");
		auto ac2 = itemMenu->addAction("删除");
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
	//根据pos判断鼠标右击位置是树的根节点root，还是孩子节点child，或者都不是
	//鼠标右击的位置是树的节点，则item是对应的节点信息，否则为NULL
	QTreeWidgetItem* item = itemAt(pos);//关键
	if (item)
	{
		if (item->parent()) //有根
		{
			getItemMenu()->move(cursor().pos());
			getItemMenu()->show();
		}
		else { //根节点菜单
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
	item->setText(0, tr("解决方案"));
}

void QModelTreeWidget::addModel()
{
	if (currentItem()) {
		QString fileName = QFileDialog::getOpenFileName(this, tr("文件对话框！"), "F:", tr("模型文件(*osgurl)"));
		if (QFile::exists(fileName))
		{
			QTreeWidgetItem* item = new QTreeWidgetItem(currentItem());
			//item->setFlags(Qt::ItemIsSelectable | Qt::ItemIsUserCheckable | Qt::ItemIsEnabled | Qt::ItemIsAutoTristate);
			item->setCheckState(0, Qt::CheckState::Unchecked);
			item->setText(0, "新节点");
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


