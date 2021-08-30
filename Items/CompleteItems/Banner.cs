using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Actives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Banner : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Banner of Command");
            Tooltip.SetDefault("Increases armor by 6" +
                "\nIncreases resist by 4" +
                "\nIncreases your max number of minions" +
                "\nIncreases your max number of sentries" +
                "\nIncreases life regeneration by 2");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 50, 0, 0);
            Item.rare = ItemRarityID.Lime;
            Item.accessory = true;

            Active = new Rally(3, 20, 100);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.maxMinions += 1;
            player.maxTurrets += 1;
            player.GetModPlayer<PLAYERGLOBAL>().armor += 6;
            player.GetModPlayer<PLAYERGLOBAL>().resist += 4;
            player.lifeRegen += 2;
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<Aegis>(), 1)
            .AddIngredient(ItemType<RaptorCloak>(), 1)
            .AddIngredient(ItemID.PygmyNecklace)
            .AddIngredient(ItemType<BrassBar>(), 8)
            .AddTile(TileID.MythrilAnvil)
            .Register();
            
        }

        public override string GetStatText()
        {
            if (Active.currentlyActive)
            {
                if (Active.cooldownCount > 0)
                    return (Active.cooldownCount / 60).ToString();
            }
            return "";
        }

        public override bool OnCooldown(Player player)
        {
            if (Active.cooldownCount > 0 || !Active.currentlyActive)
                return true;
            else
                return false;
        }
    }
}

