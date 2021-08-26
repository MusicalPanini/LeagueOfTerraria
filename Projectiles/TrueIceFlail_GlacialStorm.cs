using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace TerraLeague.Projectiles
{
    class TrueIceFlail_GlacialStorm : ModProjectile
    {
        int effectRadius = 256;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Glacial Storm");
        }

        public override void SetDefaults()
        {
            Projectile.width = effectRadius * 2;
            Projectile.height = effectRadius * 2;
            Projectile.timeLeft = 160;
            Projectile.penetrate = -1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void AI()
        {
            if (Main.rand.Next(0, 1) == 0)
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center - (Vector2.One * 64), 128, 128, DustID.Ice, 0, 0, 50, default, 1.5f);
                dust.velocity *= 5f;
                dust.noGravity = true;
                dust.noLight = true;
            }

            TerraLeague.DustBorderRing(effectRadius, Projectile.Center, DustID.Ice, default, 1, true, true, 0.05f);
            if (Projectile.timeLeft % 15 == 0)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (!npc.friendly && !npc.immortal && !npc.townNPC && npc.active && npc.CanBeChasedBy())
                    {
                        if (npc.Hitbox.Intersects(Projectile.Hitbox))
                        {
                            npc.AddBuff(BuffType<Buffs.Slowed>(), 15);
                        }
                    }
                }
            }

            if (Projectile.timeLeft <= 2)
                Projectile.friendly = true;
        }

        public override void Kill(int timeLeft)
        {
            Projectile.friendly = true;

            TerraLeague.DustRing(67, Projectile, default);
            TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 82, -0.7f);

            base.Kill(timeLeft);
        }

        public void Prime()
        {
            Projectile.velocity = Vector2.Zero;
            Projectile.alpha = 255;
            Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
            Projectile.width = 256;
            Projectile.height = 256;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            TerraLeague.DrawCircle(Projectile.Center, effectRadius, Color.SkyBlue);
            TerraLeague.DrawCircle(Projectile.Center, effectRadius - (effectRadius * Projectile.timeLeft / 160f), Color.SkyBlue);
            base.PostDraw(lightColor);
        }
    }
}
