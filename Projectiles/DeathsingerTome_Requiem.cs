using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class DeathsingerTome_Requiem : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            
            DisplayName.SetDefault("Requiem");
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
            //Projectile.extraUpdates = 8;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            if(Projectile.soundDelay == 0)
            {
                TerraLeague.DustLine(Projectile.Center, Projectile.Center + new Vector2(0, -500), 261, 0.25f, 3f, new Color(24, 86, 69, 255), true, Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-2, 2));

                Terraria.Audio.SoundEngine.PlaySound(new LegacySoundStyle(2, 88), Projectile.Center);
                if (Projectile.owner == Main.LocalPlayer.whoAmI)
                    Projectile.netUpdate = true;

                for (int i = 0; i < 3; i++)
                {
                    Gore gore = Gore.NewGoreDirect(Projectile.Center + new Vector2(0, -500), default, Main.rand.Next(61, 64), 1f);
                    gore.velocity.Y = gore.velocity.Y + 1.5f;
                }
            }
            Projectile.soundDelay = 100;

            if (!Main.npc[(int)Projectile.ai[0]].active)
            {
                Projectile.Kill();
            }
            else if (Projectile.Hitbox.Intersects(Main.npc[(int)Projectile.ai[0]].Hitbox))
            {
                Projectile.velocity = Vector2.Zero;
                Projectile.Center = Main.npc[(int)Projectile.ai[0]].Center;
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

                Projectile.Center = new Vector2(npc.Center.X, Projectile.Center.Y);

                Vector2 newMove = npc.Center - Projectile.Center;
                float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                move = newMove;
                AdjustMagnitude(ref move);
                Projectile.velocity = (10 * Projectile.velocity + move) / 2f;
                AdjustMagnitude(ref Projectile.velocity);

                //for (int i = 0; i < 3; i++)
                //{
                //    Vector2 dustBoxPosition = new Vector2(Projectile.position.X + 6, Projectile.position.Y + 6);
                //    int dustBoxWidth = Projectile.width - 12;
                //    int dustBoxHeight = Projectile.height - 12;
                //    Dust dust = Dust.NewDustDirect(dustBoxPosition, dustBoxWidth, dustBoxHeight, 261, 0f, 0f, 100, new Color(24, 86, 69, 255), 1.5f);
                //    dust.noGravity = true;
                //    dust.velocity *= 0.1f;
                //    dust.velocity += Projectile.velocity * 0.1f;
                //    dust.position.X -= Projectile.velocity.X / 3f * (float)i;
                //    dust.position.Y -= Projectile.velocity.Y / 3f * (float)i;
                //}

                //Dust dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 261, 0, 0, 0, new Color(24, 86, 69, 255), 1.5f);
                //dust2.noGravity = true;
                //dust2.velocity *= 3f;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            Terraria.Audio.SoundEngine.PlaySound(new LegacySoundStyle(2, 92), Projectile.Center);
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.AncientLight, 0, 0, 0, new Color(24, 86, 69, 150), 2f);
                dust.velocity *= 3f;
                dust.noGravity = true;
            }
            base.Kill(timeLeft);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if ((int)Projectile.ai[0] == target.whoAmI)
                return base.CanHitNPC(target);
            else
                return false;
        }

        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 12f)
            {
                vector *= 12f / magnitude;
            }
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
