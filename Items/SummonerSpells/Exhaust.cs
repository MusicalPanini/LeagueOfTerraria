using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.SummonerSpells
{
    public class ExhaustRune : SummonerSpell
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Exhaust Rune");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override string GetIconTexturePath()
        {
            return "TerraLeague/Items/SummonerSpells/Exhaust";
        }

        public override string GetSpellName()
        {
            return "Exhaust";
        }

        public override int GetRawCooldown()
        {
            return 40;
        }
        public override string GetTooltip()
        {
            return "Reduce an enemies damage by 60% at you cursor for 10 seconds";
        }

        public override void DoEffect(Player player, int spellSlot)
        {
            int npc = Targeting.NPCMouseIsHovering();
            if (npc != -1)
            {
                Projectile.NewProjectile(player.GetProjectileSource_Item(Item), player.Center, Vector2.Zero, ProjectileType<Summoner_Exhaust>(), 1, 0, player.whoAmI, npc);

                SetCooldowns(player, spellSlot);
            }
        }
    }
}
