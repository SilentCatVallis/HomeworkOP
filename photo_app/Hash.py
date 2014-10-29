from PIL import Image


def compare_pictures(first, second):
    w = 8
    h = 8
    im1 = Image.open(first).convert('L').resize((h, w))
    im2 = Image.open(second).convert('L').resize((h, w))
    matrix1 = im1.load()
    matrix2 = im2.load()
    average1 = 0.0
    average2 = 0.0
    for i in range(h):
        for j in range(w - 1):
            average1 += matrix1[(i, j)]
            average2 += matrix2[(i, j)]
    average1 //= w * h
    average2 //= w * h
    ans = 0.0
    for i in range(h):
        for j in range(w - 1):
            if matrix1[(i, j)] >= average1 and matrix2[(i, j)] >= average2:
                ans += 1
            if matrix1[(i, j)] < average1 and matrix2[(i, j)] < average2:
                ans += 1
    return ans // (w * h)
