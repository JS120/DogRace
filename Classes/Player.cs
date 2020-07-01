using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DogGame.Common
{
    public abstract class Player
    {
        // class created with these function , which we can use in main class with writing again 
        public int Cash;

        public bool Busted;

        public Bet MyBet;

        public Label MyLabel;

        public RadioButton MyRadioButton;

        public string Name;

        public bool Winner;

        public TextBox MyText;
    }
}
