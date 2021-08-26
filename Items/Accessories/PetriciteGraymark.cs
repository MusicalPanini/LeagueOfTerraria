using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerraLeague.NPCs;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Accessories
{
    public class PetriciteGraymark : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Petricite Graymark");
            Tooltip.SetDefault("Gain 10 resist after taking damage from a projectile for 4 seconds");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.rare = ItemRarityID.LightRed;
            Item.value = 6400 * 5;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            modPlayer.greymark = true;

            if (modPlayer.greymarkBuff)
                modPlayer.resist += 10;
        }

        public override void AddRecipes()
        {
            //CreateRecipe()
            //.AddIngredient(ItemType<SilversteelBar>(), 12);
            //.AddTile(TileID.MythrilAnvil);
            //.Register();
            //
        }

        public class GraymarkNPC : GlobalNPC
        {
            public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
            {
                if (npc.netID == NPCID.GreekSkeleton || npc.netID == NPCID.Medusa)
                {
                    npcLoot.Add(ItemDropRule.NormalvsExpert(ItemType<PetriciteGraymark>(), 250, 125));
                    Item.NewItem(npc.getRect(), ItemType<PetriciteGraymark>(), 1);
                }
                base.ModifyNPCLoot(npc, npcLoot);
            }
        }
    }
}
