using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Terraria.Audio;

namespace TerraLeague.Projectiles
{
    class RadiantStaff_LucentSingularity : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lucent Singularity");
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.timeLeft = 360;
            Projectile.penetrate = 1000;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffType<Illuminated>(), 300);

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void AI()
        {
            if (Projectile.width == 8)
            {
                if (Projectile.velocity.X < 0 && Projectile.Center.X < Projectile.ai[0])
                {
                    if (Projectile.velocity.Y < 0 && Projectile.Center.Y < Projectile.ai[1])
                    {
                        Projectile.Center = new Vector2(Projectile.ai[0], Projectile.ai[1]);
                        Prime();
                    }
                    else if (Projectile.velocity.Y >= 0 && Projectile.Center.Y > Projectile.ai[1])
                    {
                        Projectile.Center = new Vector2(Projectile.ai[0], Projectile.ai[1]);
                        Prime();
                    }
                }
                else if (Projectile.velocity.X >= 0 && Projectile.Center.X > Projectile.ai[0])
                {
                    if (Projectile.velocity.Y < 0 && Projectile.Center.Y < Projectile.ai[1])
                    {
                        Projectile.Center = new Vector2(Projectile.ai[0], Projectile.ai[1]);
                        Prime();
                    }
                    else if (Projectile.velocity.Y >= 0 && Projectile.Center.Y > Projectile.ai[1])
                    {
                        Projectile.Center = new Vector2(Projectile.ai[0], Projectile.ai[1]);
                        Prime();
                    }
                }

                if (Projectile.timeLeft < 300)
                {
                    Prime();
                }

                for (int i = 0; i < 3; i++)
                {
                    Vector2 dustBoxPosition = new Vector2(Projectile.position.X + 6, Projectile.position.Y + 6);
                    int dustBoxWidth = Projectile.width - 12;
                    int dustBoxHeight = Projectile.height - 12;
                    Dust dust = Dust.NewDustDirect(dustBoxPosition, dustBoxWidth, dustBoxHeight, DustID.GoldFlame, 0f, 0f, 100, default, 1.5f);
                    dust.noGravity = true;
                    dust.velocity *= 0.1f;
                    dust.velocity += Projectile.velocity * 0.1f;
                    dust.position.X -= Projectile.velocity.X / 3f * (float)i;
                    dust.position.Y -= Projectile.velocity.Y / 3f * (float)i;
                }
            }
            else
            {
                if (Projectile.soundDelay == 0)
                {
                    Projectile.soundDelay = 25;
                    TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 15, 0.5f - (Projectile.timeLeft / 300f));
                }

                Dust dust = Dust.NewDustDirect(Projectile.Center - (Vector2.One*8), 16, 16, DustID.GoldFlame, 0, 0, 0, default, 1.5f);
                dust.velocity *= 0;
                dust.noGravity = true;

                TerraLeague.DustBorderRing(Projectile.width / 2, Projectile.Center, 246, default, 1, true, true, 0.05f);

                if (Projectile.timeLeft % 15 == 0)
                {
                    Targeting.GiveNPCsInRangeABuff(Projectile.Center, Projectile.width / 2f, BuffType<Buffs.Slowed>(), 15, true, true);
                }
            }

            if (Projectile.timeLeft <= 2)
                Projectile.friendly = true;
            
            if (Projectile.timeLeft < 30)
            {
                Projectile.alpha += 9;
            }
        }

        public override void Kill(int timeLeft)
        {
            Projectile.friendly = true;
            Terraria.Audio.SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 20), Projectile.position);

            TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 15, 0.5f);

            TerraLeague.DustRing(246, Projectile, default);

            base.Kill(timeLeft);
        }

        public void Prime()
        {
            Projectile.velocity = Vector2.Zero;
            Projectile.timeLeft = 299;
            Projectile.alpha = 255;
            Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
            Projectile.width = 256;
            Projectile.height = 256;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (Projectile.friendly && !target.townNPC)
                return Targeting.IsHitboxWithinRange(Projectile.Center, target.Hitbox, Projectile.width / 2);
            else
                return false;
        }

        public override bool? CanCutTiles()
        {
            return Projectile.friendly;
        }

        public override void PostDraw(Color lightColor)
        {
            if (Projectile.width != 8)
            {
                int rad = (Projectile.width / 2);
                TerraLeague.DrawCircle(Projectile.Center, rad, Color.Yellow);
                TerraLeague.DrawCircle(Projectile.Center, rad - (rad * Projectile.timeLeft / 300f), Color.Yellow * 0.5f);
            }
            base.PostDraw(lightColor);
        }
    }
}
