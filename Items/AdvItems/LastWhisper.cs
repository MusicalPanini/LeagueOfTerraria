using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class LastWhisper : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Last Whisper");
            Tooltip.SetDefault("3% increased melee and ranged damage");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;

            Passives = new Passive[]
            {
                new CustomItems.Passives.LastWhisper(15, true)
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Melee) += 0.03f;
            player.GetDamage(DamageClass.Ranged) += 0.03f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<LongSword>(), 1)
            .AddIngredient(ItemID.SharkToothNecklace, 1)
            .AddIngredient(ItemID.DemonBow, 1)
            .AddTile(TileID.Anvils)
            .Register();

            CreateRecipe()
            .AddIngredient(ItemType<LongSword>(), 1)
            .AddIngredient(ItemID.SharkToothNecklace, 1)
            .AddIngredient(ItemID.TendonBow, 1)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
