using Microsoft.Xna.Framework;
using TerraLeague.NPCs;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Accessories
{
    public class EternalFlame : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eternal Flame");
            Tooltip.SetDefault("Emit small flames to light your way");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 0;
            Item.width = 22;
            Item.height = 32;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.value = Item.buyPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = new Terraria.Audio.LegacySoundStyle(2, 45);
            Item.buffType = BuffType<Buffs.CandlePet>();
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(Item.buffType, 3600);
            }
        }
    }

    public class EternalFlameGlobalNPC : GlobalNPC
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
                npcLoot.Add(ItemDropRule.NormalvsExpert(ItemType<EternalFlame>(), 250, 125));
            }

        }
    }
}
