#include "qtui/Uncertainly.h"
#include <QtWidgets/QApplication>

int main(int argc, char *argv[])
{
    QApplication a(argc, argv);
    Uncertainly w;
    w.show();
    return a.exec();
}
