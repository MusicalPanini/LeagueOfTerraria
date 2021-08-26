using System.Collections.Generic;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Actives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.BasicItems
{
    public class Stopwatch : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magic Stopwatch");
            Tooltip.SetDefault("\'Seems fragile\'");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
            Item.material = true;

            Active = new Stasis(2, 120, true);
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {

        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Stopwatch, 1)
            .AddIngredient(ItemType<HextechCore>(), 2)
            .AddIngredient(ItemID.SoulofLight, 6)
            .AddIngredient(ItemID.SoulofNight, 6)
            .AddTile(TileID.CrystalBall)
            .Register();
            
        }

        public override bool OnCooldown(Player player)
        {
            if (Active.currentlyActive)
            {
                if (!Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().stopWatchActive)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
    }
}
