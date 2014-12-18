class SortedArrayDict:
    def __init__(self):
        self.array = []

    def add(self, key, value):
        local_array = []
        is_put = False
        for i in range(len(self.array)):
            if key <= self.array[i][0] and not is_put:
                local_array.append((key, value))
                is_put = True
            local_array.append(self.array[i])
        if not is_put:
            local_array.append((key, value))
        self.array = local_array

    def remove(self, key):
        index = self.get_index_of_element(key)
        local_array = []
        for i in range(len(self.array)):
            if i == index:
                continue
            local_array.append(self.array[i])
        self.array = local_array

    def get(self, key):
        index = self.get_index_of_element(key)
        if index is None:
            return None
        return self.array[index][1]

    def get_index_of_element(self, key):
        l = 0
        r = len(self.array) - 1
        while l < r:
            mid = (l + r) // 2
            if self.array[mid][0] == key:
                return mid
            if self.array[l][0] == key:
                return l
            if self.array[r][0] == key:
                return r
            if self.array[mid][0] >= key:
                r = mid
            else:
                l = mid + 1
        return None