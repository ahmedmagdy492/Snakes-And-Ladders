﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAndLadders.Services
{
    public class DiceRollerService
    {
        private int _currentValue = 0;

        public int GenRandNum()
        {
            Random random = new Random();
            _currentValue = random.Next(1, 7);
            GC.Collect();
            return _currentValue;
        }
    }
}
