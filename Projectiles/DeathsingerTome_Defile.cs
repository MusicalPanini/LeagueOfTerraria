using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    class DeathsingerTome_Defile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Defile");
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 400;
            Projectile.height = 400;
            Projectile.timeLeft = 180;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.scale = 1;
            Projectile.alpha = 180;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
        }

        public override void AI()
        {
            Projectile.Center = Main.player[Projectile.owner].Center;
                Lighting.AddLight(Projectile.Center, 0f, 0.75f, 0.3f);

            int num = Main.rand.Next(0, 3);
            Dust dust;
            if (num == 0)
            {
                dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 186, 0, -1, 150);
                dust.velocity.X *= 0.3f;
                dust.color = new Color(0, 255, 150);
                dust.noGravity = false;
            }
            else if (num == 2)
            {
                dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 186, 0, -1, 150);
                dust.velocity.X *= 0.3f;
                dust.color = new Color(0, 255, 0);
                dust.noGravity = false;
            }

            if (Projectile.timeLeft < 15)
            {
                Projectile.alpha += 5;
            }
            AnimateProjectile();
        }

        public void AnimateProjectile()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5)
            {
                Projectile.frame++;
                Projectile.frame %= 4; 
                Projectile.frameCounter = 0;
            }
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
