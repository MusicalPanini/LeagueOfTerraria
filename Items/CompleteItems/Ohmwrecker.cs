using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Ohmwrecker : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ohmwrecker");
            Tooltip.SetDefault("Increases maximum life by 20" +
                "\nIncreases armor by 6" +
                "\nIncreases life regeneration by 3" +
                "\nIncreases ability haste by 10" +
                "\nIncreases your max number of sentries" +
                "\n8% increased movement speed");
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
                new Disruption()
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.moveSpeed += 0.08f;
            player.statLifeMax2 += 20;
            player.lifeRegen += 3;
            player.maxTurrets += 1;
            player.GetModPlayer<PLAYERGLOBAL>().armor += 6;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 10;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<RaptorCloak>(), 1)
            .AddIngredient(ItemType<Kindlegem>(), 1)
            .AddIngredient(ItemID.SoulofLight, 5)
            .AddIngredient(ItemID.CrystalShard, 10)
            .AddTile(TileID.Anvils)
            .Register();
            
        }
    }
}
