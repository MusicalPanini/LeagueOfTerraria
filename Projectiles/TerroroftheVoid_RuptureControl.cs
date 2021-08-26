using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class TerrorOfTheVoid_RuptureControl : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rupture Control");
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
            Projectile.timeLeft = 16 * 24;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.extraUpdates = 2;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Projectile.ai[1]++;

            if (Projectile.ai[1] == 16)
            {
                Player player = Main.player[Projectile.owner];

                if (Main.LocalPlayer.whoAmI == Projectile.owner)
                {
                    int projType = ModContent.ProjectileType<TerrorOfTheVoid_RuptureSpike>();
                    FindRestingSpot(projType, out int xPos, out int yPos, out int yDis);
                    Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), new Vector2(xPos, yPos - 32), Vector2.Zero, ModContent.ProjectileType<TerrorOfTheVoid_RuptureSpike>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Main.rand.Next(0, 2) == 0 ? -1 : 1, (int)Projectile.ai[0]);
                }
                Projectile.ai[1] = 0;
            }

        }

		public void FindRestingSpot(int checkProj, out int worldX, out int worldY, out int pushYUp)
		{
			bool flag = false;
			Vector2 pointPoisition = Projectile.Center;
			int num = (int)pointPoisition.X / 16;
			int i = (int)pointPoisition.Y / 16;
			worldX = num * 16 + 8;
			pushYUp = 0;
			if (!flag)
			{
				for (; i < Main.maxTilesY - 10 && Main.tile[num, i] != null && !WorldGen.SolidTile(num, i) && Main.tile[num - 1, i] != null && !WorldGen.SolidTile(num - 1, i) && Main.tile[num + 1, i] != null && !WorldGen.SolidTile(num + 1, i); i++)
				{
				}
				i++;
			}
			i--;
			pushYUp -= 14;
			worldY = i * 16;
		}
	}
}
