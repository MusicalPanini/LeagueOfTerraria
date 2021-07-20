using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace TerraLeague.Items.CustomItems.Actives
{
    public class ManaShield : Active
    {
        readonly int duration;
        readonly int percentMana;
        readonly int manaScaling;
        readonly int baseShield;

        public ManaShield(int Duration, int PercentMana, int ManaScaling, int BaseShield, int Cooldown)
        {
            duration = Duration;
            percentMana = PercentMana;
            manaScaling = ManaScaling;
            baseShield = BaseShield;
            activeCooldown = Cooldown;
        }

        public override string Tooltip(Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            return TooltipName("MANA Barrier") + LeagueTooltip.CreateColorString(ActiveSecondaryColor, "Consume ") + LeagueTooltip.TooltipValue(0, false, "", new System.Tuple<int, ScaleType>(percentMana, ScaleType.CurMana))
                + LeagueTooltip.CreateColorString(ActiveSecondaryColor, " mana\nGain a ") + LeagueTooltip.TooltipValue(baseShield, true, "", new System.Tuple<int, ScaleType>(manaScaling, ScaleType.CurMana))
                + LeagueTooltip.CreateColorString(ActiveSecondaryColor, " shield for " + duration + " seconds")
                +"\n" + LeagueTooltip.CreateColorString(ActiveSubColor, GetScaledCooldown(player) + " second cooldown"); ;
        }

        public override void DoActive(Player player, LeagueItem modItem)
        {
            if (cooldownCount <= 0)
            {
                PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

                int manaUsed = (int)(player.statMana * percentMana * 0.01);
                player.CheckMana(manaUsed, true);

                modPlayer.AddShield(modPlayer.ScaleValueWithHealPower(manaUsed * ((float)manaScaling / percentMana) + baseShield), duration * 60, Color.SkyBlue, ShieldType.Basic);
                SetCooldown(player);

                Efx(player);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    PacketHandler.SendActiveEfx(-1, player.whoAmI, player.whoAmI, modItem.item.type);
            }
        }


        public override void PostPlayerUpdate(Player player, LeagueItem modItem)
        {
            base.PostPlayerUpdate(player, modItem);
        }

        override public void Efx(Player user)
        {
            Main.PlaySound(new LegacySoundStyle(2, 29).WithPitchVariance(-0.5f), user.Center);
        }
    }
}

