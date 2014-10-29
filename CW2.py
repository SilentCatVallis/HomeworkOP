import socket
from select import select

server = socket.socket(
    socket.AF_INET, socket.SOCK_STREAM)
server.bind(('0.0.0.0', 31337))
server.listen(10)

clients = []

def on_accept(server):
    client, addr = server.accept()
    clients.append(client)
    client.send(b'hello')

def on_read(client):
    pass

while True:
    can_read, _, _ = select([server], [ ], [ ], 0.01)

    for sock in can_read:
        if sock == server:
            on_accept(server)
        else:
            on_read(sock)
        #if can_read:
        #    pair = client, addr = server.accept()
        #    clients.append(pair)
        #    client.send(b'lalala')