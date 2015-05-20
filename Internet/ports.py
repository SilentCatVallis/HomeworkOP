import socket
import sys
socket.setdefaulttimeout(0.1)

def listener(left_port, right_port):
    open_ports = []
    for port in range(left_port, right_port+1):
        s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        try:
            s.connect(('cs.usu.edu.ru', port))
            open_ports.append(port)
            print(str(port))
        except:
            pass
        finally:
            s.close()
    return open_ports
    
if len(sys.argv) == 2:
    if sys.argv[1] == "/?" or sys.argv[1] == "--help":
        print("usage: python ports.py <lower_bound> <upper_bound>")
elif len(sys.argv) == 3:
    listener(int(sys.argv[1]), int(sys.argv[2]))
else:
    print("usage: python ports.py <lower_bound> <upper_bound>")