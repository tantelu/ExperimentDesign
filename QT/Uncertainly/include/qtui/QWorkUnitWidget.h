#pragma once
#include <QtWidgets>

class QWorkUnitWidget :
	public QWidget
{
	Q_OBJECT
private:
	QLabel* label;
	QLineEdit* vars;
	QPushButton* param;
	QPushButton* del;
	QPushButton* up;
	QPushButton* down;
	QLineEdit* description;
	QHBoxLayout* horizon;
	QSpacerItem* empty;

	void InitComponent();

public:
	QWorkUnitWidget(QWidget* parent = Q_NULLPTR);

	QWorkUnitWidget(int index, QWidget* parent = Q_NULLPTR);

Q_SIGNALS:
	void UpUnit(QWidget* widget);
	void DownUnit(QWidget* widget);
};

