import sys
import copy

def main():
    Map = []
    empty = []
    block = []
    f = open("map.txt","r")
    n = int(f.readline())
    for i in range(n):
        line = f.readline()
        Map.append(list(map(int, line.split())))
    Lens = []
    for i in range(n):
        Lens.append([])
        for j in range(len(Map[i])):
            Lens[i].append([n * n + 1, (-1, -1)])
    startCoord = f.readline()
    start = tuple(map(int, startCoord.split()))
    endCoord = f.readline()
    end = tuple(map(int, endCoord.split()))
    bombs = f.readline()
    bombs = int(bombs)
    empty.append(start)
    Lens[start[0]][start[1]] = [0,(start[0], start[1])]
    FindPath(Map, Lens, empty, block)
    PrintAnswer(bombs, Lens, Map, end, start)
    
def FindPath(Map, Lens, empty, block):
    n = len(Map)
    while (len(empty) > 0 or len(block) > 0):
        if len(empty) > 0:
            current = empty.pop(len(empty) - 1)
            for i in [current[0] - 1, current[0], current[0] + 1]:
                for j in [current[1] - 1, current[1], current[1] + 1]:
                    if i < n and j < len(Map[0]) and (i == current[0] or j == current[1]) and i >= 0 and j >= 0:
                        if Lens[i][j][0] > Lens[current[0]][current[1]][0] + Map[i][j]:
                            if Map[i][j] == 1:
                                block.append((i, j))
                            else:
                                empty.append((i, j))
                            Lens[i][j][0] = Lens[current[0]][current[1]][0] + Map[i][j]
                            Lens[i][j][1] = current
        else:
            current = block.pop(0)
            for i in [current[0] - 1, current[0], current[0] + 1]:
                for j in [current[1] - 1, current[1], current[1] + 1]:
                    if i < n and j < len(Map[0]) and (i == current[0] or j == current[1]) and i >= 0 and j >= 0:
                        if Lens[i][j][0] > Lens[current[0]][current[1]][0] + Map[i][j]:
                            if Map[i][j] == 1:
                                block.append((i, j))
                            else:
                                empty.append((i, j))
                            Lens[i][j][0] = Lens[current[0]][current[1]][0] + Map[i][j]
                            Lens[i][j][1] = current

def PrintAnswer(bombs, Lens, Map, end, start):
    n = len(Map)
    if Lens[end[0]][end[1]][0] > bombs:
        print('Impossible')
    else:
        answer = []
        for i in range(n):
            answer.append([])
            for j in range(len(Map[i])):
                if Map[i][j] == 0:
                    answer[i].append('0')
                else:
                    answer[i].append('1')
        curr = Lens[end[0]][end[1]][1]
        while True:
            if answer[curr[0]][curr[1]] == '1':
                answer[curr[0]][curr[1]] = 'X'
            else:
                answer[curr[0]][curr[1]] = '^'
            if curr == start:
                break
            else:
                curr = Lens[curr[0]][curr[1]][1]
        if answer[end[0]][end[1]] == '%':
            answer[end[0]][end[1]] = 'X'
        else:
            answer[end[0]][end[1]] = '^'
        for i in range(n):
            for j in range(len(answer[i])):
                sys.stdout.write(str(answer[i][j]))
            print()
            
main()