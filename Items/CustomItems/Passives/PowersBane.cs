using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CompleteItems;
using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class PowersBane : Passive
    {
        readonly int damageThreshold;
        readonly int damageReduction;

        public PowersBane(int DamageThreshold, int DamageReduction, LeagueItem legItem) : base(legItem)
        {
            damageThreshold = DamageThreshold;
            damageReduction = DamageReduction;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            return TooltipName("POWER'S BANE") + LeagueTooltip.CreateColorString(PassiveSecondaryColor, "All damage taken above " + damageThreshold + " will be reduced by " + damageReduction);
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            base.UpdateAccessory(player, modItem);
        }

        public override void OnHitByNPC(NPC npc, ref int damage, ref bool crit, Player player)
        {
            if (damage >= damageThreshold)
                damage -= damageReduction;
            base.OnHitByNPC(npc, ref damage, ref crit, player);
        }

        public override void OnHitByProjectile(NPC npc, ref int damage, ref bool crit, Player player)
        {
            if (damage >= damageThreshold)
                damage -= damageReduction;
            base.OnHitByProjectile(npc, ref damage, ref crit, player);
        }

        public override void OnHitByProjectile(Projectile proj, ref int damage, ref bool crit, Player player)
        {
            if (damage >= damageThreshold)
                damage -= damageReduction;
            base.OnHitByProjectile(proj, ref damage, ref crit, player);
        }
    }
}
