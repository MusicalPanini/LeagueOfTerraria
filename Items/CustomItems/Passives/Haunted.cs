using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class Haunted : Passive
    {
        public static int critChance = 5;

        public Haunted(LeagueItem legItem) : base(legItem)
        {
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            return TooltipName("HAUNTED") + LeagueTooltip.CreateColorString(PassiveSecondaryColor, "Your minions have a " + critChance + "% chance to crit");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            player.GetModPlayer<PLAYERGLOBAL>().haunted = true;
            base.UpdateAccessory(player, modItem);
        }

        public override void NPCHitWithProjectile(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection, ref int OnHitDamage, Player player)
        {

            base.NPCHitWithProjectile(proj, target, ref damage, ref knockback, ref crit, ref hitDirection, ref OnHitDamage, player);
        }
    }
}
