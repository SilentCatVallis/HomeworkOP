import array_dict
import hash_dict
import sorted_array_dict
import tree_dict
import treap_dict
import random
import time

class Profiler(object):
    def __init__(self, results, dict_name):
        self.results = results
        self.dict_name = dict_name

    def __enter__(self):
        self._startTime = time.time()

    def __exit__(self, type, value, traceback):
        self.results.append((self.dict_name, "{:.3f}".format(time.time() - self._startTime)))


class Tester:
    def __init__(self):
        self.array_dict = array_dict.ArrayDict()
        self.hash_dict = hash_dict.HashDict()
        self.sorted_array_dict = sorted_array_dict.SortedArrayDict()
        self.tree_dict = tree_dict.TreeDict()
        self.treap_dict = treap_dict.Treap()
        self.default_dict = {}
        self.dicts = []
        self.dicts.append(("array_dict", self.array_dict))
        self.dicts.append(("hash_dict", self.hash_dict))
        self.dicts.append(("sorted_array_dict", self.sorted_array_dict))
        self.dicts.append(("tree_dict", self.tree_dict))
        self.dicts.append(("treap_dict", self.treap_dict))

    def add_to_all(self, key, value):
        self.tree_dict.add(key, value)
        self.sorted_array_dict.add(key, value)
        self.array_dict.add(key, value)
        self.hash_dict.add(key, value)
        self.treap_dict.add(key, value)

    def remove_from_all(self, key):
        self.sorted_array_dict.remove(key)
        self.array_dict.remove(key)
        self.hash_dict.remove(key)
        self.tree_dict.remove(key)
        self.treap_dict.remove(key)

    def start_tests(self):
        global_results = []
        for exp in range(1, 4):
            data = []
            for pair in self.test_generator(10 ** exp):
                data.append(pair)

            results_add_function = []
            for local_dict in self.dicts:
                self.test(data, local_dict[1].add, results_add_function, local_dict[0])

            global_results.append((10 ** exp, results_add_function))

    def test(self, data, func, results, dict_name):
        with Profiler(results, dict_name) as p:
            for local_data in data:
                func(local_data[0], local_data[1])

    def test_generator(self, size):
        for _ in range(size):
            yield random.randint(0, 1e9), random.randint(0, 1e9)


if __name__ == "__main__":
    tester = Tester()
    tester.start_tests()





