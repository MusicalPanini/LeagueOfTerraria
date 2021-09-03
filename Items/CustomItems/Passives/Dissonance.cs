using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class Dissonance : Passive
    {
        readonly int magicMinionDamage;
        readonly int perMana;
        
        public Dissonance(int magicMinionDamageIncrease, int PerMana, LeagueItem item) : base(item)
        {
            magicMinionDamage = magicMinionDamageIncrease;
            perMana = PerMana;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            return TooltipName("DISSONANCE") + LeagueTooltip.CreateColorString(PassiveSecondaryColor, "Gain " + magicMinionDamage + "% magic and summon damage per " + perMana + " current mana")
                + "\n" + LeagueTooltip.CreateColorString(PassiveSubColor, "Disables HARMONY passive");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            player.GetDamage(DamageClass.Magic) += (player.statMana / perMana) * (magicMinionDamage * 0.01f);
            modPlayer.TrueMinionDamage += (player.statMana / 40) * 0.01;

            base.UpdateAccessory(player, modItem);
        }
    }
}
