using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class ChemCrossbow_Contaminate : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            
            DisplayName.SetDefault("Contaminate");
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.alpha = 255;
            Projectile.scale = 1.2f;
            Projectile.timeLeft = 301;
            Projectile.extraUpdates = 120;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            if(Projectile.soundDelay == 0)
                Terraria.Audio.SoundEngine.PlaySound(new LegacySoundStyle(2, 73), Projectile.Center);
            Projectile.soundDelay = 100;
            if (!Main.npc[(int)Projectile.ai[0]].active)
            {
                Projectile.Kill();
            }
            else
            {
                Projectile.timeLeft = 300;

                if (Projectile.localAI[0]  == 0f)
                {
                    AdjustMagnitude(ref Projectile.velocity);
                    Projectile.localAI[0]  = 1f;
                }
                Vector2 move = Vector2.Zero;

                NPC npc = Main.npc[(int)Projectile.ai[0]];

                Vector2 newMove = npc.Center - Projectile.Center;
                float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                move = newMove;
                AdjustMagnitude(ref move);
                Projectile.velocity = (10 * Projectile.velocity + move) / 20f;
                AdjustMagnitude(ref Projectile.velocity);

                Dust dust = Dust.NewDustPerfect(Projectile.position, 200, Vector2.Zero, 0, new Color(0, 192, 255), 1f);
                dust.noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage *= target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().DeadlyVenomStacks + 1;
            crit = false;

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Dirt, 0,0,0, new Color(0, 192, 255), 2);
                dust.noGravity = true;
            }
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);

            base.Kill(timeLeft);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if ((int)Projectile.ai[0] == target.whoAmI)
                return true;
            else
                return false;
        }

        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 20f)
            {
                vector *= 8f / magnitude;
            }
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
