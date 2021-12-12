using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;
using Terraria.ID;

namespace TerraLeague.Buffs
{
    public class AngelsProtection : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Angel's Blessing");
            Description.SetDefault("Upon taking fatal damage, heal for 50% of your max life." +
                "\nYou will gain 'Cursed' for a short period after");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            BuffID.Sets.TimeLeftDoesNotDecrease[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<PLAYERGLOBAL>().angelsProtection = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
