using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class ZzRotFlyer_Shot : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Void Energy");
        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.ignoreWater = false;
            Projectile.alpha = 255;
            Projectile.scale = 1.2f;
            Projectile.timeLeft = 900;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            if (Projectile.soundDelay == 0)
            {
                //TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 45, 0.25f);
                TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 91, 0f);
            }
            Projectile.soundDelay = 10;

            if (Projectile.timeLeft < 896)
            {
                for (int i = 0; i < 10; i++)
                {
                    float x2 = Projectile.position.X - Projectile.velocity.X / 10f * (float)i;
                    float y2 = Projectile.position.Y - Projectile.velocity.Y / 10f * (float)i;
                    int num141 = Dust.NewDust(new Vector2(x2, y2), 1, 1, 112, 0f, 0f, 0, new Color(59, 0, 255), 0.5f);
                    Main.dust[num141].alpha = Projectile.alpha;
                    Main.dust[num141].position.X = x2;
                    Main.dust[num141].position.Y = y2;
                    Dust obj77 = Main.dust[num141];
                    obj77.velocity *= 0f;
                    Main.dust[num141].noGravity = true;
                }
            }
        }

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            target.GetModPlayer<PLAYERGLOBAL>().AddVoidInfluence(25, false);

            base.ModifyHitPlayer(target, ref damage, ref crit);
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }

        public override bool? CanHitNPC(NPC target)
        {
            return base.CanHitNPC(target);
        }
    }
}
