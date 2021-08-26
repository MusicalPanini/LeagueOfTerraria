using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class AtlasGauntlets_Left : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Atlas Gauntlets");
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.alpha = 0;
            Projectile.timeLeft = 14;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            if (Projectile.soundDelay == 0)
            {
                TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 1, -1);
            }
            Projectile.soundDelay = 100;

            if (Projectile.timeLeft > 3)
            {
                if (Projectile.ai[0] < 3)
                {
                    Projectile.ai[0]++;
                }
            }
            else
                Projectile.ai[0]--;

            if (Projectile.velocity.X < 0)
                Projectile.spriteDirection = -1;

            Player player = Main.player[Projectile.owner];
            //player.heldProj = Projectile.whoAmI;
            Projectile.Center = player.MountedCenter + (Projectile.velocity * (Projectile.ai[0] + 0.5f)) + new Vector2(-6, 0);
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            player.ChangeDir(Projectile.direction);
        }

        public override void Kill(int timeLeft)
        {
            
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10; 
            return true;
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (!Main.player[Projectile.owner].CanHit(target))
                return false;
            return base.CanHitNPC(target);
        }
    }
}