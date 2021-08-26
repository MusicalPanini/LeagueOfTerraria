using System;
using TerraLeague.Items.Weapons;
using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Buffs
{
    public class CelestialExpansion : ModBuff
    {
        public bool initial = true;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Celestial Expansion");
            Description.SetDefault("Expanding your horizon!" +
                "\nsummon damage has been increased!");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;

        }
        public override void Update(Player player, ref int buffIndex)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            modPlayer.minionFlatDamage += Items.Weapons.Abilities.CelestialExpansion.GetBonusDamage(modPlayer);

            if (player.buffTime[buffIndex] == 1)
            {
                if (player.CheckMana(30, true))
                {
                    player.manaRegenDelay = 120;
                }
                else
                {
                    player.ClearBuff(Type);
                }
                player.buffTime[buffIndex] = 61;
            }
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
