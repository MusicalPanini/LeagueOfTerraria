using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class Tear : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tear of the Goddess");
            Tooltip.SetDefault("Increases maximum mana by 10" +
                "\nCan only have one AWE item equiped at a time");
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

            Passives = new Passive[]
            {
                new ManaCharge(this),
                new Awe(6, 0, 0, this)
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statManaMax2 += 10;
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<SapphireCrystal>(), 1)
            .AddIngredient(ItemType<FaerieCharm>(), 1)
            .AddIngredient(ItemID.NaturesGift, 1)
            .AddIngredient(ItemType<CelestialBar>(), 4)
            .AddIngredient(ItemType<ManaBar>(), 4)
            .AddTile(TileID.Anvils)
            .Register();
            
        }

        public override string GetStatText()
        {
            if (Passives[0].currentlyActive)
                return Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().manaChargeStacks.ToString();
            else
                return "";
        }
    }

    public class ManaChargeGLOBAL : GlobalItem
    {
        public override bool OnPickup(Item item, Player player)
        {
            if (item.type == ItemID.Star || item.type == ItemID.SoulCake || item.type == ItemID.SugarPlum)
            {
                PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
                if (modPlayer.manaCharge && modPlayer.manaChargeStacks < 750)
                    modPlayer.manaChargeStacks++;
            }

            return base.OnPickup(item, player);
        }
    }
}
