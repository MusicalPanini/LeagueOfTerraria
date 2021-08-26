using Microsoft.Xna.Framework;
using TerraLeague.Biomes;
using TerraLeague.Gores;
using TerraLeague.Items;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.NPCs
{
    public class Ghoul : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ghoul");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.BloodZombie];
        }
        public override void SetDefaults()
        {
            NPC.width = 18;
            NPC.height = 40;
            NPC.aiStyle = 3;
            NPC.damage = 12;
            NPC.defense = 0;
            NPC.lifeMax = 20;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath2;
            AIType = NPCID.BloodZombie;
            AnimationType = NPCID.BloodZombie;
            NPC.scale = 1f;
            base.SetDefaults();
        }

        public override bool PreAI()
        {
            Lighting.AddLight(NPC.Center, new Color(5, 245, 150).ToVector3());
            if (NPC.localAI[3] == 0)
            {
                for (int j = 0; j < 50; j++)
                {
                    Dust dust = Dust.NewDustDirect(NPC.position, 18, 40, 188);
                    dust.noGravity = true;
                    dust.scale = 2;
                }

                NPC.localAI[3] = 1;
            }
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
                int count = 0;
                while ((double)count < damage / (double)NPC.lifeMax * 50.0)
                {
                    Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Cloud, 0f, 0f, 0, new Color(100, 100, 100), 1.5f);
                    dust.velocity *= 2f;
                    dust.noGravity = true;
                    count++;
                }
            }
            else
            {
                for (int i = 0; i < 20; i++)
                {
                    Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Cloud, 0f, 0f, 0, new Color(5, 245, 150), 1.5f);
                    dust.velocity *= 2f;
                    dust.noGravity = true;
                }

                Gore.NewGore(NPC.Center, NPC.velocity / 2, GoreType<Ghoul_1>(), 1f);
                Gore.NewGore(NPC.Top, NPC.velocity / 2, GoreType<Ghoul_2>(), 1f);
                Gore.NewGore(NPC.Center, NPC.velocity / 2, GoreType<Ghoul_3>(), 1f);
                Gore.NewGore(NPC.Bottom, NPC.velocity / 2, GoreType<Ghoul_4>(), 1f);
            }
            base.HitEffect(hitDirection, damage);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                ModContent.GetInstance<BlackMistBiome>().ModBiomeBestiaryInfoElement,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("Reanimated bodies whos souls were taken before their time. Their decaying minds have been pushed to madness and violence by necrotic magic")
            });
        }
    }
}
