using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogGame.Common
{
    public static class Balance
    {
        public static Player GetAPlayer(int num)
        {
            // setting the amount for each player with their names you can change amount from here 
            if( num == 1)
            {
                return new Lee() { Name = "Lee", Cash = 100 };
            }
            else if( num == 2 )
            {
                return new Den() { Name = "Den", Cash = 100 };
            }
            else
            {
                return new ALI() { Name = "ALI", Cash = 100 };
            }
        }
    }
}
