using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class KnightsVow : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Knight's Vow");
            Tooltip.SetDefault("Increases health by 40" +
                "\nIncreases armor by 8" +
                 "\nIncreases ability haste by 10" +
                 "\nAbsorbs 25% of damage done to players on your team" +
                 "\nOnly active above 25% life");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 50, 0, 0);
            Item.rare = ItemRarityID.Lime;
            Item.accessory = true;
            Item.material = true;

            Passives = new Passive[]
            {
               new TheVow(600, this)
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += 40;
            player.GetModPlayer<PLAYERGLOBAL>().armor += 8;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 10;
            player.hasPaladinShield = true;
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<ChainVest>(), 1)
            .AddIngredient(ItemType<Kindlegem>(), 1)
            .AddIngredient(ItemID.PaladinsShield, 1)
            .AddIngredient(ItemID.HallowedHelmet, 1)
            .AddTile(TileID.MythrilAnvil)
            .Register();
            
        }
    }
}
