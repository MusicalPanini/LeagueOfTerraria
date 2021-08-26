using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Rylais : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rylai's Crystal Scepter");
            Tooltip.SetDefault("5% increased magic damage" +
                "\nIncreases health by 30");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 30, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;

            Passives = new Passive[]
            {
                new Frosty(2)
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Magic) += 0.05f;
            player.statLifeMax2 += 30;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<BlastingWand>(), 1)
            .AddIngredient(ItemType<AmpTome>(), 1)
            .AddIngredient(ItemType<RubyCrystal>(), 1)
            .AddIngredient(ItemType<TrueIceChunk>(), 10)
            .AddIngredient(ItemID.FrostStaff, 1)
            .AddTile(TileID.Anvils)
            .Register();
            
        }
    }
}
