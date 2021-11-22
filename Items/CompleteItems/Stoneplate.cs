using TerraLeague.Items.AdvItems;
using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Actives;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Stoneplate : MasterworkItem
    {
        public override string MasterworkName => "Dvarapala's Stoneplate";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gargoyle's Stoneplate");
            Tooltip.SetDefault("Increases armor by 5" +
                "\nIncreases resist by 5");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 45, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.accessory = true;

            Active = new Metallicize(90);
            Passives = new Passive[]
            {
                new StoneSkin(5, 5, this)
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().armor += 5;
            player.GetModPlayer<PLAYERGLOBAL>().resist += 5;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<ChainVest>(), 1)
            .AddIngredient(ItemType<Stopwatch>(), 1)
            .AddIngredient(ItemType<NegatronCloak>(), 1)
            .AddIngredient(ItemType<Petricite>(), 20)
            .AddIngredient(ItemType<SilversteelBar>(), 5)
            .AddRecipeGroup("TerraLeague:Tier3Bar", 5)
            .AddIngredient(ItemID.Marble, 50)
            .AddIngredient(ItemID.SoulofMight, 10)
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

        public override string MasterworkTooltip()
        {
            return "Increases armor by " + LeagueTooltip.CreateColorString(MasterColor, "8") +
                "\nIncreases resist by " + LeagueTooltip.CreateColorString(MasterColor, "8");
        }

        public override void UpdateMasterwork(Player player)
        {
            player.GetModPlayer<PLAYERGLOBAL>().armor += 3;
            player.GetModPlayer<PLAYERGLOBAL>().resist += 3;
        }
    }
}
