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
        int healing { get { return (int)Projectile.ai[1]; } }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ebb and Flow");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.alpha = 255;
            Projectile.timeLeft = 91;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.netImportant = true;

            CanRetarget = true;
            TurningFactor = 0.93f;
        }

        public override void AI()
        {
            if (Projectile.soundDelay == 0)
            {
                Terraria.Audio.SoundEngine.PlaySound(new LegacySoundStyle(2, 21, Terraria.Audio.SoundType.Sound));
            }
            Projectile.soundDelay = 100;

            if (Projectile.timeLeft < 84)
            {
                HomingAI();
            }

            Dust dust;
            for (int i = 0; i < 5; i++)
            {
                Color color = new Color(0, 255, 255);
                if (i > 3)
                    color = new Color(100, 100, 255);

                Vector2 dustBoxPosition = new Vector2(Projectile.position.X + 6, Projectile.position.Y + 6);
                int dustBoxWidth = Projectile.width - 12;
                int dustBoxHeight = Projectile.height - 12;
                dust = Dust.NewDustDirect(dustBoxPosition, dustBoxWidth, dustBoxHeight, DustID.Ice, 0f, 0f, 100, color, 1.5f);
                dust.noGravity = true;
                dust.velocity *= 0.1f;
                dust.velocity += Projectile.velocity * 0.1f;
                dust.position.X -= Projectile.velocity.X / 3f * (float)i;
                dust.position.Y -= Projectile.velocity.Y / 3f * (float)i;
            }
            if (Main.rand.Next(5) == 0)
            {
                Vector2 dustBoxPosition = new Vector2(Projectile.position.X + 6, Projectile.position.Y + 6);
                int dustBoxWidth = Projectile.width - 12;
                int dustBoxHeight = Projectile.height - 12;
                dust = Dust.NewDustDirect(dustBoxPosition, dustBoxWidth, dustBoxHeight, DustID.WaterCandle, 0f, 0f, 100, default, 0.8f);
                dust.velocity *= 0.25f;
                dust.velocity += Projectile.velocity * 0.5f;
            }

            Lighting.AddLight(Projectile.position, 0f, 0f, 0.5f);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 12; i++)
            {
                Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 211, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f, 0, default, 1f);
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10;
            return true;
        }
    }
}
