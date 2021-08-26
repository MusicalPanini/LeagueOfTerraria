using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class CrystalStaff_PrimordialBurst : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Primordial Burst");
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.timeLeft = 300;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.alpha = 255;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }


        public override void AI()
        {
            if (Projectile.soundDelay == 0)
            {
                TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 8, 0f);
                TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 113, -0.5f);
            }
            Projectile.soundDelay = 100;

            if (Projectile.ai[0] != -2)
            {
                Projectile.friendly = true;

                NPC npc = Main.npc[(int)Projectile.ai[0]];

                if (!npc.active && Projectile.owner == Main.LocalPlayer.whoAmI)
                    Projectile.Kill();

                float MaxSpeed = 18;

                float XDist = (float)npc.Center.X - Projectile.Center.X;
                float YDist = (float)npc.Center.Y - Projectile.Center.Y;

                float TrueDist = (float)System.Math.Sqrt((double)(XDist * XDist + YDist * YDist));
                if (TrueDist > MaxSpeed)
                {
                    TrueDist = MaxSpeed / TrueDist;
                    XDist *= TrueDist;
                    YDist *= TrueDist;
                    int num118 = (int)(XDist * 1000f);
                    int num119 = (int)(Projectile.velocity.X * 1000f);
                    int num120 = (int)(YDist * 1000f);
                    int num121 = (int)(Projectile.velocity.Y * 1000f);
                    if (num118 != num119 || num120 != num121)
                    {
                        Projectile.netUpdate = true;
                    }

                    if (Projectile.timeLeft > 270)
                    {
                        Projectile.velocity.X = XDist * (1 - ((Projectile.timeLeft - 270) / 30f));
                        Projectile.velocity.Y = YDist * (1 - ((Projectile.timeLeft - 270) / 30f));
                    }
                    else
                    {
                        Projectile.velocity.X = XDist;
                        Projectile.velocity.Y = YDist;
                    }

                }
            }

            for (int i = 0; i < 3; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position + Vector2.One * 4, Projectile.width - 8, Projectile.height - 8, i == 1 ? 173 : 172, 0, 0, 0, default, 3f);
                dust.noGravity = true;
                dust.velocity *= 0.3f;
                dust.velocity += Projectile.velocity * 0.3f;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage = (int)(damage * (2 - (target.life / (float)target.lifeMax)));
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if ((int)Projectile.ai[0] == target.whoAmI)
                return base.CanHitNPC(target);
            else
                return false;
        }


        public override void Kill(int timeLeft)
        {
            TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 45, -0.5f);
            Terraria.Audio.SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode.WithVolume(1f), Projectile.position);

            Dust dust;
            for (int i = 0; i < 25; i++)
            {
                dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.WaterCandle, 0, 0, 0, default, 2f);
                dust.velocity *= 1.4f;
                dust.noGravity = true;
            }
            for (int i = 0; i < 40; i++)
            {
                dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame, 0, 0, 0, default, 2f);
                dust.noGravity = true;
                dust.velocity *= 5f;
                dust.color = new Color(255, 0, 220);
                dust.noGravity = true;

                dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.WaterCandle, 0, 0, 0, default, 2f);
                dust.velocity *= 3f;
                dust.color = new Color(255, 0, 220);
                dust.noGravity = true;
            }
            base.Kill(timeLeft);
        }
    }
}
