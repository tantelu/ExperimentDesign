#pragma once
#pragma execution_character_set("utf-8")
#include <qmainwindow.h>

class MainWindow :
    public QMainWindow
{
    Q_OBJECT
public:
    MainWindow();

public Q_SLOTS:
    void openSecens();
    void openUncertainly();
};

