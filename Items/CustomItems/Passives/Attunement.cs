﻿using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class Attunement : Passive
    {
        readonly int maxStacks;
        readonly decimal magicPerStack;
        readonly decimal armorPerStack;

        public Attunement(int MaxStacks, decimal MagicPerStack, decimal ArmorPerStack, LeagueItem item) : base(item)
        {
            maxStacks = MaxStacks;
            magicPerStack = MagicPerStack;
            armorPerStack = ArmorPerStack;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            return TooltipName("ATTUNEMENT") +
                LeagueTooltip.CreateColorString(PassiveSecondaryColor, "Kills grant stacks up to " + maxStacks +
                "\nGain " + magicPerStack + "% magic damage and " + armorPerStack + " armor per stack") +
                "\n" + LeagueTooltip.CreateColorString(PassiveSubColor, "Lose all stacks on death");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            player.GetDamage(DamageClass.Magic) += (float)magicPerStack * passiveStat * 0.01f;
            modPlayer.armor += (int)(armorPerStack * (int)passiveStat);
            base.UpdateAccessory(player, modItem);
        }

        public override void OnKilledNPC(NPC npc, ref int damage, ref bool crit, Player player)
        {
            AddStat(player, maxStacks, 1);

            base.OnKilledNPC(npc, ref damage, ref crit, player);
        }

        public override int PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, Player player)
        {
            passiveStat = 0;

            return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource, player);
        }
    }
}
