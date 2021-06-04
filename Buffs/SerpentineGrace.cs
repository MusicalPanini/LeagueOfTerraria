using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class SerpentineGrace : ModBuff
    {
        public bool initial = true;
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Serpentine Grace");
            Description.SetDefault("Ranged attack speed increased by 30%" +
                "\nMovement speed increased by 30%");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<PLAYERGLOBAL>().rangedAttackSpeed += 0.3;
            player.moveSpeed *= 1.30f;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
