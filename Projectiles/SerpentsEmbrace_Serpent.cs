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
            Main.projFrames[Projectile.type] = 4;
            DisplayName.SetDefault("Serpent");
        }

        public override void SetDefaults()
        {
            Projectile.arrow = true;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.alpha = 0;
            Projectile.timeLeft = 1200;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.aiStyle = 1;
            AIType = ProjectileID.WoodenArrowFriendly;
        }

        public override void AI()
        {
            Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.CorruptPlants, 0f, 0f, 50, default);
            dust.noGravity = true;
            dust.velocity *= 0;
            Lighting.AddLight(Projectile.position, 0.5f, 0, 0.5f);
            AnimateProjectile();
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Venom, 60 * 2);
            base.OnHitPlayer(target, damage, crit);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            //if (target.HasBuff(BuffID.Venom))
            //{
            //    int tfDamage = Projectile.damage + (Items.Weapons.SerpentsEmbrace.MAGScaling * Main.player[Projectile.owner].GetModPlayer<PLAYERGLOBAL>().MAG / 100);

            //    Vector2 center = Main.player[Projectile.owner].MountedCenter;

            //    var targets = Targeting.GetAllNPCsInRange(center, 500, true);
            //    for (int i = 0; i < targets.Count; i++)
            //    {
            //        if (Main.npc[targets[i]].HasBuff(BuffID.Venom))
            //        {
            //            Projectile.NewProjectileDirect(center, TerraLeague.CalcVelocityToPoint(center, Main.npc[targets[i]].Center, 16), ProjectileType<SerpentsEmbrace_TwinFangs>(), tfDamage, 1, Projectile.owner, targets[i]);
            //        }
            //    }
            //}
            target.AddBuff(BuffID.Venom, 60 * 4);

            base.OnHitNPC(target, damage, knockback, false);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
            Projectile.Kill();
            return false;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.t_Cactus, 0f, 0f, 100, default, 0.7f);
            }
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

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            
        }
    }
}
