using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broos_Gerrit_PRG282_prj
{
    class Obstacle
    {
        int index;
        public int height { get; set; }
        public Image img { get; set; }

        public Obstacle(int _height, Image _img, int _index)
        {
            height = _height;
            img = _img;
            index = _index;
        }
    }
}
