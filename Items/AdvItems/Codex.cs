using TerraLeague.Items.BasicItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class Codex : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fiendish Codex");
            Tooltip.SetDefault("3% increased magic and summon damage" +
                "\nIncreases ability haste by 10");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
            Item.material = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Magic) += 0.03f;
            player.GetDamage(DamageClass.Summon) += 0.03f;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 10;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<AmpTome>(), 1)
            .AddIngredient(ItemType<DamnedSoul>(), 12)
            .AddTile(TileID.Bookcases)
            .Register();
            
        }
    }
}
