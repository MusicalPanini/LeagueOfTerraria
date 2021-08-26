using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class ChainedRocketHand_StaticField : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Static Field");
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 100;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            if (Projectile.soundDelay == 0)
                Prime();
            Projectile.soundDelay = 100;
            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Confused, 300);

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool PreKill(int timeLeft)
        {
           
            return base.PreKill(timeLeft);
        }

        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(new LegacySoundStyle(3, 53), Projectile.position);
            for (int i = 0; i < 100; i++)
            {
                Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 175, Projectile.Center.Y - 175), 350, 350, DustID.AncientLight, 0, 0, 50, new Color(0, 255, 255), 1.5f);
                dust.velocity *= 15f;
                dust.noGravity = true;
                dust.noLight = true;
                dust.fadeIn = 2;
            }
            for (int i = 0; i < 100; i++)
            {
                Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 175, Projectile.Center.Y - 175), 350, 350, DustID.AncientLight, 0, 0, 50, new Color(0, 255, 255), 1);
                dust.velocity *= 10f;
            }

            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 18; i++)
                {
                    Vector2 pos = new Vector2(350, 0).RotatedBy(MathHelper.ToRadians((20 * i) + (j * 6))) + Projectile.Center;

                    Dust dustR = Dust.NewDustPerfect(pos, 261, Vector2.Zero, 0, new Color(0, 255, 255), 1);
                    dustR.noGravity = true;
                    dustR.fadeIn = 1.5f;
                }
            }

            base.Kill(timeLeft);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public void Prime()
        {
            int size = 700;

            Projectile.tileCollide = false;
            Projectile.velocity = Vector2.Zero;
            Projectile.alpha = 255;
            Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
            Projectile.width = size;
            Projectile.height = size;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            Projectile.timeLeft = 2;
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (target.townNPC)
                return false;
            return Targeting.IsHitboxWithinRange(Projectile.Center, target.Hitbox, Projectile.width / 2);
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
