using Microsoft.Xna.Framework;
using System;
using TerraLeague.Biomes;
using TerraLeague.Gores;
using TerraLeague.Items;
using TerraLeague.Items.Accessories;
using TerraLeague.Items.Banners;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.NPCs
{
    public class TheUndying_Necromancer : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Undying Necromancer");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Necromancer];
        }
        public override void SetDefaults()
        {
            NPC.width = 18;
            NPC.height = 40;
            NPC.damage = 10;
            NPC.defense = 6;
            NPC.lifeMax = 60;
            NPC.aiStyle = 0;
            NPC.HitSound = SoundID.NPCHit2;
            NPC.DeathSound = SoundID.NPCDeath2;
            NPC.knockBackResist = 0.55f;
            NPC.npcSlots = 4;
            NPC.value = 100f;
            AnimationType = NPCID.Necromancer;
            NPC.scale = 1f;
            Banner = NPC.type;
            BannerItem = ItemType<UndyingNecromancerBanner>();
            base.SetDefaults();
        }

        public override bool PreAI()
        {
            Lighting.AddLight(NPC.Center, new Color(5, 245, 150).ToVector3());
            return base.PreAI();
        }

        public override void AI()
        {
            NPC.TargetClosest(true);
            NPC.velocity.X *= 0.93f;
            if ((double)NPC.velocity.X > -0.1 && (double)NPC.velocity.X < 0.1)
            {
                NPC.velocity.X = 0f;
            }
            if (NPC.ai[0] == 0f)
            {
                NPC.ai[0] = 500f;
            }
            if (NPC.ai[2] != 0f && NPC.ai[3] != 0f)
            {
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item8, NPC.position);
                for (int i = 0; i < 50; i++)
                {
                    Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Cloud, 0f, 0f, 100, new Color(5, 245, 150), 3);
                    dust.velocity *= 3f;
                    dust.fadeIn = 3;
                    dust.noGravity = true;
                }
                NPC.position.X = NPC.ai[2] * 16f - (float)(NPC.width / 2) + 8f;
                NPC.position.Y = NPC.ai[3] * 16f - (float)NPC.height;
                NPC.velocity.X = 0f;
                NPC.velocity.Y = 0f;
                NPC.ai[2] = 0f;
                NPC.ai[3] = 0f;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item8, NPC.position);
                for (int i = 0; i < 50; i++)
                {
                    Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Cloud, 0f, 0f, 100, new Color(5, 245, 150), 3);
                    dust.velocity *= 3f;
                    dust.fadeIn = 3;
                    dust.noGravity = true;
                }
            }
            NPC.ai[0] += 1f;
            if (NPC.ai[0] == 50f || NPC.ai[0] == 100f || NPC.ai[0] == 150f)
            {
                NPC.ai[1] = 30f;
                NPC.netUpdate = true;
            }

            if (NPC.ai[0] >= 350 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.ai[0] = 1f;
                int playerBlockX = (int)Main.player[NPC.target].position.X / 16;
                int playerBlockY = (int)Main.player[NPC.target].position.Y / 16;
                int npcBlockX = (int)NPC.position.X / 16;
                int npcBlockY = (int)NPC.position.Y / 16;
                int extraDistance = 20;
                int loops = 0;
                bool flag4 = false;
                if (Math.Abs(NPC.position.X - Main.player[NPC.target].position.X) + Math.Abs(NPC.position.Y - Main.player[NPC.target].position.Y) > 2000f)
                {
                    loops = 100;
                    flag4 = true;
                }
                while (!flag4 && loops < 100)
                {
                    loops++;
                    int num89 = Main.rand.Next(playerBlockX - extraDistance, playerBlockX + extraDistance);
                    int num90 = Main.rand.Next(playerBlockY - extraDistance, playerBlockY + extraDistance);
                    int num2;
                    for (int num91 = num90; num91 < playerBlockY + extraDistance; num2 = num91, num91 = num2 + 1)
                    {
                        bool flag5;
                        if ((num91 < playerBlockY - 4 || num91 > playerBlockY + 4 || num89 < playerBlockX - 4 || num89 > playerBlockX + 4) && (num91 < npcBlockY - 1 || num91 > npcBlockY + 1 || num89 < npcBlockX - 1 || num89 > npcBlockX + 1) && !Main.tile[num89, num91].IsActive)
                        {
                            flag5 = true;
                            if (Main.tile[num89, num91 - 1].LiquidType == LiquidID.Lava)
                            {
                                flag5 = false;
                            }

                            if (flag5 && Main.tileSolid[Main.tile[num89, num91].type] && !Collision.SolidTiles(num89 - 1, num89 + 1, num91 - 4, num91 - 1))
                            {
                                NPC.ai[1] = 20f;
                                NPC.ai[2] = (float)num89;
                                NPC.ai[3] = (float)num91;
                                flag4 = true;
                                break;
                            }
                        }
                        
                    }
                }
                NPC.netUpdate = true;
            }
            if (NPC.ai[1] > 0f)
            {
                NPC.ai[1] -= 1f;

                if (NPC.ai[1] == 25f)
                {
                    NPC.netUpdate = true;
                        spawnBoy();


                }
            }

            if (Main.rand.Next(2) == 0)
            {
                Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Electric, 0f, 0f, 0, new Color(5, 245, 150), 1f);
                dust.velocity.X *= 0.5f;
                dust.velocity.Y *= 0.5f;
                dust.fadeIn = 3;
                dust.noGravity = true;
            }


            base.AI();
        }

        void spawnBoy()
        {
            int NpcBlockX = (int)Main.player[NPC.target].position.X / 16;
            int NpcBlockY = (int)Main.player[NPC.target].position.Y / 16;
            int boyBlockX = (int)NPC.position.X / 16;
            int boyBlockY = (int)NPC.position.Y / 16;
            int extraDistance = 20;
            int loops = 0;

            while (loops < 100)
            {
                loops++;
                int randX = Main.rand.Next(NpcBlockX - extraDistance, NpcBlockX + extraDistance);
                int randY = Main.rand.Next(NpcBlockY - extraDistance, NpcBlockY + extraDistance);
                int num2;
                for (int i = randY; i < NpcBlockY + extraDistance; num2 = i, i = num2 + 1)
                {
                    bool flag5;
                    if ((i < NpcBlockY - 4 || i > NpcBlockY + 4 || randX < NpcBlockX - 4 || randX > NpcBlockX + 4) && (i < boyBlockY - 1 || i > boyBlockY + 1 || randX < boyBlockX - 1 || randX > boyBlockX + 1) && !Main.tile[randX, i].IsActive)
                    {
                        flag5 = true;
                        if (Main.tile[randX, i - 1].LiquidType == LiquidID.Lava)
                        {
                            flag5 = false;
                        }

                        if (flag5 && Main.tileSolid[Main.tile[randX, i].type] && !Collision.SolidTiles(randX - 1, randX + 1, i - 4, i - 1))
                        {
                            Vector2 boyPos = new Vector2(((randX * 16f) - (float)(NPC.width / 2) + 8f), (int)(i * 16f));

                            if (Main.netMode != NetmodeID.MultiplayerClient && NPC.CountNPCS(NPCType<Ghoul>()) < 12)
                            {
                                NPC.NewNPC((int)boyPos.X, (int)boyPos.Y, NPCType<Ghoul>());
                            }
                            Terraria.Audio.SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 113), boyPos);
                            boyPos.Y -= 40;
                            

                            return;
                        }
                    }

                }
            }
        }

        public override void PostAI()
        {
            base.PostAI();
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.player.GetModPlayer<PLAYERGLOBAL>().zoneBlackMist && (spawnInfo.player.ZoneBeach || NPC.downedBoss3))
                return SpawnCondition.OverworldNightMonster.Chance * 0.25f;
            return 0;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            NPC.ai[0] = 200;

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {

            base.OnHitPlayer(target, damage, crit);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            for (int i = 0; i < 20; i++)
            {
                int num620;
                if (i > 10)
                    num620 = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Cloud, 0f, 0f, 50, Color.DarkGray, 1.5f);
                else
                    num620 = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Cloud, 0f, 0f, 50, new Color(5, 245, 150), 1.5f);
                Dust dust = Main.dust[num620];
                dust.velocity *= 2f;
                Main.dust[num620].noGravity = true;
            }
            int num621 = Gore.NewGore(new Vector2(NPC.position.X, NPC.position.Y - 10f), new Vector2((float)hitDirection, 0f), 99, NPC.scale);
            Gore gore = Main.gore[num621];
            gore.velocity *= 0.3f;
            num621 = Gore.NewGore(new Vector2(NPC.position.X, NPC.position.Y + (float)(NPC.height / 2) - 15f), new Vector2((float)hitDirection, 0f), GoreType<MistPuff_1>(), NPC.scale);
            gore = Main.gore[num621];
            gore.velocity *= 0.3f;
            num621 = Gore.NewGore(new Vector2(NPC.position.X, NPC.position.Y + (float)NPC.height - 20f), new Vector2((float)hitDirection, 0f), 99, NPC.scale);
            gore = Main.gore[num621];
            gore.velocity *= 0.3f;
            gore.velocity *= 0.3f;
            base.HitEffect(hitDirection, damage);
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemType<DamnedSoul>()));

            npcLoot.Add(ItemDropRule.NormalvsExpert(ItemType<Nightbloom>(), 250, 125));

            npcLoot.Add(ItemDropRule.NormalvsExpert(ItemType<PossessedSkull>(), 100, 50));
            base.ModifyNPCLoot(npcLoot);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                ModContent.GetInstance<BlackMistBiome>().ModBiomeBestiaryInfoElement,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("In another life, they were a powerful mage. Now, their magic proficiency has been corrupted and turned by the Black Mist.")
            });
        }
    }
}
