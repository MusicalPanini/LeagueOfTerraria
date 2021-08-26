using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class FrozenMallet : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frozen Mallet");
            Tooltip.SetDefault("6% increased melee and ranged damage" +
                "\nIncreases health by 40");
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
                new Icy(2)
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Melee) += 0.06f;
            player.GetDamage(DamageClass.Ranged) += 0.06f;
            player.statLifeMax2 += 40;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<Jaurim>(), 1)
            .AddIngredient(ItemType<GiantsBelt>(), 1)
            .AddIngredient(ItemType<TrueIceChunk>(), 10)
            .AddIngredient(ItemID.Pwnhammer, 1)
            .AddTile(TileID.Anvils)
            .Register();
            
        }
    }
}
