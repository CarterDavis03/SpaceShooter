using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

namespace SpaceShooter
{
    public class Bullet : Sprite
    {


        public Bullet(float bPosX, float bPosY, ContentManager contentManager, GraphicsDevice graphics, Color bulletColor)
        {
            // Set texture and pos
            Load(contentManager, "bullet");
            posX = bPosX-(Image.Width/2);
            posY = bPosY;
            colour = bulletColor;

            // Determines which way to shoot based on Y
            if (posY > graphics.Viewport.Height/2)
            {
                velocity = Settings.bulletVelocity;
            }
            else
            {
                velocity = Settings.bulletVelocity*-1;
            }

        }

        public void Update()
        {
            posY -= velocity;
        }

        public bool toDestroy(GraphicsDevice graphics, Ships enemy, SoundEffect hitSound)
        {
            // If image outside frame, true
            if (posY < 0 - Image.Height || posY > graphics.Viewport.Height )
            {
                return true;
            }

            // If hit the ship's enemy, true
            if (collideCheck(enemy))
            {
                hitSound.Play();
                enemy.damage++;
                return true;
            }

            return false;
        }
    }
}