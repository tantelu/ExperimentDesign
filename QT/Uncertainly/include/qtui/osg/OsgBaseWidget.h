#pragma once
#ifndef OSGCONTAINER_H
#define OSGCONTAINER_H

#include <QOpenGLWidget>
#include <osgQOpenGL/osgQOpenGLWidget>
#include <osgViewer/Viewer>
#include "layer/DiscreteLayer.h"
class QInputEvent;

class OsgBaseWidget : public osgQOpenGLWidget{
    Q_OBJECT
public:
    OsgBaseWidget(QWidget* parent = 0);
    ~OsgBaseWidget();

    osg::Group* getRoot() {
        return getOsgViewer()->getSceneData()->asGroup();;
    }
private:
    GslibModel<int>* model;
    DiscreteLayer* layer;

private Q_SLOTS:
    void initOsg();

public Q_SLOTS:
    void addDiscreteLayer(DiscreteLayer* layer);

    void addTestModel();

    void shift();
};
#endif // OSGCONTAINER_H


