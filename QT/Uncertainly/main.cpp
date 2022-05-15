#include "qtui/MainWindow.h"
#include "qtui/osg/ModelShow.h"
#include <QtWidgets/QApplication>

int main(int argc, char *argv[])
{
    QApplication a(argc, argv);
    MainWindow w;
    w.show();
    return a.exec();
    /*QApplication a(argc, argv);
    ModelShow window;
    window.setGeometry(0, 0, 1000, 800);
    window.show();
    return a.exec();*/
}
