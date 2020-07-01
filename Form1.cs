using DogGame.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DogGame
{
    public partial class Form1 : Form
    {
        //classes called here
        Greyhound[] dogs= new Greyhound[4];
        Player[] player = new Player[3];
        Greyhound winnerdog;
        int stop;
        //timer set here
        Timer[] timers = new Timer[4];
        public Form1()
        {
            InitializeComponent();
            PrepareInitialData();
            SetBet();
            //enabling the buttons
            btnGameOver.Enabled = false;
            btnStartRace.Enabled = false;
        }

        private void PrepareInitialData()
        {
            //players and dogs are ready on track to race and bet
            dogs[0] = new Greyhound() { dogname = "dog 1", RaceTrackLength = 970, MyPictureBox = picturedog1 };
            dogs[1] = new Greyhound() { dogname = "dog 2", RaceTrackLength = 970, MyPictureBox = picturedog2 };
            dogs[2] = new Greyhound() { dogname = "dog 3", RaceTrackLength = 970, MyPictureBox = picturedog3 };
            dogs[3] = new Greyhound() { dogname = "dog 4", RaceTrackLength = 970, MyPictureBox = picturedog4 };
            player[0] = Balance.GetAPlayer(1);
            player[1] = Balance.GetAPlayer(2);
            player[2] = Balance.GetAPlayer(3);
            player[0].MyLabel = lblBets;
            player[0].MyRadioButton = radioPlayer1;
            player[0].MyText = txtPlayer1;
            player[1].MyLabel = lblBets;
            player[1].MyRadioButton = radioPlayer2;
            player[1].MyText = txtPlayer2;
            player[2].MyLabel = lblBets;
            player[2].MyRadioButton = radioPlayer3;
            player[2].MyText = txtPlayer3;
            player[0].MyRadioButton.Text = player[0].Name;
            player[1].MyRadioButton.Text = player[1].Name;
            player[2].MyRadioButton.Text = player[2].Name;
            numericdogNumber.Minimum = 1;
            numericdogNumber.Maximum = 4;
            numericdogNumber.Value = 1;
        }
       

        private void SetBet()
        {

            // setting the bet limits and text to be display on main form
            foreach(Player player in player )
            {
                if (player.Busted)
                {
                    player.MyText.Text = "BUSTED";
                }
                else
                {
                    if (player.MyBet == null)
                    {
                        player.MyText.Text = player.Name + " Please place a bet";
                    }
                    else
                    {
                        player.MyText.Text = player.Name + " bets $" + player.MyBet.Amount + " on " + player.MyBet.dog.dogname;
                    }
                    if (player.MyRadioButton.Checked)
                    {
                        lblMaxBet.Text = "Max Bet is $" + player.Cash.ToString();
                        btnPlaceBet.Text = "Place Bet for " + player.Name;
                        numericBetAmount.Minimum = 1;
                        numericBetAmount.Maximum = player.Cash;
                        numericBetAmount.Value = 1;
                    }
                }
            }
        }

      
        private void radioplayer_CheckedChanged(object sender, EventArgs e)
        {
            SetBet();
        }

        private void btnPlaceBet_Click(object sender, EventArgs e)
        {
            //activating the place bet button for the players to bet
            int count = 0;
            int total_active = 0;
            foreach(Player players in player)
            {
                if(!players.Busted)
                {
                    total_active++;
                }
                if(players.MyRadioButton.Checked)
                {
                    if( players.MyBet == null )
                    {
                        int number = (int)numericdogNumber.Value;
                        int amount = (int)numericBetAmount.Value;
                        bool alreadyPlaced = false;
                        foreach(Player ply in player)
                        {
                            if( ply.MyBet != null && ply.MyBet.dog == dogs[number-1])
                            {
                                alreadyPlaced = true;
                                break;
                            }
                        }
                        if (alreadyPlaced)
                        {
                            MessageBox.Show("Number is Already Taken. Try With Different dog Number");
                        }
                        else
                        {
                            players.MyBet = new Bet() { Amount = amount, dog = dogs[number - 1] };
                        }
                        
                    }
                    else
                    {
                        MessageBox.Show("You Already Bet for " + players.Name);
                    }
                }
                if (players.MyBet != null)
                {
                    count++;
                }
            }
            if( count == total_active)
            {
                btnPlaceBet.Enabled = false;
                btnStartRace.Enabled = true;
            }
            SetBet();
        }

        private void btnStartRace_Click(object sender, EventArgs e)
        {
            // when we start the race timer will be enabled
            for(int index = 0; index < timers.Length; index++)
            {
                timers[index] = new Timer();
                timers[index].Interval = 15;
                timers[index].Tick += Timer_Tick;
                timers[index].Start();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // timer length set  and will display the winner dog 
            if (sender is Timer)
            {
                Timer timer = sender as Timer;
                int index = 0;
                while( index < timers.Length)
                {
                    if(timers[index] == timer)
                    {
                        break;
                    }
                    index++;
                }
                PictureBox picture = dogs[index].MyPictureBox;
                if(picture.Location.X + picture.Width > dogs[index].RaceTrackLength)
                {
                    timer.Stop();
                    stop++;
                    if(winnerdog==null)
                    {
                        winnerdog = dogs[index];
                    }                    
                }
                else
                {
                    int jump = new Random().Next(1,15);
                    picture.Location = new Point(picture.Location.X + jump, picture.Location.Y);
                }
                
            }
            if (stop == timers.Length)
            {
                // message for the dog who came first
                MessageBox.Show(winnerdog.dogname + " Finished First!!!");
                SetBet();
                foreach (Player player in player)
                {
                    if (player.MyBet != null)
                    {
                        if (player.MyBet.dog == winnerdog)
                        {
                            player.Cash += player.MyBet.Amount;
                            // player amount after winning
                            player.MyText.Text = player.Name + "You Won and total comes to $" + player.Cash;
                            player.Winner = true;

                        }
                        else
                        {
                            player.Cash -= player.MyBet.Amount;
                            if( player.Cash == 0 )
                            {
                                player.MyText.Text = "BUSTED";
                                player.Busted = true;
                                player.MyRadioButton.Enabled = false;
                            }
                            else
                            {
                                // player amount deducation when lost
                                player.MyText.Text = player.Name + " You Lost and remaning with $" + player.Cash;
                            }                            
                        }
                    }
                }
                winnerdog = null;
                stop = 0;
                timers = new Timer[4];
                btnPlaceBet.Enabled = true;
                btnStartRace.Enabled = false;
                int count = 0;
                foreach(Player player in player)
                {
                    if (player.Busted)
                    {
                        count++;
                    }
                    if (player.MyRadioButton.Enabled && player.MyRadioButton.Checked)
                    {
                        //enabling the max bet button to it maxlimit
                        lblMaxBet.Text = "Max Bet is $" + player.Cash;
                    }
                    player.MyBet = null;
                    player.Winner = false;
                }
                if(count==player.Length)
                {
                    btnPlaceBet.Enabled = false;
                    btnGameOver.Enabled = true;
                }
                foreach(Greyhound dog in dogs)
                {
                    dog.MyPictureBox.Location = new Point(1,dog.MyPictureBox.Location.Y);
                }
            }
        }

        private void btnGameOver_Click(object sender, EventArgs e)
        {
            // exit button message display
            MessageBox.Show("Bye have a good day");
            Application.Exit();
        }

        private void lblMaxBet_Click(object sender, EventArgs e)
        {

        }

        private void panelRadio_Paint(object sender, PaintEventArgs e)
        {

        }

        private void radioPlayer3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void pictureTrack1_Click(object sender, EventArgs e)
        {

        }
    }
}
