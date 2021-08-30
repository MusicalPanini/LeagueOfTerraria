using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class Madness : Passive
    {
        readonly int minionDamagePerStack;

        public Madness(int MinionDamagePerStack, LeagueItem legItem) : base(legItem)
        {
            minionDamagePerStack = MinionDamagePerStack;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            return TooltipName("MADNESS") + LeagueTooltip.CreateColorString(PassiveSecondaryColor, "Every second in combat will grant a stack\nGain " + minionDamagePerStack + "% summon damage per stack up to " + minionDamagePerStack * 10 + "%");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (modPlayer.CombatTimer >= 240)
            {
                cooldownCount = 0;
                passiveStat = 0;
            }
            if (modPlayer.CombatTimer < 240 && passiveStat < 10)
            {
                cooldownCount++;
                if (cooldownCount >= 60)
                {
                    cooldownCount = 0;
                    AddStat(player, 10, 1);
                }
            }

            player.GetDamage(DamageClass.Summon) += minionDamagePerStack * 0.01f * passiveStat;

            base.UpdateAccessory(player, modItem);
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
