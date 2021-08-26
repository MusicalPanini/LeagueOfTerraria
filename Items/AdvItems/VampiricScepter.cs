using TerraLeague.Items.BasicItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class VampiricScepter : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vampiric Scepter");
            Tooltip.SetDefault("3% increased melee and ranged damage" +
                "\n+1 melee and ranged life steal" /*+
                "\n10% decreased maximum life" +
                "\n10% increased damage taken"*/);
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;
            Item.material = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Melee) += 0.03f;
            player.GetDamage(DamageClass.Ranged) += 0.03f;

            player.GetModPlayer<PLAYERGLOBAL>().lifeStealMelee += 1;
            player.GetModPlayer<PLAYERGLOBAL>().lifeStealRange += 1;
            //player.GetModPlayer<PLAYERGLOBAL>().damageTakenModifier += 0.1;
            //player.GetModPlayer<PLAYERGLOBAL>().healthModifier -= 0.1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<LongSword>(), 1)
            .AddIngredient(ItemID.MeteoriteBar, 5)
            .AddIngredient(ItemID.HellstoneBar, 5)
            .AddIngredient(ItemID.Ruby, 2)
            .AddTile(TileID.Anvils)
            .Register();
            
        }
    }
}
