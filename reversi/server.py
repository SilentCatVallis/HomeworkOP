import socket


class Server(object):
    def __init__(self, ip):
        self.sock = socket.socket()
        self.sock.bind((ip, 9090))
        self.sock.listen(1)
        self.conn, self.addres = self.sock.accept()

    def find_connect(self):
        while True:
            data = self.conn.recv(1024)
            if not data:
                break
            if data == b"yeap":
                self.conn.send(data)
                return True
            else:
                return False

    def send_color(self, color):
        while True:
            data = self.conn.recv(1024)
            if not data:
                break
            if data == b"who":
                print('send color')
                self.conn.send(str(color).encode())
                return True
            else:
                return False

    def get_cell(self):
        print('get cell')
        while True:
            data = self.conn.recv(1024)
            if not data:
                break
            if ':' in data.decode():
                print('get' + str(data))
                return data.decode()

    def set_cell(self, cell):
        self.conn.send(cell.encode())

