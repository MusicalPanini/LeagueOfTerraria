using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using TerraLeague.NPCs.TargonBoss;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items
{
    public class TargonMedallion : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Targon Medallion");
            Tooltip.SetDefault("Use at night to summon the Gate Keeper");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
            ItemID.Sets.SortingPriorityBossSpawns[Type] = 12;
            NPCID.Sets.MPAllowedEnemies[NPCType<TargonBossNPC>()] = true;
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Blue;
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 99;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
            Item.UseSound = new LegacySoundStyle(2, 4);
        }

        public override bool CanUseItem(Player player)
        {
            // If you decide to use the below UseItem code, you have to include !NPC.AnyNPCs(id), as this is also the check the server does when receiving MessageID.SpawnBoss.
            // If you want more constraints for the summon item, combine them as boolean expressions:
            //    return !Main.dayTime && !NPC.AnyNPCs(ModContent.NPCType<MinionBossBody>()); would mean "not daytime and no MinionBossBody currently alive"
            return !NPC.AnyNPCs(ModContent.NPCType<TargonBossNPC>()) && !Main.dayTime;
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                //SoundEngine.PlaySound(SoundID.Roar, player.position, 0);

                Vector2 position = player.Bottom;
                position.Y -= 700;

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (Main.netMode == NetmodeID.SinglePlayer)
                        Main.NewText("Targon's Challenge has begun", 0, 200, 255);

                    NPC.NewNPC((int)position.X, (int)position.Y, NPCType<TargonBossNPC>());
                }
                else
                {
                    ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Targon's Challenge has begun", new object[0]), new Color(0, 200, 255), -1);

                    TerraLeagueNPCsGLOBAL.PacketHandler.SendSpawnNPC(-1, Main.LocalPlayer.whoAmI, NPCType<TargonBossNPC>(), position);
                }
            }

            return true;
        }

        public override void OnConsumeItem(Player player)
        {
            

            base.OnConsumeItem(player);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<CelestialBar>(), 4)
            .AddTile(TileID.Anvils)
            .Register();
            
        }
    }
}
