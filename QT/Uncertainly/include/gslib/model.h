#pragma once
#include <string>
#include "gslib/gslibfile.h"

using namespace std;

template<class T>
class __declspec(dllexport) GslibModel {
private:
	shared_ptr<vector<T>> data;
	int icount;
	int jcount;
	int kcount;
public:
	GslibModel(const string& file, size_t iCount, size_t jCount, size_t kCount);
	GslibModel(const string& url);

	T getValue(size_t index) const { return data.get()->at(index); }

	vector<T>* getValues() const { return data.get(); }

	T getValue(size_t i, size_t j, size_t k) const { 
		return data.get()->at(i + j * icount + k * icount * jcount);
	}

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
		data = make_shared<vector<T>>();
		icount = iCount;
		jcount = jCount;
		kcount = kCount;
		data.get()->reserve(size());
		if (isint) {
			
			auto pt = gslibfile::readfilei(file);
			auto res = pt.get();
			if (res->size() >= size()) {
				for (size_t i = 0; i < size(); i++)
				{
					data.get()->push_back((T)res->at(i));
				}
			}
		}
		else {
			auto pt = gslibfile::readfiled(file);
			auto res = pt.get();
			if (res->size() >= size()) {
				for (size_t i = 0; i < size(); i++)
				{
					data.get()->push_back((T)res->at(i));
				}
			}
		}
		
	}
}

template<class T>
inline GslibModel<T>::GslibModel(const string& url)
{
	bool isint = std::is_same<typename std::decay<T>::type, int>::value;
	data = make_shared<vector<T>>();
	gslibfile::readijk(url, icount, jcount, kcount);
	data.get()->reserve(size());
	if (isint) {
		auto pt = gslibfile::readfilei(url);
		auto res = pt.get();
		if (res->size() >= size()) {
			for (size_t i = 0; i < size(); i++)
			{
				data.get()->push_back((T)res->at(i));
			}
		}
		else {
			throw exception("");
		}
	}
	else {
		auto pt = gslibfile::readfiled(url);
		auto res = pt.get();
		if (res->size() >= size()) {
			for (size_t i = 0; i < size(); i++)
			{
				data.get()->push_back((T)res->at(i));
			}
		}
		else {
			throw exception("");
		}
	}
}

