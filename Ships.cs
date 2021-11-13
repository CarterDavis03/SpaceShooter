using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;

namespace SpaceShooter
{
    public class Ships : Sprite
    {

        public Texture2D destroyedTexture;
        public Texture2D defaultTexture;
        public List<Bullet> bullets = new List<Bullet>(); 
        public int damage = 0;
        public bool canShoot = true;
        public float defaultVelocity, defaultPosX;
        public SoundEffect shootSound, hitSound, explosionSound;

        public void loadVarious(ContentManager contentManager)
        {
            // Loads textures and sounds specific to ships
            defaultTexture = this.Image;            
            destroyedTexture = contentManager.Load<Texture2D>("explosionTexture");
            shootSound = contentManager.Load<SoundEffect>("shoot");
            hitSound = contentManager.Load<SoundEffect>("hit");
            explosionSound = contentManager.Load<SoundEffect>("explosion");
        }

        public void shoot(ContentManager contentManager, GraphicsDevice graphics, Color bulletColor)
        {
            if (canShoot)
            {
                this.bullets.Add(new Bullet(posX+(Image.Width/2), posY, contentManager, graphics, bulletColor));
                shootSound.Play();
            }

        }

        public void destroy()
        {
            Image = destroyedTexture;
            velocity = 0;
            canShoot = false;
        }

        public void pause()
        {
            velocity = 0;
            canShoot = false;
        }

        public void restore()
        {
            // Brings back default values
            Image = defaultTexture;
            velocity = defaultVelocity;
            posX = defaultPosX;
            canShoot = true;
            damage = 0;
        }

        public void drawBullets(SpriteBatch spriteBatch)
        {
            foreach (Bullet bullet in bullets)
            {
                bullet.Draw(spriteBatch);
            }
        }

        public void removeBullets(GraphicsDevice graphics, Ships enemy)
        {
            // Remove bullets from the bullet list and updates each pos
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Update();

                if (bullets[i].toDestroy(graphics, enemy, hitSound))
                {
                    bullets.RemoveAt(i);
                    i--;
                }
                else if (!canShoot)
                {
                    bullets.Clear();
                }
            }
        }


    }
}