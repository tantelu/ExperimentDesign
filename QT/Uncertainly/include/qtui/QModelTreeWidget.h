#pragma once
#pragma execution_character_set("utf-8")
#include <QWidget>
#include <QTreeWidget>
#include <QMenu>
#include <QContextMenuEvent>

class QModelTreeWidget :public QTreeWidget
{
	Q_OBJECT
private:
	QMenu* rootMenu = nullptr;
	QMenu* blankMenu = nullptr;
	QMenu* itemMenu = nullptr;
public:
	QMenu* getRootMenu();
	QMenu* getBlankMenu();
	QMenu* getItemMenu();

	QModelTreeWidget(QWidget* parent = nullptr);

Q_SIGNALS:
	void previewScenes(QModelTreeWidget* widget);

public Q_SLOTS:
	void addRoot();

	void addModel();

	void delModel();

	void calConnectVolumn();

	void showTreeRightMenu(QPoint pos);
};

