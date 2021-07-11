using Microsoft.Xna.Framework;
using System;
using TerraLeague.Projectiles;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Actives
{
    public class LightningBolt : Active
    {
        readonly int baseDamage;
        readonly int magicMinionScaling;

        public LightningBolt(int BaseDamage, int MagicMinionScaling, int Cooldown)
        {
            baseDamage = BaseDamage;
            magicMinionScaling = MagicMinionScaling;
            cooldownCount = Cooldown;
        }

        public override string Tooltip(Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            string scaleText;
            if (modPlayer.SUM > modPlayer.MAG)
                scaleText = LeagueTooltip.TooltipValue(baseDamage, false, "", new Tuple<int, ScaleType>(magicMinionScaling, ScaleType.Summon));
            else
                scaleText = LeagueTooltip.TooltipValue(baseDamage, false, "", new Tuple<int, ScaleType>(magicMinionScaling, ScaleType.Magic));

            return TooltipName("FROST BOLT") + LeagueTooltip.CreateColorString(ActiveSecondaryColor, "Target an enemy to deal ") + baseDamage + " + " + scaleText + LeagueTooltip.CreateColorString(ActiveSecondaryColor, " magic damage and apply 'Slowed' to them")
                + "\n" + LeagueTooltip.CreateColorString(ActiveSubColor, GetScaledCooldown(player) + " second cooldown. Damage scales with either MAG or SUM");
        }

        public override void DoActive(Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (cooldownCount <= 0)
            {
                int npc = Targeting.NPCMouseIsHovering();
                if (npc != -1)
                {
                    int damage = baseDamage + (int)(Math.Max(modPlayer.SUM, modPlayer.MAG) * magicMinionScaling / 100d);
                    Projectile.NewProjectile(Main.npc[npc].Center, Vector2.Zero, ProjectileType<Item_LightningBolt>(), damage, 0, player.whoAmI, npc);
                    SetCooldown(player);
                }
            }
        }

        public override void PostPlayerUpdate(Player player, LeagueItem modItem)
        {
            base.PostPlayerUpdate(player, modItem);
        }
    }
}

