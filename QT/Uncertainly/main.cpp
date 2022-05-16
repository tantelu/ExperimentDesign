#include "qtui/MainWindow.h"
#include "evaluation/connect.h"
#include "qtui/osg/ModelShow.h"
#include <QtWidgets/QApplication>

int main(int argc, char* argv[])
{
	/*vector<int> facie = { 1 };
	auto pt = Connect::connectBodyVolumn(model, facie, Connect::ConnectType::SurfaceEdge);
	auto volumn = pt.get();
	auto siz = volumn->size();*/
	QApplication a(argc, argv);
	MainWindow w;
	w.show();
	return a.exec();
}
