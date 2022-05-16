#include "gslib/gslibfile.h"
#include <iostream>
#include <fstream>
#include <boost/algorithm/string/classification.hpp>
#include <boost/algorithm/string/split.hpp>

unique_ptr<vector<int>> gslibfile::readfilei(const std::string& file)
{
	vector<int>* data = new vector<int>();
	ifstream infile;
	infile.open(file, ios::in);
	if (infile.is_open())
	{
		string buf;
		getline(infile, buf);
		getline(infile, buf);
		getline(infile, buf);
		while (getline(infile, buf))
		{
			data->push_back(std::stoi(buf));
		}
	}
	return unique_ptr<vector<int>>(data);
}

unique_ptr<vector<double>> gslibfile::readfiled(const std::string& file)
{
	vector<double>* data = new vector<double>();
	ifstream infile;
	infile.open(file, ios::in);
	if (infile.is_open())
	{
		string buf;
		getline(infile, buf);
		getline(infile, buf);
		getline(infile, buf);
		while (getline(infile, buf))
		{
			data->push_back(std::stod(buf));
		}
	}
	return unique_ptr<vector<double>>(data);
}
