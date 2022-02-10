using Microsoft.Xna.Framework;
using System;
using TerraLeague.Items;
using Terraria;
using Terraria.ID;
using TerraLeague.Gores;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TerraLeague.Items.Banners;
using Terraria.ModLoader.Utilities;
using TerraLeague.Items.Accessories;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
using TerraLeague.Biomes;

namespace TerraLeague.NPCs.VoidNPCs
{
    public class XersaiStoneSwimmer : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Xer'Sai Stone Swimmer");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.SandShark];

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            { // Influences how the NPC looks in the Bestiary
                Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
                Position = new Vector2(32, 0),
                PortraitPositionXOverride = 8,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }
        public override void SetDefaults()
        {
			NPC.width = 100;
			NPC.height = 24;
            NPC.damage = 20;
            NPC.defense = 10;
            NPC.lifeMax = 50;
            NPC.aiStyle = -1;
            NPC.behindTiles = true;
            NPC.noGravity = true;
            NPC.HitSound = SoundID.NPCHit54;
            NPC.DeathSound = SoundID.NPCHit52;
            NPC.noTileCollide = true;
            AnimationType = NPCID.SandShark;
            //AIType = NPCID.SandShark;
			NPC.gfxOffY = 20;
            NPC.value = 75;
            NPC.knockBackResist = 0.9f;
            NPC.scale = 1f;
            base.SetDefaults();

            //Banner = NPC.type;
        }

        public override bool PreAI()
        {
			
			return base.PreAI();
        }

