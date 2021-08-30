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
    public class GuardianAngel : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Guardian Angel");
            Tooltip.SetDefault("6% increased melee and ranged damage" +
                "\nIncreases armor by 4" +
                "\nNegates fall damage");
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
                new AngelsBlessing(240, this)
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.noFallDmg = true;
            player.GetDamage(DamageClass.Melee) += 0.06f;
            player.GetDamage(DamageClass.Ranged) += 0.06f;
            player.GetModPlayer<PLAYERGLOBAL>().armor += 4;
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<BFSword>(), 1)
            .AddIngredient(ItemType<Stopwatch>(), 1)
            .AddIngredient(ItemType<ChainVest>(), 1)
            .AddIngredient(ItemID.AngelWings, 1)
            .AddIngredient(ItemID.Excalibur, 1)
            .AddIngredient(ItemID.Ectoplasm, 5)
            .AddTile(TileID.MythrilAnvil)
            .Register();
            
        }

        public override string GetStatText()
        {
            if (Passives[0].currentlyActive)
            {
                if (Passives[0].cooldownCount > 0)
                    return (Passives[0].cooldownCount / 60).ToString();
            }
            return "";
        }

        public override bool OnCooldown(Player player)
        {
            if (Passives[0].cooldownCount > 0 || !Passives[0].currentlyActive)
                return true;
            else
                return false;
        }
    }
}
