using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class Item_RunaansShot : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Runaan's Hurricane");
        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.alpha = 255;
            Projectile.timeLeft = 900;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 12;
            Projectile.alpha = 255;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.position, 0.5f, 0.45f, 0.30f);

            Dust dust = Dust.NewDustPerfect(Projectile.position, 204, Vector2.Zero, 0, default, 0.75f);
            dust.noGravity = true;
            dust.alpha = 100;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            return true;
        }

        public override void Kill(int timeLeft)
        {
        }

        public override bool? CanHitNPC(NPC target)
        {
            if ((int)Projectile.ai[0] != target.whoAmI)
                return base.CanHitNPC(target);
            else
                return false;
        }
    }
}
