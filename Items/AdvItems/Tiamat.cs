using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class Tiamat : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tiamat");
            Tooltip.SetDefault("3% increased melee damage" +
                "\nIncreases life regeneration by 2");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.value = Item.buyPrice(0, 15, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;

            Passives = new Passive[]
            {
                new Cleave(40, CleaveType.Basic)
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Melee) += 0.03f;
            player.lifeRegen += 2;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<LongSword>(), 2)
            .AddIngredient(ItemType<RejuvBead>(), 1)
            .AddIngredient(ItemID.Spear, 1)
            .AddIngredient(ItemID.Spike, 10)
            .AddTile(TileID.Anvils)
            .Register();
            
        }
    }
}
