using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace SpaceShooter
{
    public class Player : Ships
    {

        KeyboardState oldState;

        public void Init()
        {
            // Default values
            defaultVelocity = velocity = 5.0f;
            defaultPosX = posX = 200;
            posY = scrHeight - 100 - height;
        }

        public void Update(ContentManager contentManager, GraphicsDevice graphics, Ships enemy, ref bool isWinner, ref string winnerString)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            // Handles movement
            if (keyboardState.IsKeyDown(Keys.Left)) 
            {
                posX -= velocity;
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                posX += velocity;
            }
            if (oldState.IsKeyUp(Keys.Space) && keyboardState.IsKeyDown(Keys.Space))
            {
                shoot(contentManager, graphics);
            }

            // Prevents repeats
            oldState = keyboardState;
            
            removeBullets(graphics, enemy);

            // Prevents user going off scren
            posX = Math.Clamp(posX, 0 , scrWidth-width);


            // End game checks
            if (damage >= Settings.winScore && enemy.damage >= Settings.winScore)
            {
                destroy();
                enemy.destroy();
                if(!isWinner) explosionSound.Play();
                isWinner = true;
                winnerString = "It's a draw!";
            }
            else if (damage >= Settings.winScore)
            {
                destroy();
                enemy.pause();
                if(!isWinner) explosionSound.Play();
                isWinner = true;
                winnerString = "You lose!";
            }
        }

    }
}