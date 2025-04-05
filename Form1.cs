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
        bool eKeyPressed = false;
        bool increasedArmoured = false;

        int increasedHealthCounter = 0;
        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED
                return cp;
            }
        }
        private void MainGameEvent(object sender, EventArgs e)
        {
            scoreNum.Text = score.ToString();

            Player.Top += jumbSpeed;

            if(increasedArmoured)
            {

            }
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
                    if ((string)x.Tag == "Borders")
                    {
                        if (Player.Bounds.IntersectsWith(x.Bounds) && (string)x.Name == "B1")
                        {
                            Player.Left = -2;
                        }
                        if (Player.Bounds.IntersectsWith(x.Bounds) && (string)x.Name == "B2")
                        {
                            Player.Left = 1331;
                        }
                    }
                    if ((string)x.Tag == "Ladder")
                    {
                        Player.BringToFront();
                        if (eKeyPressed && Player.Bounds.IntersectsWith(x.Bounds))
                        {
                            Player.Top = (int)x.Top + 10;

                        }
                    }
                    if ((string)x.Tag == "Perks")
                    {
                        if (score % 10 == 0 && playerLevel != 0 && !apeared)
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
                        if (Player.Bounds.IntersectsWith(x.Bounds))
                        {
                            x.Visible = false;
                            if ((string)x.Name == "superJumb")
                            {

                            }
                        }
                    }
                    if ((string)x.Tag == "ActionPlateForm" || (string)x.Name == "horezontalWall2" || (string)x.Name == "horezontalPlatForm1")
                    {

                        if (Player.Bounds.IntersectsWith(x.Bounds))
                        {
                            if (!increasedJumb)
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
                            if (!increasedJumb)
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

                            if (score % 10 == 0)
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
            if (e.KeyCode == Keys.E)
            {
                eKeyPressed = true;
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
            if (e.KeyCode == Keys.E)
            {
                eKeyPressed = false;
            }
        }

        private void RestartGame()
        {
            score = 0;
            increaseHealth.Visible = false;
            increaseHealth.Enabled = false;
            increaseSpeed.Visible = false;
            increaseSpeed.Enabled = false;
            increaseStrength.Visible = false;
            increaseStrength.Enabled = false;
            armourPerk.Enabled = false;
            armourPerk.Visible = false;

            playerHealth = 100;
            playerHealthNum.Text = playerHealth.ToString();
            levelNum.Text = "0";
            scoreNum.Text = "0";
            increasedHealthCounter = 0;
            goingLeft = false;
            goingRight = false;
            isJumbing = false;
            isGameOver = false;

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && x.Visible == false && (string)x.Tag != "Perks")
                {
                    x.Visible = true;
                }
            }

            Player.Left = 12;
            Player.Top = 430;

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
            armourPerk.Enabled = false;
            armourPerk.Visible = false;
            if (playerHealth < 100)
            {
                playerHealth = 100;
            }
            playerHealth = playerHealth + 10 * increasedHealthCounter - 10;
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
            armourPerk.Enabled = false;
            armourPerk.Visible = false;
            playerSpeed += 1;
            score++;
            apeared = false;
            gameTimer.Start();
        }

        private void increaseStrength_Click(object sender, EventArgs e)
        {
            increasedJumb = true;
            increaseHealth.Visible = false;
            increaseHealth.Enabled = false;
            increaseSpeed.Visible = false;
            increaseSpeed.Enabled = false;
            increaseStrength.Visible = false;
            increaseStrength.Enabled = false;
            armourPerk.Enabled = false;
            armourPerk.Visible = false;
            jumbSpeed += 1;
            score++;
            apeared = false;
            gameTimer.Start();
        }

        private void HealthIcon_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            increasedArmoured = true;
            increaseHealth.Visible = false;
            increaseHealth.Enabled = false;
            increaseSpeed.Visible = false;
            increaseSpeed.Enabled = false;
            increaseStrength.Visible = false;
            increaseStrength.Enabled = false;
            armourPerk.Enabled = false;
            armourPerk.Visible = false;
            jumbSpeed += 1;
            score++;
            apeared = false;
            gameTimer.Start();
        }
    }
}
