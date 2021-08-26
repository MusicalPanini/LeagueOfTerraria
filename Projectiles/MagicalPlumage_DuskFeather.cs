using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class MagicalPlumage_DuskFeather : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dusk Feather");
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.alpha = 0;
            Projectile.timeLeft = 45;
            Projectile.penetrate = 3;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.aiStyle = 1;
        }

        public override void AI()
        {
            Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.WaterCandle, 0f, 0f, 147, Main.rand.Next(2) == 0 ? new Color(255,0,201) : new Color(0, 255, 255), 1.7f);
            dust.noGravity = true;

            if (Projectile.timeLeft < 10)
            {
                Projectile.alpha += 26;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
            return true;
        }

        public override void Kill(int timeLeft)
        {
            if (Projectile.alpha < 255)
            {
                for (int i = 0; i < 10; i++)
                {
                    Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.WaterCandle, 0f, 0f, 147, Main.rand.Next(2) == 0 ? new Color(255, 0, 201) : new Color(0, 255, 255), 1.7f);
                    dust.noGravity = true;
                }
            }
        }
    }
}
