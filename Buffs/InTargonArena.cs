using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs.TargonBoss;
using Terraria.ID;

namespace TerraLeague.Buffs
{
    public class InTargonArena : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Arena");
            Description.SetDefault("Prove your worth");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            BuffID.Sets.TimeLeftDoesNotDecrease[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (NPC.CountNPCS(ModContent.NPCType<TargonBossNPC>()) <= 0)
            {
                player.AddBuff(BuffID.Featherfall, 60 * 10);
                player.mount.Dismount(player);
                player.ClearBuff(Type);
            }
            player.GetModPlayer<PLAYERGLOBAL>().targonArena = true;
        }
    }
}
