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
    public class EtherealRemitter : ModNPC
    {
        readonly int effectRadius = 500;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ethereal Remitter");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Wraith];
        }
        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.width = 34;
            NPC.height = 50;
            NPC.damage = 10;
            NPC.defense = 8;
            NPC.lifeMax = 60;
            NPC.aiStyle = 22;
            NPC.HitSound = SoundID.NPCHit54;
            NPC.DeathSound = SoundID.NPCDeath52;
            NPC.value = 100f;
            NPC.npcSlots = 2;
            AIType = NPCID.Wraith;
            AnimationType = NPCID.Wraith;
            NPC.scale = 1f;
            Banner = NPC.type;
            BannerItem = ItemType<EtheralRemitterBanner>();
            base.SetDefaults();
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

        public override void PostAI()
        {
            NPC.ai[3]++;

            if (NPC.ai[3] > 240)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    var npcs = Targeting.GetAllNPCsInRange(NPC.Center, effectRadius);

                    for (int i = 0; i < npcs.Count; i++)
                    {
                        NPC healTarget = Main.npc[npcs[i]];

                        if (i != NPC.whoAmI)
                        {
                            int heal = (int)((healTarget.lifeMax - healTarget.life) * 0.3);

                            if (heal == 0 && healTarget.lifeMax != healTarget.life)
                                heal = 1;

                            if (heal > 0)
                            {
                                healTarget.life += heal;
                                if (healTarget.life > healTarget.lifeMax)
                                    healTarget.life = healTarget.lifeMax;
                                healTarget.netUpdate = true;

                                if (Main.netMode == NetmodeID.Server)
                                {
                                    NetMessage.SendData(MessageID.CombatTextInt, -1, -1, null, (int)Color.DarkGreen.PackedValue, healTarget.position.X, healTarget.position.Y, (float)heal, 0, 0, 0);
                                }
                                else
                                {
                                    CombatText.NewText(healTarget.Hitbox, Color.DarkGreen, heal);
                                }
                            }
                        }
                    }
                }

                TerraLeague.DustRing(261, NPC, new Color(0, 255, 0, 0));
                TerraLeague.DustBorderRing(effectRadius, NPC.Center, 267, new Color(0, 255, 0, 0), 2);

                NPC.ai[3] = 0;
            }

            base.PostAI();
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.player.GetModPlayer<PLAYERGLOBAL>().zoneBlackMist && (spawnInfo.player.ZoneBeach || NPC.downedBoss3))
                return SpawnCondition.OverworldNightMonster.Chance * 0.25f;
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

                int count = 0;
                while ((double)count < damage / (double)NPC.lifeMax * 50.0)
                {
                    Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Cloud, 0f, 0f, 50, default, 1.5f);
                    dust.velocity *= 2f;
                    dust.noGravity = true;
                    count++;
                }
            }
            else
            {
                for (int i = 0; i < 20; i++)
                {
                    Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Cloud, 0f, 0f, 50, default, 1.5f);
                    dust.velocity *= 2f;
                    dust.noGravity = true;
                }

                Gore gore = Gore.NewGoreDirect(new Vector2(NPC.position.X, NPC.position.Y - 10f), new Vector2((float)hitDirection, 0f), 99, NPC.scale);
                gore.velocity *= 0.3f;
                gore = Gore.NewGoreDirect(new Vector2(NPC.position.X, NPC.position.Y + (float)(NPC.height / 2) - 15f), new Vector2((float)hitDirection, 0f), 99, NPC.scale);
                gore.velocity *= 0.3f;
                gore = Gore.NewGoreDirect(new Vector2(NPC.position.X, NPC.position.Y + (float)NPC.height - 20f), new Vector2((float)hitDirection, 0f), 99, NPC.scale);
                gore.velocity *= 0.3f;
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemType<DamnedSoul>()));
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                ModContent.GetInstance<BlackMistBiome>().ModBiomeBestiaryInfoElement,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("A rallier of spirits and dead alike, it strengthens its allies to bring upon unending darkness")
            });
        }
    }
}
