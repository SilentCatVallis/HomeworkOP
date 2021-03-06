from tkinter import *
import field
import server
import client
import socket
import threading
import time
import os


class Buttons():
    def __init__(self, window):
        self.root = window.root

        self.player_select = IntVar()
        self.difficult_select = IntVar()
        self.difficult_select.set(1)
        self.dif0 = Radiobutton(self.root, text="easy", variable=self.difficult_select, value=1)
        self.dif1 = Radiobutton(self.root, text="hard", variable=self.difficult_select, value=2)
        self.player_select.set(2)
        self.rad0 = Radiobutton(self.root, text="White", variable=self.player_select, value=1)
        self.rad1 = Radiobutton(self.root, text="Black", variable=self.player_select, value=2)

        self.start_game_button = Button(self.root, text="New game", width=20, command=window.create_game)
        self.load_game_button = Button(self.root, text="Load game", width=20, command=window.load_game)
        self.server_button = Button(self.root, text="Create server", width=20, command=window.create_server)
        self.client_button = Button(self.root, text="Connect", width=20, command=window.create_client)
        self.label_ip = Label(self.root, text=" IP:  " + window.ip)
        self.text_ip = Entry(self.root)
        self.help_button = Button(self.root, text="?", width=5, command=window.open_help)
        print("lol")

    def grid_window_buttons(self):
        self.start_game_button.grid(row=0, column=0)
        self.rad0.grid(row=0, column=1)
        self.rad1.grid(row=1, column=1)
        self.dif0.grid(row=0, column=2)
        self.dif1.grid(row=1, column=2)
        self.load_game_button.grid(row=0, column=3)
        self.server_button.grid(row=3, column=0)
        self.label_ip.grid(row=3, column=1)
        self.client_button.grid(row=4, column=0)
        self.text_ip.grid(row=4, column=1)
        self.help_button.grid(row=0, column=4)

    def destroy_main_menu(self):
        self.start_game_button.destroy()
        self.rad0.destroy()
        self.rad1.destroy()
        self.load_game_button.destroy()
        self.text_ip.destroy()
        self.label_ip.destroy()
        self.client_button.destroy()
        self.server_button.destroy()
        self.dif0.destroy()
        self.dif1.destroy()


class Booleans():
    def __init__(self):
        self.is_need_menu = False
        self.is_game_come = False
        self.is_was_load = False
        self.is_server = False
        self.is_client = False
        self.is_online = False

class Window():
    #def create_bools(self):
    #    self.is_need_menu = False
    #    self.is_game_come = False
    #    self.is_was_load = False
    #    self.is_server = False
    #    self.is_client = False
    #    self.is_online = False

    def found_user_ip(self):
        try:
            self.ip = [(s.connect(('8.8.8.8', 80)), s.getsockname()[0], s.close()) for s in
                   [socket.socket(socket.AF_INET, socket.SOCK_DGRAM)]][0][1]
        except:
            self.ip = "connect to internet"

    def __init__(self):
        self.found_user_ip()
        #self.create_bools()
        self.Booleans = Booleans()
        self.X = -1
        self.Y = -1
        self.step = 0
        self.root = Tk()
        self.Buttons = Buttons(self)
        self.create_window()
        self.Buttons.grid_window_buttons()
        self.file_path = os.path.abspath(os.path.dirname(sys.argv[0]))

    def create_window(self):
        self.root.title("Reverse")
        self.root.minsize(width=700, height=500)
        self.root.maxsize(width=700, height=500)

    def start(self):
        self.root.mainloop()

    def create_server(self):
        self.server = server.Server(self.ip)
        if self.server.find_connect():
            self.Booleans.is_server = True
            self.Booleans.is_online = True
            self.server.send_color(self.Buttons.player_select.get())
            self.create_game()

    def create_client(self):
        self.client = client.Client(self.Buttons.text_ip.get())
        color = self.client.check_color()
        if color != 0:
            self.Booleans.is_client = True
            self.Booleans.is_online = True
            if color == 1:
                self.Buttons.player_select.set(2)
            else:
                self.Buttons.player_select.set(1)
            self.create_game()

    #def grid_window_buttons(self):
    #    self.start_game_button.grid(row=0, column=0)
    #    self.rad0.grid(row=0, column=1)
    #    self.rad1.grid(row=1, column=1)
    #    self.dif0.grid(row=0, column=2)
    #    self.dif1.grid(row=1, column=2)
    #    self.load_game_button.grid(row=0, column=3)
    #    self.server_button.grid(row=3, column=0)
    #    self.label_ip.grid(row=3, column=1)
    #    self.client_button.grid(row=4, column=0)
    #    self.text_ip.grid(row=4, column=1)
    #    self.help_button.grid(row=0, column=4)

    #def create_radiobuttons(self):
    #    self.player_select = IntVar()
    #    self.difficult_select = IntVar()
    #    self.difficult_select.set(1)
    #    self.dif0 = Radiobutton(self.root, text="easy", variable=self.difficult_select, value=1)
    #    self.dif1 = Radiobutton(self.root, text="hard", variable=self.difficult_select, value=2)
    #    self.player_select.set(2)
    #    self.rad0 = Radiobutton(self.root, text="White", variable=self.player_select, value=1)
    #    self.rad1 = Radiobutton(self.root, text="Black", variable=self.player_select, value=2)
