using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class KircheisShard : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Kircheis Shard");
            Tooltip.SetDefault("12% increased melee and ranged attack speed");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 52;
            Item.height = 20;
            Item.value = Item.buyPrice(0, 15, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;
            Item.material = true;

            Passives = new Passive[]
            {
                new Energized(10, 15, this),
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.meleeSpeed += 0.12f;
            player.GetModPlayer<PLAYERGLOBAL>().rangedAttackSpeed += 0.10;
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<Dagger>(), 1)
            .AddIngredient(ItemID.MeteoriteBar, 5)
            .AddIngredient(ItemID.HellstoneBar, 5)
            .AddTile(TileID.Anvils)
            .Register();
            
        }

        public override string GetStatText()
        {
            if (Passives[0].currentlyActive)
                return ((int)Passives[0].passiveStat).ToString() + "%";
            else
                return "";
        }

        public override bool OnCooldown(Player player)
        {
            return !Passives[0].currentlyActive;
        }
    }
}
