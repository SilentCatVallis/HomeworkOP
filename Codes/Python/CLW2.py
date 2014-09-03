import socket

sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

sock.bind(('0.0.0.0', 31337))
sock.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
sock.listen(socket.SOMAXCONN)

while True:
    client, address = sock.accept()
    data = client.recv()
    client.send(message)
    client.close()
