using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Randuins : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Randuin's Omen");
            Tooltip.SetDefault("Increases maximum life by 30" +
                "\nIncreases armor by 8" +
                "\nPuts a shell around the owner when below 50% life that reduces damage");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 50, 0, 0);
            Item.rare = ItemRarityID.Lime;
            Item.accessory = true;
            Item.material = true;

            Passives = new Passive[]
            {
                new PowersBane(100, 30, this),
                new ColdSteel(3, 350, this)
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().armor += 6;
            player.statLifeMax2 += 30;

            if (player.GetModPlayer<PLAYERGLOBAL>().GetRealHeathWithoutShield() <= player.GetModPlayer<PLAYERGLOBAL>().GetRealHeathWithoutShield(true) / 2)
                player.AddBuff(BuffID.IceBarrier, 10);
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<Wardens>(), 1)
            .AddIngredient(ItemType<GiantsBelt>(), 1)
            .AddIngredient(ItemID.FrozenTurtleShell, 1)
            .AddIngredient(ItemType<SilversteelBar>(), 10)
            .AddIngredient(ItemID.HallowedBar, 10)
            .AddIngredient(ItemID.Ectoplasm, 10)
            .AddTile(TileID.MythrilAnvil)
            .Register();
            
        }
    }
}
