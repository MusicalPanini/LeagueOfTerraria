using Microsoft.Xna.Framework;
using System;
using TerraLeague.Items;
using Terraria;
using Terraria.ID;
using TerraLeague.Gores;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TerraLeague.Items.Banners;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader.Utilities;

namespace TerraLeague.NPCs
{
    public class SpectralBitter : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spectral Bitter");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.SandShark];
        }
        public override void SetDefaults()
        {
            NPC.width = 24;
            NPC.height = 18;
            NPC.alpha = 110;
            NPC.aiStyle = 0;
            NPC.damage = 20;
            NPC.defense = 10;
            NPC.lifeMax = 50;
            NPC.behindTiles = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit54;
            NPC.DeathSound = SoundID.NPCHit52;
            AIType = NPCID.Piranha;
            AnimationType = NPCID.SandShark;
            NPC.value = 75;
            NPC.knockBackResist = 0.9f;
            NPC.scale = 1f;
            base.SetDefaults();
            Banner = NPC.type;
            BannerItem = ItemType<MistBitterBanner>();
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
             if (spawnInfo.player.GetModPlayer<PLAYERGLOBAL>().zoneBlackMist && spawnInfo.player.ZoneJungle)
                return SpawnCondition.OverworldNightMonster.Chance;
            else if(spawnInfo.player.GetModPlayer<PLAYERGLOBAL>().zoneBlackMist && NPC.downedBoss3)
                return SpawnCondition.OverworldNightMonster.Chance * 0.25f;
            return 0;
        }

        public override bool PreAI()
        {
            Lighting.AddLight(NPC.Center, new Color(5, 245, 150).ToVector3());
            for (int i = 0; i < 3; i++)
            {
                Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Cloud, 0f, 0f, 100, new Color(5, 245, 150), 1f);
                dust.noGravity = true;
                dust.velocity *= 0.1f;
                dust.velocity += NPC.velocity * 0.1f;
                dust.position.X -= NPC.velocity.X / 3f * (float)i;
                dust.position.Y -= NPC.velocity.Y / 3f * (float)i;
            }


            return base.PreAI();
        }

        public override void AI()
        {
            if (NPC.direction == 0)
            {
                NPC.TargetClosest(true);
            }

            bool attackTarget = false;
            if (NPC.type != NPCID.Goldfish)
            {
                NPC.TargetClosest(false);
                if (!Main.player[NPC.target].dead)
                {
                    attackTarget = true;
                }
            }
            if (!attackTarget)
            {
                if (NPC.collideX)
                {
                    NPC.velocity.X *= -1f;
                    NPC.direction *= -1;
                    NPC.netUpdate = true;
                }
                if (NPC.collideY)
                {
                    NPC.netUpdate = true;
                    if (NPC.velocity.Y > 0f)
                    {
                        NPC.velocity.Y = Math.Abs(NPC.velocity.Y) * -1f;
                        NPC.directionY = -1;
                        NPC.ai[0] = -1f;
                    }
                    else if (NPC.velocity.Y < 0f)
                    {
                        NPC.velocity.Y = Math.Abs(NPC.velocity.Y);
                        NPC.directionY = 1;
                        NPC.ai[0] = 1f;
                    }
                }
            }
            if (attackTarget)
            {
                NPC.TargetClosest(true);
                NPC.velocity.X += (float)NPC.direction * 0.3f;
                NPC.velocity.Y += (float)NPC.directionY * 0.15f;
                if (NPC.velocity.X > 2.5f)
                {
                    NPC.velocity.X = 2.5f;
                }
                if (NPC.velocity.X < -2.5f)
                {
                    NPC.velocity.X = -2.5f;
                }
                if (NPC.velocity.Y > 2.5f)
                {
                    NPC.velocity.Y = 2.5f;
                }
                if (NPC.velocity.Y < -2.5f)
                {
                    NPC.velocity.Y = -2.5f;
                }
            }
            else
            {
                NPC.velocity.X += (float)NPC.direction * 0.1f;
                if (NPC.velocity.X < -1f || NPC.velocity.X > 1f)
                {
                    NPC.velocity.X *= 0.95f;
                }
                if (NPC.ai[0] == -1f)
                {
                    NPC.velocity.Y -= 0.01f;
                    if ((double)NPC.velocity.Y < -0.3)
                    {
                        NPC.ai[0] = 1f;
                    }
                }
                else
                {
                    NPC.velocity.Y += 0.01f;
                    if ((double)NPC.velocity.Y > 0.3)
                    {
                        NPC.ai[0] = -1f;
                    }
                }

                int num250 = (int)(NPC.position.X + (float)(NPC.width / 2)) / 16;
                int num251 = (int)(NPC.position.Y + (float)(NPC.height / 2)) / 16;
                if (Main.tile[num250, num251 - 1] == null)
                {
                    Tile[,] tile3 = Main.tile;
                    int num252 = num250;
                    int num253 = num251 - 1;
                    Tile tile4 = new Tile();
                    tile3[num252, num253] = tile4;
                }
                if (Main.tile[num250, num251 + 1] == null)
                {
                    Tile[,] tile5 = Main.tile;
                    int num254 = num250;
                    int num255 = num251 + 1;
                    Tile tile6 = new Tile();
                    tile5[num254, num255] = tile6;
                }
                if (Main.tile[num250, num251 + 2] == null)
                {
                    Tile[,] tile7 = Main.tile;
                    int num256 = num250;
                    int num257 = num251 + 2;
                    Tile tile8 = new Tile();
                    tile7[num256, num257] = tile8;
                }
                if (NPC.type != NPCID.Arapaima && ((double)NPC.velocity.Y > 0.4 || (double)NPC.velocity.Y < -0.4))
                {
                    NPC.velocity.Y *= 0.95f;
                }

            }
            NPC.rotation = NPC.direction * NPC.velocity.Y * 0.1f;

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
                    Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Cloud, 0f, 0f, 0, new Color(5, 245, 150), 1.5f);
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

                Gore gore = Gore.NewGoreDirect(new Vector2(NPC.position.X, NPC.position.Y - 10f), new Vector2((float)hitDirection, 0f), GoreType<MistPuff_1>(), NPC.scale);
                gore.velocity *= 0.3f;
                gore = Gore.NewGoreDirect(new Vector2(NPC.position.X, NPC.position.Y + (float)(NPC.height / 2) - 15f), new Vector2((float)hitDirection, 0f), GoreType<MistPuff_2>(), NPC.scale);
                gore.velocity *= 0.3f;
                gore = Gore.NewGoreDirect(new Vector2(NPC.position.X, NPC.position.Y + (float)NPC.height - 20f), new Vector2((float)hitDirection, 0f), GoreType<MistPuff_3>(), NPC.scale);
                gore.velocity *= 0.3f;
            }
            base.HitEffect(hitDirection, damage);
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemType<DamnedSoul>(), 1));
            base.ModifyNPCLoot(npcLoot);
        }
    }
}
