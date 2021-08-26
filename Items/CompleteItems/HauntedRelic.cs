using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using TerraLeague.Items.AdvItems;
using static Terraria.ModLoader.ModContent;
using TerraLeague.Items.BasicItems;
using TerraLeague.Items.Accessories;

namespace TerraLeague.Items.CompleteItems
{
    public class HauntedRelic : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Haunted Relic");
            Tooltip.SetDefault("6% increased summon damage" +
                "\nIncreases health by 20" +
                "\nIncreases your max number of minions");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 60, 0, 0);
            Item.rare = ItemRarityID.Lime;
            Item.accessory = true;

            Passives = new Passive[]
            {
                new Haunted()
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.maxMinions += 1;
            player.statLifeMax2 += 20;
            player.GetDamage(DamageClass.Summon) += 0.06f;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<RubyCrystal>(), 1)
            .AddIngredient(ItemType<BlastingWand>(), 1)
            .AddIngredient(ItemType<CloakofAgility>(), 1)
            .AddIngredient(ItemType<PossessedSkull>(), 1)
            .AddIngredient(ItemID.EyeoftheGolem, 1)
            .AddIngredient(ItemType<DamnedSoul>(), 50)
            .AddTile(TileID.MythrilAnvil)
            .Register();
            
        }
    }
}
