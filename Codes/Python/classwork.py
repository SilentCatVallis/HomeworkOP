def even(x):
    while True:
        pr = True
        for i in range(2, x):
            if x % i == 0:
                pr = False
        if pr:
            yield x
        x += 1


def take(i, func):
    amount = 0
    for e in func(i):
        amount += 1
        if amount == 101:
            break
        print(e)

take(800, even)


def merge_zip(*args):
    e = []
    e.add

    iters = [iter(x) for x in args]
    for i in iters

'urgu/org/150'