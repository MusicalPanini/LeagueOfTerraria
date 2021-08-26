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
            Tooltip.SetDefault("Use in the Arena to summon the Gate Keeper");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
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
            if (player.HasBuff(BuffType<InTargonArena>()) && NPC.CountNPCS(NPCType<TargonBossNPC>()) <= 0)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (Main.netMode == NetmodeID.SinglePlayer)
                        Main.NewText("Targon's Challenge has begun", 0, 200, 255);

                    NPC.NewNPC(Common.ModSystems.WorldSystem.TargonCenterX, ((int)Main.worldSurface * 16) + 64, NPCType<TargonBossNPC>());
                }
                else
                {
                    ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Targon's Challenge has begun", new object[0]), new Color(0, 200, 255), -1);

                    TerraLeagueNPCsGLOBAL.PacketHandler.SendSpawnNPC(-1, Main.LocalPlayer.whoAmI, NPCType<TargonBossNPC>(), new Vector2(Common.ModSystems.WorldSystem.TargonCenterX, (float)(Main.worldSurface * 16) + 64));
                }

                Item.stack -= 1;

                return base.CanUseItem(player);
            }
            return false;
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
