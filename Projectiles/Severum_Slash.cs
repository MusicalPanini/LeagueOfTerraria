using Microsoft.Xna.Framework;
using System;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class Severum_Slash : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Severum");
            Main.projFrames[Projectile.type] = 28;
        }

        public override void SetDefaults()
        {
            Projectile.width = 136;
            Projectile.height = 128;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.alpha = 0;
            Projectile.timeLeft = 60;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.localNPCHitCooldown = 14;
            Projectile.usesLocalNPCImmunity = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Main.player[Projectile.owner].GetModPlayer<PLAYERGLOBAL>().lifeStealMelee += 2;

            if (player.GetModPlayer<PLAYERGLOBAL>().severumAmmo < 2)
            {
                Projectile.Kill();
            }

            Projectile.localNPCHitCooldown = (int)(14 * player.meleeSpeed);
            //if (player.channel && !player.noItems && !player.CCed)
            //{
            //    player.itemAnimation = 5;
            //    player.itemTime = 5;
            //    Projectile.rotation = player.itemRotation;
            //    Projectile.Center = player.MountedCenter + new Vector2(100, 0).RotatedBy(Projectile.rotation);
            //    Projectile.timeLeft = 60;

            //    AnimateProjectile();
            //}
            //else
            //{
            //    Projectile.Kill();
            //}

            float num;
            Vector2 vector = player.RotatedRelativePoint(player.MountedCenter, false);
            float num3 = 0f;
            player.itemAnimation = 5;
            player.itemTime = 5;
            num = 0f;
            if (Projectile.spriteDirection == -1)
            {
                num = 3.14159274f;
            }
            AnimateProjectile();
            if (Main.myPlayer == Projectile.owner)
            {
                if (player.channel && !player.noItems && !player.CCed)
                {
                    float scaleFactor6 = 1f;
                    if (player.inventory[player.selectedItem].shoot == Projectile.type)
                    {
                        scaleFactor6 = player.inventory[player.selectedItem].shootSpeed * Projectile.scale;
                    }
                    Vector2 vector19 = Main.MouseWorld - vector;
                    vector19.Normalize();
                    if (vector19.HasNaNs())
                    {
                        vector19 = Vector2.UnitX * (float)player.direction;
                    }
                    vector19 *= scaleFactor6;
                    if (vector19.X != Projectile.velocity.X || vector19.Y != Projectile.velocity.Y)
                    {
                        Projectile.netUpdate = true;
                    }
                    Projectile.velocity = vector19;
                }
                else
                {
                    Projectile.Kill();
                }
            }
            Vector2 vector20 = Projectile.Center + Projectile.velocity * 3f;

            Projectile.position = player.RotatedRelativePoint(player.MountedCenter, false) - Projectile.Size / 2f;
            Projectile.rotation = Projectile.velocity.ToRotation() + num;
            Projectile.spriteDirection = Projectile.direction;
            Projectile.timeLeft = 2;
            player.ChangeDir(Projectile.direction);
            player.heldProj = Projectile.whoAmI;
            player.itemRotation = MathHelper.WrapAngle((float)Math.Atan2((double)(Projectile.velocity.Y * (float)Projectile.direction), (double)(Projectile.velocity.X * (float)Projectile.direction)) + num3);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            //Main.player[Projectile.owner].GetModPlayer<PLAYERGLOBAL>().lifeToHeal++;

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            return true;
        }

        public override void Kill(int timeLeft)
        {

            base.Kill(timeLeft);
        }

        public void AnimateProjectile()
        {
            Projectile.frameCounter++;
            int frameCounterMax = Main.player[Projectile.owner].meleeSpeed < 0.5f ? 1 : 2;
            if (Projectile.frameCounter >= frameCounterMax)
            {
                Projectile.frame++;
                Projectile.frame %= 28;
                Projectile.frameCounter = 0;
            }

            if (Projectile.frameCounter == 1 && Projectile.frame % 7 == 0)
            {
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
            }

            if (Projectile.frameCounter == 1 && Projectile.frame % 7 == 2)
            {
                Projectile.friendly = true;
                if (Main.LocalPlayer.whoAmI == Projectile.owner)
                    Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), Projectile.Center, (Main.player[Projectile.owner].MountedCenter - Projectile.Center).RotatedBy(Main.rand.NextFloat(-0.5f, 0.5f)) / -6, ProjectileID.DD2SquireSonicBoom, Projectile.damage/2, Projectile.knockBack, Projectile.owner);
                Main.player[Projectile.owner].GetModPlayer<PLAYERGLOBAL>().severumAmmo -= 2;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
            }
        }
    }
}
