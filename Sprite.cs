using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace SpaceShooter
{
    public class Sprite
    {

        // Declare variables

        public float posX, posY, velocity;
        public int scrWidth, scrHeight, width, height;
        public Texture2D Image; 

        public Color colour = Color.White;


        public void Load(ContentManager contentManager, string assetName)
        {
            Image = contentManager.Load<Texture2D>(assetName);

            this.width = Image.Width;
            this.height = Image.Height;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image, new Rectangle((int)posX, (int)posY, width, height), colour);
        }

        public Rectangle bBox()
        {
            return new Rectangle((int)posX, (int)posY, width, height);
        }

        public bool collideCheck(Sprite sprite)
        {
            if (this.bBox().Intersects(sprite.bBox()))
            {
                return true;
            }
            return false;
        }
    }
}