using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class CrystalineVoidEnergy_VoidEnergy : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            
            DisplayName.SetDefault("Void Energy");
        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.alpha = 255;
            Projectile.scale = 1.2f;
            Projectile.timeLeft = 900;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.extraUpdates = 2;
        }

        public override void AI()
        {
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

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.netUpdate = true;

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            TerraLeagueNPCsGLOBAL modNPC = target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>();
            PLAYERGLOBAL modPlayer = Main.player[Projectile.owner].GetModPlayer<PLAYERGLOBAL>();


            target.AddBuff(BuffType<CausticWounds>(), 240);
            modNPC.CausticWounds = true;
            modNPC.CausticStacks++;

            int stacks = target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().CausticStacks;
            if (stacks == 5)
            {
                Projectile.DamageType = DamageClass.Magic;

                int damCap = (int)(modPlayer.MAG + 50);

                damage += (target.lifeMax - target.life) / 4 > damCap ? damCap : (target.lifeMax - target.life) / 4;

                Projectile.netUpdate = true;
                Projectile.ai[0] = 1;

                modPlayer.CausticWoundsEffect(target);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    modPlayer.PacketHandler.SendCausticEFX(-1, Projectile.owner, target.whoAmI);

            }
            if (stacks > 5)
            {
                modNPC.CausticStacks = 1;
            }

            if (Main.netMode == NetmodeID.MultiplayerClient)
                TerraLeagueNPCsGLOBAL.PacketHandler.SendSyncStats(-1, Main.LocalPlayer.whoAmI, 1, target.whoAmI, modNPC.CausticStacks);

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
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
