using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_alpha
{
    public class Bridge
    {
        public Bridge(Point point)
        {
            Location = point;
            //Number = number;
        }
        public Point Location;
        public bool IsOpen = false;
       // private int Number;

        public void SwitchState()
        {
            IsOpen = !IsOpen;
        }
    }
}
