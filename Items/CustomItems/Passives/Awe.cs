using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class Awe : Passive
    {
        readonly int manaReduction;
        readonly int manaToADConversion;
        readonly int manaToAPConversion;

        public Awe(int ManaReduction, int ManaToADConversion, int ManaToAPConversion)
        {
            manaReduction = ManaReduction;
            manaToADConversion = ManaToADConversion;
            manaToAPConversion = ManaToAPConversion;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            string text = TooltipName("AWE") + LeagueTooltip.CreateColorString(PassiveSecondaryColor, "Reduced mana usage by " + manaReduction + "%");

            if (manaToADConversion > 0)
                text += "\n" + LeagueTooltip.CreateColorString(PassiveSecondaryColor, "Gain 1% melee and ranged damage per " + manaToADConversion + " max mana");

            if (manaToAPConversion > 0)
                text += "\n" + LeagueTooltip.CreateColorString(PassiveSecondaryColor, "Gain 1% magic and summon damage per " + manaToAPConversion + " max mana");

            return text;
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            player.manaCost -= manaReduction * 0.01f;
            player.GetModPlayer<PLAYERGLOBAL>().awe = true;

            if (manaToADConversion > 0)
            {
                player.GetDamage(DamageClass.Melee) += (player.statManaMax2 / manaToADConversion) * 0.01f;
                player.GetDamage(DamageClass.Ranged) += (player.statManaMax2 / manaToADConversion) * 0.01f;
            }

            if (manaToAPConversion > 0)
            {
                player.GetDamage(DamageClass.Magic) += (player.statManaMax2 / manaToAPConversion) * 0.01f;
                player.GetDamage(DamageClass.Summon) += (player.statManaMax2 / manaToAPConversion) * 0.01f;
            }

            base.UpdateAccessory(player, modItem);
        }
    }
}
