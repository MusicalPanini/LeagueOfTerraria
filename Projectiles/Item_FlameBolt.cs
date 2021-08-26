using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class Item_FlameBolt: ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hextech Protobelt-01");
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 10;
            Projectile.alpha = 0;
            Projectile.timeLeft = 60;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = 0;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.FireworksRGB);
            dust.color = new Color(0, 125, 255);
            dust.noGravity = true;
            Lighting.AddLight(Projectile.position, 0f, 0f, 0.5f);
        }

        public override void Kill(int timeLeft)
        {
            Dust dust;
            for (int i = 0; i < 24; i++)
            {
                dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.FireworksRGB, 0f, 0f, 100, default, 1);
                dust.noGravity = true;
                dust.velocity *= 2f;
            }
            for (int i = 0; i < 8; i++)
            {
                dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.FireworksRGB, 0f, 0f, 100, default, 1);
                dust.velocity *= 1f;
            }
            Terraria.Audio.SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 14, Terraria.Audio.SoundType.Sound), Projectile.position);
            base.Kill(timeLeft);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Prime();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Prime();
            return false;
        }

        public void Prime()
        {
            Projectile.velocity = Vector2.Zero;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
            Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
            Projectile.width = 115;
            Projectile.height = 115;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            Projectile.timeLeft = 3;
        }
    }
}
