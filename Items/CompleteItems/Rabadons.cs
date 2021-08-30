using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Rabadons : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rabadon's Deathcap");
            Tooltip.SetDefault("12% increased magic damage" +
                "\n8% increased magic critical strike chance");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.accessory = true;
            Item.material = true;

            Passives = new Passive[]
            {
                new RawPower(this)
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Magic) *= 1.12f;
            player.GetCritChance(DamageClass.Magic) += 8;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<LargeRod>(), 2)
            .AddIngredient(ItemID.RuneHat, 1)
            .AddIngredient(ItemID.SorcererEmblem, 2)
            .AddIngredient(ItemID.FragmentNebula, 10)
            .AddTile(TileID.MythrilAnvil)
            .Register();
            
        }
    }
}
