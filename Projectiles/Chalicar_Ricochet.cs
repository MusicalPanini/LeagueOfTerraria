using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TerraLeague.Projectiles.Homing;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class Chalicar_Ricochet : RichochetProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chalicar Ricochet");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 36;
            Projectile.timeLeft = 60;
            Projectile.penetrate = 6;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.alpha = 0;
            Projectile.netImportant = true;
            //Projectile.usesIDStaticNPCImmunity = true;
            //Projectile.idStaticNPCHitCooldown = 30;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -2;


            SetHomingDefaults(true, 480, 301);
            CanOnlyHitTarget = false;
        }

        public override void AI()
        {
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 8;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item7, Projectile.position);
            }

            Projectile.rotation += 0.3f * (float)Projectile.direction;

            if (hitCounter != 0)
            {
                base.AI();
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (hitCounter == 0)
            {
                Projectile.tileCollide = false;
                Projectile.velocity *= 0.75f;
            }
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 12; i++)
            {
                Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Silver, Projectile.oldVelocity.X * 0.25f, Projectile.oldVelocity.Y * 0.25f, 157, new Color(234, 255, 0));
            }

            base.Kill(timeLeft);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 16;
            return true;
        }

        public override bool? CanHitNPC(NPC target)
        {
                return base.CanHitNPC(target);
        }
    }
}
