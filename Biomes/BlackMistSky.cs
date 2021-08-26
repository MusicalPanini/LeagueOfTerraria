using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace TerraLeague.Biomes
{
    public class BlackMistSky : CustomSky
    {
        private bool _isActive;
        private float _opacity;
        private bool _isLeaving;


        public override void Update(GameTime gameTime)
        {
            if (!Main.gamePaused && Main.hasFocus)
            {
                System.TimeSpan elapsedGameTime;
                if (_isLeaving)
                {
                    float opacity = _opacity;
                    elapsedGameTime = gameTime.ElapsedGameTime;
                    _opacity = opacity - (float)elapsedGameTime.TotalSeconds;
                    if (_opacity < 0f)
                    {
                        _isActive = false;
                        _opacity = 0f;
                    }
                }
                else
                {
                    float opacity2 = _opacity;
                    elapsedGameTime = gameTime.ElapsedGameTime;
                    _opacity = opacity2 + (float)elapsedGameTime.TotalSeconds;
                    if (_opacity > 1f)
                    {
                        _opacity = 1f;
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            if (!(minDepth < 1f) && maxDepth != 3.40282347E+38f)
            {
                return;
            }
            float quotient;
            if (Main.time <= 3600)
                quotient = (float)Main.time / 3600f;
            else if (Main.time >= 28800)
                quotient = (float)(32400 - Main.time) / 3600f;
            else
                quotient = 1;

            float scale = 1f;
            Color color = new Color(new Vector4(0, 2.5f, 1f, 0.3f) * 1f * Main.ColorOfTheSkies.ToVector4()) * _opacity * scale * quotient;

            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("TerraLeague/Textures/Backgrounds/Fog").Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), color);
        }

        public override float GetCloudAlpha()
        {
            return 0f;
        }

        public override void Activate(Vector2 position, params object[] args)
        {
            _isActive = true;
            _isLeaving = false;
        }

        public override void Deactivate(params object[] args)
        {
            _isLeaving = true;
        }

        public override void Reset()
        {
            _opacity = 0f;
            _isActive = false;
        }

        public override bool IsActive()
        {
            return _isActive;
        }
    }
}