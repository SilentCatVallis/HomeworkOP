from tkinter import *
import sys, os
row_count = 20
column_count = 20


class World():
    def initialise_map(self, start_file):
        try:
            row = 0
            with open(start_file, "r") as lines:
                lines = lines.read()
                for line in lines.split(('\n')):
                    new_line = []
                    for i in range(column_count):
                        new_line.append(' ')
                    col = 0
                    for word in line:
                        new_line[col] = word
                        col += 1
                    self.map[row] = new_line
                    row += 1
            for line in range(len(self.map)):
                for column in range(len(self.map[line])):
                    if self.map[line][column] == '*':
                        self.map[line][column] = True
                    else:
                        self.map[line][column] = False
        except:
            print("Invalid map, map size must be 20x20")
            exit(1)

    def __init__(self, start_file):
        self.map = []
        for i in range(row_count):
            self.map.append([])
            self.map[i] = []
            for j in range(column_count):
                self.map[len(self.map) - 1].append(False)
        self.reserved_map = []
        for i in range(row_count):
            self.reserved_map.append([])
            self.reserved_map[i] = []
            for j in range(column_count):
                self.reserved_map[len(self.reserved_map) - 1].append(False)
        self.initialise_map(start_file)

    def alive_neighbours_count(self, line, column):
        count = 0
        for i in range(3):
            for j in range(3):
                if i != 1 or j != 1:
                    line_number = (line + i - 1 + row_count) % row_count
                    column_number = (column + j - 1 + column_count) % column_count
                    if self.map[line_number][column_number]:
                        count += 1
        return count

    def get_next_state(self):
        for line in range(len(self.map)):
            for column in range(len(self.map[line])):
                neighbour_count = self.alive_neighbours_count(line, column)
                if self.map[line][column]:
                    if neighbour_count == 2 or neighbour_count == 3:
                        self.reserved_map[line][column] = True
                    else:
                        self.reserved_map[line][column] = False
                else:
                    if neighbour_count == 3:
                        self.reserved_map[line][column] = True
                    else:
                        self.reserved_map[line][column] = False
        for i in range(len(self.map)):
            for j in range(len(self.map)):
                self.map[i][j] = self.reserved_map[i][j]


def print_usage():
    print("use: life.py <name>")
    print('where "name" - filename of start position')
    print('use "life.py /?" for help')
    exit()


def print_help():
    print("use: life.py <name>")
    print('where "name" - filename of start position')
    print("field have size 20x20")
    print('"*" - alife cell')
    print("any another char - death cell")


def print_miss_file(file):
    print(file + " does not exist")


def start_game(file):
    world = World(file)
    root = Tk()
    root.title("Life")
    frame = Frame(root)
    frame.pack()

    def next_step():
        for i in range(row_count):
            for j in range(column_count):
                if world.map[i][j]:
                    Label(frame_for_field, text=" ").place(x=j * 25, y=i * 25, width=25, heigh=25)
        world.get_next_state()
        for i in range(row_count):
            for j in range(column_count):
                if world.map[i][j]:
                    Label(frame_for_field, text="☺").place(x=j * 25, y=i * 25, width=25, heigh=25)

    frame_for_field = Frame(root, width=500, height=500)
    frame_for_field.pack()
    generation_button = Button(frame, text="Next generation", width=20, command=next_step)
    generation_button.grid(row=1, column=1)
    for i in range(row_count):
        for j in range(column_count):
            if world.map[i][j]:
                Label(frame_for_field, text="☺").place(x=j * 25, y=i * 25, width=25, heigh=25)
            else:
                Label(frame_for_field, text=" ").place(x=j * 25, y=i * 25, width=25, heigh=25)
    root.mainloop()


def __main__():
    if len(sys.argv) != 2:
        print_usage()
    if sys.argv[1] == "--help":
        print_help()
    if not os.path.exists(sys.argv[1]):
        print_miss_file(sys.argv[1])
    file = sys.argv[1]
    start_game(file)

if __name__ == "__main__":
    __main__()





