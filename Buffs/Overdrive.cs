using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class Overdrive : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Overdrive");
            Description.SetDefault("10% increased damage" +
                "\n20% increased move speed");
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Melee) += 0.1f;
            player.GetDamage(DamageClass.Ranged) += 0.1f;
            player.GetDamage(DamageClass.Magic) += 0.1f;
            player.GetDamage(DamageClass.Summon) += 0.1f;
            player.moveSpeed += 0.2f;
        }
    }
}
