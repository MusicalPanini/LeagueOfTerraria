using TerraLeague.Items.Accessories;
using TerraLeague.NPCs;
using TerraLeague.NPCs.TargonBoss;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.BossBags
{
    public class TargonBossBag : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag");
			Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
		}

		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.consumable = true;
			Item.width = 24;
			Item.height = 24;
			Item.rare = ItemRarityID.Expert;
			Item.expert = true;
		}

		public override bool CanRightClick()
		{
			return true;
		}

		public override void OpenBossBag(Player player)
		{
			player.TryGettingDevArmor();
			int choice = Main.rand.Next(5);
			if (choice == 0)
			{
				player.QuickSpawnItem(ModContent.ItemType<Placeable.TargonMonolith>());
			}
			player.QuickSpawnItem(ModContent.ItemType<CelestialBar>(), Main.rand.Next(4, 11));
			player.QuickSpawnItem(ModContent.ItemType<BottleOfStardust>());
		}

		public override int BossBagNPC => ModContent.NPCType<TargonBossNPC>();
	}
}