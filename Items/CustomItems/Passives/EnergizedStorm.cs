using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class Storm : Passive
    {
        public Storm(LeagueItem item) : base(item)
        {
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            return TooltipName("STORM") + LeagueTooltip.CreateColorString(PassiveSecondaryColor, "Enhances the bonus effects of ENERGIZED and applies 'Slowed'");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            modPlayer.EnergizedStorm = true;
            base.UpdateAccessory(player, modItem);
        }

        public override void PostPlayerUpdate(Player player)
        {
            base.PostPlayerUpdate(player);
        }

        public override void NPCHit(Item item, NPC target, ref int damage, ref float knockback, ref bool crit, ref int OnHitDamage, Player player)
        {
            base.NPCHit(item, target, ref damage, ref knockback, ref crit, ref OnHitDamage, player);
        }

        public override void NPCHitWithProjectile(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection, ref int OnHitDamage, Player player)
        {
            base.NPCHitWithProjectile(proj, target, ref damage, ref knockback, ref crit, ref hitDirection, ref OnHitDamage, player);
        }
    }
}
