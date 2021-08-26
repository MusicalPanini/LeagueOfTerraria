using System.Collections.Generic;
using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class Jaurim : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Jaurim's Fist");
            Tooltip.SetDefault("5% increased melee damage" +
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
                new Strengthen(20, 1)
            };
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += 10;
            player.GetDamage(DamageClass.Melee) += 0.05f;
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<LongSword>(), 1)
            .AddIngredient(ItemType<RubyCrystal>(), 1)
            .AddIngredient(ItemID.MeteoriteBar, 10)
            .AddIngredient(ItemType<DarksteelBar>(), 10)
            .AddIngredient(ItemID.Spike, 5)
            .AddTile(TileID.Anvils)
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
