using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace SpaceShooter
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Player player;
        Enemy enemy;
        SpriteFont font;
        public string winnerString;
        public bool isWinner;
        Texture2D background;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            // Set screen res
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = 1366;
            _graphics.PreferredBackBufferHeight = 768;
            _graphics.ApplyChanges();


            // Make objects for players
            player = new Player();
            enemy = new Enemy();

            // Give screen info here, takes less lines than doing it in the class
            enemy.scrWidth = player.scrWidth = _graphics.PreferredBackBufferWidth;
            enemy.scrHeight = player.scrHeight = _graphics.PreferredBackBufferHeight;            


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            // Load ship's texture
            player.Load(this.Content, "spaceship");
            enemy.Load(this.Content, "alienship");

            font = this.Content.Load<SpriteFont>("ScoreFont");
            background = this.Content.Load<Texture2D>("space");


            // Load destroyed state & sounds
            player.loadVarious(this.Content);
            enemy.loadVarious(this.Content);

            // Put them into position
            player.Init();
            enemy.Init();

            // Prevents game from starting instantly
            isWinner = true;
            winnerString = $"First to {Settings.winScore} wins!";
            player.pause();
            enemy.pause();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here


            KeyboardState keyboardState = Keyboard.GetState();

            // q is a normal key to exit a program with
            if (keyboardState.IsKeyDown(Keys.Q)) Exit();

            // Quits menu and plays game
            if (isWinner && keyboardState.IsKeyDown(Keys.Enter))
            {
                isWinner = false;
                player.restore();
                enemy.restore();
            }

            // Run ship updates
            player.Update(this.Content, _graphics.GraphicsDevice, enemy, ref isWinner, ref winnerString);
            enemy.Update(this.Content, _graphics.GraphicsDevice, player, ref isWinner, ref winnerString);



            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();

            _spriteBatch.Draw(background, new Rectangle(0,0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);

            // Draw ships and their bullets
            player.Draw(_spriteBatch);
            enemy.Draw(_spriteBatch);
            player.drawBullets(_spriteBatch);
            enemy.drawBullets(_spriteBatch);

            // Display scores
            _spriteBatch.DrawString(font, $"Your Score: {enemy.damage}", new Vector2(25,50), Color.White);
            _spriteBatch.DrawString(font, $"Enemy Score: {player.damage}", new Vector2(_graphics.PreferredBackBufferWidth-200,50), Color.White);

            // If in pause state, inform player
            if (isWinner)
            {
                _spriteBatch.DrawString(font, $"{winnerString}\n Press enter!", new Vector2(_graphics.PreferredBackBufferWidth/2-50, _graphics.PreferredBackBufferHeight/2-25), Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
