from tkinter import *
import field
import server
import client
import socket
import threading
import time


class Window():
    def create_bools(self):
        self.is_need_menu = False
        self.is_game_come = False
        self.is_was_load = False
        self.is_server = False
        self.is_client = False
        self.is_online = False

    def found_user_ip(self):
        try:
            self.ip = socket.gethostbyname_ex(socket.gethostname())[2]
            self.ip = self.ip[len(self.ip) - 1]
        except:
            self.ip = ""

    def __init__(self):
        self.found_user_ip()
        self.create_bools()
        self.X = -1
        self.Y = -1
        self.step = 0
        self.root = Tk()
        self.crete_window()
        self.create_window_buttons()

    def crete_window(self):
        self.root.title("Reverse")
        self.root.minsize(width=700, height=500)
        self.root.maxsize(width=700, height=500)

    def start(self):
        self.root.mainloop()

    def create_server(self):
        print('server')
        self.server = server.Server(self.ip)
        if self.server.find_connect():
            print("yeap")
            self.is_server = True
            self.is_online = True
            self.server.send_color(self.player_select.get())
            self.create_game()

    def create_client(self):
        print('client')
        self.client = client.Client(self.text_ip.get())
        color = self.client.check_color()
        if color != 0:
            self.is_client = True
            self.is_online = True
            if color == 1:
                self.player_select.set(2)
            else:
                self.player_select.set(1)
            self.create_game()

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

    def create_radiobuttons(self):
        self.player_select = IntVar()
        self.difficult_select = IntVar()
        self.difficult_select.set(1)
        self.dif0 = Radiobutton(self.root, text="easy", variable=self.difficult_select, value=1)
        self.dif1 = Radiobutton(self.root, text="hard", variable=self.difficult_select, value=2)
        self.player_select.set(2)
        self.rad0 = Radiobutton(self.root, text="White", variable=self.player_select, value=1)
        self.rad1 = Radiobutton(self.root, text="Black", variable=self.player_select, value=2)

    def put_main_buttons(self):
        self.start_game_button = Button(self.root, text="New game", width=20, command=self.create_game)
        self.load_game_button = Button(self.root, text="Load game", width=20, command=self.load_game)
        self.server_button = Button(self.root, text="Create server", width=20, command=self.create_server)
        self.client_button = Button(self.root, text="Connect", width=20, command=self.create_client)
        self.label_ip = Label(self.root, text=" IP:  " + self.ip)
        self.text_ip = Entry(self.root)

    def create_window_buttons(self):
        self.put_main_buttons()
        self.create_radiobuttons()
        self.grid_window_buttons()

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
        if not self.is_online:
            self.save_button.place(x=200, y=0, width=200, height=30)  # .grid(row=0, column=1)
            self.load_button.place(x=400, y=0, width=200, height=30)  # .grid(row=0, column=2)
        self.label.place(x=410, y=50)  # .grid(row=1, column=1)

    def put_canvas(self):
        self.is_game_come = True
        self.put_canvas_objects()
        self.destroy_main_menu()
        self.place_canvas_objects()
        self.draw_canvas()
        self.start_game()

    def go_to_main_menu(self):
        self.is_need_menu = True

    def save_game(self):
        file = open("load.txt", "w")
        for i in range(8):
            for j in range(8):
                file.write(str(self.game.field[i][j]))
            file.write('\n')
        file.write(str(self.game.step) + "\n")
        file.write(str(self.player_select.get()) + "\n")
        file.write(str(self.difficult_select.get()))
        file.close()

    def load_game(self):
        self.is_game_come = False
        self.game = field.Field()
        with open("load.txt", "r") as lines:
            lines = lines.read().split("\n")
            for i in range(8):
                for j in range(8):
                    self.game.field[i][j] = int(lines[i][j])
            self.game.step = int(lines[8])
            self.player_select.set(int(lines[9]))
            self.difficult_select.set(int(lines[10]))
        self.put_canvas()
        self.game.check_next_moves(self.game.step)

    def load_game_2(self):
        self.is_was_load = True

    def create_game(self):
        print(self.step, self.player_select.get())
        self.game = field.Field()
        self.step = 2
        self.put_canvas()

    def mouse_move(self, event):
        self.X = event.x
        self.Y = event.y

    def send_your_step(self, event):
        try:
            if self.is_client:
                self.client.set_cell(str(min(7, event.y // 50)) + ':' + str(min(7, event.x // 50)))
            else:
                self.server.set_cell(str(min(7, event.y // 50)) + ':' + str(min(7, event.x // 50)))
        except:
            self.label.config(text="connection brake")
            time.sleep(0.5)
            self.go_to_main_menu()

    def change_cell(self, event):
        if not self.game.is_has_move:
            return
        if self.game.step == self.player_select.get():
            if self.game.is_correct(min(7, event.y // 50), min(7, event.x // 50), self.game.step):
                self.game.put(min(7, event.y // 50), min(7, event.x // 50), self.game.step)
                if self.is_online:
                    self.send_your_step(event)
                if self.game.is_has_move:
                    self.change_step()
                    self.opponent_step()
                else:
                    self.draw_result()

    def start_game(self):
        if self.game.step != self.player_select.get():
            self.opponent_step()

    def use_menu_button(self):
        self.destroy_canvas()
        self.create_window_buttons()
        self.is_need_menu = False

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
        if not self.is_online and self.difficult_select.get() == 1:
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

    def draw_canvas(self):
        if self.is_was_load:
            self.destroy_canvas()
            self.is_was_load = False
            self.load_game()
            return
        if self.is_need_menu:
            self.use_menu_button()
            return
        try:
            self.canvas.delete("all")
            self.draw_lines()
            self.draw_ovals()
            self.draw_help()
            self.label.config(text=self.do_text())
            self.root.after(100, self.draw_canvas)
            return
        except:
            self.use_menu_button()

    def change_step(self):
        if self.step == 1:
            self.step = 2
        else:
            self.step = 1

    def opponent_step(self):
        if not self.is_online:
            th = threading.Thread(target=self.game.bot_step, args=(self.game.step, self.difficult_select.get(),))
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
            if self.is_client:
                cell = self.client.get_cell().split(':')
                #self.game.put(int(cell[0]), int(cell[1]), self.game.step)
            else:
                cell = self.server.get_cell().split(':')
                #self.game.put(int(cell[0]), int(cell[1]), self.game.step)
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
        if not self.is_online:
            self.save_button.destroy()
            self.load_button.destroy()
        self.label.destroy()
        self.menu_button.destroy()


if __name__ == "__main__":
    window = Window()
    window.start()