using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Items.CustomItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles.Vanity
{
    public class ForgeGod : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Forge God");
            Main.projFrames[Projectile.type] = 16;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.alpha = 0;
            Projectile.scale = 1f;
            Projectile.timeLeft = Projectile.SentryLifeTime;
            Projectile.tileCollide = false;
            Projectile.gfxOffY = -96 - 32;
        }

        public override void AI()
        {
            if (Projectile.frame < 15)
            {
                Projectile.frameCounter++;
            }
            else
            {
                Projectile.alpha += 5;
                if (Projectile.alpha > 250)
                    Projectile.Kill();
            }
            //if (Projectile.frameCounter >= ((Projectile.frame == 3 || Projectile.frame == 7) ? 12 : 8))
            if (Projectile.frameCounter >= 2)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;

                if (Projectile.frame == 11)
                {
                    TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 37, -0.5f);
                    for (int i = 0; i < 10; i++)
                    {
                        Dust.NewDustDirect(Projectile.Center + new Vector2(0, 52), 1, 1, DustID.Torch, Main.rand.NextFloat(1, 3), 0);
                        Dust.NewDustDirect(Projectile.Center + new Vector2(0, 52), 1, 1, DustID.Torch, Main.rand.NextFloat(-3, -1), 0);
                    }

                    if (ModContent.GetModItem((int)Projectile.ai[0]) is MasterworkItem masterItem)
                    {
                        CombatText.NewText(Projectile.Hitbox, MasterworkItem.MasterColor, masterItem.MasterworkName + " Forged!", true);
                    }
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }
    }
}
