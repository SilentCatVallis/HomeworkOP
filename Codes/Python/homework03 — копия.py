#!/usr/bin/python3



from urllib.request import build_opener, pathname2url, urlopen, url2pathname
import re
import sys

host = "http://ru.wikipedia.org"
start_line = "mw-content-text"
end_line = "references-small"
bane_line = "Вики (значения)"
url_line = "href=\"/wiki/"
#start_url = host + pathname2url("/wiki/Конь_в_пальто_(памятник)")
start_url = host + pathname2url(sys.argv[1])
pictures = [".png", ".svg", "JPG", "jpg"]
picture_line = "?uselang=ru"
where = {}
second_start_line = "<p>"
second_end_line = "</p>"
steck = []

def next_site(url, num):
    if len(steck) - 1 >= num:
        steck[num] = url
    else:
        steck.append(url)
    if url == "http://ru.wikipedia.org/wiki/%D0%A4%D0%B8%D0%BB%D0%BE%D1%81%D0%BE%D1%84%D0%B8%D1%8F":
        for i in range(num + 1):
            print(steck[i])
        print("YES, amount of steps = ", num + 1)
        exit(0)
    is_start_was = False
    mistake = False
    data = ''
    try:
        data = urlopen(url)
        if (data in [404]):
            mistake = True
    except:
        mistake = False
    if not mistake and data != '':
        for x in data:
            x = x.decode('utf8')
            if start_line in x:
                is_start_was = True
            if end_line in x:
                is_start_was = False
            if is_start_was:
                if second_start_line in x:
                    if bane_line not in x and start_line not in x:
                        pattern = '/wiki/[^"]*'
                        found = re.findall(pattern, x)
                        for line in found:
                            if line not in where:
                                where[line] = 1
                                if picture_line not in line:
                                    flag = True
                                    for j in pictures:
                                        if j in line:
                                            flag = False
                                            break
                                    if flag:
                                        next_site(host + line, num + 1)

steck.append(start_url)
next_site(start_url, 0)
print("Can't found")