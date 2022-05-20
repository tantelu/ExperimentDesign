#pragma once
#pragma execution_character_set("utf-8")

#include <QtCharts/QtCharts>
QT_CHARTS_USE_NAMESPACE

#ifdef _DEBUG
#pragma comment(lib,"Qt5Chartsd.lib") //Debug
#else
#pragma comment(lib,"Qt5Charts.lib") //Release
#endif 

#include "qtui/QWorkWidget.h"

class Uncertainly : public QWidget
{
    Q_OBJECT
private:
    /// <summary>
    /// 主窗体中心布局
    /// </summary>
    QGridLayout* layout1;
    /// <summary>
    /// ‘新建’选择框
    /// </summary>
    QCheckBox* newproject;
    /// <summary>
    /// ‘编辑现有’选择框
    /// </summary>
    QCheckBox* editExit;
    /// <summary>
    /// 工程名称 输入框
    /// </summary>
    QTextEdit* projectName;
    /// <summary>
    /// 选择现有 复选框
    /// </summary>
    QComboBox* hasProject;
    /// <summary>
    /// 主窗体中心tab页
    /// </summary>
    QTabWidget* tab;
    /// <summary>
    /// 工作流页
    /// </summary>
    QWorkWidget* workTab;
    /// <summary>
    /// 变量页
    /// </summary>
    QTableWidget* variableTab;
    /// <summary>
    /// 不确定分析页
    /// </summary>
    QWidget* uncertainTab;

    /// <summary>
    /// 运行 按钮
    /// </summary>
    QPushButton* ok;
    /// <summary>
    /// 保存工作流 按钮
    /// </summary>
    QPushButton* save;
    /// <summary>
    /// 取消 按钮
    /// </summary>
    QPushButton* cancel;
    /// <summary>
    /// 按钮行 弹簧
    /// </summary>
    QSpacerItem* lastRowSpace;
    /// <summary>
    /// 按钮行Widget
    /// </summary>
    QWidget* lastWidget;
    /// <summary>
    /// 按钮行 布局
    /// </summary>
    QGridLayout* lastlayout;

    void InitComponent();

    void SetTable();
    
public:
    Uncertainly(QWidget *parent = Q_NULLPTR);
};
