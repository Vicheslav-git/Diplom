#include <iostream>
#include <vector>
#include <algorithm>

int main()
{
    std::cout << "Test algorithm\n";
	std::vector<double> a(20);
	double init[21] = { 0,1,2,3,4,5,6,7,8,9,8,7,2,3,4,5,7,7,8,8,9 };
	memcpy_s(&a[0], a.size()*sizeof(a[0]), init, 20 * sizeof(double));


	size_t init_ind = 0;
	std::vector<double> y;
	for (int i = 0; i < 7; i++)
	{
		y.resize(3);
		for (size_t j = 0; j < y.size(); j++)
		{
size_t RandomIndex = (size_t) ((double)rand() / (double)RAND_MAX * (double)a.size());
			init[init_ind++] = y[j] = a[RandomIndex];
		}
		auto ptrMaxRandomValue = std::max_element(y.begin(), y.end());
		std::cout << *ptrMaxRandomValue;
	}

	std::vector<int> b(20);
	for (int i = 0; i < 20; i++)
	{
		size_t RandomIndex = (size_t)((double)rand() / (double)RAND_MAX * 21.0);
		b[i] = (int)init[RandomIndex];
	}
	auto ptrMinRandomValue = std::min_element(b.begin(), b.end());
	std::cout << *ptrMinRandomValue;

	return 0;
}
