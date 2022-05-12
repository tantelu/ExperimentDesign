#pragma once
#pragma execution_character_set("utf-8")

#include <QtCharts/QtCharts>
QT_CHARTS_USE_NAMESPACE

#ifdef _DEBUG
#pragma comment(lib,"Qt5Chartsd.lib") //Debug
#else
#pragma comment(lib,"Qt5Charts.lib") //Release
#endif 

#include <QtWidgets/QMainWindow>
#include "ui_Uncertainly.h"

class Uncertainly : public QMainWindow
{
    Q_OBJECT
private:
    QWidget* center;
    QGridLayout* layout1;

    QCheckBox* newproject;
    QCheckBox* editExit;
    QTextEdit* projectName;
    QComboBox* hasProject;
    QTabWidget* tab;

    QPushButton* ok;
    QSpacerItem* horizonok;
    QWidget* lastWidget;
    QGridLayout* lastlayout;

    void InitComponent();
    
public:
    Uncertainly(QWidget *parent = Q_NULLPTR);

public Q_SLOTS:
    void Response();

//private:
//    Ui::UncertainlyClass ui;
};
