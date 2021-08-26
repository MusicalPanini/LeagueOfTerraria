using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class LostChapter : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lost Chapter");
            Tooltip.SetDefault("3% increased magic and summon damage" +
                "\nIncreases maximum mana by 20");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
            Item.material = true;

            Passives = new Passive[]
            {
                new Haste()
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Magic) += 0.03f;
            player.GetDamage(DamageClass.Summon) += 0.03f;
            player.statManaMax2 += 20;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<AmpTome>(), 2)
            .AddIngredient(ItemType<SapphireCrystal>(), 1)
            .AddIngredient(ItemID.Book, 1)
            .AddIngredient(ItemID.Moonglow, 5)
            .AddTile(TileID.Bookcases)
            .Register();
            
        }
    }
}
