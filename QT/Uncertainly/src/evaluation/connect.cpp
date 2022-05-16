#include "evaluation/connect.h"
#include "stack"
#include "tuple"

unique_ptr<ConnectVolumn>  Connect::connectBodyVolumn(GslibModel<int>& gslibModel, vector<int>& facies, ConnectType connectType)
{
	ConnectVolumn* volumn = new ConnectVolumn();
	set<int> hasConnected;
	for (int i = 0; i < gslibModel.getIcount(); i++)
	{
		for (int j = 0; j < gslibModel.getJcount(); j++)
		{
			for (int k = 0; k < gslibModel.getKcount(); k++)
			{
				int curindex = canContinueSearch(i, j, k, gslibModel, facies, hasConnected);
				if (curindex >= 0)
				{
					set<int> tempConnected;
					std::stack<std::tuple<int, int, int>> neighbors;
					neighbors.push(std::tuple<int, int, int>(i, j, k));
					tempConnected.insert(curindex);
					while (!neighbors.empty())
					{
						std::tuple<int, int, int> curPixel = neighbors.top();
						int curX = std::get<0>(curPixel);
						int curY = std::get<1>(curPixel);
						int curZ = std::get<2>(curPixel);
						neighbors.pop();
						int left = curX - 1;
						int leftindex = canContinueSearch(left, curY, curZ, gslibModel, facies, tempConnected);
						if (leftindex >= 0) {
							neighbors.push(std::tuple<int, int, int>(left, curY, curZ));
							tempConnected.insert(leftindex);
						}
						int right = curX + 1;
						int rightindex = canContinueSearch(right, curY, curZ, gslibModel, facies, tempConnected);
						if (rightindex >= 0) {
							neighbors.push(std::tuple<int, int, int>(right, curY, curZ));
							tempConnected.insert(rightindex);
						}
						int front = curY - 1;
						int frontindex = canContinueSearch(curX, front, curZ, gslibModel, facies, tempConnected);
						if (frontindex >= 0) {
							neighbors.push(std::tuple<int, int, int>(curX, front, curZ));
							tempConnected.insert(frontindex);
						}
						int back = curY + 1;
						int backindex = canContinueSearch(curX, back, curZ, gslibModel, facies, tempConnected);
						if (backindex >= 0) {
							neighbors.push(std::tuple<int, int, int>(curX, back, curZ));
							tempConnected.insert(backindex);
						}
						int top = curZ + 1;
						int topindex = canContinueSearch(curX, curY, top, gslibModel, facies, hasConnected);
						if (topindex >= 0) {
							neighbors.push(std::tuple<int, int, int>(curX, back, top));
							tempConnected.insert(topindex);
							
						}
						int bot = curZ - 1;
						int botindex = canContinueSearch(curX, curY, bot, gslibModel, facies, hasConnected);
						if (botindex >= 0) {
							neighbors.push(std::tuple<int, int, int>(curX, back, bot));
							tempConnected.insert(botindex);
						}
					}
					hasConnected.insert(tempConnected.begin(), tempConnected.end());
					volumn->pushConnectVolumn(tempConnected);
				}
			}
		}
	}
	return unique_ptr<ConnectVolumn>(volumn);
}

int Connect::canContinueSearch(int& curi, int& curj, int& curk, GslibModel<int>& gslibModel, vector<int>& facies, set<int>& hasConnected)
{
	if (curk >= 0 && curk < gslibModel.getKcount() && curi >= 0 && curi < gslibModel.getIcount() && curj >= 0 && curj < gslibModel.getJcount())
	{
		auto curindex = curi + curj * gslibModel.getIcount() + curk * gslibModel.getIcount() * gslibModel.getJcount();
		int curfacie = gslibModel.getValue(curindex);
		if (std::find(facies.begin(), facies.end(), curfacie) != facies.end() && hasConnected.find(curindex) == hasConnected.end()) {
			return curindex;
		}
	}
	return -1;
}

void Connect::surfaceDfs(int& curi, int& curj, int& curk, GslibModel<int>& gslibModel, vector<int>& facies, set<int>& hasConnected)
{
	int left = curi - 1;
	int leftindex = canContinueSearch(left, curj, curk, gslibModel, facies, hasConnected);
	if (leftindex >= 0) {
		hasConnected.insert(leftindex);
		surfaceDfs(left, curj, curk, gslibModel, facies, hasConnected);
	}
	int right = curi + 1;
	int rightindex = canContinueSearch(right, curj, curk, gslibModel, facies, hasConnected);
	if (rightindex >= 0) {
		hasConnected.insert(rightindex);
		surfaceDfs(right, curj, curk, gslibModel, facies, hasConnected);
	}
	int top = curk + 1;
	int topindex = canContinueSearch(curi, curj, top, gslibModel, facies, hasConnected);
	if (topindex >= 0) {
		hasConnected.insert(topindex);
		surfaceDfs(curi, curj, top, gslibModel, facies, hasConnected);
	}
	int bot = curk - 1;
	int botindex = canContinueSearch(curi, curj, bot, gslibModel, facies, hasConnected);
	if (botindex >= 0) {
		hasConnected.insert(botindex);
		surfaceDfs(curi, curj, bot, gslibModel, facies, hasConnected);
	}

	int front = curj - 1;
	int frontindex = canContinueSearch(curi, front, curk, gslibModel, facies, hasConnected);
	if (frontindex >= 0) {
		hasConnected.insert(frontindex);
		surfaceDfs(curi, front, curk, gslibModel, facies, hasConnected);
	}

	int back = curj + 1;
	int backindex = canContinueSearch(curi, back, curk, gslibModel, facies, hasConnected);
	if (backindex >= 0) {
		hasConnected.insert(backindex);
		surfaceDfs(curi, back, curk, gslibModel, facies, hasConnected);
	}
}

ConnectVolumn::ConnectVolumn()
{
	volumn = new vector<vector<int>>();
}

ConnectVolumn::~ConnectVolumn()
{
	if (volumn != nullptr) {
		delete volumn;
		volumn = nullptr;
	}
}

void ConnectVolumn::pushConnectVolumn(set<int>& indexs)
{
	volumn->emplace_back(indexs.begin(), indexs.end());
}

unique_ptr<vector<int>> ConnectVolumn::getAllIndexs()
{
	size_t count = 0;
	for (auto it = volumn->begin(); it != volumn->end(); it++)
	{
		count += it->size();
	}
	vector<int>* vec = new vector<int>();
	vec->reserve(count);
	for (auto it = volumn->begin(); it != volumn->end(); it++)
	{
		vec->insert(vec->begin(), it->begin(), it->end());
	}
	return unique_ptr<vector<int>>(vec);
}

const vector<vector<int>>* ConnectVolumn::getVolumnList()
{
	return volumn;
}

size_t ConnectVolumn::size()
{
	return volumn->size();
}

bool ConnectVolumn::compare(const vector<int>& v1, const vector<int>& v2)
{
	return v1.size() < v2.size();
}