using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Actives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class Cutlass : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bilgewater Cutlass");
            Tooltip.SetDefault("4% increased melee and ranged damage" +
                "\n+1 melee and ranged life steal" /*+
                "\n10% decreased maximum life" +
                "\n10% increased damage taken"*/);
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 15, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;

            Active = new Damnation(100, 75);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Melee) += 0.04f;
            player.GetDamage(DamageClass.Ranged) += 0.04f;
            player.GetModPlayer<PLAYERGLOBAL>().lifeStealMelee += 1;
            player.GetModPlayer<PLAYERGLOBAL>().lifeStealRange += 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<VampiricScepter>(), 1)
            .AddIngredient(ItemType<LongSword>(), 1)
            .AddIngredient(ItemType<BrassBar>(), 6)
            .AddIngredient(ItemID.HellstoneBar, 5)
            .AddTile(TileID.Anvils)
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
