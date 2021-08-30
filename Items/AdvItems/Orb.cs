using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class Orb : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Oblivion Orb");
            Tooltip.SetDefault("3% increased magic damage" +
                "\nIncreases health by 20");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 15, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;

            Passives = new Passive[]
            {
                new TouchOfDeath(7, this)
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Magic) += 0.03f;
            player.statLifeMax2 += 20;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<RubyCrystal>(), 1)
            .AddIngredient(ItemType<AmpTome>(), 1)
            .AddIngredient(ItemID.SharkToothNecklace, 1)
            .AddIngredient(ItemType<VoidFragment>(), 50)
            .AddTile(TileID.Anvils)
            .Register();
            
        }
    }
}
