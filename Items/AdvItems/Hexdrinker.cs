using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class Hexdrinker : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hexdrinker");
            Tooltip.SetDefault("3% increased melee and ranged damage" +
                "\nIncreases resist by 3");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 15, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;

            Passives = new Passive[]
            {
                new Lifeline(90)
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Melee) += 0.03f;
            player.GetDamage(DamageClass.Ranged) += 0.03f;
            player.GetModPlayer<PLAYERGLOBAL>().resist += 3;
            base.UpdateAccessory(player, hideVisual);

        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<LongSword>(), 1)
            .AddIngredient(ItemType<NullMagic>(), 1)
            .AddIngredient(ItemType<Petricite>(), 12)
            .AddIngredient(ItemID.MeteoriteBar, 5)
            .AddIngredient(ItemID.Amber, 4)
            .AddTile(TileID.Anvils)
            .Register();
            
        }

        public override string GetStatText()
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();

            if (Passives[0].currentlyActive)
            {
                if (modPlayer.lifeLineCooldown > 0)
                    return (modPlayer.lifeLineCooldown / 60).ToString();
            }
            return "";
        }

        public override bool OnCooldown(Player player)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (modPlayer.lifeLineCooldown > 0 || !Passives[0].currentlyActive)
                return true;
            else
                return false;
        }
    }
}
