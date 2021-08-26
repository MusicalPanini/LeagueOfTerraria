using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Items.CompleteItems;
using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class Harmony : Passive
    {
        readonly int lifeRegen;
        readonly int preMana;

        public Harmony(int LifeRegenIncrease, int PerMana)
        {
            lifeRegen = LifeRegenIncrease;
            preMana = PerMana;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            return TooltipName("HARMONY") + LeagueTooltip.CreateColorString(PassiveSecondaryColor, "Gain " + lifeRegen + " life regen per " + preMana + " current mana");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            base.UpdateAccessory(player, modItem);
        }

        public override void PostPlayerUpdate(Player player, ModItem modItem)
        {
            for (int i = 3; i < 10; i++)
            {
                if (player.armor[i].ModItem != null)
                {
                    LeagueItem legItem = player.armor[i].ModItem as LeagueItem;

                    if (legItem != null)
                    {
                        if (legItem.Passives != null)
                        {
                            for (int j = 0; j < legItem.Passives.Length; j++)
                            {
                                if (legItem.Passives[j].GetType().Name == "Dissonance")
                                {
                                    currentlyActive = false;
                                    player.lifeRegen += (int)(player.statMana / preMana) * lifeRegen;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            

            base.PostPlayerUpdate(player, modItem);
        }


    }
}
