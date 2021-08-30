using TerraLeague.Items.AdvItems;
using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Actives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class MoonflairSpellblade : MasterworkItem
    {
        public override string MasterworkName => "Eclipse";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Moonflair Spellblade");
            Tooltip.SetDefault("4% increased magic and summon damage" +
                "\nIncreases armor by 5" +
                "\nIncreases resist by 4" +
                "\nImmunity to Slow and Chilled");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 20, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Magic) += 0.04f;
            player.GetDamage(DamageClass.Summon) += 0.04f;
            player.GetModPlayer<PLAYERGLOBAL>().armor += 5;
            player.GetModPlayer<PLAYERGLOBAL>().resist += 4;
            player.buffImmune[BuffID.Slow] = true;
            player.buffImmune[BuffID.Chilled] = true;
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<Seekers>(), 1)
            .AddIngredient(ItemType<NegatronCloak>(), 1)
            .AddIngredient(ItemType<CelestialBar>(), 12)
            .AddTile(TileID.Anvils)
            .Register();
            
        }

        public override string MasterworkTooltip()
        {
            return LeagueTooltip.CreateColorString(MasterColor, "6%") + " increased magic and summon damage" +
                "\nIncreases armor by " + LeagueTooltip.CreateColorString(MasterColor, "8") +
                "\nIncreases resist by " + LeagueTooltip.CreateColorString(MasterColor, "8") +
                "\nImmunity to Slow and Chilled";
        }

        public override void UpdateMasterwork(Player player)
        {
            player.GetDamage(DamageClass.Magic) += 0.02f;
            player.GetDamage(DamageClass.Summon) += 0.02f;
            player.GetModPlayer<PLAYERGLOBAL>().armor += 3;
            player.GetModPlayer<PLAYERGLOBAL>().resist += 4;
            throw new System.NotImplementedException();
        }
    }
}

