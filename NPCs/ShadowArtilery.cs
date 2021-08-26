using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Gores;
using TerraLeague.Items;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.NPCs
{
    public class ShadowArtilery : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shadow Artilery");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Antlion];
        }
        public override void SetDefaults()
        {
            NPC.width = 24;
            NPC.height = 24;
            NPC.aiStyle = -1;
            NPC.damage = 10;
            NPC.defense = 4;
            NPC.lifeMax = 60;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath2;
            NPC.knockBackResist = 0f;
            NPC.behindTiles = true;
            AnimationType = NPCID.Antlion;
            AIType = NPCID.Antlion;
            NPC.value = 100;
            NPC.scale = 1f;
            base.SetDefaults();
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.player.GetModPlayer<PLAYERGLOBAL>().zoneBlackMist && spawnInfo.player.ZoneDesert && Main.hardMode)
                return SpawnCondition.OverworldNightMonster.Chance;
            else if (spawnInfo.player.GetModPlayer<PLAYERGLOBAL>().zoneBlackMist && NPC.downedMechBossAny)
                return SpawnCondition.OverworldNightMonster.Chance * 0.25f;
            return 0;
        }

        public override bool PreAI()
        {
            Lighting.AddLight(NPC.Center, new Color(5, 245, 150).ToVector3());

            return base.PreAI();
        }

        public override void AI()
        {
            {
                NPC.TargetClosest(true);
                float num274 = 12f;
                Vector2 npcCenter = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
                float playerCenterXdist = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - npcCenter.X;
                float playerPositionYdist = Main.player[NPC.target].position.Y - npcCenter.Y;
                float absoDist = (float)Math.Sqrt((double)(playerCenterXdist * playerCenterXdist + playerPositionYdist * playerPositionYdist));
                absoDist = num274 / absoDist;
                playerCenterXdist *= absoDist;
                playerPositionYdist *= absoDist;
                if (NPC.directionY < 0)
                {
                    NPC.rotation = (float)(Math.Atan2((double)playerPositionYdist, (double)playerCenterXdist) + 1.57);
                    if ((double)NPC.rotation < -0.6)
                    {
                        NPC.rotation = -0.6f;
                    }
                    else if ((double)NPC.rotation > 0.6)
                    {
                        NPC.rotation = 0.6f;
                    }
                    if (NPC.velocity.X != 0f)
                    {
                        NPC.velocity.X *= 0.9f;
                        if ((double)NPC.velocity.X > -0.1 || (double)NPC.velocity.X < 0.1)
                        {
                            NPC.netUpdate = true;
                            NPC.velocity.X = 0f;
                        }
                    }
                }
                if (NPC.ai[0] > 0f)
                {
                    if (NPC.ai[0] == 200f)
                    {
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item5, NPC.position);
                    }
                    NPC.ai[0] -= 1f;
                }
                if (Main.netMode != NetmodeID.MultiplayerClient && NPC.ai[0] == 0f)
                {
                    NPC.ai[0] = 200f;
                    for (int i = 0; i < 3; i++)
                    {
                        int damage = 16;
                        int type = ProjectileType<Projectiles.ShadowArtillery_LiquidShadow>();
                        int num280 = Projectile.NewProjectile(NPC.GetProjectileSpawnSource(), npcCenter, new Vector2(0,-16).RotatedBy(NPC.rotation + MathHelper.Pi * (-1 + i) / 12f), type, damage, 0f, Main.myPlayer, 0f, 0f);
                        Main.projectile[num280].ai[0] = 2f;
                        Main.projectile[num280].timeLeft = 300;
                        Main.projectile[num280].friendly = false;
                        NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, num280, 0f, 0f, 0f, 0, 0, 0);
                        NPC.netUpdate = true;
                    }

                }
                try
                {
                    int num281 = (int)NPC.position.X / 16;
                    int num282 = (int)(NPC.position.X + (float)(NPC.width / 2)) / 16;
                    int num283 = (int)(NPC.position.X + (float)NPC.width) / 16;
                    int num284 = (int)(NPC.position.Y + (float)NPC.height) / 16;
                    if (Main.tile[num281, num284] == null)
                    {
                        Tile[,] tile15 = Main.tile;
                        int num285 = num281;
                        int num286 = num284;
                        Tile tile16 = new Tile();
                        tile15[num285, num286] = tile16;
                    }
                    if (Main.tile[num282, num284] == null)
                    {
                        Tile[,] tile17 = Main.tile;
                        int num287 = num281;
                        int num288 = num284;
                        Tile tile18 = new Tile();
                        tile17[num287, num288] = tile18;
                    }
                    if (Main.tile[num283, num284] == null)
                    {
                        Tile[,] tile19 = Main.tile;
                        int num289 = num281;
                        int num290 = num284;
                        Tile tile20 = new Tile();
                        tile19[num289, num290] = tile20;
                    }
                    if (Main.tile[num281, num284].IsActuated && Main.tileSolid[Main.tile[num281, num284].type] ||
                        Main.tile[num282, num284].IsActuated && Main.tileSolid[Main.tile[num282, num284].type] ||
                        Main.tile[num283, num284].IsActuated && Main.tileSolid[Main.tile[num283, num284].type])
                    {
                        NPC.noGravity = true;
                        NPC.noTileCollide = true;
                        NPC.velocity.Y = -0.2f;
                    }
                    else
                    {
                        NPC.noGravity = false;
                        NPC.noTileCollide = false;
                    }
                }
                catch
                {
                }
            }

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
            npcLoot.Add(ItemDropRule.Common(ItemType<DamnedSoul>()));
            npcLoot.Add(ItemDropRule.Common(ItemID.AntlionMandible, 3, 1, 2));
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("TerraLeague/NPCs/ShadowArtilery_Body").Value;

            Main.spriteBatch.Draw(texture, new Vector2(NPC.Center.X - Main.screenPosition.X, NPC.position.Y - Main.screenPosition.Y + (float)NPC.height + 10f), new Rectangle(0, 0, texture.Width, texture.Height), drawColor, (0f - NPC.rotation) * 0.3f, new Vector2((float)(texture.Width / 2), (float)(texture.Height / 2)), 1f, SpriteEffects.None, 0f);

        }
    }
}
