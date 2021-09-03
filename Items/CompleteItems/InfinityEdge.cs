using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class InfinityEdge : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Infinity Edge");
            Tooltip.SetDefault("10% increased ranged damage" +
                "\n10% increased ranged critical strike chance");
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
                new CriticalEdge(25, this)
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Ranged) += 0.10f;
            player.GetCritChance(DamageClass.Ranged) += 10;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<BFSword>(), 1)
            .AddIngredient(ItemType<Pickaxe>(), 1)
            .AddIngredient(ItemType<CloakofAgility>(), 1)
            .AddIngredient(ItemID.DestroyerEmblem, 1)
            .AddIngredient(ItemID.TrueExcalibur, 1)
            .AddIngredient(ItemID.FragmentVortex, 10)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            return base.CanEquipAccessory(player, slot);
        }
    }
}
