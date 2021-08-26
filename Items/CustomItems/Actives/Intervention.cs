using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Actives
{
    public class Intervention : Active
    {
        readonly int baseHeal;

        public Intervention(int BaseHeal, int Cooldown)
        {
            baseHeal = BaseHeal;
            activeCooldown = Cooldown;
        }

        int modifiedHealPower(Player player)
        {
            float healPower = (float)(1 + ((player.GetModPlayer<PLAYERGLOBAL>().healPowerLastStep - 1) * 3));
            return (int)System.Math.Round(baseHeal * healPower, 0);
        }

        public override string Tooltip(Player player, LeagueItem modItem)
        {
            string statis = "";
            int value = modifiedHealPower(player);

            if (UI.ItemUI.extraStats)
                statis += LeagueTooltip.CreateColorString(Color.White.Hex3(), baseHeal + "") + " + " + LeagueTooltip.CreateColorString(LeagueTooltip.HealPowerColor, "HEAL(" + (value - baseHeal) + ")");
            else
                statis += LeagueTooltip.CreateColorString(Color.White.Hex3(), value + "");

            return TooltipName("INTERVENTION") + LeagueTooltip.CreateColorString(ActiveSecondaryColor, "Create a healing rune at the target location.\nAfter 2.5 seconds it heals all nearby allies for ") + statis + LeagueTooltip.CreateColorString(ActiveSecondaryColor, " life.") + "\n"
                + LeagueTooltip.CreateColorString(ActiveSecondaryColor, "It also damages enemies for twice the amount.") + "\n"
                + LeagueTooltip.CreateColorString(ActiveSecondaryColor, "Heal Power is 3 times as effective for this heal") +
                 "\n" + LeagueTooltip.CreateColorString(ActiveSubColor, GetScaledCooldown(player) + " second cooldown");
        }

        public override void DoActive(Player player, LeagueItem modItem)
        {
            if (cooldownCount <= 0)
            {
                SetCooldown(player);
                int lifeToHeal = modifiedHealPower(player);

                Projectile.NewProjectileDirect(player.GetProjectileSource_Item(modItem.Item), Main.MouseWorld, Vector2.Zero, ProjectileType<Item_HealField>(), lifeToHeal, 0, player.whoAmI);
            }
        }

        public override void PostPlayerUpdate(Player player, LeagueItem modItem)
        {
            base.PostPlayerUpdate(player, modItem);
        }

        override public void Efx(Player user)
        {
        }
    }
}

