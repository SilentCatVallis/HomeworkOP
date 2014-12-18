import random

class Item:
    def __init__(self, key, priority, value, left=None, right=None):
        self.key = key
        self.priority = priority
        self.value = value
        self.left = left
        self.right = right
        self.parent = None

    def GetKey(self):
        return self.key

    def GetPriority(self):
        return self.priority

    def getValue(self):
        return self.value

    def GetLeftMap(self):
        return self.left

    def GetRightMap(self):
        return self.right

    def SetParent(self, parent):
        self.parent = parent


class Treap:
    def __init__(self, head=None):
        self.head = head
        self.size = 0

    def Merge(self, L, R):
        if L is None:
            return R
        if R is None:
            return L
        if L.GetPriority() > R.GetPriority():
            newR = self.Merge(L.GetRightMap(), R)
            answer = Item(L.GetKey(), L.GetPriority(), L.getValue(), L.GetLeftMap(), newR)
            if L.GetLeftMap() is not None:
                L.GetLeftMap().SetParent(answer)
            if newR is not None:
                newR.SetParent(answer)
            return answer
        else:
            newL = self.Merge(L, R.GetLeftMap())
            answer = Item(R.GetKey(), R.GetPriority(), R.getValue(), newL, R.GetRightMap())
            if newL is not None:
                newL.SetParent(answer)
            if R.GetRightMap() is not None:
                R.GetRightMap().SetParent(answer)
            return answer

    def Split(self, x, L, R, IsExceptionalCase):
        newTree = None
        if self.head is None:
            return (None, None)
        comparisonResult = self.compareTo(self.head.GetKey(), x)
        boolComparisonResult = False
        if IsExceptionalCase:
            boolComparisonResult = comparisonResult < 0
        else:
            boolComparisonResult = comparisonResult <= 0
        if boolComparisonResult:
            if self.head.GetRightMap() is None:
                R = None
            else:
                LR = Treap(self.head.GetRightMap()).Split(x, newTree, R, IsExceptionalCase)
                newTree = LR[0]
                R = LR[1]
            L = Item(self.head.GetKey(), self.head.GetPriority(), self.head.getValue(), self.head.GetLeftMap(), newTree)
            if self.head.GetLeftMap() is not None:
                self.head.GetLeftMap().SetParent(L)
            if newTree is not None:
                newTree.SetParent(L)
            return L, R
        else:
            if self.head.GetLeftMap() is None:
                L = None
            else:
                LR = Treap(self.head.GetLeftMap()).Split(x, L, newTree, IsExceptionalCase)
                L = LR[0]
                newTree = LR[1]
            R = Item(self.head.GetKey(), self.head.GetPriority(), self.head.getValue(), newTree, self.head.GetRightMap())
            if newTree is not None:
                newTree.SetParent(R)
            if self.head.GetRightMap() is not None:
                self.head.GetRightMap().SetParent(R)
            return L, R

    def get(self, key):
        element = self.findElement(key)
        if element is None:
            return None
        return element.getValue()

    def compareTo(self, x, y):
        if x < y:
            return -1
        elif x == y:
            return 0
        else:
            return 1

    def findElement(self, key):
        if self.head is None:
            return None
        comareResult = self.compareTo(key, self.head.GetKey())
        if comareResult == 0:
            return self.head
        elif comareResult > 0:
            if self.head.GetRightMap() is None:
                return None
            return Treap(self.head.GetRightMap()).findElement(key)
        else:
            if self.head.GetLeftMap() is None:
                return None
            return Treap(self.head.GetLeftMap()).findElement(key)

    def add(self, key, value):
        if self.head is None:
            self.head = Item(key, random.randint(0, 1e9), value)
            self.size += 1
            return None
        element = self.findElement(key)
        if element is not None:
            lastValue = element.getValue()
            element.setValue(value)
            return lastValue
        l = None
        r = None
        LR = self.Split(key, l, r, False)
        m = Item(key, random.randint(0, 1e9), value)
        self.head = self.Merge(self.Merge(LR[0], m), LR[1])
        self.size += 1
        return None

    def remove(self, key):
        element = self.findElement(key)
        if element is None:
            return None
        l = None
        m = None
        r = None
        LR1 = self.Split(key, l, r, True)
        l = LR1[0]
        r = LR1[1]
        LR2 = Treap(r).Split(key, m, r, False)
        m = LR2[0]
        r = LR2[1]
        self.head = self.Merge(l, r)
        self.size -= 1
        return m.getValue()
