using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
    class DeathsingerTome_LayWaste : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Death Singer's Tome");
            Main.projFrames[Projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.width = 75;
            Projectile.height = 75;
            Projectile.timeLeft = 42;
            Projectile.penetrate = -1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.scale = 1f;
            Projectile.alpha = 255;
        }

        public override void AI()
        {
            Projectile.Center = new Vector2(Projectile.ai[0], Projectile.ai[1]);

            int num = Main.rand.Next(0, 3);
            Dust dust;
            if (num == 0)
            {
                dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 186, 0, -1, 150);
                dust.velocity.X *= 0.3f;
                dust.color = new Color(0, 255, 150);
                dust.noGravity = false;
                dust.alpha = Projectile.alpha;
            }
            else if (num == 2)
            {
                dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 186, 0, -1, 150);
                dust.velocity.X *= 0.3f;
                dust.color = new Color(0, 255, 0);
                dust.noGravity = false;
                dust.alpha = Projectile.alpha;
            }

            Lighting.AddLight(Projectile.Center, 0f, 0.75f, 0.3f);
            if (Projectile.timeLeft > 12)
            {
                Projectile.alpha -= 1;
            }
            if (Projectile.timeLeft == 12)
            {
                Projectile.friendly = true;
                Projectile.frame++;
                Projectile.alpha = 20;
                TerraLeague.PlaySoundWithPitch(Projectile.Center, 3, 54, -0.6f);

                for (int i = 0; i < 15; i++)
                {
                    dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 186, 0, -1, 150);
                    dust.color = new Color(0, 255, 150);
                    dust.noGravity = false;
                }

                int totalHit = 0;
                for (int i = 0; i < 200; i++)
                {
                    NPC npc = Main.npc[i];
                    if (Projectile.Hitbox.Intersects(npc.Hitbox) && npc.active && !npc.townNPC)
                    {
                        totalHit++;
                        if (totalHit > 1)
                            break;
                    }
                }

                if (totalHit == 1)
                    Projectile.damage *= 2;
            }
            if (Projectile.timeLeft <= 12)
            {
                AnimateProjectile();
            }
            if (Projectile.timeLeft == 11)
            {
                Projectile.friendly = false;
            }
        }

        public void AnimateProjectile() 
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 3)
            {
                Projectile.frame++;
                Projectile.frame %= 5;
                Projectile.frameCounter = 0;
            }
        }
    }
}
