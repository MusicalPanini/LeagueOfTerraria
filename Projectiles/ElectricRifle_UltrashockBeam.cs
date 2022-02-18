using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using TerraLeague.NPCs;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace TerraLeague.Projectiles
{
    class ElectricRifle_UltrashockBeam : ModProjectile
    {
        int AttachedNPC { get { return (int)Projectile.ai[0]; } }
        float Rotation { get { return Projectile.ai[1]; } }
        bool fired = false;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ultrashock Laser");
        }

        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.timeLeft = 140;
            Projectile.penetrate = -1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
            Projectile.hide = true;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            if (!fired)
            {
                if (AttachedNPC != -1)
                {
                    if (!Main.npc[AttachedNPC].active)
                        Projectile.ai[0] = -1;
                    else
                        Projectile.Center = Main.npc[AttachedNPC].Center;
                }

                Projectile.rotation = Rotation;

                if (Projectile.alpha > 0)
                {
                    Projectile.alpha -= 4;
                }
                if (Projectile.alpha < 0)
                {
                    Projectile.alpha = 0;
                }

                if (Projectile.timeLeft == 140)
                {
                    TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 13, -1f);
                }

                if (Projectile.timeLeft == 80)
                {
                    fired = true;
                    Projectile.velocity = new Vector2(-10, 0).RotatedBy(Projectile.rotation);
                    Projectile.friendly = true;
                    Projectile.extraUpdates = 40;
                    TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 72, -1f);
                }

                for (int i = 0; i < 1; i++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.Center - (Vector2.One * 4), 8, 8, 264, 0, 0, 0, new Color(110, 254, 125), 3f - (3f * (Projectile.alpha / 255f)));
                    dust.noGravity = true;
                    dust.noLightEmittence = true;
                    dust.noLight = true;
                    dust.velocity = new Vector2(dust.scale * -2, 0).RotatedBy(Projectile.rotation);
                }
            }

            for (int i = 0; i < 8; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center - (Vector2.One * 8), 16, 16, 264, 0, 0, 0, new Color(110, 254, 125), 3f - (3f* (Projectile.alpha/255f)));
                dust.noGravity = true;
                dust.noLightEmittence = true;
                dust.noLight = true;
                dust.velocity *= 0.1f;
                dust.fadeIn = 1.2f;
            }

            for (int i = 0; i < 1; i++)
            {
                Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 264, 0, 0, 100, new Color(110, 254, 125), 0.75f);
                dust.noGravity = true;
                dust.velocity *= 3;
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Slowed>(), 2 * 60);
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (target.whoAmI == AttachedNPC)
                return false;
            return base.CanHitNPC(target);
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return base.OnTileCollide(oldVelocity);
        }
    }
}
