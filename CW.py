suff = {}
pref = {}

def main():
    local_len = -1
    tmp = ''
    tmp1 = ''
    adadadadadadadadadadadadadadada = ''
    adadadadadadadadadadadadadada = ''
    with open("CW.py") as f:
        file = f.readlines()
    for line in file:
        for word in line.split(' '):
            func(word)
    for suf in suff.keys():
        if suf in pref:
            if len(suf) > local_len and suff[suf] != pref[suf]:
                local_len = len(suf)
                tmp = suff[suf]
                tmp1 = pref[suf]
    print(tmp, tmp1, local_len)



def func(word):
    suf = ''
    prf = ''
    iter = 0
    while (suf != word):
        suf = word[0:iter]
        prf = word[-iter:]
        iter += 1
        suff[suf] = word
        pref[prf] = word

def func1(line1, line2):
    pref = ''
    suf = ''
    iter1 = 0
    iter2 = len(line2) - 1
    if (len(line1) == 0 or len(line2) == 0):
        return 0
    while (iter1 != len(line1) and iter2 >= 0):
        if (line1[iter1] != line2[iter2]):
            return iter1
        iter1 += 1
        iter2 -= 1
    return iter1



def main1():
    dict = {}
    with open("wwww.txtx", encoding="utf8") as f:
        text = f.read()
        for line in text:
            for word in line:
                if word in dict:
                    dict[word] += 1
                else:
                    dict[word] = 1
    for key in dict:
        print(dict[key])



if __name__ == "__main__":
    main()

