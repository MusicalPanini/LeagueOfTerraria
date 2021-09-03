﻿using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class CauterizedWounds : Passive
    {
        readonly int percentDamageReduction;

        public CauterizedWounds(int PercentDamageReduction, LeagueItem item) : base(item)
        {
            percentDamageReduction = PercentDamageReduction;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            return TooltipName("IGNORE PAIN") + LeagueTooltip.CreateColorString(PassiveSecondaryColor, "Stores " + percentDamageReduction + "% of damage taken" +
                   "\nTake a third of the stored damage every second");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            passiveStat = modPlayer.cauterizedDamage;

            base.UpdateAccessory(player, modItem);
        }

        public override void PostPlayerUpdate(Player player)
        {
            base.PostPlayerUpdate(player);
        }

        public override void OnHitByNPC(NPC npc, ref int damage, ref bool crit, Player player)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            modPlayer.cauterizedDamage += (int)(damage * (percentDamageReduction * 0.01));
            damage = (int)(damage * (1 - (percentDamageReduction * 0.01)));
            base.OnHitByNPC(npc, ref damage, ref crit, player);
        }

        public override void OnHitByProjectile(NPC npc, ref int damage, ref bool crit, Player player)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            modPlayer.cauterizedDamage += (int)(damage * (percentDamageReduction * 0.01));
            damage = (int)(damage * (1 - (percentDamageReduction * 0.01)));
            base.OnHitByProjectile(npc, ref damage, ref crit, player);
        }

        public override void OnHitByProjectile(Projectile proj, ref int damage, ref bool crit, Player player)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            modPlayer.cauterizedDamage += (int)(damage * (percentDamageReduction * 0.01));
            damage = (int)(damage * (1 - (percentDamageReduction * 0.01)));
            base.OnHitByProjectile(proj, ref damage, ref crit, player);
        }
    }
}
