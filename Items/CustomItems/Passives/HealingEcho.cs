using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Items.CompleteItems;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class HealingEcho : Passive
    {
        public int BaseHealing { get; }
        public int MagicScaling { get; }
        public int Targets { get; }

        public HealingEcho(int baseHealing, int magicScaling, int targets, int cooldown, LeagueItem legItem) : base(legItem)
        {
            BaseHealing = baseHealing.GetIfLower(0);
            MagicScaling = magicScaling.GetIfLower(0);
            Targets = targets.GetIfLower(1);
            passiveCooldown = cooldown.GetIfLower(1);
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            return TooltipName("ECHO") + LeagueTooltip.CreateColorString(PassiveSecondaryColor, "Your next heal on an ally will heal up to " + Targets + " nearby allies for ") +
            LeagueTooltip.TooltipValue(BaseHealing, true, "", new Tuple<int, ScaleType>((int)MagicScaling, ScaleType.Magic)) +
            "\n" + LeagueTooltip.CreateColorString(PassiveSubColor, "Healing will reduce the cooldown by 1 second" +
            "\n" + GetScaledCooldown(player) + " second cooldown.");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            base.UpdateAccessory(player, modItem);
        }

        public override void PostPlayerUpdate(Player player)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (modPlayer.echo)
            {
                player.AddBuff(BuffType<Buffs.Echo>(), 1);
                if (Main.rand.Next(0, 6) == 0)
                {
                    Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.AncientLight, 0, 0, 0, new Color(0, 255, 0, 150));
                    dust.noGravity = true;
                }
            }
            else
                player.ClearBuff(BuffType<Buffs.Echo>());

            base.PostPlayerUpdate(player);
        }

        public override void SendHealPacket(ref int healAmount, int healTarget, Player player)
        {
            if (cooldownCount <= 0)
            {
                PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
                int healing = modPlayer.ScaleValueWithHealPower(BaseHealing + (int)(modPlayer.MAG * MagicScaling * 0.01));

                var players = Targeting.GetAllPlayersInRange(Main.player[healTarget].MountedCenter, 700, healTarget, player.team).SortPlayersByDistance(player.MountedCenter);

                for (int i = 0; i < players.Count; i++)
                {
                    Projectile.NewProjectileDirect(player.GetProjectileSource_Item(modItem.Item), Main.player[healTarget].MountedCenter, new Vector2(14, 0).RotatedBy(MathHelper.ToRadians(i * 360f / players.Count)), ProjectileType<Item_Heal>(), healing, 0, player.whoAmI, players[i], healTarget);
                }

                Efx(Main.player[healTarget]);
                SendEfx(Main.player[healTarget], modItem);

                cooldownCount = passiveCooldown * 60;
                modPlayer.echo = false;
            }
            else
            {
                cooldownCount -= 60;
            }
            base.SendHealPacket(ref healAmount, healTarget, player);
        }

        public override void Efx(Player user)
        {
            Terraria.Audio.SoundEngine.PlaySound(new LegacySoundStyle(2, 12).WithPitchVariance(-0.6f), user.MountedCenter);
        }
    }
}
