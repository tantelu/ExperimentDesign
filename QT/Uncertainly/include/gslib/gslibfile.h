#pragma once
#include <memory>
#include <vector>
#include <string>
using namespace std;
class gslibfile
{
public:
	static unique_ptr<vector<int>> readfilei(const std::string& file);

	static unique_ptr<vector<double>> readfiled(const std::string& file);
};

