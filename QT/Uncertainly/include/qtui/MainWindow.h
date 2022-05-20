#pragma once
#pragma execution_character_set("utf-8")
#include <qmainwindow.h>
#include <QtWidgets>
#include <qtui/SolutionDockWidget.h>
#include "qtui\QModelTreeWidget.h"
#include <QTreeWidgetItem>

class MainWindow :
    public QMainWindow
{
    Q_OBJECT
private:
    int index = 0;
    QMenu* mainMenu;
    SolutionDockWidget* solu;
    QAction* showsecen;
public:
    MainWindow();

public Q_SLOTS:
    void openSecens(QModelTreeWidget* tree);
    void secenclosed();
    void openAndCloseSecens(bool open);
    void openUncertainly();
    void treeItemClick(QTreeWidgetItem* item, int index);
};

