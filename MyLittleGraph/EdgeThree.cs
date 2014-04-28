using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLittleGraph
{
    public class EdgeThree
    {
        private readonly int[] _three;
        private readonly int _nodeCount;
        private readonly int _extraDifferense;
        private readonly int _power;

        public EdgeThree(int nodeCount)
        {
            _three = new int[4 * nodeCount];
            _nodeCount = nodeCount;
            _power = (int) Math.Ceiling(Math.Log(nodeCount)/Math.Log(2));
            _extraDifferense = (int)Math.Pow(2, _power);
        }
        
        public void AddEdge(int first, int second)
        {
            AddNodeLocal(first);
            AddNodeLocal(second);
        }

        private void AddNodeLocal(int localNode)
        {
            var index = localNode + _extraDifferense;
            _three[index]++;
            while (index > 0)
            {
                index /= 2;
                _three[index] ++;
            }
        }

        public int GetIndexForRandomNumber(int random)
        {
            var power = 0;
            var pointer = 1;
            while (power != _power)
            {
                if (random <= _three[pointer*2])
                {
                    pointer = pointer*2;
                }
                else
                {
                    random -= _three[pointer * 2];
                    pointer = pointer * 2 + 1;
                }
                power++;
            }
            return pointer - _extraDifferense;
        }

        public int GetTop()
        {
            return _three[1];
        }
    }
}
