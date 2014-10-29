import unittest
from field import *


class TestField(unittest.TestCase):
    def test_get_result(self):
        field = Field()
        result = field.get_result()
        assert result[0] == 2 and result[1] == 2

    def test_get_result2(self):
        field = Field()
        field.field[0][0] = 1
        result = field.get_result()
        assert result[0] == 2 and result[1] == 3

    def test_put(self):
        field = Field()
        field.put(3, 5, 1)
        result = field.get_result()
        assert result[0] == 1 and result[1] == 4

    def test_put2(self):
        field = Field()
        field.put(-1, -1, 1)
        result = field.get_result()
        assert result[0] == 2 and result[1] == 2 and not field.is_has_move

    def test_check_next_moves(self):
        field = Field()
        field.check_next_moves(1)
        assert field.is_has_move

    def test_check_next_moves2(self):
        field = Field()
        for i in range(8):
            for j in range(8):
                field.field[i][j] = 1
        field.check_next_moves(1)
        assert not field.is_has_move

    def test_check_next_moves3(self):
        field = Field()
        for i in range(8):
            for j in range(8):
                field.field[i][j] = 1
        field.check_next_moves(2)
        assert not field.is_has_move

    def test_change_color(self):
        field = Field()
        color = field.change_color(1)
        assert color == 2

    def test_change_color2(self):
        field = Field()
        color = field.change_color(2)
        assert color == 1

    def test_is_correct(self):
        field = Field()
        result = field.is_correct(3, 5, 1)
        assert result

    def test_is_correct2(self):
        field = Field()
        result = field.is_correct(1, 5, 1)
        assert not result

    def test_is_correct3(self):
        field = Field()
        result = field.is_correct(4, 4, 1)
        assert not result

    def test_is_correct4(self):
        field = Field()
        result = field.is_correct(3, 5, 2)
        assert not result

    def test_is_correct5(self):
        field = Field()
        result = field.is_correct(4, 5, 2)
        assert result

    def test_up_subs1(self):
        field = Field()
        result = field.up_subs(3, 3)
        assert len(result) == 3

    def test_up_subs2(self):
        field = Field()
        result = field.up_subs(5, 4)
        assert len(result) == 5

    def test_down_subs1(self):
        field = Field()
        result = field.down_subs(4, 3)
        assert len(result) == 3

    def test_down_subs2(self):
        field = Field()
        result = field.down_subs(5, 4)
        assert len(result) == 2

    def test_left_subs1(self):
        field = Field()
        result = field.left_subs(4, 3)
        assert len(result) == 3

    def test_left_subs2(self):
        field = Field()
        result = field.left_subs(5, 4)
        assert len(result) == 4

    def test_right_subs1(self):
        field = Field()
        result = field.right_subs(4, 3)
        assert len(result) == 4

    def test_right_subs2(self):
        field = Field()
        result = field.right_subs(5, 4)
        assert len(result) == 3

    def test_field_hash1(self):
        field = Field()
        result = field.get_field_hash(1)
        assert result == 0

    def test_field_hash2(self):
        field = Field()
        result = field.get_field_hash(2)
        assert result == 0

    def test_field_hash3(self):
        field = Field()
        field.field[2][2] = 1
        result = field.get_field_hash(2)
        assert result == -1

    def test_field_hash4(self):
        field = Field()
        field.field[2][2] = 1
        result = field.get_field_hash(1)
        assert result == 1

if __name__ == "__main__":
    unittest.main()