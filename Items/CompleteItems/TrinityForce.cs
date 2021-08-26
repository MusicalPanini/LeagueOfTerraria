using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class TrinityForce : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Trinity Force");
            Tooltip.SetDefault("15% increased melee damage and speed" +
                "\n7% increased movement speed" +
                "\nIncreases maximum life by 20" +
                "\nIncreases maximum mana by 30" +
                "\nIncreases ability haste by 20");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.value = Item.buyPrice(0, 53, 33, 33);
            Item.rare = ItemRarityID.Yellow;
            Item.accessory = true;
            Item.material = true;

            Passives = new Passive[]
            {
                new Spellblade(2, 1),
                new Rage(5)
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Melee) += 0.15f;
            player.meleeSpeed += 0.15f;
            player.moveSpeed += 0.07f;
            player.statLifeMax2 += 20;
            player.statManaMax2 += 30;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 20;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<Sheen>(), 1)
            .AddIngredient(ItemType<Phage>(), 1)
            .AddIngredient(ItemType<Stinger>(), 1)
            .AddIngredient(ItemType<HarmonicBar>(), 3)
            .AddIngredient(ItemID.ShroomiteBar, 3)
            .AddIngredient(ItemID.SpectreBar, 3)
            .AddTile(TileID.MythrilAnvil)
            .Register();
            
        }
    }
}
