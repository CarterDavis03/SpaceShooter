using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace SpaceShooter
{
    public class Enemy : Ships
    {
        public int range;
        public int shootDelay;

        public void Init()
        {
            // Sets default values, some of this could go but its midnight and i want sleep
            defaultVelocity = velocity = 2.5f;
            range = Settings.aiRange;
            defaultPosX = posX = scrWidth - 200 - this.Image.Width;
            posY = 100;
        }

        public void Update(ContentManager contentManager, GraphicsDevice graphics, Ships enemy, ref bool isWinner, ref string winnerString)
        {
            // Makes the ship follow it's enemy
            if (posX < enemy.posX) posX += velocity;
            if (posX > enemy.posX) posX -= velocity;

            // Makes ship shoot if in range and delay has been reached
            if (posX > enemy.posX - range &&  posX < enemy.posX + enemy.Image.Width + range && shootDelay>Settings.aiShootDelay)
            {
                shoot(contentManager, graphics, Color.LimeGreen);
                shootDelay = 0;
            }

            // Adds to delay counter, does bullet update
            shootDelay++;
            removeBullets(graphics, enemy);


            // If damaged and the other ship is not player wins
            if (damage >= Settings.winScore && enemy.damage != Settings.winScore)
            {
                destroy();
                enemy.pause();
                if(!isWinner) explosionSound.Play();
                isWinner = true;
                winnerString = "You win!";
            }

        }

    }
}