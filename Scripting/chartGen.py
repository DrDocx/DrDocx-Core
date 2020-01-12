import matplotlib.pyplot as plt; plt.rcdefaults()
import numpy as np
import matplotlib.pyplot as plt
import os
import json


#Test results will contain tuples of the tests results rom the ingest engine
#_TestResultGroup = []

#Contain all of the organized groups
_TestGroups = []
_decodedJson = dict()
_localDir = ""

DEBUG = False
SHOWCHART = False

def main():
	print("Running Visualization Tool")
	ingestDataFile()
	generateChart()
	saveChart()

#Assume the data file is in the same directory with name dataFile.txt
def ingestDataFile():
	print("Intaking Data File")
	global DEBUG
	global _decodedJson
	global _localDir

	_localDir = os.getcwd()
	if DEBUG:
		print("Local Directory is: " + _localDir)

	_dataFilePath = _localDir + "\dataFile.json"
	_dataFile = open(_dataFilePath, "r") #read only access of the data file

	_rawDataJson = _dataFile.read()
	_decodedJson = json.loads(_rawDataJson)
	#print(_decodedJson.pop("ATTENTION").pop("W4-DSFd"))

	_dataFile.close()

def generateChart():
	global DEBUG
	global _decodedJson
	global SHOWCHART

	print("Generating Chart")

	_categories = []
	for _key in _decodedJson.keys():
		_categories.append(_key)

	if DEBUG:
		print("Categories: ")
		print(_categories)

	_xList = []
	_yList = []
	_colors = []

	for _cat in _categories:
		_xList.append(r"$\bf{" + str(_cat.replace(" ", "")) + "}$") #magic, don't touch
		_yList.append(0)
		_colors.append('black')

		_testList = []
		for _key in _decodedJson[_cat].keys():
			_testList.append(_key)

		for index in range(0, len(_testList)):
			_testName = _testList[index]
			_testVal = _decodedJson[_cat][_testList[index]]
			_xList.append(_testName)
			_yList.append(_testVal)
			_colors.append(getColor(_testVal))


		_xList.append("")
		_yList.append(0)
		_colors.append('black')
	
	_xList.pop()
	_yList.pop()
	_colors.pop()

	if DEBUG:
		print(_xList)
		print(_yList)

	_xList.reverse()
	_yList.reverse()
	_colors.reverse()

	_yScal = np.arange(len(_xList))
	plt.barh(_yScal, _yList, color = _colors, align='center', alpha=1, ls = '--', lw = 1)
	plt.yticks(_yScal, _xList)
	plt.xlim([-4, 4])
	plt.grid(b=True, axis = 'x')


	frame1 = plt.gca()
	#frame1.axes.get_xaxis().set_ticks([])

	plt.tight_layout()

	if SHOWCHART:
		plt.show()

def getColor(_val):
	#color = 'green'

	if _val < 0 and _val >= -1:
		color = 'yellow'
	elif _val < -1 and _val >= -2:
		color = 'orange'
	elif _val < -2: 
		color = 'red'
	else: 
		color = 'green'

	return color


def saveChart():
	global _localDir

	print("Saving Graph")

	plt.savefig(_localDir + "\graph.png", tranparent=True)

if __name__ == "__main__":
    main()