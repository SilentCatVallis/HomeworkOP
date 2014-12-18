class Node:
    def __init__(self, key, value):
        self.key = key
        self.value = value
        self.left = None
        self.right = None
        self.parent = None


class TreeDict:
    def __init__(self):
        self.head = None

    def add(self, key, value):
        if self.head is None:
            self.head = Node(key, value)
            return
        cur_node = self.head
        while True:
            if cur_node.key == key:
                cur_node.value = value
                return
            if cur_node.key > key:
                if cur_node.left is None:
                    cur_node.left = Node(key, value)
                    cur_node.left.parent = cur_node
                    return
                cur_node = cur_node.left
            else:
                if cur_node.right is None:
                    cur_node.right = Node(key, value)
                    cur_node.right.parent = cur_node
                    return
                cur_node = cur_node.right

    def get(self, key):
        node = self.find_node(key)
        if node is None:
            return None
        return node.value

    def find_node(self, key):
        local_node = self.head
        while True:
            if local_node is None:
                return
            if local_node.key == key:
                return local_node
            if local_node.key > key:
                if local_node.left is None:
                    return None
                local_node = local_node.left
            else:
                if local_node.right is None:
                    return None
                local_node = local_node.right

    def compare_to(self, x, y):
        if x < y:
            return -1
        elif x == y:
            return 0
        else:
            return 1

    def remove_right(self, current, parent):
        if current == self.head:
            self.head = current.left
        else:
            result = self.compare_to(current.key, parent.key)
            if result < 0:
                parent.left = current.left
            else:
                parent.right = current.left

    def remove_right_left(self, current, parent):
        current.right.left = current.left
        if current == self.head:
            self.head = current.right
        else:
            result = self.compare_to(current.key, parent.key)
            if result < 0:
                parent.left = current.right
            else:
                parent.right = current.right

    def remove_left(self, current, parent):
        minimum = current.right.left
        prev = current.right
        while minimum.left is not None:
            prev = minimum
            minimum = minimum.left
        prev.left = minimum.right
        minimum.left = current.left
        minimum.right = current.right
        if current == self.head:
            self.head = minimum
        else:
            result = self.compare_to(current.key, parent.key)
            if result < 0:
                parent.left = minimum
            else:
                parent.right = minimum

    def remove(self, item):
        if self.head is None:
            return

        current = self.head
        parent = None

        result = self.compare_to(item, current.key)
        while result != 0:
            if result < 0:
                parent = current
                current = current.left
            elif result > 0:
                parent = current
                current = current.right
            if current is None:
                return
            result = self.compare_to(item, current.key)

        if current.right is None:
            self.remove_right(current, parent)
        elif current.right.left is None:
            self.remove_right_left(current, parent)
        else:
            self.remove_left(current, parent)