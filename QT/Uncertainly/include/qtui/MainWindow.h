#pragma once
#pragma execution_character_set("utf-8")
#include <qmainwindow.h>
#include <QtWidgets>
#include <qtui/SolutionDockWidget.h>

class MainWindow :
    public QMainWindow
{
    Q_OBJECT
private:
    QMenu* mainMenu;
    SolutionDockWidget* solu;
public:
    MainWindow();

public Q_SLOTS:
    void openSecens();
    void openUncertainly();
};

