using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class Phage : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Phage");
            Tooltip.SetDefault("4% increased melee and ranged damage" +
                "\nIncreases maximum life by 10");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 15, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;
            Item.material = true;

            Passives = new Passive[]
            {
                new Rage(3, this)
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += 10;
            player.GetDamage(DamageClass.Melee) += 0.04f;
            player.GetDamage(DamageClass.Ranged) += 0.04f;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<RubyCrystal>(), 1)
            .AddIngredient(ItemID.WoodenHammer, 1)
            .AddRecipeGroup("TerraLeague:DemonGroup", 5)
            .AddRecipeGroup("TerraLeague:DemonPartGroup", 5)
            .AddTile(TileID.Anvils)
            .Register();
            
        }
    }
}
