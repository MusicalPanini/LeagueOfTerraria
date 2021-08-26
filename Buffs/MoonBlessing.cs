using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class MoonBlessing : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blessing of The Silver Sister");
            Description.SetDefault("10% increased critical strike chance");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
            Main.persistentBuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetCritChance(DamageClass.Generic) += 10;
            player.GetDamage(DamageClass.Summon) += 0.1f;

            if (player.GetModPlayer<PLAYERGLOBAL>().bottleOfStardust)
            {
                player.GetCritChance(DamageClass.Generic) += 5;
                player.GetDamage(DamageClass.Summon) += 0.05f;
            }
        }
        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            if (modPlayer.bottleOfStardust)
                tip = "15% increased critical strike chance";
            else
                tip = "10% increased critical strike chance";
            base.ModifyBuffTip(ref tip, ref rare);
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