        public override void AI()
        {
            if (NPC.direction == 0)
            {
                NPC.TargetClosest(true);
            }
            bool CanSwim = true;
            Point npcCenterTilePoint = NPC.Center.ToTileCoordinates();
            Tile SafeTile = Framing.GetTileSafely(npcCenterTilePoint);
            CanSwim = (SafeTile.HasUnactuatedTile && Main.tileSolid[SafeTile.TileType]);
            CanSwim |= NPC.wet;
            bool TargetAbove = false;
            NPC.TargetClosest(false);
            Vector2 TargetCenter = NPC.targetRect.Center.ToVector2();
            if (Main.player[NPC.target].velocity.Y > -0.1f && !Main.player[NPC.target].dead && NPC.Distance(TargetCenter) > 150f)
            {
                TargetAbove = true;
            }
            if (NPC.localAI[0] == -1f && !CanSwim)
            {
                NPC.localAI[0] = 20f;
            }
            if (NPC.localAI[0] > 0f)
            {
                NPC.localAI[0]--;
            }
            if (CanSwim)
            {
                if (NPC.soundDelay == 0)
                {
                    float soundDelay = NPC.Distance(TargetCenter) / 40f;
                    if (soundDelay < 10f)
                    {
                        soundDelay = 10f;
                    }
                    if (soundDelay > 20f)
                    {
                        soundDelay = 20f;
                    }
                    NPC.soundDelay = (int)soundDelay;
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Roar, NPC.Center, 4);
                }
                float num1408 = NPC.ai[1];
                bool TileBelowIsSwimable = false;
                Point TilePointBelowNPC = (NPC.Center + new Vector2(0f, 24f)).ToTileCoordinates();
                SafeTile = Framing.GetTileSafely(TilePointBelowNPC.X, TilePointBelowNPC.Y - 2);
                if (SafeTile.HasUnactuatedTile && Main.tileSolid[SafeTile.TileType])
                {
                    TileBelowIsSwimable = true;
                }
                NPC.ai[1] = (float)TileBelowIsSwimable.ToInt();
                if (NPC.ai[2] < 30f)
                {
                    NPC.ai[2]++;
                }
                if (TargetAbove)
                {
                    NPC.TargetClosest(true);
                    NPC.velocity.X += (float)NPC.direction * 0.15f;
                    NPC.velocity.Y += (float)NPC.directionY * 0.15f;
                    if (NPC.velocity.X > 5)
                    {
                        NPC.velocity.X = 5;
                    }
                    if (NPC.velocity.X < -5)
                    {
                        NPC.velocity.X = -5;
                    }
                    if (NPC.velocity.Y > 3)
                    {
                        NPC.velocity.Y = 3;
                    }
                    if (NPC.velocity.Y < -3)
                    {
                        NPC.velocity.Y = -3;
                    }
                    Vector2 npcCenter = NPC.Center;
                    Vector2 npcVelocityNorm = NPC.velocity.SafeNormalize(Vector2.Zero);
                    Vector2 npcSize = NPC.Size;
                    Vector2 pointInMovementPath = npcCenter + npcVelocityNorm * npcSize.Length() / 2f + NPC.velocity;
                    Point tilePointInMovementPath = pointInMovementPath.ToTileCoordinates();
                    SafeTile = Framing.GetTileSafely(tilePointInMovementPath);
                    bool TileIsSwimable = SafeTile.HasUnactuatedTile && Main.tileSolid[SafeTile.TileType];
                    if (!TileIsSwimable && NPC.wet)
                    {
                        TileIsSwimable = (SafeTile.LiquidType > 0);
                    }
                    if (!TileIsSwimable && Math.Sign(NPC.velocity.X) == NPC.direction && NPC.Distance(TargetCenter) < 400f && (NPC.ai[2] >= 30f || NPC.ai[2] < 0f))
                    {
                        if (NPC.localAI[0] == 0f)
                        {
                            Terraria.Audio.SoundEngine.PlaySound(SoundID.ZombieMoan, NPC.Center, 542);
                            NPC.localAI[0] = -1f;
                        }
                        NPC.ai[2] = -30f;
                        Vector2 vector242 = NPC.DirectionTo(TargetCenter /*+ new Vector2(0f, -80f)*/);
                        NPC.velocity = vector242 * 12f;
                    }
                }
                else
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
                        NPC.velocity.Y *= -1f;
                        NPC.directionY = Math.Sign(NPC.velocity.Y);
                        NPC.ai[0] = (float)NPC.directionY;
                    }
                    float num1409 = 6f;
                    NPC.velocity.X += (float)NPC.direction * 0.1f;
                    //if (NPC.velocity.X < 0f - num1409 || NPC.velocity.X > num1409)
                    //{
                    //    NPC.velocity.X *= 0.95f;
                    //}
                    if (TileBelowIsSwimable)
                    {
                        NPC.ai[0] = -1f;
                    }
                    else
                    {
                        NPC.ai[0] = 1f;
                    }
                    float num1410 = 0.06f;
                    float num1411 = 0.01f;
                    if (NPC.ai[0] == -1f)
                    {
                        NPC.velocity.Y -= num1411;
                        if (NPC.velocity.Y < 0f - num1410)
                        {
                            NPC.ai[0] = 1f;
                        }
                    }
                    else
                    {
                        NPC.velocity.Y += num1411;
                        if (NPC.velocity.Y > num1410)
                        {
                            NPC.ai[0] = -1f;
                        }
                    }
                    if (NPC.velocity.Y > 0.4f || NPC.velocity.Y < -0.4f)
                    {
                        NPC.velocity.Y *= 0.95f;
                    }
                }
            }
            else
            {
                if (NPC.velocity.Y == 0f)
                {
                    if (TargetAbove)
                    {
                        NPC.TargetClosest(true);
                    }
                    float num1412 = 1f;
                    NPC.velocity.X += (float)NPC.direction * 0.1f;
                    //if (NPC.velocity.X < 0f - num1412 || NPC.velocity.X > num1412)
                    //{
                    //    NPC.velocity.X *= 0.95f;
                    //}
                }
                NPC.velocity.Y += 0.3f;
                if (NPC.velocity.Y > 10f)
                {
                    NPC.velocity.Y = 10f;
                }
                NPC.ai[0] = 1f;
            }
            NPC.rotation = NPC.velocity.Y * (float)NPC.direction * 0.1f;
            if (NPC.rotation < -0.2f)
            {
                NPC.rotation = -0.2f;
            }
            if (NPC.rotation > 0.2f)
            {
                NPC.rotation = 0.2f;
            }
            //NPC.velocity = Collision.TileCollision(NPC.position, NPC.velocity, NPC.width, NPC.height, false, false, 1);
			base.AI();
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            base.OnHitPlayer(target, damage, crit);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                //for (int k = 0; k < 20; k++)
                //{
                //    Dust.NewDust(new Vector2(NPC.Center.X, NPC.Center.Y - 15), 1, 1, DustID.BlueCrystalShard, 0, 0, 0, default, 1.2f);
                //}

                Gore.NewGore(NPC.position, NPC.velocity, GoreType<StoneSwimmer_1>(), NPC.scale);
                Gore.NewGore(NPC.position, NPC.velocity, GoreType<StoneSwimmer_2>(), NPC.scale);
                Gore.NewGore(NPC.position, NPC.velocity, GoreType<StoneSwimmer_3>(), NPC.scale);
                Gore.NewGore(NPC.position, NPC.velocity, GoreType<StoneSwimmer_4>(), NPC.scale);
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemType<VoidbornFlesh>(), 2, 1));
            npcLoot.Add(ItemDropRule.NormalvsExpert(ItemType<VoidStone>(), 300, 150));
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				ModContent.GetInstance<VoidBiome>().ModBiomeBestiaryInfoElement,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("With a smooth but hard exoskeleton, this voidborn made in the image of a shark is able to swim through stone as if it was water using brute force")
            });
        }
    }
}
