using Microsoft.Xna.Framework;
using TerraLeague.Items;
using TerraLeague.Items.Banners;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.NPCs
{
    public class SoulBoundSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Soulbound Slime");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.BlueSlime];
        }
        public override void SetDefaults()
        {
            NPC.width = 44;
            NPC.height = 32;
            NPC.aiStyle = 1;
            NPC.damage = 18;
            NPC.defense = 6;
            NPC.lifeMax = 50;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.05f;
            NPC.value = 25f;
            AIType = NPCID.BlueSlime;
            AnimationType = NPCID.BlueSlime;
            base.SetDefaults();
            NPC.scale = 1f;
            Banner = NPC.type;
            BannerItem = ItemType<SoulBoundSlimeBanner>();
        }

        public override bool PreAI()
        {
            Lighting.AddLight(NPC.Center, new Color(5, 245, 150).ToVector3());

            return base.PreAI();
        }

        public override void AI()
        {
            base.AI();
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.player.GetModPlayer<PLAYERGLOBAL>().zoneBlackMist)
                return SpawnCondition.OverworldNightMonster.Chance * 0.5f;
            return 0;
        }

        

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            
            base.OnHitPlayer(target, damage, crit);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life > 0)
            {
                int num262 = 0;
                while ((double)num262 < damage / (double)NPC.lifeMax * 100.0)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.t_Slime, (float)hitDirection, -1f, NPC.alpha, new Color(5, 245, 150), 1f);
                    int num5 = num262;
                    num262 = num5 + 1;
                }
            }
            else
            {
                for (int i = 0; i < 20; i++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.t_Slime, (float)hitDirection, -1f, NPC.alpha, new Color(5, 245, 150), 1f);
                }
            }

            base.HitEffect(hitDirection, damage);
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemType<DamnedSoul>(), 1));
            npcLoot.Add(ItemDropRule.Common(ItemID.Gel, 1, 1, 3));
            //Main.item[item].color = new Color(67,248,175);
            base.ModifyNPCLoot(npcLoot);
        }
    }
}
