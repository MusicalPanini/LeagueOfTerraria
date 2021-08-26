using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Tools
{
    public class FadingMemories : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Faded Mirror");
            Tooltip.SetDefault("Return to where you last died" +
                "\n10 minute cooldown");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Melee;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.value = Item.buyPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = new LegacySoundStyle(2, 6);
        }

        public override bool CanUseItem(Player player)
        {
            if (player.lastDeathPostion != Vector2.Zero && !player.HasBuff(BuffType<Faded>()))
            {
                player.Teleport(player.lastDeathPostion);
                player.AddBuff(BuffType<Faded>(), 60 * 60 * 10);
                return true;
            }

            return false;
        }

        public override void AddRecipes()
        {
            base.AddRecipes();
        }
    }

    public class FadedMirrorGlobalNPC : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (NPCType<TheUndying_1>() == npc.type ||
                NPCType<TheUndying_2>() == npc.type ||
                NPCType<TheUndying_Archer>() == npc.type || 
                NPCType<TheUndying_Necromancer>() == npc.type || 
                NPCType<Mistwraith>() == npc.type ||
                NPCType<UnleashedSpirit>() == npc.type)
            {
                npcLoot.Add(ItemDropRule.NormalvsExpert(ItemType<FadingMemories>(), 250, 125));
            }

            base.ModifyNPCLoot(npc, npcLoot);
        }
    }
}
