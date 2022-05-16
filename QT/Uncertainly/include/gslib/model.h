#pragma once
#include <string>
#include "gslib/gslibfile.h"

using namespace std;

template<class T>
class __declspec(dllexport) GslibModel {
private:
	vector<T>* data;
	size_t icount;
	size_t jcount;
	size_t kcount;
public:
	GslibModel(const string& file, size_t iCount, size_t jCount, size_t kCount);
	//GslibModel(vector<T>& file, size_t iCount, size_t jCount, size_t kCount);
	GslibModel(GslibModel& other) = delete;
	~GslibModel();

	T getValue(size_t index) const { return data->at(index); }

	size_t getIcount() const { return icount; }
	size_t getJcount() const { return jcount; }
	size_t getKcount() const { return kcount; }
	size_t size() const {
		return icount * jcount * kcount;
	}
};

template<class T>
inline GslibModel<T>::GslibModel(const std::string& file, size_t iCount, size_t jCount, size_t kCount)
{
	if (iCount > 0 && jCount > 0 && kCount > 0) {
		bool isint = std::is_same<typename std::decay<T>::type, int>::value;
		data = new vector<T>();
		icount = iCount;
		jcount = jCount;
		kcount = kCount;
		data->reserve(size());
		if (isint) {
			auto pt = gslibfile::readfilei(file);
			auto res = pt.get();
			if (res->size() >= size()) {
				for (size_t i = 0; i < size(); i++)
				{
					data->push_back((T)res->at(i));
				}
			}
		}
		else {
			auto pt = gslibfile::readfiled(file);
			auto res = pt.get();
			if (res->size() >= size()) {
				for (size_t i = 0; i < size(); i++)
				{
					data->push_back((T)res->at(i));
				}
			}
		}
	}
}

template<class T>
inline GslibModel<T>::~GslibModel()
{
	if (data != nullptr) {
		delete data;
		data = nullptr;
	}
}

