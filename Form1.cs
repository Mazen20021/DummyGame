using System;
using System.Drawing;
using System.Reflection.Emit;
using System.Windows.Forms;

namespace HittingObject
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        bool goingRight, goingLeft, isJumbing, isGameOver;
        int jumbSpeed;
        int force;
        int score = 0;
        int playerHealth = 100;
        int playerSpeed = 7;
        int playerLevel = 0;
        int HorezontalSpeed = 3;
        int VerticalSpeed = 3;
        int playerHealingSpeed = 1;
        bool intialMovement = false;
        bool reached1 = false;
        bool reached2 = false;
        bool apeared = false;
        bool gotSuperJumb = false;
        bool increasedHealth = false;
        bool increasedSpeed = false;
        bool increasedJumb = false;
        int increasedHealthCounter = 0;
        public Form1() { InitializeComponent(); }
        private void MainGameEvent(object sender, EventArgs e)
        {
            scoreNum.Text = score.ToString();

            Player.Top += jumbSpeed;


            if (!horezontalPlatForm1.Bounds.IntersectsWith(wall6.Bounds) && !reached1)
            {
                horezontalPlatForm1.Left += HorezontalSpeed;
                if (horezontalPlatForm1.Bounds.IntersectsWith(wall6.Bounds))
                {
                    reached1 = true;
                }
            }
            else if (!horezontalPlatForm1.Bounds.IntersectsWith(wall4.Bounds) && reached1)
            {

                horezontalPlatForm1.Left -= HorezontalSpeed;
                if (horezontalPlatForm1.Bounds.IntersectsWith(wall4.Bounds))
                {
                    reached1 = false;
                }
            }
            if (!horezontalWall2.Bounds.IntersectsWith(wall2.Bounds) && !reached2)
            {
                horezontalWall2.Left += HorezontalSpeed;
                if (horezontalWall2.Bounds.IntersectsWith(wall2.Bounds))
                {
                    reached2 = true;
                }
            }

            else if (!horezontalWall2.Bounds.IntersectsWith(wall1.Bounds) && reached2)
            {

                horezontalWall2.Left -= HorezontalSpeed;
                if (horezontalWall2.Bounds.IntersectsWith(wall1.Bounds))
                {
                    reached2 = false;
                }
            }

            if (goingLeft == true)
            {
                Player.Left -= playerSpeed;
            }
            if (goingRight == true)
            {
                Player.Left += playerSpeed;
            }
            if (isJumbing == true && force < 0)
            {
                isJumbing = false;
            }
            if (isJumbing == true)
            {
                jumbSpeed = -8;
                force -= 1;
            }
            else
            {
                jumbSpeed = 10;
            }

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox)
                {
                    if ((string)x.Tag == "Perks")
                    {
                        if (score % 5 == 0 && playerLevel != 0 && !apeared)
                        {
                            gameTimer.Stop();
                            increaseHealth.Visible = true;
                            increaseHealth.Enabled = true;
                            increaseSpeed.Visible = true;
                            increaseSpeed.Enabled = true;
                            increaseStrength.Visible = true;
                            increaseStrength.Enabled = true;
                            apeared = true;
                        }
                    }
                    if ((string)x.Tag == "Items")
                    {
                        if(Player.Bounds.IntersectsWith(x.Bounds))
                        {
                            x.Visible = false;
                            if((string)x.Name == "superJumb")
                            {
                                gotSuperJumb = true;
                            }
                        }
                    }
                    if ((string)x.Tag == "PlateForm")
                    {

                        if (Player.Bounds.IntersectsWith(x.Bounds))
                        {
                            if(!increasedJumb)
                            {
                                force = 8;
                            }
                            else
                            {
                                force = 16;
                            }
                                Player.Top = x.Top - Player.Height;
                            if ((string)x.Name == "horezontalPlatForm1" && goingLeft == false || (string)x.Name == "horezontalPlatForm1" && goingRight == false)
                            {
                                if (reached1 == false)
                                {
                                    Player.Left += HorezontalSpeed;
                                }
                                else
                                {
                                    Player.Left -= HorezontalSpeed;
                                }

                            }
                            if ((string)x.Name == "horezontalWall2" && goingLeft == false || (string)x.Name == "horezontalWall2" && goingRight == false)
                            {
                                if (reached2 == false)
                                {
                                    Player.Left += HorezontalSpeed;
                                }
                                else
                                {
                                    Player.Left -= HorezontalSpeed;
                                }

                            }

                        }


                        x.BringToFront();
                    }

                    if ((string)x.Tag == "Lava")
                    {
                        if (Player.Bounds.IntersectsWith(x.Bounds))
                        {
                            if(!increasedJumb)
                            {
                                force -= 4;
                            }
                            else
                            {
                                force -= 2;
                            }
                            Player.Top = x.Top - Player.Height;
                            playerHealth -= 10;
                            playerHealthNum.Text = playerHealth.ToString();
                            if (playerHealth <= 0)
                            {
                                playerHealth = 0;
                                playerHealthNum.Text = playerHealth.ToString();
                                isGameOver = true;
                            }
                        }
                    }
                    if ((string)x.Tag == "Coin")
                    {
                        if (Player.Bounds.IntersectsWith(x.Bounds) && x.Visible == true)
                        {
                            x.Visible = false;

                            score++;
                            scoreNum.Text = score.ToString();

                            if (score % 5 == 0)
                            {
                                playerLevel++;
                                levelNum.Text = playerLevel.ToString();
                            }
                        }
                    }

                    if (isGameOver == true)
                    {
                        gameTimer.Stop();
                        var result = MessageBox.Show("You are dead try again ?", "Game Over", MessageBoxButtons.OK);
                        if (result == DialogResult.OK)
                            RestartGame();

                    }
                }
            }
            if (playerHealth < 100)
            {
                if(!increasedHealth)
                {
                    playerHealth += playerHealingSpeed;
                    playerHealthNum.Text = playerHealth.ToString();
                    if (playerHealth == 100)
                    {
                        playerHealth = 100;
                        playerHealthNum.Text = playerHealth.ToString();
                    }
                }
                else
                {
                    int currentHealth = increasedHealthCounter * 10;
                    playerHealth += playerHealingSpeed;
                    playerHealthNum.Text = playerHealth.ToString();
                    if (playerHealth == 100 + currentHealth)
                    {
                        playerHealth = 100 + currentHealth;
                        playerHealthNum.Text = playerHealth.ToString();
                    }
                }
                
            }
        }

        private void DownPressed(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                goingRight = true;
            }
            if (e.KeyCode == Keys.Left)
            {
                goingLeft = true;
            }
            if (e.KeyCode == Keys.Space && isJumbing == false)
            {
                isJumbing = true;
            }
        }

        private void UpPressed(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                goingRight = false;
            }
            if (e.KeyCode == Keys.Left)
            {
                goingLeft = false;
            }
            if (isJumbing == true)
            {
                isJumbing = false;
            }
            if (e.KeyCode == Keys.Enter && isGameOver == true)
            {
                RestartGame();
            }
        }

        private void RestartGame()
        {
            playerHealth = 100;
            playerHealthNum.Text = playerHealth.ToString();
            levelNum.Text = "0";
            scoreNum.Text = "0";
            increasedHealthCounter = 0;
            goingLeft = false;
            goingRight = false;
            isJumbing = false;
            isGameOver = false;

            score = 0;
            force = 0;

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && x.Visible == false)
                {
                    x.Visible = true;
                }
            }

            Player.Left = 9;
            Player.Top = 565;
            horezontalPlatForm1.Left = 814;
            horezontalPlatForm1.Top = 543;

            gameTimer.Start();
        }


        private void increaseHealth_Click(object sender, EventArgs e)
        {
            increasedHealthCounter++;
            increasedHealth = true;
            increaseHealth.Visible = false;
            increaseHealth.Enabled = false;
            increaseSpeed.Visible = false;
            increaseSpeed.Enabled = false;
            increaseStrength.Visible = false;
            increaseStrength.Enabled = false;
            if (playerHealth < 100)
            {
                playerHealth = 100;
            }
            playerHealth += 10;
            playerHealthNum.Text = playerHealth.ToString();
            score++;
            apeared = false;
            gameTimer.Start();
        }

        private void increaseSpeed_Click(object sender, EventArgs e)
        {
            increasedSpeed = true;
            increaseHealth.Visible = false;
            increaseHealth.Enabled = false;
            increaseSpeed.Visible = false;
            increaseSpeed.Enabled = false;
            increaseStrength.Visible = false;
            increaseStrength.Enabled = false;
            playerSpeed += 1;
            score++;
            apeared = false;
            gameTimer.Start();
        }
    }
}
