using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Buffs;
using TerraLeague.Projectiles.Homing;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class Summoner_Smite : HomingProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Smite");
            base.SetStaticDefaults();
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
            Projectile.extraUpdates = 8;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            CanOnlyHitTarget = true;
            CanRetarget = false;
            TurningFactor = 0f;
            MaxVelocity = 16;
        }

        public override void AI()
        {
            if(Projectile.soundDelay == 0)
            {
                Terraria.Audio.SoundEngine.PlaySound(new LegacySoundStyle(2, 88), Projectile.Center);
                if (Projectile.owner == Main.LocalPlayer.whoAmI)
                    Projectile.netUpdate = true;
            }
            Projectile.soundDelay = 100;

            if (Projectile.Hitbox.Intersects(Main.npc[(int)Projectile.ai[0]].Hitbox))
            {
                Projectile.velocity = Vector2.Zero;
                Projectile.Center = Main.npc[(int)Projectile.ai[0]].Center;
            }
            else
            {
                HomingAI();

                for (int i = 0; i < 3; i++)
                {
                    Vector2 dustBoxPosition = new Vector2(Projectile.position.X + 6, Projectile.position.Y + 6);
                    int dustBoxWidth = Projectile.width - 12;
                    int dustBoxHeight = Projectile.height - 12;
                    Dust dust = Dust.NewDustDirect(dustBoxPosition, dustBoxWidth, dustBoxHeight, DustID.AncientLight, 0f, 0f, 100, new Color(255, 106, 0, 150), 1.5f);
                    dust.noGravity = true;
                    dust.velocity *= 0.1f;
                    dust.velocity += Projectile.velocity * 0.1f;
                    dust.position.X -= Projectile.velocity.X / 3f * (float)i;
                    dust.position.Y -= Projectile.velocity.Y / 3f * (float)i;
                }

                Dust dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.AncientLight, 0, 0, 0, new Color(255, 106, 0, 150), 1f);
                dust2.noGravity = true;
                dust2.velocity *= 3f;
                Lighting.AddLight(Projectile.position, 1f, 1f, 0f);
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.life <= 0 || NPCID.Sets.ShouldBeCountedAsBoss[target.type])
                Main.player[Projectile.owner].GetModPlayer<PLAYERGLOBAL>().lifeToHeal += (int)(Main.player[Projectile.owner].GetModPlayer<PLAYERGLOBAL>().maxLifeLastStep * 0.15);

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            Terraria.Audio.SoundEngine.PlaySound(new LegacySoundStyle(2, 92), Projectile.Center);
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.AncientLight, 0, 0, 0, new Color(255, 106, 0, 150), 2f);
                dust.velocity *= 3f;
                dust.noGravity = true;
            }

            base.Kill(timeLeft);
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
