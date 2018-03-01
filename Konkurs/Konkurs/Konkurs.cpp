#ifdef _MSC_VER
#include "stdafx.h"
#endif

#include <vector>
#include <iostream>
#include <fstream>
#include <string>
#include <stdexcept>
#include <algorithm>
#include <utility>
#include <tuple>
#include <map>
#include <set>
#include <unordered_map>

std::string inputDirectory = R"(D:\maciek\hashcode\Konkurs\Input\)";
std::string outputDirectory = R"(D:\maciek\hashcode\Konkurs\Output\)";

struct InputStructure
{
};

struct OutputStructure
{
};

InputStructure Parse(const std::string& fileName)
{
	std::cout << "parsing file started: " << fileName << std::endl;

	InputStructure is;
	std::ifstream file(fileName);

	if (!file.is_open() || !file.good())
		throw std::invalid_argument("cannot read file " + fileName);

	////////// DO THE LOGIC

	std::cout << "parsing file finished: " << fileName << std::endl;
	return is;
}

void WriteResultToFile(const OutputStructure& outputStructure, const std::string& fileName)
{
	std::cout << "writing result file started: " << fileName << std::endl;
	std::ofstream file(fileName);

	if (!file.is_open() || !file.good())
		std::invalid_argument("cannot write to file: " + fileName);

	////////// DO THE LOGIC

	std::cout << "writing result file started: " << fileName << std::endl;
}

OutputStructure Solve(const InputStructure& inputStructure)
{
	////////// DO THE LOGIC

	return OutputStructure();
}

void ProcessFile(std::string baseFileName)
{
	std::cout << "processing file started: " << baseFileName << std::endl;

	auto input = Parse(inputDirectory + baseFileName + ".in");
	auto output = Solve(input);
	WriteResultToFile(output, outputDirectory + baseFileName + ".out");

	std::cout << "processing file finished: " << baseFileName << std::endl;
}

int main()
{
	ProcessFile("aaa");
	ProcessFile("bbb");
	ProcessFile("ccc");
	ProcessFile("ddd");

	std::cout << "!!!wrzuc cos na cin zeby zakonczyc!!!" << std::endl;
	int i; std::cin >> i;
	return 0;
}

