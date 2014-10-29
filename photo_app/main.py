from tkinter import *
from PIL import Image, ImageTk
import os
import ImSim
import algorithms
import datetime
import shutil
from tkinter.filedialog import *
import tkinter.filedialog
# http://habrahabr.ru/post/163663/


class Window():
    def __init__(self):
        self.root = Tk()
        self.crete_window()
        self.create_window_button()
        self.photo_extensions = [".bmp", ".png", ".jpg", ".jpeg"]
        self.cur_path = os.path.abspath(os.path.dirname(sys.argv[0]))
        #print(self.cur_path)

    def crete_window(self):
        self.root.title("Photo viewer")
        self.root.minsize(width=700, height=500)

    def create_window_button(self):
        self.album_button = Button(self.root, text="Show albums", command=self.show_albums)
        self.photo_found_button = Button(self.root, text="Found photo", command=self.start_found_photo)
        self.help_button = Button(self.root, text="?", width=5, command=self.open_help)
        self.help_button.place(x=500, y=0, width=30, height=30)
        self.album_button.place(x=0, y=0, width=200, height=100)
        self.photo_found_button.place(x=210, y=0, width=200, height=100)

    def open_help(self):
        child = Toplevel(self.root)
        child.title('Help')
        child.minsize(width=700, height=200)
        child.maxsize(width=700, height=200)
        text = Text(child, font="Verdana 12", wrap=WORD)
        text.insert(1.0, "Photo app\n\
            Чтобы начать кластеризацию, нажмите found_photo.\n\
            Выберите нужный алгоритм кластеризации,\n\
            Папку, в которой искать фотографии,\n\
            Точность сравнения фотографий в процентах (одно число от 0 до 100)")
        text.pack()

    def start_found_photo(self):
        #self.destroy_albums_buttons()
        self.destroy_album()
        self.open_find_menu()

    def open_find_menu(self):
        self.create_radio_buttons()
        self.create_path_selector()
        self.start_button = Button(self.root, text="Start search", command=self.clustering_photo)
        self.start_button.place(x=450, y=110, width=100, height=100)
        self.matching_percent_text = Text(self.root, font="Verdana 8", wrap=WORD)
        self.matching_percent_text.place(x=450, y=220, width=100, height=30)
        self.matching_percent_text.insert(1.0, "50")

    def destroy_start_button(self):
        try:
            self.start_button.destroy()
            self.matching_percent_text.destroy()
        except AttributeError:
            return

    def clustering_photo(self):
        file_name = self.path_selectors[2].get(1.0, END)
        if os.path.isfile(file_name):
            self.path_selectors[2].insert(1.0, "It is not a folder! ")
            return
        clustering_name = self.create_clustering_name()
        algorithm = self.rad_buttons[self.algo_select.get()]['text']
        matching_percent = self.matching_percent_text.get(1.0, END)
        if not isinstance(matching_percent, float):
            self.matching_percent_text.insert(1.0, "It is not a number!")
            return
        matching_percent = int(matching_percent)
        if matching_percent > 100 or matching_percent < 0:
            self.matching_percent_text.insert(1.0, "Incorrect number")
            return
        self.search_photo(file_name, clustering_name, algorithm, matching_percent)

    def create_clustering_name(self):
        path = self.cur_path
        time = datetime.datetime.now()
        return path + "\\Clustering_" + str(time)[:-7].replace('-', '_').replace(' ', '_').replace(':', '_')

    def search_photo(self, folder, name, algorithm, matching_percent):
        if folder.endswith('\n'):
            folder = folder[:-1]
        if folder == name or self.cur_path + "\\Clustering" in folder:
            return
        for file in os.listdir(folder):
            file_name = folder + "\\" + file
            if not os.path.isfile(file_name):
                self.search_photo(file_name, name, algorithm, matching_percent)
            else:
                if self.is_photo(file_name):
                    self.clusterising(file_name, name, algorithm, matching_percent)

    def put_in_new_album(self, file_name, folder_name):
        album = self.create_album(folder_name)
        self.put_photo_in_album(file_name, album)

    def clusterising(self, file_name, folder_name, algorithm, matching_percent):
        if not os.path.exists(folder_name):
            os.makedirs(folder_name)
        albums = list(self.get_all_albums(folder_name))
        for album in albums:
            full_album_name = folder_name + "\\" + album
            if self.is_similar_with_album(full_album_name, file_name, algorithm, matching_percent):
                self.put_photo_in_album(file_name, full_album_name)
                return
        self.put_in_new_album(file_name, folder_name)


    def is_similar_with_album(self, album_name, file_name, algorithm, matching_percent):
        album_pictures = os.listdir(album_name)
        if len(album_pictures) == 0:
            return False
        picture = album_pictures[0]
        return algorithms.compare_pictures(algorithm, file_name, album_name + "\\" + picture, matching_percent)

    @staticmethod
    def create_album(folder_name):
        name = "Album_"
        index = 1
        while os.path.exists(folder_name + "\\" + name + str(index)):
            index += 1
        album = folder_name + "\\" + name + str(index)
        os.makedirs(album)
        return album

    @staticmethod
    def put_photo_in_album(photo, album):
        photo_name = photo.split('\\')[-1:]
        shutil.copy(photo, album + '\\' + photo_name[0])

    def is_photo(self, file):
        _, ext = os.path.splitext(file)
        if ext in self.photo_extensions:
            return True
        return False

    def create_path_selector(self):
        self.path_selectors = []
        self.path_selectors.append(Button(self.root, text="all folders", command=self.set_all_path))
        self.path_selectors.append(Button(self.root, text="in albums", command=self.set_this_path))
        self.path_selectors.append(Text(self.root, font="Verdana 8", wrap=WORD))
        self.set_this_path()
        index = 0
        for selector in self.path_selectors:
            selector.place(x=130, y=110 + 40 * index, width=300, height=35)
            index += 1

    def destroy_path_selector(self):
        try:
            for selector in self.path_selectors:
                selector.destroy()
            self.path_selectors.clear()
        except AttributeError:
            return

    def set_all_path(self):
        self.path_selectors[2].delete(1.0, END)
        self.path_selectors[2].insert(1.0, "C:\\")

    def set_this_path(self):
        self.path_selectors[2].delete(1.0, END)
        self.path_selectors[2].insert(1.0, self.cur_path)

    def create_radio_buttons(self):
        self.algo_select = IntVar()
        self.algo_select.set(0)
        self.rad_buttons = []
        index = 0
        for name in algorithms.get_all_algo():
            self.rad_buttons.append(Radiobutton(self.root, text=name, variable=self.algo_select, value=index))
            self.rad_buttons[index].place(x=20, y=110 + 40 * index, width=100, height=30)
            index += 1

    def destroy_radio_buttons(self):
        try:
            for button in self.rad_buttons:
                button.destroy()
            self.rad_buttons.clear()
        except AttributeError:
            return

    def show_albums(self):
        filename = askdirectory()
        self.destroy_radio_buttons()
        self.destroy_path_selector()
        self.destroy_start_button()
        self.albums = []
        self.albums.append(filename)
        self.album_index = 0
        self.show_album(self.album_index)()
        #for alb in self.get_all_albums():
        #    self.albums.append(alb)
        #self.albums_buttons = []
        #for i in range(min(3, len(self.albums))):
        #    self.albums_buttons.append(Button(self.root, text=self.albums[i], command=self.show_album(i)))
        #for i in range(len(self.albums_buttons)):
        #    self.albums_buttons[i].place(x=10, y=(i * 50) + 210, width=90, height=50)
        #if len(self.albums) > 3:
        #    self.show_albums_arrow()

    #def destroy_albums_buttons(self):
    #    try:
    #        if len(self.albums) > 3:
    #            self.destroy_albums_arrow()
    #        for button in self.albums_buttons:
    #            button.destroy()
    #        self.albums_buttons.clear()
    #    except AttributeError:
    #        return

    #def destroy_albums_arrow(self):
    #    self.arrow_up.destroy()
    #    self.arrow_down.destroy()
