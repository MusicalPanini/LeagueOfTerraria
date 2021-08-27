using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace TerraLeague.Projectiles
{
    public class RadiantStaff_LightShot : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Orb of Light");
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.alpha = 255;
            Projectile.timeLeft = 90;
            Projectile.penetrate = 2;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = 0;
        }

        public override void AI()
        {
            for (int i = 0; i < 2; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.GoldFlame, 0, 0, 0, default, 2f);
                dust.noGravity = true;
                dust.velocity *= 0.3f;
            }
            if (Projectile.timeLeft < 30)
            {
                Projectile.alpha += 9;
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (target.GetGlobalNPC<NPCs.TerraLeagueNPCsGLOBAL>().illuminated)
                Main.player[Projectile.owner].GetModPlayer<PLAYERGLOBAL>().magicOnHit += 40 + (int)(Main.player[Projectile.owner].GetModPlayer<PLAYERGLOBAL>().MAG * 0.2);

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.Next(4) == 0)
                target.AddBuff(BuffType<Illuminated>(), 300);

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 17; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.GoldFlame, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f, 0, default, 1f);
                dust.noGravity = true;
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = height = 10;
            return true;
        }
    }
}