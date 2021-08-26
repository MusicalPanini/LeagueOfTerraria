using TerraLeague.Items.AdvItems;
using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Actives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class MercScim : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mercurial Scimitar");
            Tooltip.SetDefault("5% increased ranged damage" +
                "\nIncreases resist by 5" +
                "\n+1 ranged life steal"/* +
                "\n10% decreased maximum life" +
                "\n10% increased damage taken"*/);
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 45, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.accessory = true;

            Active = new Quicksilver(6, 60);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Ranged) += 0.05f;
            player.GetModPlayer<PLAYERGLOBAL>().lifeStealRange += 1;//0.04;
            player.GetModPlayer<PLAYERGLOBAL>().resist += 5;
            //player.GetModPlayer<PLAYERGLOBAL>().healthModifier -= 0.1;
            //player.GetModPlayer<PLAYERGLOBAL>().damageTakenModifier -= 0.1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<QuicksilverSash>(), 1)
            .AddIngredient(ItemType<Pickaxe>(), 1)
            .AddIngredient(ItemType<VampiricScepter>(), 1)
            .AddIngredient(ItemID.HallowedBar, 10)
            .AddIngredient(ItemType<SilversteelBar>(), 6)
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
