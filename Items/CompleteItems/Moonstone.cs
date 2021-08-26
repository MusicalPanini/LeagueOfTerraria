using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using TerraLeague.Items.AdvItems;
using static Terraria.ModLoader.ModContent;
using TerraLeague.Items.BasicItems;

namespace TerraLeague.Items.CompleteItems
{
    public class Moonstone : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Moonstone");
            Tooltip.SetDefault("6% increased magic and summon damage" +
                "\nIncreases mana regeneration by 20%" +
                "\nIncreases maximum life by 20" +
                "\nIncreases ability haste by 20" +
                "\nReduces the cooldown of healing potions by 25%");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 60, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.accessory = true;

            Passives = new Passive[]
            {
                new StarlitGrace(12, 25, 4, 4)
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 20;
            player.GetModPlayer<PLAYERGLOBAL>().manaRegenModifer += 0.2;
            player.GetDamage(DamageClass.Magic) += 0.06f;
            player.GetDamage(DamageClass.Summon) += 0.06f;
            player.moveSpeed += 0.08f;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<Kindlegem>(), 1)
            .AddIngredient(ItemType<Codex>(), 1)
            .AddIngredient(ItemType<FaerieCharm>(), 1)
            .AddIngredient(ItemType<FragmentOfTheAspect>(), 1)
            .AddIngredient(ItemID.PhilosophersStone, 1)
            .AddTile(TileID.MythrilAnvil)
            .Register();
            
        }

        public override string GetStatText()
        {
            if (Passives[0].currentlyActive)
                return ((int)Passives[0].passiveStat / 60).ToString();
            else
                return "";
        }
    }
}
