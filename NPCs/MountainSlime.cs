using Microsoft.Xna.Framework;
using TerraLeague.Biomes;
using TerraLeague.Items;
using TerraLeague.Items.Banners;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.NPCs
{
    public class MountainSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mountain Slime");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Slimer];

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            { // Influences how the NPC looks in the Bestiary
                Position = new Vector2(0, -4),
                PortraitPositionYOverride = -24
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }
        public override void SetDefaults()
        {
            NPC.width = 40;
            NPC.height = 30;
            NPC.aiStyle = 14;
            NPC.damage = 12;
            NPC.defense = 4;
            NPC.lifeMax = 45;
            NPC.value = 20;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            AIType = NPCID.GiantBat;
            AnimationType = NPCID.Slimer;
            NPC.scale = 1f;
            Banner = NPC.type;
            BannerItem = ItemType<MountainSlimeBanner>();
            base.SetDefaults();
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.player.GetModPlayer<PLAYERGLOBAL>().zoneTargon)
                return SpawnCondition.Overworld.Chance * 1f;
            return 0;
        }

        public override bool PreAI()
        {
            //Lighting.AddLight(NPC.Center, new Color(5, 245, 150).ToVector3());
            //if (npc.localAI[3] == 0)
            //{
            //    for (int j = 0; j < 50; j++)
            //    {
            //        Dust dust = Dust.NewDustDirect(NPC.position, 18, 40, 188);
            //        dust.noGravity = true;
            //        dust.scale = 2;
            //    }

            //    npc.localAI[3] = 1;
            //}
            return base.PreAI();
        }

        public override void AI()
        {
            base.AI();
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
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.t_Slime, (float)hitDirection, -1f, NPC.alpha, Color.RosyBrown, 1f);
                    int num5 = num262;
                    num262 = num5 + 1;
                }
            }
            else
            {
                for (int i = 0; i < 20; i++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.t_Slime, (float)hitDirection, -1f, NPC.alpha, Color.RosyBrown, 1f);
                }
            }
            base.HitEffect(hitDirection, damage);
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.NormalvsExpert(ItemID.SlimeStaff, 10000, 7000));
            base.ModifyNPCLoot(npcLoot);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
                ModContent.GetInstance<MountTargonBiome>().ModBiomeBestiaryInfoElement,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("The harsh slopes of Targon are a dangerous and hellish place to live, let alone traverse. But life, uhh.. finds a way.")
            });
        }
    }
}
