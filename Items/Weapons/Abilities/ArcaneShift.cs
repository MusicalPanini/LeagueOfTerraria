﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Buffs;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons.Abilities
{
    public class ArcaneShift : Ability
    {
        public ArcaneShift(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Arcane Shift";
        }

        public override string GetIconTexturePath()
        {
            return "TerraLeague/AbilityImages/ArcaneShift";
        }

        public override string GetAbilityTooltip()
        {
            return "Blink up to 20 blocks away and fire a projectile at the nearest enemy";

        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return (int)(abilityItem.Item.damage);
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.RNG:
                    return 50;
                case DamageType.MAG:
                    return 75;
                default:
                    return 0;
            }
        }

        public override int GetRawCooldown()
        {
            return 12;
        }

        public override int GetBaseManaCost()
        {
            return 50;
        }

        public override string GetDamageTooltip(Player player)
        {
            return LeagueTooltip.TooltipValue(GetAbilityBaseDamage(player), false, "",
                new Tuple<int, ScaleType>(GetAbilityScalingAmount(player, DamageType.RNG), ScaleType.Ranged),
                new Tuple<int, ScaleType>(GetAbilityScalingAmount(player, DamageType.MAG), ScaleType.Magic)
                ) + " magic damage";
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return true;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost()))
            {
                int distance = 20;

                float xDis = Main.MouseWorld.X - player.position.X;
                float yDis = Main.MouseWorld.Y - player.position.Y;

                if (xDis < -distance * 16)
                    xDis = -distance * 16;
                else if (xDis > distance * 16)
                    xDis = distance * 16;

                if (yDis < -distance * 16)
                    yDis = -distance * 16;
                else if (yDis > distance * 16)
                    yDis = distance * 16;

                float newX = xDis + player.position.X;
                float newY = yDis + player.position.Y;

                Vector2 teleportPos = default;
                teleportPos.X = newX;
                if (player.gravDir == 1f)
                {
                    teleportPos.Y = newY - (float)player.height;
                }
                else
                {
                    teleportPos.Y = (float)Main.screenHeight - newY;
                }
                teleportPos.X -= (float)(player.width / 2);
                if (teleportPos.X > 50f && teleportPos.X < (float)(Main.maxTilesX * 16 - 50) && teleportPos.Y > 50f && teleportPos.Y < (float)(Main.maxTilesY * 16 - 50))
                {
                    int blockX = (int)(teleportPos.X / 16f);
                    int blockY = (int)(teleportPos.Y / 16f);
                    if ((Main.tile[blockX, blockY].wall != 87 || !((double)blockY > Main.worldSurface) || NPC.downedPlantBoss) && !Collision.SolidCollision(teleportPos, player.width, player.height))
                    {
                        player.velocity = TerraLeague.CalcVelocityToPoint(player.MountedCenter, teleportPos, player.velocity.Length());
                        player.Teleport(teleportPos, -1, 0);
                        NetMessage.SendData(MessageID.Teleport, -1, -1, null, 0, (float)player.whoAmI, teleportPos.X, teleportPos.Y, 10, 0, 0);

                        Projectile.NewProjectileDirect(player.GetProjectileSource_Item(abilityItem.Item), player.Center, new Vector2(0, 0), ProjectileType<NezuksGauntlet_ArcaneShift>(), GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.RNG) + GetAbilityScaledDamage(player, DamageType.MAG), 0, player.whoAmI, -2);
                        player.CheckMana(GetBaseManaCost(), true);

                        DoEfx(player, type);
                        SetCooldowns(player, type);
                    }
                }
            }
        }

        public override void Efx(Player player)
        {
            TerraLeague.PlaySoundWithPitch(player.position, 2, 72, 0.5f);
            for (int i = 0; i < 20; i++)
            {
                Dust dust = Dust.NewDustDirect(player.position - (Vector2.One * 16), 32, 32, DustID.GoldFlame, 0, 0, 0, default, 4);
                dust.noGravity = true;
                dust.noLight = true;
                dust.velocity *= 2;
            }
            base.Efx(player);
        }
    }
}
