import threading
import time
import os
import multiprocessing

x = os.fork()
print("I am {}" .format(x))

exit(0)

def printer(count=100):
    for x in range(count):
        print("Hello number {} from {}" .format(x, threading.current_thread()))

counter = 0

def adder():
    global counter
    lock.acquire()
    counter += 1
    lock.release()

lock = threading.Lock()

def add_count(count):
    for _ in range(count):
        adder()


#for _ in range(10000000):
#    adder()

#for _ in range(10):
 #   t1 = threading.Thread(target=add_count, args=(1000000,))
  #  t1.start()
   # t1.join()



threads = [threading.Thread(target=add_count, args=(250000,)),
           threading.Thread(target=add_count, args=(250000,)),
           threading.Thread(target=add_count, args=(250000,)),
           threading.Thread(target=add_count, args=(250000,))]

for t in threads:
    t.start()
for t in threads:
    t.join()

print(counter)
print("lol")









class Philosopher(threading.Thread):
    def __init__(self, left, right):
        super().__init__()
        self.left = left
        self.right = right
        self.satisfied = 0

    def run(self):
        while self.satisfied < 100:
            self.eat()

    def eat(self):
        with self.left:
            with self.right:
                self.satisfied += 1
                time.sleep(0.01)