#
    #def show_albums_arrow(self):
    #    self.arrow_up = Button(self.root, text="up", command=self.albums_up)
    #    self.arrow_down = Button(self.root, text="down", command=self.albums_down)
    #    self.arrow_up.place(x=105, y=215, width=40, height=20)
    #    self.arrow_down.place(x=105, y=335, width=40, height=20)

    #def albums_up(self):
    #    self.album_index += 1
    #    for i in range(len(self.albums_buttons)):
    #        self.albums_buttons[i].config(text=self.albums[(i + self.album_index) % len(self.albums)])

    #def albums_down(self):
    #    self.album_index -= 1
    #    for i in range(len(self.albums_buttons)):
    #        self.albums_buttons[i].config(text=self.albums[(i + self.album_index) % len(self.albums)])

    def show_album(self, index):
        def show_selected_album():
            nonlocal index
            nonlocal self
            album_index = (index + self.album_index) % len(self.albums)
            album = self.albums[album_index]
            self.destroy_album()
            files = os.listdir(album)
            self.current_album_name = album
            self.current_album = files
            self.index_in_album = 0
            if len(self.current_album) != 0:
                photo = self.get_image()
                self.photo_label = Label(self.root, image=photo)
                self.photo_label.photo = photo
                self.photo_label.place(x=200, y=120)
            self.button_right = Button(self.root, text="next photo", command=self.get_next_photo)
            self.button_left = Button(self.root, text="prev photo", command=self.get_prev_photo)
            #self.find_similar_button = Button(self.root, text="Find similar")
            #self.find_similar_button.bind('<Button-1>', lambda _: self.start_find_similar())
            #self.find_similar_button.place(x=210, y=0, width=200, height=100)
            self.button_left.place(x=250, y=100, width=100, height=20)
            self.button_right.place(x=350, y=100, width=100, height=20)

        return show_selected_album

    def start_find_similar(self):
        self.root.after(0, self.find_similar)

    def find_similar(self):
        album_length = len(self.current_album)
        i = 0
        similar_pair = []
        while i < album_length:
            j = i + 1
            while j < album_length:
                image1 = self.current_album_name + "/" + self.current_album[i]
                image2 = self.current_album_name + "/" + self.current_album[j]
                ans = ImSim.compare_pictures(image1, image2)
                if ans[1] < 5:
                    similar_pair.append((ans[1], ans[2], ans[3]))
                j += 1
            i += 1
        similar_pair.sort(key=lambda x: x[1])
        self.show_compare_window(similar_pair)

    def show_compare_window(self, similar_pairs):
        root = Toplevel()
        root.title("Similar photo")
        root.minsize(width=300, height=300)
        if len(similar_pairs) == 0:
            self.all_photo_diff(root)
            return
        else:
            self.place_diff_photo(0, similar_pairs, root)

    @staticmethod
    def all_photo_diff(root):
        photo_label = Label(root, text="All photo are different!")
        photo_label.grid(row=1, column=1)
        return

    #def place_diff_photo(self, index, similar_pairs, root, widgets=None):
    #    if widgets is not None:
    #        for widget in widgets:
    #            widget.destroy()
    #    if len(similar_pairs) == 0:
    #        self.all_photo_diff(root)
    #        return
    #    if index == len(similar_pairs):
    #        index = 0
    #    if index == -1:
    #        index = len(similar_pairs) - 1
    #    widgets = []
    #    photo1 = ImageTk.PhotoImage(Image.open(similar_pairs[index][1]))
    #    photo2 = ImageTk.PhotoImage(Image.open(similar_pairs[index][2]))
    #    label1 = Label(root, image=photo1)
    #    label1.photo = photo1
    #    widgets.append(label1)
    #    label2 = Label(root, image=photo2)
    #    label2.photo = photo2
    #    widgets.append(label2)
    #    delete_button1 = Button(root, text="delete")
    #    delete_button1.bind('<Button-1>',
    #                        lambda _: self.delete_photo(similar_pairs[index][1], label1, delete_button1, similar_pairs))
    #    widgets.append(delete_button1)
    #    delete_button2 = Button(root, text="delete")
    #    delete_button2.bind('<Button-1>',
    #                        lambda _: self.delete_photo(similar_pairs[index][2], label2, delete_button2, similar_pairs))
    #    widgets.append(delete_button2)
    #    label1.grid(row=0, column=0)
    #    delete_button1.grid(row=0, column=1)
    #    label2.grid(row=1, column=0)
    #    delete_button2.grid(row=1, column=1)
    #    button_right = Button(root, text="Next pair")
    #    button_left = Button(root, text="Prev pair")
    #    widgets.append(button_right)
    #    widgets.append(button_left)
    #    button_right.bind('<Button-1>', lambda _: self.place_diff_photo(index + 1, similar_pairs, root, widgets))
    #    button_left.bind('<Button-1>', lambda _: self.place_diff_photo(index - 1, similar_pairs, root, widgets))
    #    button_left.grid(row=2, column=0)
    #    button_right.grid(row=2, column=1)

    #@staticmethod
    #def delete_photo(name, label=None, button=None, similar_pairs=None):
    #    os.remove(name)
    #    if label is not None:
    #        label.destroy()
    #    if button is not None:
    #        button.destroy()
    #    if similar_pairs is not None:
    #        elements = []
    #        for pair in similar_pairs:
    #            if pair[1] == name or pair[2] == name:
    #                elements.append(pair)
    #        for element in elements:
    #            similar_pairs.remove(element)

    def destroy_album(self):
        try:
            self.button_right.destroy()
            self.button_left.destroy()
            self.find_similar_button.destroy()
        except:
            pass
        try:
            self.photo_label.destroy()
        except:
            pass

    def place_photo_label(self):
        self.photo_label.destroy()
        try:
            photo = self.get_image()
            self.photo_label = Label(self.root, image=photo)
            self.photo_label.photo = photo
        except:
            self.photo_label = Label(self.root, text=self.current_album_name + "/" + self.current_album[
                self.index_in_album] + "is not a picture")
        self.photo_label.place(x=200, y=120)

    def get_prev_photo(self):
        self.index_in_album -= 1
        if self.index_in_album == -1:
            self.index_in_album = len(self.current_album) - 1
        self.place_photo_label()

    def get_next_photo(self):
        self.index_in_album += 1
        if self.index_in_album == len(self.current_album):
            self.index_in_album = 0
        self.place_photo_label()

    def get_image(self):
        image = ImageTk.Image.open(self.current_album_name + "/" + self.current_album[self.index_in_album])
        height = self.root.winfo_height() - 120
        width = self.root.winfo_width() - 200
        image = image.resize((400, 400))
        photo = ImageTk.PhotoImage(image)
        return photo

    def get_all_albums(self, folder=None):
        if folder is None:
            folder = os.curdir
        files = os.listdir(folder)
        for file in files:
            if not os.path.isfile(file):
                yield file

    def start(self):
        self.root.mainloop()


if __name__ == "__main__":
    window = Window()
    window.start()