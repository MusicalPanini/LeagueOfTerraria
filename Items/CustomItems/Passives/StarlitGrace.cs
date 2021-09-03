﻿using Microsoft.Xna.Framework;
using System.Collections.Generic;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class StarlitGrace : Passive
    {
        readonly int heal;
        readonly int scaling;
        readonly int maxStacks;

        public StarlitGrace(int Heal, int Scaling, int MaxStacks, int Cooldown, LeagueItem legItem) : base(legItem)
        {
            heal = Heal;
            scaling = Scaling;
            maxStacks = MaxStacks;
            passiveCooldown = Cooldown;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            return TooltipName("Starlit Grace") + LeagueTooltip.CreateColorString(PassiveSecondaryColor, "Dealing damage heals the lowest health nearby player for ") + LeagueTooltip.TooltipValue(heal, true, "")
                + "\n" + LeagueTooltip.CreateColorString(PassiveSecondaryColor, "Every second in combat increases this heal by " + scaling + "% up to " + (maxStacks * scaling) + "%")
                + "\n" + LeagueTooltip.CreateColorString(PassiveSubColor, passiveCooldown + " second cooldown");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            base.UpdateAccessory(player, modItem);
        }

        public override void PostPlayerUpdate(Player player)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (modPlayer.CombatTimer >= 240)
            {
                passiveStat = 0;
            }
            if (modPlayer.CombatTimer < 240 && passiveStat < 60 * maxStacks)
            {
                AddStat(player, 60 * maxStacks, 1);
            }

            base.PostPlayerUpdate(player);
        }

        public override void NPCHit(Item item, NPC target, ref int damage, ref float knockback, ref bool crit, ref int OnHitDamage, Player player)
        {
            Heal(player);

            base.NPCHit(item, target, ref damage, ref knockback, ref crit, ref OnHitDamage, player);
        }

        public override void NPCHitWithProjectile(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection, ref int OnHitDamage, Player player)
        {
            Heal(player);

            base.NPCHitWithProjectile(proj, target, ref damage, ref knockback, ref crit, ref hitDirection, ref OnHitDamage, player);
        }

        void Heal(Player player)
        {
            if (cooldownCount == 0)
            {
                if (Main.LocalPlayer.whoAmI == player.whoAmI)
                {
                    List<int> players = Targeting.GetAllPlayersInRange(player.MountedCenter, 200, player.whoAmI, player.team);

                    int lowestLife = 99999;
                    int chosen = -1;

                    for (int i = 0; i < players.Count; i++)
                    {
                        Player target = Main.player[players[i]];
                        int lifeCheck = target.GetModPlayer<PLAYERGLOBAL>().GetRealHeathWithoutShield();
                        if (lifeCheck < lowestLife)
                        {
                            lowestLife = lifeCheck;
                            chosen = target.whoAmI;
                        }
                    }

                    if (chosen != -1)
                    {
                        PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
                        Projectile.NewProjectileDirect(player.GetProjectileSource_Item(player.HeldItem), player.MountedCenter, Vector2.Zero, ProjectileType<Item_Heal>(), modPlayer.ScaleValueWithHealPower(heal + (int)(heal * 0.01f * (scaling * ((int)passiveStat / 60)))), 0, player.whoAmI, chosen);
                        SetCooldown(player);
                    }
                }
            }
        }
    }
}
