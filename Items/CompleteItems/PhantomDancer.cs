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
    public class PhantomDancer : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Phantom Dancer");
            Tooltip.SetDefault("20% increased melee speed" +
                "\n15% increased melee critical strike chance" +
                "\n10% increased movement speed");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 50, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.accessory = true;
            Item.material = true;

            Passives = new Passive[]
            {
                new Lifeline(90, this)
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.meleeSpeed += 0.20f;
            player.GetCritChance(DamageClass.Melee) += 15;
            player.moveSpeed += 0.1f;
            player.accRunSpeed *= 1.1f;
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<Dagger>(), 2)
            .AddIngredient(ItemType<Zeal>(), 1)
            .AddIngredient(ItemID.Sickle, 1)
            .AddIngredient(ItemType<DamnedSoul>(), 50)
            .AddIngredient(ItemID.Ectoplasm, 5)
            .AddIngredient(ItemID.SpectreBar, 12)
            .AddTile(TileID.MythrilAnvil)
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
