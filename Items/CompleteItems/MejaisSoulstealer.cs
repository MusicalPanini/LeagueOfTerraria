using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using TerraLeague.Items.StartingItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class MejaisSoulstealer : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mejai's Soulstealer");
            Tooltip.SetDefault("Increases maximum mana by 30");
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
                new Dread(25, 6, 1.5f)
            };
        }
        
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statManaMax2 += 30;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<DarkSeal>(), 1)
            .AddIngredient(ItemID.SoulofNight, 20)
            .AddIngredient(ItemID.SpellTome, 1)
            .AddIngredient(ItemID.MagicPowerPotion, 5)
            .AddTile(TileID.CrystalBall)
            .Register();
            
        }

        public override string GetStatText()
        {
            if (Passives[0].currentlyActive)
                return ((int)Passives[0].passiveStat).ToString();
            else
                return "";
        }
    }
}
