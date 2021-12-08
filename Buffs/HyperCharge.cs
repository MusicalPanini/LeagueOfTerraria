using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class HyperCharge : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hyper Charge");
            Description.SetDefault("Increased movement and attack speed!");
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed *= 2f;
            player.GetModPlayer<PLAYERGLOBAL>().hyperCharge = true;
        }
    }
}
