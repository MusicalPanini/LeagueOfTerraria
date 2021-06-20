using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using TerraLeague.NPCs.TargonBoss;
using Terraria;
using Terraria.Audio;
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
        }

        public override void SetDefaults()
        {
            item.rare = ItemRarityID.Blue;
            item.width = 32;
            item.height = 32;
            item.maxStack = 99;
            item.useTime = 10;
            item.useAnimation = 10;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.consumable = true;
            item.UseSound = new LegacySoundStyle(2, 4);
        }

        public override bool CanUseItem(Player player)
        {
            if (player.HasBuff(BuffType<InTargonArena>()) && NPC.CountNPCS(NPCType<TargonBossNPC>()) <= 0)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (Main.netMode == NetmodeID.SinglePlayer)
                        Main.NewText("Targon's Challenge has begun", 0, 200, 255);

                    NPC.NewNPC(TerraLeagueWORLDGLOBAL.TargonCenterX, ((int)Main.worldSurface * 16) + 64, NPCType<TargonBossNPC>());
                }
                else
                {
                    NetMessage.BroadcastChatMessage(NetworkText.FromKey("Targon's Challenge has begun", new object[0]), new Color(0, 200, 255), -1);

                    TerraLeagueNPCsGLOBAL.PacketHandler.SendSpawnNPC(-1, Main.LocalPlayer.whoAmI, NPCType<TargonBossNPC>(), new Vector2(TerraLeagueWORLDGLOBAL.TargonCenterX, (float)(Main.worldSurface * 16) + 64));
                }

                item.stack -= 1;

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
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<CelestialBar>(), 4);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
