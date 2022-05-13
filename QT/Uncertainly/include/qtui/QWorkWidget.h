#pragma once
#pragma execution_character_set("utf-8")

#include <qwidget.h>
#include <QtWidgets>
#include "QWorkUnitWidget.h"

class QWorkWidget :
    public QWidget
{
private:
    QScrollArea* scroll;
    QWidget* center;
    QPushButton* add;
    QVBoxLayout* unitlistLayout;
    QSpacerItem* emptyWorkunit;
    void InitComponent();

public Q_SLOTS:
    void AddWorkUnit();

    void Up(QWidget* cur);

    void Down(QWidget* cur);
public:
    QWorkWidget(QWidget* parent = Q_NULLPTR);
};

