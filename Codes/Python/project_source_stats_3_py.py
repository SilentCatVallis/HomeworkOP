import os
import sys
import itertools

def project_stats(path, extensions):
    """
    Вернуть число строк в исходниках проекта.
    
    Файлами, входящими в проект, считаются все файлы
    в папке ``path`` (и подпапках), имеющие расширение
    из множества ``extensions``.
    """

    return total_number_of_lines(with_extensions(extensions, iter_filenames(path)))

    """ а если без итераторов, то можно и так:
    answer = 0
    folders = os.walk(path)
    for folder, anotherFolders, files in folders:
        for extension in extensions:
            for file in filter(lambda x: x.endswith(extension), files):
                answer += len(open(str(folder) + '\\' + file).readlines())
    return answer
    """


def total_number_of_lines(filenames):
    answer = 0
    for file in filenames:
        answer += number_of_lines(file)
    return answer
    
def number_of_lines(filename):
    count = 0
    with open(filename) as file:
        for line in file:
            count += 1
    return count

def iter_filenames(path):
    folders = os.walk(path)
    for folder, anotherFolders, files in folders:
        for file in files:
            yield str(folder) + '\\' + file

def with_extensions(extensions, filenames):
    for filename in filenames:
        if get_extension(filename) in extensions:
            yield filename

def get_extension(filename):
    return os.path.splitext(filename)[1]


def print_usage():
    print("Usage: python project_sourse_stats_3.py <project_path>")


if __name__ == '__main__':
    if len(sys.argv) != 2:
        print_usage()
        sys.exit(1)

    project_path = sys.argv[1]
    print(project_stats(project_path, {'.py'}))