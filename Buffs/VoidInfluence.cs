using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;
using Terraria.ID;

namespace TerraLeague.Buffs
{
    public class VoidInfluence : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Influenced by the Void");
            Description.SetDefault("Influenced");
            Main.buffNoSave[Type] = false;
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;
            BuffID.Sets.TimeLeftDoesNotDecrease[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (modPlayer.VoidInflu <= 0)
            {
                player.ClearBuff(Type);
                return;
            }

            modPlayer.damageTakenModifier += (int)modPlayer.VoidInflu/200f;

            if (modPlayer.VoidInflu > 85)
                player.blackout = true;
            else if (modPlayer.VoidInflu > 25)
                player.blind = true; ;

            if (modPlayer.VoidInflu > 50)
                player.enemySpawns = true;

            if (modPlayer.VoidInflu > 75)
                player.slow = true;
        }
        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            tip = "The Voids influence is affecting your mind, body, and soul." +
                "\nCurrent effects:";
            tip += "\nDamage taken inceased by " + (decimal)modPlayer.VoidInflu/2 + "%";
            if (modPlayer.VoidInflu > 85)
                tip += "\nVision Greatly Reduced";
            else if (modPlayer.VoidInflu > 25)
                tip += "\nVision Reduced";

            if (modPlayer.VoidInflu > 50)
                tip += "\nIncreased spawn rate";

            if (modPlayer.VoidInflu > 75)
                tip += "\nSlowed movement speed";

            if (modPlayer.VoidInflu > 100)
                tip += "\nDeath";
            else if (modPlayer.VoidInflu > 90)
                tip += "\nImminent death";




            base.ModifyBuffTip(ref tip, ref rare);
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
