using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using TerraLeague.Items.CustomItems;
using TerraLeague.NPCs;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Accessories
{
    public class SunAmulet : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sun Amulet");
            Tooltip.SetDefault("Gain damage based on the light level your in");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Tooltip0" && x.mod == "Terraria");
            Player player = Main.LocalPlayer;
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (tt != null)
            {
                tt.text = "Gain damage based on the light level your in (Current damage: " + modPlayer.sunAmuletDamage + "%)";
            }
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 32;
            Item.rare = ItemRarityID.LightRed;
            Item.value = 6400 * 5;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            modPlayer.sunAmulet = true;
        }

        public override void AddRecipes()
        {
            //CreateRecipe()
            //.AddIngredient(ItemID.CrossNecklace, 1);
            //.AddIngredient(ItemType<Nightbloom>(), 1);
            //.AddTile(TileID.TinkerersWorkbench);
            //.Register();
            //
        }

        public override string GetStatText()
        {
            int slot = TerraLeague.FindAccessorySlotOnPlayer(Main.LocalPlayer, this);
            Player player = Main.LocalPlayer;
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (slot != -1)
                return modPlayer.sunAmuletDamage + "%";
            else
                return "";
        }
    }

    public class SunAmuletNPC : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.netID == NPCID.Antlion ||
                npc.netID == NPCID.FlyingAntlion ||
                npc.netID == NPCID.WalkingAntlion ||
                npc.netID == NPCType<ShadowArtilery>())
            {
                npcLoot.Add(ItemDropRule.NormalvsExpert(ItemType<SunAmulet>(), 250, 125));
            }
            base.ModifyNPCLoot(npc, npcLoot);
        }
    }
}
