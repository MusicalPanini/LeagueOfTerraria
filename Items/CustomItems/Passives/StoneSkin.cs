﻿using TerraLeague.Buffs;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class StoneSkin : Passive
    {
        readonly int enemies;
        readonly int armorResist;

        public StoneSkin(int EnemyAmount, int ArmorResistBonus, LeagueItem legItem) : base(legItem)
        {
            enemies = EnemyAmount;
            armorResist = ArmorResistBonus;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            return TooltipName("STONE SKIN") + LeagueTooltip.CreateColorString(PassiveSecondaryColor, "While near at least " + enemies + " enemies, gain " + armorResist + " armor and resist");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            base.UpdateAccessory(player, modItem);
        }

        public override void PostPlayerUpdate(Player player)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            var npcs = Targeting.GetAllNPCsInRange(player.MountedCenter, 500);
            if (npcs.Count >= enemies)
            {
                player.AddBuff(BuffType<StonePlating>(), 5);
                modPlayer.armor += armorResist;
                modPlayer.resist += armorResist;
            }
            base.PostPlayerUpdate(player);
        }
    }
}
