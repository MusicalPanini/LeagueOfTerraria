using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Actives
{
    public class VoidCaller : Active
    {
        readonly int baseDamage;
        readonly int baseMinions;
        readonly int sumScaling;

        public VoidCaller(int BaseDamage, int BaseMinions, int SumScaling, int Cooldown)
        {
            baseDamage = BaseDamage;
            baseMinions = BaseMinions;
            sumScaling = SumScaling;
            activeCooldown = Cooldown;
        }

        public override string Tooltip(Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            return TooltipName("VOID CALLER") + LeagueTooltip.CreateColorString(ActiveSecondaryColor, "Summon a Zz'Rot portal at your cursor" +
                "\nIt ejects ") + LeagueTooltip.TooltipValue(3, false, "", new System.Tuple<int, ScaleType>(100, ScaleType.Minions)) +
                LeagueTooltip.CreateColorString(ActiveSecondaryColor, " Zz'Rots every second for 5 seconds." +
                "\nThe Zz'Rots deal ") + LeagueTooltip.TooltipValue(baseDamage, false, "", new System.Tuple<int, ScaleType>(sumScaling, ScaleType.Summon)) + LeagueTooltip.CreateColorString(ActiveSecondaryColor, " summon damage") +
                 "\n" + LeagueTooltip.CreateColorString(ActiveSubColor, GetScaledCooldown(player) + " second cooldown");
        }

        public override void DoActive(Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (cooldownCount <= 0)
            {
                player.FindSentryRestingSpot(ProjectileType<Item_ZzrotPortal>(), out int xPos, out int yPos, out int yDis);
                Projectile.NewProjectile(player.GetProjectileSource_Item(modItem.Item), (float)xPos, (float)(yPos - yDis), 0f, 0f, ProjectileType<Item_ZzrotPortal>(), baseDamage + (int)(modPlayer.SUM * sumScaling * 0.01f), 2, player.whoAmI, baseMinions);
                SetCooldown(player);

                Efx(player);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    PacketHandler.SendActiveEfx(-1, player.whoAmI, player.whoAmI, modItem.Item.type);
            }
        }

        public override void Efx(Player user)
        {
            Terraria.Audio.SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 113), user.MountedCenter);
            base.Efx(user);
        }

        public override void PostPlayerUpdate(Player player)
        {
            base.PostPlayerUpdate(player);
        }
    }
}

