using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class BlackCleaver : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Black Cleaver");
            Tooltip.SetDefault("15% increased melee damage" +
                "\nIncreases health by 40" +
                "\nIncreases ability haste by 20");
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
                new Crush(this),
                new Rage(5, this)
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 20;
            player.GetDamage(DamageClass.Melee) += 0.15f;
            player.statLifeMax2 += 40;
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<Phage>(), 1)
            .AddIngredient(ItemType<Kindlegem>(), 1)
            .AddIngredient(ItemType<DarksteelBar>(), 18)
            .AddIngredient(ItemID.ChlorophyteGreataxe, 1)
            .AddIngredient(ItemID.FragmentSolar, 10)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
            
        }
    }
}
