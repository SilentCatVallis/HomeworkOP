from tkinter import *
from PIL import Image, ImageTk
import os


class Window():
    def __init__(self):
        self.root = Tk()
        self.crete_window()
        self.create_window_button()

    def crete_window(self):
        self.root.title("Photo viewer")
        self.root.minsize(width=700, height=500)

    def create_window_button(self):
        self.albom_button = Button(self.root, text="Show alboms", command=self.show_alboms)
        self.photo_found_button = Button(self.root, text="Found photo", command=self.found_photo)
        self.albom_button.place(x=0, y=0, width=200, height=100)

    def found_photo(self):

        pass

    def show_alboms(self):
        alboms = []
        for alb in self.get_all_alboms():
            alboms.append(alb)
        alboms_buttons = []
        for i in range(min(3, len(alboms))):
            alboms_buttons.append(Button(self.root, text=alboms[i][:-4], command=self.show_albom(alboms[i])))
        for i in range(len(alboms_buttons)):
            alboms_buttons[i].place(x=i * 100, y=200, width=90, height=50)

    def show_albom(self, albom):
        def show_selected_albom():
            nonlocal albom
            nonlocal self
            files = os.listdir(albom)
            self.current_albom_name = albom
            self.current_albom = files
            self.index_in_albom = 0
            photo = self.get_image()
            self.photo_label = Label(self.root, image=photo)
            self.photo_label.photo = photo
            self.photo_label.place(x=300, y=200)
            self.button_right = Button(self.root, text="next photo", command=self.get_next_photo)
            self.button_right.pack()

        return show_selected_albom

    def get_next_photo(self):
        self.index_in_albom += 1
        if self.index_in_albom == len(self.current_albom):
            self.index_in_albom = 0
        self.photo_label.destroy()
        try:
            photo = self.get_image()
            self.photo_label = Label(self.root, image=photo)
            self.photo_label.photo = photo
        except:
            self.photo_label = Label(self.root, text=self.current_albom_name + "/" + self.current_albom[
                self.index_in_albom] + "is not a picture")
        self.photo_label.place(x=300, y=200)

    def get_image(self):
        image = Image.open(self.current_albom_name + "/" + self.current_albom[self.index_in_albom])
        photo = ImageTk.PhotoImage(image)
        return photo

    def get_all_alboms(self):
        files = os.listdir(os.curdir)
        for file in files:
            if ".alb" in file:
                yield file

    def start(self):
        self.root.mainloop()


if __name__ == "__main__":
    window = Window()
    window.start()

