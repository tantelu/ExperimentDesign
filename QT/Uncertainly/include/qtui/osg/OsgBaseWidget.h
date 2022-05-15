#pragma once
#ifndef OSGCONTAINER_H
#define OSGCONTAINER_H

#include <QOpenGLWidget>
#include <osgQOpenGL/osgQOpenGLWidget>
#include <osgViewer/Viewer>

class QInputEvent;

class OsgBaseWidget : public osgQOpenGLWidget{
    Q_OBJECT
public:
    OsgBaseWidget(QWidget* parent = 0);
    ~OsgBaseWidget();
    
    void addCylinder();

    osg::Group* getRoot() {
        return getOsgViewer()->getSceneData()->asGroup();;
    }

private Q_SLOTS:
    void initOsg();
};
#endif // OSGCONTAINER_H


