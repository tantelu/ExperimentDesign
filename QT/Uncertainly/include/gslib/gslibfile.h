#pragma once
#include <memory>
#include <vector>
#include <string>
using namespace std;
class gslibfile
{
public:
	static unique_ptr<vector<int>> readfilei(const std::string& file);

	static void gslibfile::readijk(const std::string& url, int& icount, int& jcount, int& kcount);

	static unique_ptr<vector<double>> readfiled(const std::string& file);
};

