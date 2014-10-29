import os
from PIL import Image


def compare_pictures(first, second):
    w = 4
    h = 4
    im1 = Image.open(first).convert('L').resize((h, w))
    im2 = Image.open(second).convert('L').resize((h, w))
    ans = 0.0
    matrix1 = im1.load()
    matrix2 = im2.load()
    for i in range(h):
        for j in range(w):
            ans += abs(matrix1[(i, j)] - matrix2[(i, j)]) / 255
    return 100 * ans / (w * h)
