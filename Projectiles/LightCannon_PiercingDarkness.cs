using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class LightCannon_PiercingDarkness : ModProjectile
    {
        readonly bool[] hasHitPlayer = new bool[200];

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Piercing Darkness");
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.timeLeft = 300;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if ((int)Projectile.ai[1] == 1)
            {
                Dust dust;
                for (int i = 0; i < 3; i++)
                {
                    Vector2 dustBoxPosition = new Vector2(Projectile.position.X + 6, Projectile.position.Y + 6);
                    int dustBoxWidth = Projectile.width - 12;
                    int dustBoxHeight = Projectile.height - 12;
                    dust = Dust.NewDustDirect(dustBoxPosition, dustBoxWidth, dustBoxHeight, DustID.Smoke, 0f, 0f, 100, default, 2);
                    dust.noGravity = true;
                    dust.velocity *= 0.1f;
                    dust.velocity += Projectile.velocity * 0.1f;
                    dust.position.X -= Projectile.velocity.X / 3f * (float)i;
                    dust.position.Y -= Projectile.velocity.Y / 3f * (float)i;
                }

                for (int i = 0; i < 3; i++)
                {
                    Vector2 pos = new Vector2(Projectile.Center.X + (-4 + (i * 4)), Projectile.Center.Y);
                    dust = Dust.NewDustPerfect(pos, 188);
                    dust.velocity /= 10;
                    dust.scale = 0.75f;
                    dust.alpha = 100;
                }

                for (int i = 0; i < 200; i++)
                {
                    Player healTarget = Main.player[i];
                    if (Projectile.Hitbox.Intersects(healTarget.Hitbox) && i != Projectile.owner)
                    {
                        HitPlayer(healTarget);
                    }
                }

            }
            else
            {
                //int dir = player.MountedCenter.X > Projectile.Center.X ? -1 : 1;
                //player.ChangeDir(dir);

                Projectile.Center = player.MountedCenter + new Vector2(-16, -14) + new Vector2(80, 0).RotatedBy(Projectile.velocity.ToRotation() + player.fullRotation) + Main.OffsetsPlayerOnhand[player.bodyFrame.Y / 56];

                for (int k = 0; k < 3; k++)
                {
                    int type = DustID.Smoke;
                    float scale = 0.8f;
                    if (k == 1)
                    {
                        type = 66;
                        scale = 1f;
                    }
                    
                    Vector2 position = Projectile.Center + ((float)Main.rand.NextDouble() * 6.28318548f).ToRotationVector2() * (12f - (float)(2 * 2));
                    Dust dust = Dust.NewDustDirect(position - Vector2.One * 8f, 16, 16, type, 0, 0, 0, default, scale);
                    dust.velocity = Vector2.Normalize(Projectile.Center - position) * 1.5f * (10f - (float)2 * 2f) / 10f;
                    dust.noGravity = true;
                    dust.customData = player;
                }

                Projectile.localAI[0] ++;
                if (Projectile.localAI[0]  > 15)
                {
                    Projectile.ai[1] = 1;
                    Projectile.friendly = true;
                    Projectile.timeLeft = 80;
                    Projectile.extraUpdates = 16;
                    //Projectile.velocity = new Vector2(10, 0).RotatedBy(Projectile.rotation);

                    TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 72, -1f);

                    if (Projectile.owner == Main.LocalPlayer.whoAmI)
                        player.GetModPlayer<PLAYERGLOBAL>().lifeToHeal += (int)Projectile.ai[0];
                }
            }

            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public void HitPlayer(Player player)
        {
            Terraria.Audio.SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 29), player.Center);

            Projectile.netUpdate = true;
            if (Projectile.owner == Main.LocalPlayer.whoAmI)
            {
                if (player.whoAmI != Projectile.owner)
                {
                    if (!hasHitPlayer[player.whoAmI])
                    {
                        Main.player[Projectile.owner].GetModPlayer<PLAYERGLOBAL>().SendHealPacket((int)Projectile.ai[0], player.whoAmI, -1, Projectile.owner);
                        hasHitPlayer[player.whoAmI] = true;
                    }
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            if ((int)Projectile.ai[1] == 1 && timeLeft > 0)
            {
                for (int i = 0; i < 12; i++)
                {
                    Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Smoke, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f, 0);
                    dust.noGravity = true;
                    dust.scale = 1 * (timeLeft / 80f) + 1;
                }
            }
            base.Kill(timeLeft);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10;
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }
    }
}
