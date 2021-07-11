using Microsoft.Xna.Framework;
using System;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class SerpentsEmbrace_Serpent : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 4;
            DisplayName.SetDefault("Serpent");
        }

        public override void SetDefaults()
        {
            projectile.arrow = true;
            projectile.width = 10;
            projectile.height = 10;
            projectile.alpha = 0;
            projectile.timeLeft = 1200;
            projectile.penetrate = 1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ranged = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = false;
            projectile.aiStyle = 1;
            aiType = ProjectileID.WoodenArrowFriendly;
        }

        public override void AI()
        {
            Dust dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Venom, 0f, 0f, 50, default);
            dust.noGravity = true;
            dust.velocity *= 0;
            Lighting.AddLight(projectile.position, 0.5f, 0, 0.5f);
            AnimateProjectile();
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Venom, 60 * 4);
            base.OnHitPlayer(target, damage, crit);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            //if (target.HasBuff(BuffID.Venom))
            //{
            //    int tfDamage = projectile.damage + (Items.Weapons.SerpentsEmbrace.MAGScaling * Main.player[projectile.owner].GetModPlayer<PLAYERGLOBAL>().MAG / 100);

            //    Vector2 center = Main.player[projectile.owner].MountedCenter;

            //    var targets = Targeting.GetAllNPCsInRange(center, 500, true);
            //    for (int i = 0; i < targets.Count; i++)
            //    {
            //        if (Main.npc[targets[i]].HasBuff(BuffID.Venom))
            //        {
            //            Projectile.NewProjectileDirect(center, TerraLeague.CalcVelocityToPoint(center, Main.npc[targets[i]].Center, 16), ProjectileType<SerpentsEmbrace_TwinFangs>(), tfDamage, 1, projectile.owner, targets[i]);
            //        }
            //    }
            //}
            target.AddBuff(BuffID.Venom, 60 * 4);

            base.OnHitNPC(target, damage, knockback, false);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(SoundID.Dig, projectile.Center);
            projectile.Kill();
            return false;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.t_Cactus, 0f, 0f, 100, default, 0.7f);
            }
        }

        public void AnimateProjectile()
        {
            projectile.frameCounter++;
            if (projectile.frameCounter >= 5)
            {
                projectile.frame++;
                projectile.frame %= 4;
                projectile.frameCounter = 0;
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            
        }
    }
}
