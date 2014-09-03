import sys
import re


class PagesInfo:
    slow_page = str
    slow_page_time = 1000000000
    quick_page = str
    quick_page_time = 0
    slowest_average_page = {}
    most_visited_page = {}

    def add_page_info(self, page, time):
        if time != '':
            if int(time) < int(self.slow_page_time):
                self.slow_page = page
                self.slow_page_time = time
            if int(time) > int(self.quick_page_time):
                self.quick_page = page
                self.quick_page_time = time

            if page != '':
                if self.slowest_average_page.__contains__(page):
                    self.slowest_average_page[page] += time
                else:
                    self.slowest_average_page[page] = time
        if page != '':
            if self.most_visited_page.__contains__(page):
                self.most_visited_page[page] += 1
            else:
                self.most_visited_page[page] = 1

                #def sort_all_statistic(self):


class ClientInfo:
    more_active_client = {}
    days = {}

    def add_client_info(self, ip, date):
        if ip != '':
            if self.more_active_client.__contains__(ip):
                self.more_active_client[ip] += 1
            else:
                self.more_active_client[ip] = 1
        day = re.findall('[^:]*', date)
        day = day[0]
        if self.days.__contains__(day):
            if self.days[day].__contains__(ip):
                self.days[day][ip] += 1
            else:
                self.days[day][ip] = 1
        else:
            self.days[day] = {}
            self.days[day][ip] = 1


class BrowserInfo:
    more_active_browser = {}

    def add_browser_info(self, browser):
        if browser != '':
            if self.more_active_browser.__contains__(browser):
                self.more_active_browser[browser] += 1
            else:
                self.more_active_browser[browser] = 1


class Statistic:
    page_info = PagesInfo
    client_info = ClientInfo
    browser_info = BrowserInfo

    def add_statistic(self, dictionary):
        self.page_info.add_page_info(self.page_info, dictionary['page'], dictionary['time'])
        self.client_info.add_client_info(self.client_info, dictionary['ip'], dictionary['date'])
        self.browser_info.add_browser_info(self.browser_info, dictionary['browser'])


class Parser:
    @staticmethod
    def find_from(line, answer):
        from_site = re.findall('"GET [^"]*', line)
        if from_site.__len__() == 0:
            from_site = re.findall('"HEAD [^"]*', line)
            if from_site.__len__() == 0:
                answer['from'] = ''
            else:
                answer['from'] = from_site[0][5:]
        else:
            answer['from'] = from_site[0][4:]

    @staticmethod
    def find_ip(line, answer):
        ip = re.findall("[^ ]*", line)
        answer['ip'] = ip[0]

    @staticmethod
    def find_date(line, answer):
        date = re.findall('\[.*\]', line)
        answer['date'] = date[0][1:-1]

    @staticmethod
    def find_code_and_bytes(line, answer):
        numbers = re.findall('\d+ \d+', line)
        answer['code'] = re.findall('\d+ ', numbers[0])[0][:-1]
        answer['bytes'] = re.findall(' \d+', numbers[0])[0][1:]

    @staticmethod
    def find_where(line, answer):
        where = re.findall('http:[^"]*', line)
        if where.__len__() == 0:
            answer['page'] = ''
        else:
            answer['page'] = where[0]

    @staticmethod
    def find_time(line, answer):
        time = re.findall('[1-9]*$', line)
        answer['time'] = time[0]

    @staticmethod
    def find_browser(line, answer):
        browser = re.findall('"[^"]*" [0-9]*$', line)
        if browser.__len__() == 0:
            answer['browser'] = ''
        else:
            browser = browser[0]
            browser = re.findall('".*"', browser)
            answer['browser'] = browser[0][1:-1]


def parse_line(global_statistic, log_line):
    answer = {}
    parser = Parser
    parser.find_ip(log_line, answer)
    parser.find_code_and_bytes(log_line, answer)
    parser.find_date(log_line, answer)
    parser.find_from(log_line, answer)
    parser.find_where(log_line, answer)
    parser.find_time(log_line, answer)
    parser.find_browser(log_line, answer)
    global_statistic.add_statistic(global_statistic, answer)


def main():
    file_name = sys.argv[1]
    #file_name = 'log.log'
    statistic = Statistic
    with open(file_name) as logs:
        for line in logs:
            parse_line(statistic, line)

    def f(x):
        return x[1]

    print("Slowest page: ", statistic.page_info.slow_page)
    print("Fastest page: ", statistic.page_info.quick_page)
    print("Slowest average page: ", sorted(statistic.page_info.slowest_average_page.items(), key=f)[
        statistic.page_info.slowest_average_page.__len__() - 1][0])
    print("More popular page: ", sorted(statistic.page_info.most_visited_page.items(), key=f)[
        statistic.page_info.most_visited_page.__len__() - 1][0])
    print("More active client: ", sorted(statistic.client_info.more_active_client.items(), key=f)[
        statistic.client_info.more_active_client.__len__() - 1][0])
    print("More popular browser: ", sorted(statistic.browser_info.more_active_browser.items(), key=f)[
        statistic.browser_info.more_active_browser.__len__() - 1][0])
    for day in statistic.client_info.days:
        print("More popular client in ", day, ": ",
              sorted(statistic.client_info.days[day].items(), key=f)[statistic.client_info.days[day].__len__() - 1][0])


main()