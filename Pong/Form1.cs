﻿/*
 * Description:     A basic PONG simulator
 * Author:           Sammy McNab
 * Date:            09/21
 */

#region libraries

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Media;

#endregion

namespace Pong
{
    public partial class Form1 : Form
    {
        #region global values

        //graphics objects for drawing
        SolidBrush whiteBrush = new SolidBrush(Color.White);
        SolidBrush redBrush = new SolidBrush(Color.Red);
        SolidBrush blueBrush = new SolidBrush(Color.Blue);

        Font drawFont = new Font("Courier New", 10);

        // Sounds for game
        SoundPlayer scoreSound = new SoundPlayer(Properties.Resources.score);
        SoundPlayer collisionSound = new SoundPlayer(Properties.Resources.collision);

        //determines whether a key is being pressed or not
        Boolean aKeyDown, zKeyDown, jKeyDown, mKeyDown;

        // check to see if a new game can be started
        Boolean newGameOk = true;

        // check to see whether game is in main menu to differentiate the use of space bar and "Y"
        Boolean gameSwitch = false;

        // check to see if AI boolean is true
        Boolean botPad = false;
        //ball directions, speed, and rectangle
        Boolean ballMoveRight = true;
        Boolean ballMoveDown = true;
        int BALL_SPEED = 8;
        Rectangle ball;

        //paddle speeds and rectangles
        const int PADDLE_SPEED = 12;
        Rectangle p1, p2;

        //power up coordinate and rectangle
        Rectangle powerUp;
        //player and game scores
        int player1Score = 0;
        int player2Score = 0;
        int gameWinScore = 3;  // number of points needed to win game

        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        // -- YOU DO NOT NEED TO MAKE CHANGES TO THIS METHOD
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //check to see if a key is pressed and set is KeyDown value to true if it has
            switch (e.KeyCode)
            {
                case Keys.A:
                    aKeyDown = true;
                    break;
                case Keys.Z:
                    zKeyDown = true;
                    break;
                case Keys.J:
                    jKeyDown = true;
                    break;
                case Keys.M:
                    mKeyDown = true;
                    break;
                case Keys.Y:
                    if (newGameOk && gameSwitch)
                    {
                        SetParameters();
                    }
                    break;
                case Keys.Space:
                    if (newGameOk && gameSwitch == false)
                    {
                        gameSwitch = true;
                        SetParameters();
                    }
                    break;
                case Keys.N:
                    if (newGameOk)
                    {
                        Close();
                    }
                    break;
                case Keys.V:
                    if (newGameOk)
                    {

                        SetParameters();
                    }
                    break;
            }
        }

