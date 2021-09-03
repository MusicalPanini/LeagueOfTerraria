using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class Dread : Passive
    {
        readonly int maxStacks;
        readonly int lostStacks;
        readonly float magicMinionDamage;

        public Dread(int MaxStacks, int LostStacks, float magicMinionDamageIncrease, LeagueItem item) : base(item)
        {
            maxStacks = MaxStacks;
            lostStacks = LostStacks;
            magicMinionDamage = magicMinionDamageIncrease;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            return TooltipName("DREAD") + LeagueTooltip.CreateColorString(PassiveSecondaryColor, "Kills grant stacks up to " + maxStacks)
                + "\n" + LeagueTooltip.CreateColorString(PassiveSecondaryColor, "Lose " + lostStacks + " stacks when enemies damage you")
                 + "\n" + TooltipName("DO OR DIE") + LeagueTooltip.CreateColorString(PassiveSecondaryColor, "Gain " + magicMinionDamage + "% magic and summon damage per stack of DREAD");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            player.GetDamage(DamageClass.Magic) += magicMinionDamage * 0.01f * passiveStat;
            player.GetDamage(DamageClass.Summon) += magicMinionDamage * 0.01f * passiveStat;

            base.UpdateAccessory(player, modItem);
        }

        public override void OnKilledNPC(NPC npc, ref int damage, ref bool crit, Player player)
        {
            AddStat(player, maxStacks, 1);

            base.OnKilledNPC(npc, ref damage, ref crit, player);
        }

        public override void OnHitByNPC(NPC npc, ref int damage, ref bool crit, Player player)
        {
            AddStat(player, maxStacks, -lostStacks, true);
            base.OnHitByNPC(npc, ref damage, ref crit, player);
        }

        public override void OnHitByProjectile(NPC npc, ref int damage, ref bool crit, Player player)
        {
            AddStat(player, maxStacks, -lostStacks, true);
            base.OnHitByProjectile(npc, ref damage, ref crit, player);
        }

        public override void OnHitByProjectile(Projectile proj, ref int damage, ref bool crit, Player player)
        {
            AddStat(player, maxStacks, -lostStacks, true);
            base.OnHitByProjectile(proj, ref damage, ref crit, player);
        }
    }
}
