using TerraLeague.Items.AdvItems;
using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class LordDoms : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lord Dominik's Regards");
            Tooltip.SetDefault("7% increased ranged damage");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 60, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.accessory = true;

            Passives = new Passive[]
            {
                new CustomItems.Passives.LastWhisper(30, false)
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Ranged) += 0.07f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<LastWhisper>(), 1)
            .AddIngredient(ItemType<Pickaxe>(), 1)
            .AddIngredient(ItemID.EyeoftheGolem, 1)
            .AddIngredient(ItemID.ShroomiteBar, 8)
            .AddIngredient(ItemID.SoulofSight, 5)
            .AddTile(TileID.MythrilAnvil)
            .Register();
            
        }
    }
}
