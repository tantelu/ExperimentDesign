#include "layer/DiscreteLayer.h"
#include <osg/ShapeDrawable>

DiscreteLayer::DiscreteLayer(const GslibModel<int>& model)
{
	osg::ref_ptr<osg::Switch> _switch = new osg::Switch;
	sw = _switch.get();
	set<int> sortfacie;
	for (size_t i = 0; i < model.size(); i++)
	{
		sortfacie.insert(model.getValue(i));
	}
	vector<int> tempfacie(sortfacie.begin(), sortfacie.end());
	for (size_t fi = 0; fi < tempfacie.size(); fi++)
	{
		osg::ref_ptr<osg::Geometry> geometry = new osg::Geometry;
		osg::Vec3Array* vecArray = new osg::Vec3Array;
		osg::ref_ptr<osg::DrawElementsUInt> drawElemUInt = new osg::DrawElementsUInt(GL_QUADS);
		for (size_t i = 0; i < model.size(); i++)
		{
			//vecArray
		}
		geometry->setVertexArray(vecArray);
		geometry->addPrimitiveSet(drawElemUInt);
		osg::ref_ptr<osg::Geode> geode = new osg::Geode;
		osg::ref_ptr<osg::ShapeDrawable> sd = new osg::ShapeDrawable(geometry);
		sd->setColor(osg::Vec4(0.8f, 0.5f, 0.8f, 1.f));
		geode->addDrawable(sd);
	}

}

void DiscreteLayer::setVisible(bool checked)
{
	if (checked) {
		sw->setAllChildrenOn();
	}
	else {
		sw->setAllChildrenOff();
	}
}

void DiscreteLayer::setVisibleFilter(set<int>& filter)
{
	for (auto it = facies.begin(); it != facies.end(); it++)
	{
		if (filter.find((*it).first) != filter.end()) {
			sw->setChildValue((*it).second, true);
		}
		else {
			sw->setChildValue((*it).second, false);
		}
	}
}

osg::Geode* DiscreteLayer::getGeode(const int& facie)
{
	auto it = facies.find(facie);
	if (it != facies.end()) {
		return (*it).second;
	}
	else {
		return nullptr;
	}
}

osg::Switch* DiscreteLayer::getSwitch()
{
	return sw;
}

vector<int> DiscreteLayer::getFacies()
{
	std::vector<int> r;
	r.reserve(facies.size());
	for (const auto& kvp : facies)
	{
		r.push_back(kvp.first);
	}
	return r;
}