#
    #def put_main_buttons(self):
    #    self.start_game_button = Button(self.root, text="New game", width=20, command=self.create_game)
    #    self.load_game_button = Button(self.root, text="Load game", width=20, command=self.load_game)
    #    self.server_button = Button(self.root, text="Create server", width=20, command=self.create_server)
    #    self.client_button = Button(self.root, text="Connect", width=20, command=self.create_client)
    #    self.label_ip = Label(self.root, text=" IP:  " + self.ip)
    #    self.text_ip = Entry(self.root)
    #    self.help_button = Button(self.root, text="?", width=5, command=self.open_help)

    def open_help(self):
        child = Toplevel(self.root)
        child.title('Help')
        child.minsize(width=700, height=200)
        child.maxsize(width=700, height=200)
        text = Text(child, font="Verdana 12", wrap=WORD)
        text.insert(1.0, "Reversi\n\
            Чтобы начать игру, нажмите кнопку 'start game'.\n\
            Можно вырать цвет и сложность игры.\n\
            Доступен один слот для сохранения.\n\
            \t\tКак играть по сети: \n\
            если есть поключение к сети, появится ваш IP\n\
            Один человек создает сервер, и говорит свой IP другому человеку\n\
            другой вводит его IP и нажимает подключиться")
        text.pack()

    #def create_window_buttons(self):
    #    self.put_main_buttons()
    #    self.create_radiobuttons()
    #    self.grid_window_buttons()

    #def destroy_main_menu(self):
    #    self.start_game_button.destroy()
    #    self.rad0.destroy()
    #    self.rad1.destroy()
    #    self.load_game_button.destroy()
    #    self.text_ip.destroy()
    #    self.label_ip.destroy()
    #    self.client_button.destroy()
    #    self.server_button.destroy()
    #    self.dif0.destroy()
    #    self.dif1.destroy()

    def put_canvas_objects(self):
        self.canvas = Canvas(self.root, width=400, height=400, bg="green")
        self.save_button = Button(self.root, text="Save game", width=20, command=self.save_game)
        self.load_button = Button(self.root, text="Load game", width=20, command=self.load_game_2)
        self.menu_button = Button(self.root, text="Main menu", width=20, command=self.go_to_main_menu)
        self.label = Label(self.root, font="Arial 18")
        self.canvas.bind('<Button-1>', self.change_cell)
        self.canvas.bind('<Motion>', self.mouse_move)

    def place_canvas_objects(self):
        self.canvas.place(x=0, y=30)  # .grid(row=1, column=0)
        self.menu_button.place(x=0, y=0, width=200, height=30)  # .grid(row=0, column=0)
        if not self.Booleans.is_online:
            self.save_button.place(x=200, y=0, width=200, height=30)  # .grid(row=0, column=1)
            self.load_button.place(x=400, y=0, width=200, height=30)  # .grid(row=0, column=2)
        self.label.place(x=410, y=50)  # .grid(row=1, column=1)

    def put_canvas(self):
        self.Booleans.is_game_come = True
        self.put_canvas_objects()
        self.Buttons.destroy_main_menu()
        self.place_canvas_objects()
        self.draw_canvas()
        self.start_game()

    def go_to_main_menu(self):
        self.Booleans.is_need_menu = True

    def save_game(self):
        file = open(self.file_path + "\\load.txt", "w")
        for i in range(8):
            for j in range(8):
                file.write(str(self.game.field[i][j]))
            file.write('\n')
        file.write(str(self.game.step) + "\n")
        file.write(str(self.Buttons.player_select.get()) + "\n")
        file.write(str(self.Buttons.difficult_select.get()))
        file.close()

    def load_game(self):
        self.Booleans.is_game_come = False
        self.game = field.Field()
        with open(self.file_path + "\\load.txt", "r") as lines:
            lines = lines.read().split("\n")
            for i in range(8):
                for j in range(8):
                    self.game.field[i][j] = int(lines[i][j])
            self.game.step = int(lines[8])
            self.Buttons.player_select.set(int(lines[9]))
            self.Buttons.difficult_select.set(int(lines[10]))
        self.put_canvas()
        self.game.check_next_moves(self.game.step)

    def load_game_2(self):
        self.is_was_load = True

    def create_game(self):
        #print(self.step, self.player_select.get())
        self.game = field.Field()
        self.step = 2
        self.put_canvas()

    def mouse_move(self, event):
        self.X = event.x
        self.Y = event.y

    def send_your_step(self, event):
        try:
            post = str(min(7, event.y // 50)) + ':' + str(min(7, event.x // 50))
            if self.Booleans.is_client:
                self.client.set_cell(post)
            else:
                self.server.set_cell(post)
        except():
            self.label.config(text="connection brake")
            time.sleep(0.5)
            self.go_to_main_menu()

    def change_cell(self, event):
        if not self.game.is_has_move:
            return
        if self.game.step == self.Buttons.player_select.get():
            if self.game.is_correct(min(7, event.y // 50), min(7, event.x // 50), self.game.step):
                self.game.put(min(7, event.y // 50), min(7, event.x // 50), self.game.step)
                if self.Booleans.is_online:
                    self.send_your_step(event)
                if self.game.is_has_move:
                    self.change_step()
                    self.opponent_step()
                else:
                    self.draw_result()

    def start_game(self):
        if self.game.step != self.Buttons.player_select.get():
            self.opponent_step()

    def use_menu_button(self):
        self.destroy_canvas()
        self.Buttons = Buttons(self) # .create_window_buttons()
        self.Buttons.grid_window_buttons()
        self.Booleans.is_need_menu = False

    def draw_ovals(self):
        for i in range(8):
            for j in range(8):
                if self.game.field[i][j] == 2:
                    self.canvas.create_oval(j * 50 + 5, i * 50 + 5, (j + 1) * 50 - 5, (i + 1) * 50 - 5,
                                            fill="black")
                if self.game.field[i][j] == 1:
                    self.canvas.create_oval(j * 50 + 5, i * 50 + 5, (j + 1) * 50 - 5, (i + 1) * 50 - 5,
                                            fill="white")

    def draw_help(self):
        if not self.Booleans.is_online and self.Buttons.difficult_select.get() == 1:
            helped = self.game.get_help(min(7, self.Y // 50), min(7, self.X // 50), self.game.step)
            for cell in helped:
                self.canvas.create_rectangle(cell[1] * 50, cell[0] * 50, (cell[1] + 1) * 50, (cell[0] + 1) * 50,
                                             outline="red", width=3)
        self.canvas.create_rectangle(min(7, self.X // 50) * 50, min(7, self.Y // 50) * 50,
                                     (min(7, self.X // 50) + 1) * 50, (min(7, self.Y // 50) + 1) * 50,
                                     outline="yellow", width=1.5)

    def draw_lines(self):
        for i in range(8):
            self.canvas.create_line(i * 50, 0, i * 50, 400, width=1, fill="blue")
            self.canvas.create_line(0, i * 50, 400, i * 50, width=1, fill="blue")

    def draw_if_was_load(self):
        self.destroy_canvas()
        self.Booleans.is_was_load = False
        self.load_game()

    def draw_if_need_menu(self):
        self.canvas.delete("all")
        self.draw_lines()
        self.draw_ovals()
        self.draw_help()
        self.label.config(text=self.do_text())
        self.root.after(100, self.draw_canvas)

    def draw_canvas(self):
        if self.Booleans.is_was_load:
            self.draw_if_was_load()
            return
        if self.Booleans.is_need_menu:
            self.use_menu_button()
            return
        try:
            self.draw_if_need_menu()
            return
        except():
            self.use_menu_button()

    def change_step(self):
        if self.step == 1:
            self.step = 2
        else:
            self.step = 1

    def opponent_step(self):
        if not self.Booleans.is_online:
            th = threading.Thread(target=self.game.bot_step, args=(self.game.step, self.Buttons.difficult_select.get(),))
            th.start()
        else:
            try:
                th = threading.Thread(target=self.online_opponent_step)
                th.start()
            except:
                self.label.config(text="connection brake")
                time.sleep(0.5)
                self.go_to_main_menu()
        if self.game.is_has_move:
            self.change_step()
        else:
            self.draw_result()

    def online_opponent_step(self):
        try:
            if self.Booleans.is_client:
                cell = self.client.get_cell().split(':')
                # self.game.put(int(cell[0]), int(cell[1]), self.game.step)
            else:
                cell = self.server.get_cell().split(':')
                # self.game.put(int(cell[0]), int(cell[1]), self.game.step)
            self.game.put(int(cell[0]), int(cell[1]), self.game.step)
        except:
            self.draw_result()

    def do_text(self):
        if self.game.is_has_move:
            res = self.game.get_result()
            return "Black: " + str(res[0]) + "   " + "White: " + str(res[1])
        else:
            self.draw_result()

    def draw_result(self):
        a = self.game.get_result()
        if a[0] > a[1]:
            self.label.config(text="Black win!")
        else:
            self.label.config(text="White win!")

    def destroy_canvas(self):
        self.canvas.destroy()
        if not self.Booleans.is_online:
            self.save_button.destroy()
            self.load_button.destroy()
        self.label.destroy()
        self.menu_button.destroy()


if __name__ == "__main__":
    window = Window()
    window.start()