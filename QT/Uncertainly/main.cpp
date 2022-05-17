#include "qtui/MainWindow.h"
#include <QtWidgets/QApplication>

int main(int argc, char* argv[])
{
	/*GslibModel<int> model("C:\\Users\\24249\\Desktop\\face.gslib", 204, 366, 40);
	auto values = model.getValues();
	size_t N = 0;
	auto res = cc3d::connected_components3d<int>(values, 204, 366, 40,26, N);
	for (size_t i = 0; i < model.size(); i++)
	{
		auto label = *(res + i);
	}*/
	QApplication a(argc, argv);
	MainWindow w;
	w.show();
	return a.exec();
}
