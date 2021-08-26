using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Buffs;
using TerraLeague.Gores;
using TerraLeague.NPCs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class TheFallenCelestialsDarkMagic_SoulShackles : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Soul Shackles");
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = 3;
            Projectile.alpha = 255;
            Projectile.scale = 1f;
            Projectile.timeLeft = 210;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            NPC NPC = Main.npc[(int)Projectile.ai[0]];
            Player player = Main.player[Projectile.owner];

            if (!NPC.active || Projectile.Distance(player.Center) > Items.Weapons.Abilities.SoulShackles.range)
            {
                ChainBreak(player.Center);
                Projectile.Kill();
            }
            else
            {
                if (Projectile.timeLeft < 180)
                {
                    if ((int)Projectile.ai[1] == 0)
                    {
                        Projectile.friendly = true;
                    }
                    Projectile.Center = NPC.Center;

                    Dust dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.Bottom.Y - NPC.height / 4f), NPC.width, NPC.height / 4, DustID.EnchantedNightcrawler, 0, -2, 0, new Color(159, 0, 255), 1.5f);
                    dust.noGravity = true;

                    if (Projectile.timeLeft == 1)
                    {
                        Projectile.friendly = true;
                        DustChain(player, (int)Projectile.Distance(player.Center) / 4, 2f);

                        for (int i = 0; i < 20; i++)
                        {
                            dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.EnchantedNightcrawler, 0, 0, 0, new Color(159, 0, 255), 1.5f);
                            dust.noGravity = true;
                        }

                        TerraLeague.PlaySoundWithPitch(Projectile.Center, 3, 54, -0.5f);
                        var sound = TerraLeague.PlaySoundWithPitch(Projectile.Center, 3, 4, -1f);
                        if (sound != null)
                            sound.Volume = sound.Volume / 3f;
                    }

                    if (Projectile.soundDelay == 0)
                    {
                        Projectile.soundDelay = 25;
                        TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 15, 0.5f - (Projectile.timeLeft / 180f));
                    }
                }
                else
                {
                    Vector2 npcPos = NPC.Center;
                    Vector2 playerPos = player.MountedCenter;

                    float projectileX = ((npcPos.X - playerPos.X) * (-(Projectile.timeLeft - 210) / 30f)) + playerPos.X;
                    float projectileY = ((npcPos.Y - playerPos.Y) * (-(Projectile.timeLeft - 210) / 30f)) + playerPos.Y;

                    Projectile.Center = new Vector2(projectileX, projectileY);
                }
            }

            float timepassed = ((210 - Projectile.timeLeft) / 210f);
            DustChain(player, (int)(Projectile.Distance(player.Center) * (1 + timepassed)) / 64, 1 + (timepassed * 0.5f));
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if ((int)Projectile.ai[1] == 0)
            {
                Projectile.ai[1] = 1;
                Projectile.friendly = false;
                target.AddBuff(BuffType<Slowed>(), 180);
            }
            else
            {
                target.AddBuff(BuffType<Stunned>(), 240);
            }
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        void DustChain(Player player, int loops, float scale)
        {
            Vector2 ChainLine = Projectile.position - player.Center;
            ChainLine.Normalize();

            for (int i = 0; i < loops; i++)
            {
                int distance = Main.rand.Next((int)Projectile.Distance(player.Center));
                Vector2 dustPoint = ChainLine * distance;

                Dust dust = Dust.NewDustDirect(dustPoint + player.Center, 1, 1, DustID.EnchantedNightcrawler, 0, 0, 100, new Color(159, 0, 255), scale);
                dust.noGravity = true;
            }
        }

        public void ChainBreak(Vector2 source)
        {
            Vector2 ChainLine = Projectile.position - source;
            ChainLine.Normalize();
            int links = (int)Projectile.Distance(source) / 32;

            for (int i = 0; i < links; i++)
            {
                int distance = 32 * i;
                Vector2 gorePoint = ChainLine * distance;

                int gore = Gore.NewGore(gorePoint + source, Vector2.Zero, GoreType<SoulShackleGoreA>(), 1f);

                Main.gore[gore].timeLeft /= 10;

                gorePoint = ChainLine * (distance + 16);
                gore = Gore.NewGore(gorePoint + source, Vector2.Zero, GoreType<SoulShackleGoreB>(), 1f);
                Main.gore[gore].timeLeft /= 15;
            }
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if ((int)Projectile.ai[0] == target.whoAmI && Projectile.friendly)
                return true;
            else
                return false;
        }


        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Request<Texture2D>("TerraLeague/Projectiles/TheFallenCelestialsDarkMagic_SoulShackleChain").Value;

            Vector2 position = Projectile.Center;
            Vector2 mountedCenter = Main.player[Projectile.owner].MountedCenter;
            mountedCenter.Y += 4;
            Rectangle? sourceRectangle = new Rectangle?();
            Vector2 origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);
            float num1 = (float)texture.Height;
            Vector2 vector2_4 = mountedCenter - position;
            float rotation = (float)Math.Atan2((double)vector2_4.Y, (double)vector2_4.X) - 1.57f;
            bool flag = true;
            if (float.IsNaN(position.X) && float.IsNaN(position.Y))
                flag = false;
            if (float.IsNaN(vector2_4.X) && float.IsNaN(vector2_4.Y))
                flag = false;
            while (flag)
            {
                if ((double)vector2_4.Length() < (double)num1 + 1.0)
                {
                    flag = false;
                }
                else
                {
                    Vector2 vector2_1 = vector2_4;
                    vector2_1.Normalize();
                    position += vector2_1 * num1;
                    Lighting.AddLight(position, 178 / 255f, 0, 1);
                    vector2_4 = mountedCenter - position;
                    Color color2 = Color.White;
                    Main.spriteBatch.Draw(texture, position - Main.screenPosition, sourceRectangle, color2, rotation, origin, 1f, SpriteEffects.None, 0.0f);
                }
            }

            texture = Request<Texture2D>("TerraLeague/Projectiles/TheFallenCelestialsDarkMagic_SoulShackleBorder").Value;
            origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);
            Vector2 BorderVector =  position - mountedCenter;
            BorderVector.Normalize();
            BorderVector = BorderVector * Items.Weapons.Abilities.SoulShackles.range;
            BorderVector += mountedCenter;
            Main.spriteBatch.Draw(texture, BorderVector - Main.screenPosition, sourceRectangle, Color.White, rotation + (float)Math.PI/2f, origin, 1f, SpriteEffects.None, 0.0f);

            return true;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
