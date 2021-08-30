using TerraLeague.Buffs;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class Frosty : Passive
    {
        readonly int debuffDuration;

        public Frosty(int DebuffDuration, LeagueItem legItem) : base(legItem)
        {
            debuffDuration = DebuffDuration;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            return TooltipName("RIMEFROST") + LeagueTooltip.CreateColorString(PassiveSecondaryColor, "Magic damage will slow enemies " + debuffDuration + " seconds");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            base.UpdateAccessory(player, modItem);
        }

        public override void NPCHitWithProjectile(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection, ref int OnHitDamage, Player player)
        {
            if (proj.DamageType == DamageClass.Magic)
            {
                target.AddBuff(BuffType<Slowed>(), debuffDuration * 60);
            }
            base.NPCHitWithProjectile(proj, target, ref damage, ref knockback, ref crit, ref hitDirection, ref OnHitDamage, player);
        }
    }
}
