using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class RequiemChannel : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Channeling Requiem");
            Description.SetDefault("The world is about to experience beauty..");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.noKnockback = true;
            player.jump = 0;
            player.noItems = true;
            player.silence = true;
            player.GetModPlayer<PLAYERGLOBAL>().requiemChannel = true;
            player.GetModPlayer<PLAYERGLOBAL>().requiemChannelTime = player.buffTime[buffIndex];
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            
        }
    }
}
