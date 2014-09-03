#set, dict, tuple, list

import collections

start_string = 'href="http://cs.usu.edu.ru/home/'
substr = r'/">'
site = "http://cs.usu.edu.ru/home/"
studentsName = []
year = 2012
isStartWas = False
globalStatistic = collections.defaultdict(int)
n = collections.defaultdict(int)



def printAnswer():
    def f(x):
        return x[1]
    for nameLine in studentsName:
        n[nameLine] += 1
        globalStatistic[nameLine] += 1
    print(year + 2)
    print(sorted(n.items(), key=f)[n.__len__() - 1][0])
    print()
    n.clear()

with open('test', encoding='utf8') as f:
    for line in f:
        if str(year) in line:
            year -= 1
            if isStartWas:
                printAnswer()
                studentsName.clear()
            isStartWas = True
        if start_string in line:
            nick_idx = line.find(substr) + len(substr)
            names = line[nick_idx:]
            names = names[:names.find('<')]
            name = names[names.find(' ') + 1:]
            studentsName.append(name)
year -= 1
printAnswer()
print("in all years:")
def f(x):
    return x[1]
print(sorted(globalStatistic.items(), key=f)[n.__len__() - 1][0])
for i in sorted(globalStatistic.items(), key=f):
    print(i[0])