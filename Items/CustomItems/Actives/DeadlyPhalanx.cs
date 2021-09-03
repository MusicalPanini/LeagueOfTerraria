using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Actives
{
    public class DeadlyPhalanx : Active
    {
        public int ShieldDuration { get; }
        public int PercentMaxLife { get; }
        public int MaxCastDistance { get; }
        public Color ShieldColor { get; }
        public DeadlyPhalanx(int shieldDuration, int percentMaxLife, int maxCastDistance, int cooldown, Color shieldColor)
        {
            ShieldDuration = shieldDuration.GetIfLower(1);
            PercentMaxLife = percentMaxLife.GetIfLower(1);
            activeCooldown = cooldown.GetIfLower(1);
            MaxCastDistance = maxCastDistance;
            ShieldColor = shieldColor;
        }

        public override string Tooltip(Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            return TooltipName("Deadly Phalanx") + LeagueTooltip.CreateColorString(ActiveSecondaryColor, "Target an ally and grant both of you a ")
                + LeagueTooltip.TooltipValue(0, true, "", new System.Tuple<int, ScaleType>(PercentMaxLife, ScaleType.MaxLife)) + LeagueTooltip.CreateColorString(ActiveSecondaryColor, " life shield for " + ShieldDuration + " seconds." 
                + "\nTargeting yourself will grant the shield to the nearest ally in range.")
                + "\n" + LeagueTooltip.CreateColorString(ActiveSubColor, GetScaledCooldown(player) + " second cooldown");
        }

        public override void DoActive(Player player, LeagueItem modItem)
        {
            if (cooldownCount <= 0)
            {
                int target = Targeting.PlayerMouseIsHovering(30);
                if (target != -1)
                {
                    DoAction(target, player, modItem);
                }
                else
                {
                    TerraLeague.DustBorderRing(MaxCastDistance, player.MountedCenter, DustID.GemSapphire, default, 2);
                }
            }
        }

        public override void PostPlayerUpdate(Player player)
        {
            base.PostPlayerUpdate(player);
        }

        public void DoAction(int target, Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            int shieldAmount = modPlayer.ScaleValueWithHealPower((int)(modPlayer.GetRealHeathWithoutShield(true) * PercentMaxLife * 0.01));

            modPlayer.AddShield(shieldAmount, ShieldDuration * 60, ShieldColor, ShieldType.Basic);

            if (target == player.whoAmI)
                target = Targeting.GetClosestPlayer(player.MountedCenter, MaxCastDistance, player.whoAmI, player.team);

            if (target != -1)
                modPlayer.SendShieldPacket(shieldAmount, target, ShieldType.Basic, ShieldDuration * 60, -1, player.whoAmI, ShieldColor);

            //Efx(Main.player[target]);
            if (Main.netMode == NetmodeID.MultiplayerClient)
                PacketHandler.SendActiveEfx(-1, player.whoAmI, target, modItem.Item.type);

            SetCooldown(player);
        }

        override public void Efx(Player user)
        {
            Terraria.Audio.SoundEngine.PlaySound(new LegacySoundStyle(2, 29).WithPitchVariance(-0.3f), user.Center);
            //for (int j = 0; j < 18; j++)
            //{
            //    Dust dust = Dust.NewDustDirect(new Vector2(Main.rand.Next((int)user.position.X - 8, (int)user.position.X + 8), user.position.Y + 16), user.width, user.height, DustID.AncientLight, 0, -Main.rand.Next(6, 18), 0, new Color(255, 255, 255, 0), Main.rand.Next(Main.rand.Next(2, 3)));
            //    dust.noGravity = true;
            //}
        }
    }
}

