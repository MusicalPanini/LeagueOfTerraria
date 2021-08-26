using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Actives
{
    public class SpectralPursuit : Active
    {
        readonly int baseDamage;
        readonly int minionScaling;

        public SpectralPursuit(int BaseDamage, int MinionScaling, int Cooldown)
        {
            baseDamage = BaseDamage;
            minionScaling = MinionScaling;
            activeCooldown = Cooldown;
        }

        public override string Tooltip(Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            return TooltipName("SPECTURAL PURSUIT") + LeagueTooltip.CreateColorString(ActiveSecondaryColor, "Send out ") + LeagueTooltip.TooltipValue(0, false, "", new System.Tuple<int, ScaleType>(100, ScaleType.Minions)) + LeagueTooltip.CreateColorString(ActiveSecondaryColor, " spooky ghosts that track down a nearby enemy" +
                "\nThey deal ") + LeagueTooltip.TooltipValue(baseDamage, false, "", new System.Tuple<int, ScaleType>(minionScaling, ScaleType.Summon)) + LeagueTooltip.CreateColorString(ActiveSecondaryColor, " summon damage and apply 'Slowed'") +
                 "\n" + LeagueTooltip.CreateColorString(ActiveSubColor, GetScaledCooldown(player) + " second cooldown");
        }

        public override void DoActive(Player player, LeagueItem modItem)
        {
            if (cooldownCount <= 0)
            {
                PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

                Efx(player);
                SetCooldown(player);

                if (player.whoAmI == Main.myPlayer)
                {
                    var npcs = Targeting.GetAllNPCsInRange(player.MountedCenter, 600);

                    for (int i = 0; i < modPlayer.maxMinionsLastStep; i++)
                    {
                        float hSpeed = 5;
                        if (modPlayer.maxMinionsLastStep > 1)
                            hSpeed = 10 * ((i) / (float)(modPlayer.maxMinionsLastStep - 1));

                        if (npcs.Count > 0)
                            Projectile.NewProjectile(player.GetProjectileSource_Item(modItem.Item), player.position.X, player.position.Y, hSpeed-5, -4, ProjectileType<Item_SpookyGhost>(), baseDamage + (int)(modPlayer.SUM * minionScaling / 100d), 0, player.whoAmI, npcs[Main.rand.Next(npcs.Count)]);
                        else
                            Projectile.NewProjectile(player.GetProjectileSource_Item(modItem.Item), player.position.X, player.position.Y, hSpeed - 5, -4, ProjectileType<Item_SpookyGhost>(), baseDamage + (int)(modPlayer.SUM * minionScaling / 100d), 0, player.whoAmI, -2);
                    }
                }

                // For Server
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    PacketHandler.SendActiveEfx(-1, player.whoAmI, player.whoAmI, modItem.Item.type);
                }
            }
        }

        public override void PostPlayerUpdate(Player player, LeagueItem modItem)
        {
            base.PostPlayerUpdate(player, modItem);
        }

        override public void Efx(Player user)
        {
            TerraLeague.DustRing(261, user, new Color(0, 255, 255, 0));
            Terraria.Audio.SoundEngine.PlaySound(new LegacySoundStyle(3, 54).WithPitchVariance(-0.2f), user.position);
        }
    }
}

