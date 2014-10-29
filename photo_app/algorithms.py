import ImSim
import landscaper
import Gamma
import Hash
import fingers

_algorithms = {"ImSim": ImSim, "landscaper": landscaper, "Gamma": Gamma, "Hash": Hash, "Fingers": fingers}


def get_all_algo():
    for name in _algorithms:
        yield name


def compare_pictures(algorithm, first, second, matching_percent):
    if algorithm == "ImSim":
        return _algorithms[algorithm].compare_pictures(first, second)[1] < matching_percent
    else:
        ans = _algorithms[algorithm].compare_pictures(first, second)
        #print(first[-8:], second[-8:], ans)
        return ans < matching_percent