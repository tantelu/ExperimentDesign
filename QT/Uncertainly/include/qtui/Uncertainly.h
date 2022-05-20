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
    /// ���������Ĳ���
    /// </summary>
    QGridLayout* layout1;
    /// <summary>
    /// ���½���ѡ���
    /// </summary>
    QCheckBox* newproject;
    /// <summary>
    /// ���༭���С�ѡ���
    /// </summary>
    QCheckBox* editExit;
    /// <summary>
    /// �������� �����
    /// </summary>
    QTextEdit* projectName;
    /// <summary>
    /// ѡ������ ��ѡ��
    /// </summary>
    QComboBox* hasProject;
    /// <summary>
    /// ����������tabҳ
    /// </summary>
    QTabWidget* tab;
    /// <summary>
    /// ������ҳ
    /// </summary>
    QWorkWidget* workTab;
    /// <summary>
    /// ����ҳ
    /// </summary>
    QTableWidget* variableTab;
    /// <summary>
    /// ��ȷ������ҳ
    /// </summary>
    QWidget* uncertainTab;

    /// <summary>
    /// ���� ��ť
    /// </summary>
    QPushButton* ok;
    /// <summary>
    /// ���湤���� ��ť
    /// </summary>
    QPushButton* save;
    /// <summary>
    /// ȡ�� ��ť
    /// </summary>
    QPushButton* cancel;
    /// <summary>
    /// ��ť�� ����
    /// </summary>
    QSpacerItem* lastRowSpace;
    /// <summary>
    /// ��ť��Widget
    /// </summary>
    QWidget* lastWidget;
    /// <summary>
    /// ��ť�� ����
    /// </summary>
    QGridLayout* lastlayout;

    void InitComponent();

    void SetTable();
    
public:
    Uncertainly(QWidget *parent = Q_NULLPTR);
};
