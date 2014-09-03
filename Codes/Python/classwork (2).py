import sys

#decorator

def dec(n):
    def wrapper(fn):
        def wrapped(a, b):
            for i in range(n):
                fn(a, b)
        return wrapped
    return wrapper


@dec(4)
def test(a, b):
    print(a, b)


#dec(test, 10)
test(5, 5)


def memoize(fn):
    cashe = {}
    def wrapper(*args, **kwargs):
        key = (args, tuple(kwargs.items()))
        if key not in cashe:
            cashe[key] = fn(*args, **kwargs)
        return cashe[key]
    return wrapper


def logging(fn):
    def wrapper(*args, **kwargs):
        print(fn.__name__, args, kwargs, fn(*args, **kwargs))
        return fn(*args, **kwargs)
    return wrapper


def varry_on(*kwarg_names):
    u = 9


@memoize
@logging
def fibb(n):
    if n == 0:
        return 0
    elif n == 1:
        return 1
    else:
        return fibb(n - 1) + fibb(n - 2)


print(fibb(3))


def cash_class(cls):
    for name in dir(cls):
        if not name.startswith('_'):
            fn = getattr(cls, name)
            if callable(fn):
                fn = memoize(fn)
                setattr(cls, name, fn)
    return cls


@cash_class
class A():
    def x(self): print("X")

    def y(self): print("y")

    def z(self): print("z")


a = A()
a.x()
a.y()