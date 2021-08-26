using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
	public class XanCrestBlades_BladeSurge : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blade Surge");
        }

        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.timeLeft = 15;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            //if (!Main.npc[(int)Projectile.ai[0]].active)
            //{
            //    Projectile.Kill();
            //}
            //else
            //{
            //    Projectile.timeLeft = 300;

            //    if (Projectile.localAI[0]  == 0f)
            //    {
            //        AdjustMagnitude(ref Projectile.velocity);
            //        Projectile.localAI[0]  = 1f;
            //    }
            //    Vector2 move = Vector2.Zero;

            //    NPC npc = Main.npc[(int)Projectile.ai[0]];

            //    Vector2 newMove = NPC.Center - Projectile.Center;
            //    float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
            //    move = newMove;
            //    AdjustMagnitude(ref move);
            //    Projectile.velocity = (10 * Projectile.velocity + move) / 20f;
            //    AdjustMagnitude(ref Projectile.velocity);

            //    Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            //    Main.player[Projectile.owner].MountedCenter = Projectile.Center;
            //    Main.player[Projectile.owner].fullRotationOrigin = new Vector2(16, 32);
            //    Main.player[Projectile.owner].fullRotation = Projectile.rotation;
            //    Main.player[Projectile.owner].itemTime = 5;
            //}

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Main.player[Projectile.owner].MountedCenter = Projectile.Center;
            Main.player[Projectile.owner].fullRotationOrigin = new Vector2(8, 24);
            Main.player[Projectile.owner].fullRotation = Projectile.rotation;
            Main.player[Projectile.owner].GetModPlayer<PLAYERGLOBAL>().contactDodge = true;
            Main.player[Projectile.owner].itemTime = 5;
        }

        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 8)
            {
                vector *= 24f / magnitude;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.life <= 0)
            {
                Main.player[Projectile.owner].GetModPlayer<PLAYERGLOBAL>().AbilityCooldowns[(int)AbilityType.Q] = 0;
            }
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            Main.player[Projectile.owner].fullRotation = 0;
            Main.player[Projectile.owner].velocity = Projectile.oldVelocity / 4;
        }

        public override bool? CanHitNPC(NPC target)
        {
            //if ((int)Projectile.ai[0] == target.whoAmI)
            //    return true;
            //else
            //    return false;
            return base.CanHitNPC(target);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }
    }
}
