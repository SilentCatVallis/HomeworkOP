import socket


class Client(object):
    def __init__(self, ip):
        #ip = '192.168.0.86'
        self.sock = socket.socket()
        self.sock.connect((ip, 9090))
        self.sock.send(b"yeap")

    def check_color(self):
        print('check color')
        data = self.sock.recv(1024)
        if data == b"yeap":
            self.sock.send(b"who")
            color = self.sock.recv(1024).decode()
            print('get color: ' + str(color))
            return int(color)
        else:
            return 0

    def get_cell(self):
        print('get cell')
        return self.sock.recv(1024).decode()

    def set_cell(self, cell):
        print('set cell')
        self.sock.send(cell.encode())