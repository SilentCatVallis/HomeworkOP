import unittest
import array_dict
import sorted_array_dict
import hash_dict
import tree_dict
import treap_dict
import main
import random


class TestDicts(unittest.TestCase):
    def test_1(self):
        tester = main.Tester()
        for i in range(100):
            tester.add_to_all(i, i)
        for i in range(0, 100):
            assert tester.tree_dict.get(i) == i
            assert tester.sorted_array_dict.get(i) == i
            assert tester.array_dict.get(i) == i
            assert tester.hash_dict.get(i) == i
            assert tester.treap_dict.get(i) == i

    def test_2(self):
        tester = main.Tester()
        for i in range(100):
            tester.add_to_all(str(i), i)
        for i in range(0, 100):
            assert tester.tree_dict.get(str(i)) == i
            assert tester.sorted_array_dict.get(str(i)) == i
            assert tester.array_dict.get(str(i)) == i
            assert tester.hash_dict.get(str(i)) == i
            assert tester.treap_dict.get(str(i)) == i

    def test_3(self):
        tester = main.Tester()
        for i in range(100):
            tester.add_to_all(str(i), i)
        for i in range(0, 100):
            tester.remove_from_all(str(i))
        for i in range(0, 100):
            assert tester.tree_dict.get(str(i)) is None
            assert tester.sorted_array_dict.get(str(i)) is None
            assert tester.array_dict.get(str(i)) is None
            assert tester.hash_dict.get(str(i)) is None
            assert tester.treap_dict.get(str(i)) is None

    def test_4(self):
        tester = main.Tester()
        array = []
        for i in range(1000):
            rnd = str(random.randint(0, 1e9))
            array.append(rnd)
            tester.add_to_all(rnd, rnd)
        for i in range(0, 1000):
            if i % 2 == 0:
                continue
            tester.remove_from_all(array[i])
        for i in range(0, 1000):
            if i % 2 == 0:
                continue
            assert tester.tree_dict.get(array[i]) is None
            assert tester.sorted_array_dict.get(array[i]) is None
            assert tester.array_dict.get(array[i]) is None
            assert tester.hash_dict.get(array[i]) is None
            assert tester.treap_dict.get(array[i]) is None

    def test_5(self):
        tester = main.Tester()
        array = []
        for i in range(1000):
            rnd = str(random.randint(0, 1e9))
            array.append(rnd)
            tester.add_to_all(rnd, rnd)
        for i in range(0, 1000):
            if i % 2 == 0:
                continue
            tester.remove_from_all(array[i])
        for i in range(0, 1000):
            if i % 2 == 1:
                continue
            assert tester.tree_dict.get(array[i]) == array[i]
            assert tester.sorted_array_dict.get(array[i]) == array[i]
            assert tester.array_dict.get(array[i]) == array[i]
            assert tester.hash_dict.get(array[i]) == array[i]
            assert tester.treap_dict.get(array[i]) == array[i]

    def test_6(self):
        tester = main.Tester()
        array = []
        for i in range(300):
            rnd = str(random.randint(0, 1e9))
            array.append(rnd)
            tester.add_to_all(rnd, int(rnd))
        for i in range(0, 300):
            if i % 2 == 0:
                continue
            tester.remove_from_all(array[i])
        for i in range(0, 300):
            if i % 2 == 0:
                continue
            assert tester.tree_dict.get(array[i]) is None
            assert tester.sorted_array_dict.get(array[i]) is None
            assert tester.array_dict.get(array[i]) is None
            assert tester.hash_dict.get(array[i]) is None
            assert tester.treap_dict.get(array[i]) is None

    def test_7(self):
        tester = main.Tester()
        array = []
        for i in range(300):
            rnd = str(random.randint(0, 1e9))
            array.append(rnd)
            tester.add_to_all(rnd, int(rnd))
        for i in range(0, 300):
            if i % 2 == 0:
                continue
            tester.remove_from_all(array[i])
        for i in range(0, 300):
            if i % 2 == 1:
                continue
            assert tester.tree_dict.get(array[i]) == int(array[i])
            assert tester.sorted_array_dict.get(array[i]) == int(array[i])
            assert tester.array_dict.get(array[i]) == int(array[i])
            assert tester.hash_dict.get(array[i]) == int(array[i])
            assert tester.treap_dict.get(array[i]) == int(array[i])

    def test_very_big(self):
        tester = main.Tester()
        array = []
        for i in range(10000):
            rnd = str(random.randint(0, 1e9))
            array.append(rnd)
            tester.add_to_all(rnd, rnd)
        for i in range(0, 10000):
            if i % 2 == 0:
                continue
            tester.remove_from_all(array[i])
        for i in range(0, 10000):
            if i % 2 == 1:
                assert tester.tree_dict.get(array[i]) is None
                assert tester.sorted_array_dict.get(array[i]) is None
                assert tester.array_dict.get(array[i]) is None
                assert tester.hash_dict.get(array[i]) is None
                assert tester.treap_dict.get(array[i]) is None
                continue
            assert tester.tree_dict.get(array[i]) == array[i]
            assert tester.sorted_array_dict.get(array[i]) == array[i]
            assert tester.array_dict.get(array[i]) == array[i]
            assert tester.hash_dict.get(array[i]) == array[i]
            assert tester.treap_dict.get(array[i]) == array[i]