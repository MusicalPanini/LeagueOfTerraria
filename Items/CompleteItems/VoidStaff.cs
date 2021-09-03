using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class VoidStaff : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Void Staff");
            Tooltip.SetDefault("15% increased magic damage");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.accessory = true;

            Passives = new Passive[]
            {
                new Dissolve(40, this)
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Magic) += 0.15f;
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<BlastingWand>(), 1)
            .AddIngredient(ItemType<AmpTome>(), 1)
            .AddIngredient(ItemID.StaffofRegrowth, 1)
            .AddIngredient(ItemType<VoidBar>(), 24)
            .AddIngredient(ItemID.FragmentNebula, 10)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
            
        }
    }
}
