using Microsoft.Xna.Framework;
using System.IO;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace TerraLeague.Projectiles
{
	public class ZzRotFlyer_PShot : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Plasma");
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.alpha = 255;
            Projectile.timeLeft = 600;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            if (Projectile.soundDelay == 0)
            {
                TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 45, 0.25f);
                TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 91, 0.5f);
            }
            Projectile.soundDelay = 10;

            Lighting.AddLight(Projectile.position, 0.75f, 0f, 0.75f);
            for (int i = 0; i < 3; i++)
            {
                Vector2 dustBoxPosition = new Vector2(Projectile.position.X + 0, Projectile.position.Y + 0);
                int dustBoxWidth = Projectile.width - 8;
                int dustBoxHeight = Projectile.height - 8;
                Dust dust = Dust.NewDustDirect(dustBoxPosition, dustBoxWidth, dustBoxHeight, 112, 0f, 0f, 255, new Color(59, 0, 255), 2f);
                dust.noGravity = true;
                dust.velocity *= 0.1f;
                dust.position.X -= Projectile.velocity.X / 3f * (float)i;
                dust.position.Y -= Projectile.velocity.Y / 3f * (float)i;
            }
        }

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            target.GetModPlayer<PLAYERGLOBAL>().AddVoidInfluence(50, false);

            base.ModifyHitPlayer(target, ref damage, ref crit);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, 8, 8, 112, 0f, 0f, 255, new Color(59, 0, 255), 3.5f);
                dust.noGravity = true;
                dust.noLight = true;
            }
        }
    }
}