        // -- YOU DO NOT NEED TO MAKE CHANGES TO THIS METHOD
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            //check to see if a key has been released and set its KeyDown value to false if it has
            switch (e.KeyCode)
            {
                case Keys.A:
                    aKeyDown = false;
                    break;
                case Keys.Z:
                    zKeyDown = false;
                    break;
                case Keys.J:
                    jKeyDown = false;
                    break;
                case Keys.M:
                    mKeyDown = false;
                    break;
            }
        }

        /// <summary>
        /// sets the ball and paddle positions for game start
        /// </summary>
        private void SetParameters()
        {
            if (newGameOk)
            {
                player1Score = player2Score = 0;
                newGameOk = false;
                startLabel.Visible = false;
                player1Name.Text = "Player 1 " + player1Score;
                player2Name.Text = "Player 2 " + player2Score;
                gameUpdateLoop.Start();

            }

            //set starting position for paddles on new game and point scored 
            const int PADDLE_EDGE = 20;  // buffer distance between screen edge and paddle            

            p1.Width = p2.Width = 10;    //height for both paddles set the same
            p1.Height = p2.Height = 70;  //width for both paddles set the same

            //p1 starting position
            p1.X = PADDLE_EDGE;
            p1.Y = this.Height / 2 - p1.Height / 2;

            //p2 starting position
            p2.X = this.Width - PADDLE_EDGE - p2.Width;
            p2.Y = this.Height / 2 - p2.Height / 2;

            // TODO set Width and Height of ball
            ball.Width = 12;
            ball.Height = 12;
            // TODO set starting X position for ball to middle of screen, (use this.Width and ball.Width)
            ball.X = (this.Width / 2) - (ball.Width / 2);
            // TODO set starting Y position for ball to middle of screen, (use this.Height and ball.Height)
            ball.Y = (this.Height / 2) - (ball.Height / 2);
        }

        /// <summary>
        /// This method is the game engine loop that updates the position of all elements
        /// and checks for collisions.
        /// </summary>
        private void gameUpdateLoop_Tick(object sender, EventArgs e)
        {
            #region update ball position

            // TODO create code to move ball either left or right based on ballMoveRight and using BALL_SPEED

            // TODO create code move ball either down or up based on ballMoveDown and using BALL_SPEED


            if (ballMoveRight)
            { ball.X = ball.X + BALL_SPEED; }
            else { ball.X = ball.X - BALL_SPEED; }

            if (ballMoveDown)
            { ball.Y = ball.Y + BALL_SPEED; }
            else { ball.Y = ball.Y - BALL_SPEED; }
            #endregion
            #region update paddle positions

            if (aKeyDown == true && p1.Y > 0)
            {
                // TODO create code to move player 1 paddle up using p1.Y and PADDLE_SPEED
                p1.Y = p1.Y - PADDLE_SPEED;
            }

            // TODO create an if statement and code to move player 1 paddle down using p1.Y and PADDLE_SPEED
            else if (zKeyDown == true && p1.Y + p1.Height < this.Height)
            {
                p1.Y = p1.Y + PADDLE_SPEED;
            }

            if (jKeyDown == true && p2.Y > 0)
            { p2.Y = p2.Y - PADDLE_SPEED; }

            else if (mKeyDown == true && p2.Y + p2.Height < this.Height)
            { p2.Y = p2.Y + PADDLE_SPEED; }
            // TODO create an if statement and code to move player 2 paddle up using p2.Y and PADDLE_SPEED

            // TODO create an if statement and code to move player 2 paddle down using p2.Y and PADDLE_SPEED

            #endregion

            #region ball collision with top and bottom lines

            if (ball.Y < 0) // if ball hits top line
            {
                // TODO use ballMoveDown boolean to change direction
                ballMoveDown = true;
                // TODO play a collision sound
                collisionSound.Play();
            }
            // TODO In an else if statement use ball.Y, this.Height, and ball.Width to check for collision with bottom line
            // If true use ballMoveDown down boolean to change direction
            else if (ball.Y + ball.Height > this.Height)
            { ballMoveDown = false; collisionSound.Play(); }

            #endregion

            #region ball collision with paddles

            if (ball.IntersectsWith(p1) || ball.IntersectsWith(p2))
            { ballMoveRight = !ballMoveRight; collisionSound.Play(); }

            /*  ENRICHMENT
             *  Instead of using two if statments as noted above see if you can create one
             *  if statement with multiple conditions to play a sound and change direction
             */

            #endregion

            #region ball collision with side walls (point scored)

            if (ball.X < 0)  // ball hits left wall logic
            {
                // TODO
                // --- play score sound
                scoreSound.Play();
                // --- update player 2 score
                player2Score++;
                player2Name.Text = "Player 2 " + player2Score;
                // TODO use if statement to check to see if player 2 has won the game. If true run 
                if (player2Score == gameWinScore)
                { GameOver("PLayer 2"); }
                // GameOver method. Else change direction of ball and call SetParameters method.
                else { ballMoveRight = !ballMoveRight; SetParameters(); }
                Thread.Sleep(1000);
            }

            // TODO same as above but this time check for collision with the right wall

            #endregion

            if (ball.X > this.Width)
            {
                player1Score++;
                scoreSound.Play(); player1Name.Text = "Player 1 " + player1Score;
                if (player1Score == gameWinScore)
                { GameOver("Player 1"); }
                else { ballMoveRight = !ballMoveRight; SetParameters(); }
                Thread.Sleep(1000);
            }

            //refresh the screen, which causes the Form1_Paint method to run
            this.Refresh();
        }

        /// <summary>
        /// Displays a message for the winner when the game is over and allows the user to either select
        /// to play again or end the program
        /// </summary>
        /// <param name="winner">The player name to be shown as the winner</param>
        private void GameOver(string winner)
        {

            gameUpdateLoop.Stop();
            startLabel.Visible = true;
            startLabel.Text = winner + " wins!";
            this.Refresh();
            Thread.Sleep(2000);
            startLabel.Text = "Would you like to play again?";
            newGameOk = true;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // TODO draw paddles using FillRectangle
            e.Graphics.FillRectangle(redBrush, p1);
            e.Graphics.FillRectangle(blueBrush, p2);
            // TODO draw ball using FillRectangle
            e.Graphics.FillEllipse(whiteBrush, ball);
            // TODO draw scores to the screen using DrawString
        }

    }
}
