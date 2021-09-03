﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CompleteItems;
using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class Shock : Passive
    {
        readonly int percentManaUsage;
        readonly int percentManaDamage;

        public Shock(int PercentManaUsage, int PercentManaDamage, LeagueItem legItem) : base(legItem)
        {
            percentManaUsage = PercentManaUsage;
            percentManaDamage = PercentManaDamage;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            return TooltipName("SHOCK") + LeagueTooltip.CreateColorString(PassiveSecondaryColor, "Melee and ranged damage will consume " + percentManaUsage + "% of your current mana and deal " + percentManaDamage + "% of your current mana as additional damage")
                + "\n" + LeagueTooltip.CreateColorString(PassiveSubColor, "This effect is dissable while below 20% max mana");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            base.UpdateAccessory(player, modItem);
        }

        public override void NPCHit(Item item, NPC target, ref int damage, ref float knockback, ref bool crit, ref int OnHitDamage, Player player)
        {
            if (player.statManaMax2/5 < player.statMana)
            {
                PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
                modPlayer.meleeFlatDamage += (int)(player.statMana * percentManaDamage * 0.01);
                player.CheckMana((int)(player.statMana * percentManaUsage * 0.01), true);
            }

            base.NPCHit(item, target, ref damage, ref knockback, ref crit, ref OnHitDamage, player);
        }

        public override void NPCHitWithProjectile(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection, ref int OnHitDamage, Player player)
        {
            if (proj.DamageType == DamageClass.Melee || proj.DamageType == DamageClass.Ranged) /*&& !TerraLeague.DoNotCountRangedDamage(proj)*/
            {
                if (player.statManaMax2 / 5 < player.statMana)
                {
                    PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
                    modPlayer.meleeFlatDamage += (int)(player.statMana * percentManaDamage * 0.01);
                    modPlayer.rangedFlatDamage += (int)(player.statMana * percentManaDamage * 0.01);
                    player.CheckMana((int)(player.statMana * percentManaUsage * 0.01), true);
                }
            }

            base.NPCHitWithProjectile(proj, target, ref damage, ref knockback, ref crit, ref hitDirection, ref OnHitDamage, player);
        }
    }
}
