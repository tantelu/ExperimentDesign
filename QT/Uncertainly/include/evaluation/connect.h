#pragma once
#include <vector>
#include <string>
#include <set>
#include <memory>
#include "gslib/model.h"

using namespace std;

class __declspec(dllexport) ConnectVolumn {
private:
	bool compare(const vector<int>& v1, const vector<int>& v2);

	vector<vector<int>>* volumn;

public:
	ConnectVolumn();
	ConnectVolumn(ConnectVolumn& other) = delete;
	~ConnectVolumn();
	void pushConnectVolumn(set<int>& indexs);

	unique_ptr<vector<int>> getAllIndexs();

	const vector<vector<int>>* getVolumnList();

	unique_ptr<vector<vector<int>>> getVolumnListOrder();
	unique_ptr<vector<vector<int>>> getVolumnListDesOrder();
	size_t size();
};

class __declspec(dllexport) Connect
{
public:
	enum class ConnectType
	{
		/// <summary>
		/// ���ڽӣ�6�ڽӣ�
		/// </summary>
		Surface,
		/// <summary>
		/// �桢���ڽӣ�18�ڽӣ�
		/// </summary>
		SurfaceEdge,
		/// <summary>
		/// �桢�ߡ��Խ��ڽӣ�26�ڽӣ�
		/// </summary>
		SurfaceEdgeDiagonal,
	};

	static	unique_ptr<ConnectVolumn> connectBodyVolumn(GslibModel<int>& gslibModel, vector<int>& facies, ConnectType connectType);

	static unique_ptr<ConnectVolumn> connectWellVolumn(GslibModel<int>& gslibModel, vector<size_t>& wellPos, vector<int>& wellFacies, ConnectType connectType);

private:
	static int canContinueSearch(int& curi, int& curj, int& curk, GslibModel<int>& gslibModel, vector<int>& facies, set<int>& hasConnected);
	static void surfaceDfs(int& curi, int& curj, int& curk, GslibModel<int>& gslibModel, vector<int>& facies, set<int>& hasConnected);
	static void surfaceEdgeDfs(GslibModel<int>& gslibModel, vector<int>& facies, int& curindex, set<int>& hasConnected);
	static void surfaceEdgeDiagonalDfs(GslibModel<int>& gslibModel, vector<int>& facies, int& curindex, set<int>& hasConnected);
};

