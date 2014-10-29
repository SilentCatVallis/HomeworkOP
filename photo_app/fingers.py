from PIL import Image


def compare_pictures(first, second):
    first_picture = binary(Image.open(first))

    tmp_delete(first_picture)
    prepared_first_picture = find_check_point(first_picture)
    prepared_first_picture = del_noise_point(prepared_first_picture)

    second_picture = binary(Image.open(second))

    tmp_delete(second_picture)
    prepared_second_picture = find_check_point(second_picture)
    prepared_second_picture = del_noise_point(prepared_second_picture)

    res = matching_point(prepared_first_picture, prepared_second_picture)
    answer = (res[0] / (res[1] * 1.)) * 100
    return 100 - answer


def binary(img):
    binary_image = []
    matrix = img.load()
    for i in range(img.size[0]):
        tmp = []
        for j in range(img.size[1]):
            t = matrix[(i, j)]
            p = t[0] * 0.3 + t[1] * 0.59 + t[2] * 0.11
            if p > 128:
                p = 1
            else:
                p = 0
            tmp.append(p)
        binary_image.append(tmp)
    return binary_image


def remove_double(x, y):
    z = []
    for i in x:
        c = True
        for j in y:
            if i == j:
                c = False
        if c:
            z.append(i)
    for i in y:
        c = True
        for j in x:
            if i == j:
                c = False
        if c:
            z.append(i)
    return z


def del_noise_point(r):
    tmp = []
    tmp2 = []
    for i in r[1]:
        x = range(i[0] - 5, i[0] + 5)
        y = range(i[1] - 5, i[1] + 5)
        for j in r[0]:
            if j[0] in x and j[1] in y:
                tmp.append(i)
                tmp2.append(j)
    return remove_double(r[0], tmp2), remove_double(r[1], tmp)


def matching_point(r, v):
    all_points = 0
    match = 0
    for i in v[0]:
        x = range(i[0] - 15, i[0] + 15)
        y = range(i[1] - 15, i[1] + 15)
        all_points += 1
        for j in r[0]:
            if j[0] in x and j[1] in y:
                match += 1
                break
    for i in v[1]:
        x = range(i[0] - 15, i[0] + 15)
        y = range(i[1] - 15, i[1] + 15)
        all_points += 1
        for j in r[1]:
            if j[0] in x and j[1] in y:
                match += 1
                break

    return match, all_points


def check_this_point(img, x, y):
    h = len(img)
    w = len(img[0])
    c = 0
    for i in range(x - 1, x + 2):
        for j in range(y - 1, y + 2):
            if 0 <= i < h and 0 <= j < w:
                if img[i][j] == 0:
                    c += 1
    return c - 1


def find_check_point(img):
    x = len(img)
    y = len(img[0])
    branch_point = []
    end_point = []
    for i in range(x):
        for j in range(y):
            if img[i][j] == 0:
                t = check_this_point(img, i, j)
                if t == 1:
                    end_point.append((i, j))
                if t == 3:
                    branch_point.append((i, j))
    return branch_point, end_point


def tmp_delete(img):
    w = len(img)
    h = len(img[0])
    count = 1
    while count != 0:
        count = delete(img, w, h)
        if count:
            delete2(img, w, h)


def delete(img, w, h):
    count = 0
    for i in range(1, h - 1):
        for j in range(1, w - 1):
            if img[j][i] == 0:
                if deletable(img, j, i):
                    img[j][i] = 1
                    count += 1
    return count


def delete2(img, w, h):
    for i in range(1, h - 1):
        for j in range(1, w - 1):
            if img[j][i] == 0:
                if deletable2(img, j, i):
                    img[j][i] = 1


def fringe(a):
    t = [[1, 1, 1, 1, 0, 1, 1, 1, 1],
         [1, 1, 1, 1, 0, 1, 1, 0, 0],
         [1, 1, 1, 0, 0, 1, 0, 1, 1],
         [0, 0, 1, 1, 0, 1, 1, 1, 1],
         [1, 1, 0, 1, 0, 0, 1, 1, 1],
         [1, 1, 1, 1, 0, 1, 0, 0, 1],
         [0, 1, 1, 0, 0, 1, 1, 1, 1],
         [1, 0, 0, 1, 0, 1, 1, 1, 1],
         [1, 1, 1, 1, 0, 0, 1, 1, 0],
         [1, 1, 1, 1, 0, 1, 0, 0, 0],
         [0, 1, 1, 0, 0, 1, 0, 1, 1],
         [0, 0, 0, 1, 0, 1, 1, 1, 1],
         [1, 1, 0, 1, 0, 0, 1, 1, 0]]
    for i in t:
        if a == i:
            return True


def check(a):
    t123457 = [1, 1, 0, 0, 1, 0]
    t013457 = [1, 1, 1, 0, 0, 0]
    t134567 = [0, 1, 0, 0, 1, 1]
    t134578 = [0, 0, 0, 1, 1, 1]
    t0123457 = [1, 1, 1, 0, 0, 0, 0]
    t0134567 = [1, 0, 1, 0, 0, 1, 0]
    t1345678 = [0, 0, 0, 0, 1, 1, 1]
    t1234578 = [0, 1, 0, 0, 1, 0, 1]

    t = [a[1], a[2], a[3], a[4], a[5], a[7]]
    if t == t123457:
        return True
    t = [a[0], a[1], a[3], a[4], a[5], a[7]]
    if t == t013457:
        return True
    t = [a[1], a[3], a[4], a[5], a[6], a[7]]
    if t == t134567:
        return True
    t = [a[1], a[3], a[4], a[5], a[7], a[8]]
    if t == t134578:
        return True
    t = [a[0], a[1], a[2], a[3], a[4], a[5], a[7]]
    if t == t0123457:
        return True
    t = [a[1], a[3], a[4], a[5], a[6], a[7], a[8]]
    if t == t1345678:
        return True
    t = [a[0], a[1], a[3], a[4], a[5], a[6], a[7]]
    if t == t0134567:
        return True
    t = [a[1], a[2], a[3], a[4], a[5], a[7], a[8]]
    if t == t1234578:
        return True


def deletable(img, x, y):
    a = []
    for i in range(y - 1, y + 2):
        for j in range(x - 1, x + 2):
            a.append(img[j][i])
    return check(a)


def deletable2(img, x, y):
    a = []
    for i in range(y - 1, y + 2):
        for j in range(x - 1, x + 2):
            a.append(img[j][i])
    return fringe(a)

