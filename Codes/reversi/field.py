class Field(object):
    def __init__(self):
        self.is_has_move = True
        self.step = 2
        self.field = []
        for i in range(8):
            self.field.append([])
            for j in range(8):
                self.field[i].append(0)
        self.field[3][3] = 1
        self.field[4][4] = 1
        self.field[3][4] = 2
        self.field[4][3] = 2

    def get_result(self):
        black = 0
        white = 0
        for i in range(8):
            for j in range(8):
                if self.field[i][j] == 1:
                    white += 1
                if self.field[i][j] == 2:
                    black += 1
        return black, white

    def put(self, y, x, color):
        if x == -1 and y == -1:
            self.is_has_move = False
            return
        self.checker(y, x, "swap", color)
        self.step = self.change_color(self.step)
        self.check_next_moves(self.change_color(color))

    def check_next_moves(self, color):
        for i in range(8):
            for j in range(8):
                if self.is_correct(i, j, color):
                    self.is_has_move = True
                    return
        self.is_has_move = False

    def change_color(self, color):
        if color == 1:
            color = 2
        else:
            color = 1
        return color

    def is_correct(self, y, x, color):
        return self.checker(y, x, None, color)

    def checker(self, y, x, func, color):
        if self.field[y][x] != 0:
            return False
        a = [self.check_line(self.up_subs(y, x), y, x, func, color),
             self.check_line(self.down_subs(y, x), y, x, func, color),
             self.check_line(self.left_subs(y, x), y, x, func, color),
             self.check_line(self.right_subs(y, x), y, x, func, color),
             self.check_line(self.ur_subs(y, x), y, x, func, color),
             self.check_line(self.ul_subs(y, x), y, x, func, color),
             self.check_line(self.dr_subs(y, x), y, x, func, color),
             self.check_line(self.dl_subs(y, x), y, x, func, color)]
        if func == "help":
            return a
        for i in range(8):
            if a[i]:
                return True
        return False

    def check_line(self, subset, y, x, func, color):
        a = 0
        is_end = False
        for i in range(len(subset)):
            if self.field[subset[i][0]][subset[i][1]] != color and \
                            self.field[subset[i][0]][subset[i][1]] != 0:
                a += 1
            else:
                if self.field[subset[i][0]][subset[i][1]] != 0 and a > 0:
                    is_end = True
                    break
                else:
                    break
        if func == "help":
            if is_end:
                b = []
                for i in range(a):
                    b.append(subset[i])
                return b
            else:
                return []
        if func == "swap" and is_end:
            self.field[y][x] = color
            for i in range(a):
                self.field[subset[i][0]][subset[i][1]] = color
        return is_end

    def up_subs(self, y, x):
        a = []
        for i in range(y):
            a.append((y - i - 1, x))
        return a

    def down_subs(self, y, x):
        a = []
        for i in range(7 - y):
            a.append((y + i + 1, x))
        return a

    def left_subs(self, y, x):
        a = []
        for i in range(x):
            a.append((y, x - i - 1))
        return a

    def right_subs(self, y, x):
        a = []
        for i in range(7 - x):
            a.append((y, x + i + 1))
        return a

    def ur_subs(self, y, x):
        a = []
        for i in range(min(7 - x, y)):
            a.append((y - i - 1, x + i + 1))
        return a

    def dl_subs(self, y, x):
        a = []
        for i in range(min(7 - y, x)):
            a.append((y + i + 1, x - i - 1))
        return a

    def ul_subs(self, y, x):
        a = []
        for i in range(min(y, x)):
            a.append((y - i - 1, x - i - 1))
        return a

    def dr_subs(self, y, x):
        a = []
        for i in range(min(7 - y, 7 - x)):
            a.append((y + i + 1, x + i + 1))
        return a

    def self_copy(self):
        a = Field()
        for i in range(8):
            for j in range(8):
                a.field[i][j] = self.field[i][j]
        return a

    def bot_step(self, color, difficult):
        max_val = -1000
        Y = -1
        X = -1
        if difficult == 1:
            for i in range(8):
                for j in range(8):
                    local_field = self.self_copy()
                    if local_field.is_correct(i, j, color):
                        local_field.put(i, j, color)
                        val = local_field.get_field_hash(color)
                        if val > max_val:
                            max_val = val
                            Y = i
                            X = j
        else:
            Y, X, val = self.good_bot_step(color, 3, self)
        self.put(Y, X, color)

    def good_bot_step(self, color, nesting, field):
        max_val = -1000
        Y = -1
        X = -1
        for i in range(8):
            for j in range(8):
                local_field = field.self_copy()
                if local_field.is_correct(i, j, color):
                    local_field.put(i, j, color)
                    if nesting == 1 or nesting == 2:
                        val = local_field.get_field_hash(color)
                    else:
                        val = self.good_bot_step(self.change_color(color), nesting - 1, local_field)[2]
                    if val > max_val:
                        max_val = val
                        Y = i
                        X = j
        if nesting == 2:
            local_field = field.self_copy()
            local_field.put(Y, X, color)
            return self.good_bot_step(self.change_color(color), nesting - 1, local_field)
        return Y, X, max_val

    def get_help(self, y, x, color):
        if y < 0 or x < 0:
            return []
        else:
            b = []
            a = self.checker(y, x, "help", color)
            if a is False or a is True:
                return b
            for i in range(8):
                for j in range(len(a[i])):
                    b.append(a[i][j])
            return b

    def get_field_hash(self, color):
        val = 0
        for i in range(8):
            for j in range(8):
                if self.field[i][j] != 0:
                    if self.field[i][j] == color:
                        val += 1
                    else:
                        val -= 1
        return val