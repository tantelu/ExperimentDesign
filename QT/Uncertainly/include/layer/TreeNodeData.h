#pragma once
#include <QVariant>

class TreeNodeData {
private:
	int index;
	QString url;
public:
	TreeNodeData() { index = -1; url = QString(); }
	TreeNodeData(const QString& ogsurl, const int& index) { this->index = index; this->url = ogsurl; }

	int getLayerIndex() { return index; }

	void updateLayerIndex(int newi) { this->index = newi; }

	QString getUrl() { return url; }


};
Q_DECLARE_METATYPE(TreeNodeData)


