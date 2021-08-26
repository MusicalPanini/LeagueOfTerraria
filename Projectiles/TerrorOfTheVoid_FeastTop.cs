using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Buffs;
using TerraLeague.Items.Weapons;
using TerraLeague.Items.Weapons.Abilities;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class TerrorOfTheVoid_FeastTop : ModProjectile
    {
        float YDis = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Feast");
            ProjectileID.Sets.DontAttachHideToAlpha[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 32;
            Projectile.alpha = 255;
            Projectile.timeLeft = 100;
            Projectile.penetrate = -1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = false;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Projectile.scale = 3 + 2 * (int)Projectile.ai[0];
            Projectile.width = (int)(64 * Projectile.scale);
            Projectile.height = (int)(32 * Projectile.scale);
            DrawOriginOffsetX = -32;
            DrawOriginOffsetY = 32 + 32 * (int)Projectile.ai[0];

            Vector2 pos = Main.player[Projectile.owner].MountedCenter;
            pos.Y += YDis;
            Projectile.Center = pos;

            if ((int)Projectile.ai[1] != 1)
            {
                if (Projectile.timeLeft < 100 - 14)
                {
                    Projectile.alpha = 0;
                    Projectile.friendly = true;
                    Projectile.timeLeft = 48;
                    Projectile.extraUpdates = 1;
                    Projectile.ai[1] = 1;
                    TerraLeague.PlaySoundWithPitch(Main.player[Projectile.owner].MountedCenter, 3, 30, -1f);
                }
                else
                {
                    //YDis += -8f + -4 * (int)Projectile.ai[0];
                    YDis = (4 / 7f) * (float)Math.Pow(100 - Projectile.timeLeft - 14, 2) - (112);
                    YDis *= 1 + 0.5f * (int)Projectile.ai[0];
                    Projectile.alpha -= 10;
                }
            }
            if (Projectile.timeLeft > 40 && Projectile.timeLeft <= 42 && (int)Projectile.ai[1] == 1)
                YDis += 64 + 32 * (int)Projectile.ai[0];
            else if (Projectile.timeLeft <= 40 && (int)Projectile.ai[1] == 1)
            {
                Projectile.extraUpdates = 0;
                Projectile.friendly = false;

                if (Projectile.timeLeft <= 30)
                    Projectile.alpha += 255 / 30;
            }

            if (Main.rand.Next(0, 2) == 0)
            {
                Dust dustIndex = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleMoss, 0f, 0f, 100, default, Projectile.ai[0] + 1);
                dustIndex.noGravity = true;
                dustIndex.alpha = Projectile.alpha;
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (target.life <= Main.player[Projectile.owner].GetModPlayer<PLAYERGLOBAL>().feastStacks && !target.immortal)
            {
                Main.player[Projectile.owner].ApplyDamageToNPC(target, 99999, 0, 0, false);
            }

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.life <= 0)
            {
                Player player = Main.player[Projectile.owner];
                Feast feast = new Feast(GetModItem(ItemType<TerrorOfTheVoid>()));

                if (feast != null)
                {
                    feast.DoEfx(player, AbilityType.R);
                    player.GetModPlayer<PLAYERGLOBAL>().feastStacks += target.lifeMax;
                    CombatText.NewText(player.getRect(), new Color(89, 0, 77), "+" + target.lifeMax, true);
                }
            }

            base.OnHitNPC(target, damage, knockback, crit);
        }
    }
}
