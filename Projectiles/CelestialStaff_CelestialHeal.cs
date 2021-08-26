using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class CelestialStaff_CelestialHeal : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("No one expects the Banana");
        }

        public override void SetDefaults()
        {
            
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.alpha = 255;
            Projectile.timeLeft = 90;
            Projectile.penetrate = 1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

        }

        public override void AI()
        {
            for (int i = 0; i < 2; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.GemEmerald, Projectile.velocity.X, Projectile.velocity.Y, 50, default, 1.2f);
                dust.noGravity = true;
                dust.velocity *= 0.3f;
            }

            for (int i = 0; i < Main.player.Length; i++)
            {
                if (Projectile.Hitbox.Intersects(Main.player[i].Hitbox) && i != Projectile.owner && Main.myPlayer == Projectile.owner)
                {
                    if (Main.LocalPlayer.whoAmI == Projectile.owner)
                        Main.player[Projectile.owner].GetModPlayer<PLAYERGLOBAL>().SendHealPacket(Projectile.damage, i, -1, Projectile.owner);

                    Projectile.Kill();
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            return true;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
