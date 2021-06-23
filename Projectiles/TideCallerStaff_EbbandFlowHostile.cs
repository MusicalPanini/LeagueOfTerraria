using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.NPCs;
using TerraLeague.Projectiles.Homing;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    class TideCallerStaff_EbbandFlowHostile : HomingProjectile
    {
        int healing { get { return (int)projectile.ai[1]; } }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ebb and Flow");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 24;
            projectile.alpha = 255;
            projectile.timeLeft = 91;
            projectile.magic = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.netImportant = true;

            CanRetarget = true;
            TurningFactor = 0.93f;
        }

        public override void AI()
        {
            if (projectile.soundDelay == 0)
            {
                Main.PlaySound(new LegacySoundStyle(2, 21, Terraria.Audio.SoundType.Sound));
            }
            projectile.soundDelay = 100;

            if (projectile.timeLeft < 84)
            {
                HomingAI();
            }

            Dust dust;
            for (int i = 0; i < 5; i++)
            {
                Color color = new Color(0, 255, 255);
                if (i > 3)
                    color = new Color(100, 100, 255);

                Vector2 dustBoxPosition = new Vector2(projectile.position.X + 6, projectile.position.Y + 6);
                int dustBoxWidth = projectile.width - 12;
                int dustBoxHeight = projectile.height - 12;
                dust = Dust.NewDustDirect(dustBoxPosition, dustBoxWidth, dustBoxHeight, DustID.IcyMerman, 0f, 0f, 100, color, 1.5f);
                dust.noGravity = true;
                dust.velocity *= 0.1f;
                dust.velocity += projectile.velocity * 0.1f;
                dust.position.X -= projectile.velocity.X / 3f * (float)i;
                dust.position.Y -= projectile.velocity.Y / 3f * (float)i;
            }
            if (Main.rand.Next(5) == 0)
            {
                Vector2 dustBoxPosition = new Vector2(projectile.position.X + 6, projectile.position.Y + 6);
                int dustBoxWidth = projectile.width - 12;
                int dustBoxHeight = projectile.height - 12;
                dust = Dust.NewDustDirect(dustBoxPosition, dustBoxWidth, dustBoxHeight, DustID.DungeonWater, 0f, 0f, 100, default, 0.8f);
                dust.velocity *= 0.25f;
                dust.velocity += projectile.velocity * 0.5f;
            }

            Lighting.AddLight(projectile.position, 0f, 0f, 0.5f);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 12; i++)
            {
                Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Wet, projectile.velocity.X * 0.25f, projectile.velocity.Y * 0.25f, 0, default, 1f);
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10;
            return true;
        }
    }
}
