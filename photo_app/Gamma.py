import os
from PIL import Image


def compare_pictures(first, second):
    w = 200
    h = 200
    im1 = Image.open(first).convert('L').resize((h, w))
    im2 = Image.open(second).convert('L').resize((h, w))
    matrix1 = im1.load()
    matrix2 = im2.load()
    ans = 0.0
    for i in range(h):
        for j in range(w - 1):
            matrix1[(i, j)] = matrix1[(i, j)] - matrix1[(i, j + 1)]
            matrix2[(i, j)] = matrix2[(i, j)] - matrix2[(i, j + 1)]
            ans += abs(matrix1[(i, j)] - matrix2[(i, j)]) / 255
    #print(ans)
    #print(100 * ans / (w * h), first, second)
    return 100 * ans / (w * h)
