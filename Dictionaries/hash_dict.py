class HashDict:
    def __init__(self):
        self.size = 64
        self.local_size = 0
        self.array = []
        for i in range(self.size):
            self.array.append([])

    def add(self, key, value):
        local_hash = hash(key) % self.size
        for i in range(len(self.array[local_hash])):
            if self.array[local_hash][i][0] == key:
                self.array[local_hash][i][1] = value
                return
        self.array[local_hash].append((key, value))
        self.local_size += 1
        if self.local_size >= self.size:
            self.reform_dictionary()

    def reform_dictionary(self):
        local_array = []
        for i in range(self.size * 2):
            local_array.append([])
        for i in range(self.size):
            for j in range(len(self.array[i])):
                local_hash = hash(self.array[i][j][0]) % (self.size * 2)
                local_array[local_hash].append((self.array[i][j][0], self.array[i][j][1]))
        self.size *= 2
        self.array = local_array

    def remove(self, key):
        local_hash = hash(key) % self.size
        local_array = []
        for i in range(len(self.array[local_hash])):
            if self.array[local_hash][i][0] == key:
                continue
            local_array.append(self.array[local_hash][i])
        self.array[local_hash] = local_array

    def get(self, key):
        local_hash = hash(key) % self.size
        for i in range(len(self.array[local_hash])):
            if self.array[local_hash][i][0] == key:
                return self.array[local_hash][i][1]
        return None
