class ArrayDict:
    def __init__(self):
        self.array = []

    def add(self, key, value):
        for i in range(len(self.array)):
            if self.array[i][0] == key:
                self.array[i][1] = value
                return
        self.array.append((key, value))

    def remove(self, key):
        local_array = []
        for i in range(len(self.array)):
            if self.array[i][0] == key:
                continue
            local_array.append(self.array[i])
        self.array = local_array

    def get(self, key):
        for i in range(len(self.array)):
            if self.array[i][0] == key:
                return self.array[i][1]
        return None
