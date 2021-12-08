using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class MercuryHammer_CrushingBlow : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crushing Blow");
            Main.projFrames[Projectile.type] = 7;
        }

        public override void SetDefaults()
        {
            Projectile.width = 62;
            Projectile.height = 62;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
            Projectile.timeLeft = 13;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.localNPCHitCooldown = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().channelProjectile = true;
            Projectile.hide = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.localAI[0]++;
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
            num = 0f;
            if (Projectile.spriteDirection == -1)
            {
                num = 3.14159274f;
            }

            Projectile.Center = player.RotatedRelativePoint(player.MountedCenter, false) + (Projectile.velocity.SafeNormalize(Vector2.One) * 16) + new Vector2(0, -8);
            Projectile.rotation = Projectile.velocity.ToRotation() + num;
            Projectile.spriteDirection = Projectile.velocity.X > 0 ? 1 : -1;
            player.ChangeDir(Projectile.spriteDirection);
            player.heldProj = Projectile.whoAmI;
            player.itemRotation = MathHelper.WrapAngle((float)Math.Atan2((double)(Projectile.velocity.Y * (float)Projectile.spriteDirection), (double)(Projectile.velocity.X * (float)Projectile.spriteDirection)) + num3);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            //Main.player[Projectile.owner].GetModPlayer<PLAYERGLOBAL>().lifeToHeal++;

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void Kill(int timeLeft)
        {

            base.Kill(timeLeft);
        }

        public void AnimateProjectile()
        {
            Projectile.frameCounter++;
            int frameCounterMax = 2;
            if (Projectile.frameCounter >= frameCounterMax)
            {
                Projectile.frame++;
                Projectile.frame %= 7;
                Projectile.frameCounter = 0;
            }
        }

        public override void PostDraw(Color lightColor)
        {
            float rotation = Projectile.rotation;
            if (Projectile.spriteDirection == 1)
                rotation += MathHelper.PiOver2 - (Projectile.localAI[0] * MathHelper.PiOver2 / 6f);
            else
                rotation -= MathHelper.PiOver2 - (Projectile.localAI[0] * MathHelper.PiOver2 / 6f) + MathHelper.Pi;

            Vector2 offset = Projectile.spriteDirection == 1 ? new Vector2(0, 37) : new Vector2(24, 62);

            Texture2D texture = TextureAssets.Item[ItemType<Items.Weapons.MercuryHammer>()].Value;
            Main.spriteBatch.Draw
            (
                texture,
                new Vector2
                (
                    Projectile.position.X - Main.screenPosition.X + Projectile.width * 0.5f,
                    Projectile.position.Y - Main.screenPosition.Y + Projectile.height * 0.5f
                ) + new Vector2(0, 8),
                new Rectangle(0, 0, texture.Width, texture.Height),
                Color.White,
                rotation,
                offset,
                Projectile.spriteDirection == 1 ? Projectile.scale : Projectile.scale,
                Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.None,
                0f
            );
        }
    }
}
