using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class Crystallization : Passive
    {
        public int ProcChance { get; }
        public int ShieldConversion { get; }
        public int ShieldDuration { get; }
        public Color ShieldColor { get; }
        public Crystallization(int procChance, int shieldConversion, int shieldDuration, Color shieldColor, LeagueItem item) : base(item)
        {
            ProcChance = procChance.GetBetween(1, 100);
            ShieldConversion = shieldConversion.GetIfLower(1);
            ShieldDuration = shieldDuration.GetIfLower(1);
            ShieldColor = shieldColor;
        }


        public override string Tooltip(Player player, ModItem modItem)
        {
            return TooltipName("Crystallization") + LeagueTooltip.CreateColorString(PassiveSecondaryColor, "Your heals have a " + ProcChance + "% chance to also shield the target for " + ShieldConversion + "% of the heal amount" +
                "\nThe shield lasts " + ShieldDuration + " second" + (ShieldDuration != 1 ? "s" : ""));
        }

        public override void SendHealPacket(ref int healAmount, int healTarget, Player player)
        {
            player.GetModPlayer<PLAYERGLOBAL>().SendShieldPacket((int)(healAmount * ShieldConversion * 0.01f), healTarget, ShieldType.Basic, 60 * ShieldDuration, healTarget, -1, ShieldColor);

            base.SendHealPacket(ref healAmount, healTarget, player);
        }
    }
}
