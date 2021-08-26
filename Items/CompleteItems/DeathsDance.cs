using TerraLeague.Items.AdvItems;
using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class DeathsDance : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Death's Dance");
            Tooltip.SetDefault("7% increased melee and ranged damage" +
                "\nIncreases ability haste by 10");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 50, 0, 0);
            Item.rare = ItemRarityID.Lime;
            Item.accessory = true;

            Passives = new Passive[]
            {
                new Omniheal(2),
                new CauterizedWounds(30)
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Melee) += 0.07f;
            player.GetDamage(DamageClass.Ranged) += 0.07f;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 10;
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<Warhammer>(), 1)
            .AddIngredient(ItemType<Pickaxe>(), 1)
            .AddIngredient(ItemType<VampiricScepter>(), 1)
            .AddIngredient(ItemType<Sunstone>(), 20)
            .AddIngredient(ItemType<VoidBar>(), 6)
            .AddIngredient(ItemID.Seedler)
            .AddTile(TileID.MythrilAnvil)
            .Register();
            
        }

        public override string GetStatText()
        {
            if (Passives[1].currentlyActive)
                return ((int)Passives[1].passiveStat).ToString();
            else
                return "";
        }
    }
}

