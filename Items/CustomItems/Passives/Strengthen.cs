using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class Strengthen : Passive
    {
        readonly int maxStacks;
        readonly int lifeperStack;

        public Strengthen(int MaxStacks, int LifePerStack)
        {
            maxStacks = MaxStacks;
            lifeperStack = LifePerStack;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            return TooltipName("STRENGTHEN") + LeagueTooltip.CreateColorString(PassiveSecondaryColor, "Kills grant stacks up to " + maxStacks + "\nGain " + lifeperStack + " health per stack")
                + "\n" + LeagueTooltip.CreateColorString(PassiveSubColor, "Lose all stacks on death");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            player.statLifeMax2 += (int)passiveStat;

            base.UpdateAccessory(player, modItem);
        }

        public override void OnKilledNPC(NPC npc, int damage, bool crit, Player player, ModItem modItem)
        {
            AddStat(player, maxStacks, lifeperStack);

            base.OnKilledNPC(npc, damage, crit, player, modItem);
        }

        public override int PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            passiveStat = 0;

            return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource, player, modItem);
        }
    }
}
