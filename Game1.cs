using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace SpaceShooterAI
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        SpriteFont font;
        Texture2D alienshipTexture, bulletTexture, spaceshipTexture, background, alienshipState, spaceshipState, explosionTexture;
        SoundEffect shootSound, hitSound, explosionSound;

        int ax, ay, sx, sy, velocity, sbstate, abstate, sbx, sby, abx, aby, sscore, ascore, victoryScreen, aiVelocity, aiRange, bulletVelocity;
        bool sshoot, ashoot;
        string victoryText;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        // Default values, can call back to on game reset
        protected void DefaultPos()
        {
            velocity = 5;
            bulletVelocity = 17;
            aiVelocity = 3;
            aiRange = 10;
            ay = 100;
            sy = 500 - 43; // Seems to be no way of getting texture height at this stage
            ax = _graphics.PreferredBackBufferWidth - 500 - 100; // Second 100 is ship width
            sx = 500;
            sscore = 0;
            ascore = 0;
            sshoot = false;
            ashoot = false;
            spaceshipState = spaceshipTexture;
            alienshipState = alienshipTexture;
        }


        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            _graphics.IsFullScreen = true;

            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();

            DefaultPos();

            victoryScreen = 1;
            victoryText = "Destroy the Alien's ship";


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            alienshipTexture = this.Content.Load<Texture2D>("alienship");
            spaceshipTexture = this.Content.Load<Texture2D>("spaceship");
            bulletTexture = this.Content.Load<Texture2D>("bullet");
            background = this.Content.Load<Texture2D>("space");
            font = this.Content.Load<SpriteFont>("ScoreFont");
            shootSound = this.Content.Load<SoundEffect>("shoot");
            hitSound = this.Content.Load<SoundEffect>("hit");
            explosionSound = this.Content.Load<SoundEffect>("explosion");
            explosionTexture = this.Content.Load<Texture2D>("explosionTexture");
            // States are used to change texture upon victory/loss
            alienshipState = alienshipTexture;
            spaceshipState = spaceshipTexture;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            KeyboardState keys = Keyboard.GetState();
            GamePadState gp = GamePad.GetState(PlayerIndex.One);

            // If on victory screen, enter restarts game
            if (keys.IsKeyDown(Keys.Enter) || gp.IsButtonDown(Buttons.Start) && victoryScreen == 1)
            {
                DefaultPos();
                victoryScreen = 0;
            }

            // Extra quit key
            if (keys.IsKeyDown(Keys.Q)) Exit();

            // Fullscreen Toggle
            if (keys.IsKeyDown(Keys.F11)) _graphics.ToggleFullScreen();

            // Manages input for spaceship
            if (keys.IsKeyDown(Keys.Left) || keys.IsKeyDown(Keys.A) || gp.IsButtonDown(Buttons.DPadLeft)) sx -= velocity;

            if (keys.IsKeyDown(Keys.Right) || keys.IsKeyDown(Keys.D) || gp.IsButtonDown(Buttons.DPadRight)) sx += velocity;

            if (keys.IsKeyDown(Keys.Space) || gp.IsButtonDown(Buttons.A))
            {
                sbstate = 1;
                // prevents sound from repeating
                if (sshoot==false)
                {
                    shootSound.Play();
                    sshoot=true;
                }
            }

            // AI Movement
            if (ax < sx) ax += aiVelocity;
            if (ax > sx) ax -= aiVelocity;
            if (ax > sx - aiRange &&  ax < sx + spaceshipTexture.Width + aiRange)
            {
                abstate = 1;
                if (ashoot==false)
                {
                    shootSound.Play();
                    ashoot=true;
                }
            }

            // Player boundaries
            sx = Math.Clamp(sx, 0, _graphics.PreferredBackBufferWidth-spaceshipTexture.Width);
            ax = Math.Clamp(ax, 0, _graphics.PreferredBackBufferWidth-alienshipTexture.Width);

            // Victory Process
            // Victory related processes come before bullet states
            // as victory screen overwrites some bullet properties

            if (sscore == 10 && ascore == 10)
            {
                victoryText = "It's a draw!";
                if (victoryScreen==0){explosionSound.Play();}
                alienshipState = explosionTexture;
                spaceshipState = explosionTexture;
                victoryScreen = 1;
            }
            else if (sscore == 10)
            {
                victoryText = "You win!";
                if (victoryScreen==0){explosionSound.Play();}
                alienshipState = explosionTexture;
                victoryScreen = 1;
            }
            else if (ascore == 10)
            {
                victoryText = "You lost!";
                if (victoryScreen==0){explosionSound.Play();}
                spaceshipState = explosionTexture;
                victoryScreen = 1;
            }

            // Stop people firing during victory screen
            if (victoryScreen == 1)
            {
                abstate = 0;
                sbstate = 0;
                sshoot = true;
                ashoot = true;
                velocity = 0;
                aiVelocity = 0;
            }



            if (sbstate == 1)
            {
                sby -= bulletVelocity;
                // Checks if bullet is inside alienship
                if (ay + alienshipTexture.Height > sby && sby > ay && sbx > ax && sbx < ax + alienshipTexture.Width )
                {
                    sbx = sx + 40;
                    sby = sy - bulletTexture.Height;
                    sscore++;
                    sbstate = 0;
                    sshoot = false;
                    hitSound.Play();
                }

                // Checks if bullet is out of bounds
                if (sby < 0 - bulletTexture.Height) 
                {
                    sbstate = 0;
                    sbx = sx + 40;
                    sby = sy - bulletTexture.Height;
                    sshoot = false;
                }            
            }
            else
            {
                sbx = sx + 40;
                sby = sy - bulletTexture.Height;
            }


            if (abstate == 1)
            {
                aby += bulletVelocity;
                if (aby + bulletTexture.Height < sy + spaceshipTexture.Height && aby + bulletTexture.Height > sy && abx > sx && abx < sx + spaceshipTexture.Width)
                {
                    // If bullet it inside spaceship, move back to start and + to score
                    abx = ax + 40;
                    aby = ay + alienshipTexture.Height;
                    ascore++;
                    abstate = 0;
                    ashoot = false;
                    hitSound.Play();
                }

                // Manages bullet if out of bounds
                if (aby > _graphics.PreferredBackBufferHeight)
                {
                    abstate = 0;
                    abx = ax + 40;
                    aby = ay + alienshipTexture.Height;
                    ashoot = false;
                }
            }
            else
            {
                // Ensure bullet location is at ship
                abx = ax + 40;
                aby = ay + alienshipTexture.Height;
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();
            _spriteBatch.Draw(background, new Rectangle(0,0, background.Width, background.Height), Color.White);
            _spriteBatch.Draw(alienshipState, new Rectangle(ax,ay, alienshipTexture.Width, alienshipTexture.Height), Color.White);
            _spriteBatch.Draw(spaceshipState, new Rectangle(sx,sy, spaceshipTexture.Width, spaceshipTexture.Height), Color.White);
            _spriteBatch.DrawString(font, "Spaceship score: " + sscore, new Vector2(50, 40), Color.White);
            _spriteBatch.DrawString(font, "Alien score: " + ascore, new Vector2(1090, 40), Color.White);
            if (sbstate == 1)
            {
                _spriteBatch.Draw(bulletTexture, new Rectangle(sbx,sby, 20, 30), Color.Red);
            }
            if (abstate == 1)
            {
                _spriteBatch.Draw(bulletTexture, new Rectangle(abx,aby, 20, 30), Color.LimeGreen);
            }
            if (victoryScreen == 1)
            {
                _spriteBatch.DrawString(font, victoryText, new Vector2(540, 345), Color.White);
                _spriteBatch.DrawString(font, "Press enter to continue!", new Vector2(540, 375), Color.White);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
