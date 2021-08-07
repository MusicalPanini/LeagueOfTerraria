using Microsoft.Xna.Framework;
using System;
using TerraLeague.Items;
using Terraria;
using Terraria.ID;
using TerraLeague.Gores;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TerraLeague.Items.Banners;

namespace TerraLeague.NPCs
{
    public class XersaiStoneSwimmer : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Xer'Sai Stone Swimmer");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.SandShark];
        }
        public override void SetDefaults()
        {
			npc.width = 100;
			npc.height = 24;
            npc.damage = 20;
            npc.defense = 10;
            npc.lifeMax = 50;
            npc.behindTiles = true;
            npc.noGravity = true;
            npc.HitSound = SoundID.NPCHit54;
            npc.DeathSound = SoundID.NPCHit52;
            animationType = NPCID.SandShark;
            aiType = NPCID.SandShark;
			npc.gfxOffY = 20;
            npc.value = 75;
            npc.knockBackResist = 0.9f;
            npc.scale = 1f;
            base.SetDefaults();
            //banner = npc.type;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
             if (spawnInfo.player.GetModPlayer<PLAYERGLOBAL>().zoneVoidPortal)
                return SpawnCondition.Underground.Chance * 3;
            return 0;
        }

        public override bool PreAI()
        {
			
			return base.PreAI();
        }

        public override void AI()
        {
			if (npc.direction == 0)
			{
				npc.TargetClosest(true);
			}
			bool CanSwim = true;
			Point npcCenterTilePoint = npc.Center.ToTileCoordinates();
			Tile SafeTile = Framing.GetTileSafely(npcCenterTilePoint);
			CanSwim = (SafeTile.nactive() && (SafeTile.type == TileID.Dirt || TileID.Sets.Conversion.Stone[SafeTile.type] || TileID.Sets.Conversion.Sand[SafeTile.type] || TileID.Sets.Conversion.Sandstone[SafeTile.type] || TileID.Sets.Conversion.HardenedSand[SafeTile.type]));
			CanSwim |= npc.wet;
			bool TargetAbove = false;
			npc.TargetClosest(false);
			Vector2 TargetCenter = npc.targetRect.Center.ToVector2();
			if (Main.player[npc.target].velocity.Y > -0.1f && !Main.player[npc.target].dead && npc.Distance(TargetCenter) > 150f)
			{
				TargetAbove = true;
			}
			if (npc.localAI[0] == -1f && !CanSwim)
			{
				npc.localAI[0] = 20f;
			}
			if (npc.localAI[0] > 0f)
			{
				npc.localAI[0]--;
			}
			if (CanSwim)
			{
				if (npc.soundDelay == 0)
				{
					float soundDelay = npc.Distance(TargetCenter) / 40f;
					if (soundDelay < 10f)
					{
						soundDelay = 10f;
					}
					if (soundDelay > 20f)
					{
						soundDelay = 20f;
					}
					npc.soundDelay = (int)soundDelay;
					Main.PlaySound(SoundID.Roar, npc.Center, 4);
				}
				float num1408 = npc.ai[1];
				bool TileBelowIsSwimable = false;
				Point TilePointBelowNPC = (npc.Center + new Vector2(0f, 24f)).ToTileCoordinates();
				SafeTile = Framing.GetTileSafely(TilePointBelowNPC.X, TilePointBelowNPC.Y - 2);
				if (SafeTile.nactive() && (SafeTile.type == TileID.Dirt || TileID.Sets.Conversion.Stone[SafeTile.type] || TileID.Sets.Conversion.Sand[SafeTile.type] || TileID.Sets.Conversion.Sandstone[SafeTile.type] || TileID.Sets.Conversion.HardenedSand[SafeTile.type]))
				{
					TileBelowIsSwimable = true;
				}
				npc.ai[1] = (float)TileBelowIsSwimable.ToInt();
				if (npc.ai[2] < 30f)
				{
					npc.ai[2]++;
				}
				if (TargetAbove)
				{
					npc.TargetClosest(true);
					npc.velocity.X += (float)npc.direction * 0.15f;
					npc.velocity.Y += (float)npc.directionY * 0.15f;
					if (npc.velocity.X > 5f)
					{
						npc.velocity.X = 5f;
					}
					if (npc.velocity.X < -5f)
					{
						npc.velocity.X = -5f;
					}
					if (npc.velocity.Y > 3f)
					{
						npc.velocity.Y = 3f;
					}
					if (npc.velocity.Y < -3f)
					{
						npc.velocity.Y = -3f;
					}
					Vector2 npcCenter = npc.Center;
					Vector2 npcVelocityNorm = npc.velocity.SafeNormalize(Vector2.Zero);
					Vector2 npcSize = npc.Size;
					Vector2 pointInMovementPath = npcCenter + npcVelocityNorm * npcSize.Length() / 2f + npc.velocity;
					Point tilePointInMovementPath = pointInMovementPath.ToTileCoordinates();
					SafeTile = Framing.GetTileSafely(tilePointInMovementPath);
					bool TileIsSwimable = SafeTile.nactive() && (SafeTile.type == TileID.Dirt || TileID.Sets.Conversion.Stone[SafeTile.type]  || TileID.Sets.Conversion.Sand[SafeTile.type] || TileID.Sets.Conversion.Sandstone[SafeTile.type] || TileID.Sets.Conversion.HardenedSand[SafeTile.type]);
                    if (!TileIsSwimable && npc.wet)
					{
						TileIsSwimable = (SafeTile.liquid > 0);
					}
					if (!TileIsSwimable && Math.Sign(npc.velocity.X) == npc.direction && npc.Distance(TargetCenter) < 400f && (npc.ai[2] >= 30f || npc.ai[2] < 0f))
					{
						if (npc.localAI[0] == 0f)
						{
							Main.PlaySound(SoundID.ZombieMoan, npc.Center, 542);
							npc.localAI[0] = -1f;
						}
						npc.ai[2] = -30f;
						Vector2 vector242 = npc.DirectionTo(TargetCenter + new Vector2(0f, -80f));
						npc.velocity = vector242 * 12f;
					}
				}
				else
				{
					if (npc.collideX)
					{
						npc.velocity.X *= -1f;
						npc.direction *= -1;
						npc.netUpdate = true;
					}
					if (npc.collideY)
					{
						npc.netUpdate = true;
						npc.velocity.Y *= -1f;
						npc.directionY = Math.Sign(npc.velocity.Y);
						npc.ai[0] = (float)npc.directionY;
					}
					float num1409 = 6f;
					npc.velocity.X += (float)npc.direction * 0.1f;
					if (npc.velocity.X < 0f - num1409 || npc.velocity.X > num1409)
					{
						npc.velocity.X *= 0.95f;
					}
					if (TileBelowIsSwimable)
					{
						npc.ai[0] = -1f;
					}
					else
					{
						npc.ai[0] = 1f;
					}
					float num1410 = 0.06f;
					float num1411 = 0.01f;
					if (npc.ai[0] == -1f)
					{
						npc.velocity.Y -= num1411;
						if (npc.velocity.Y < 0f - num1410)
						{
							npc.ai[0] = 1f;
						}
					}
					else
					{
						npc.velocity.Y += num1411;
						if (npc.velocity.Y > num1410)
						{
							npc.ai[0] = -1f;
						}
					}
					if (npc.velocity.Y > 0.4f || npc.velocity.Y < -0.4f)
					{
						npc.velocity.Y *= 0.95f;
					}
				}
			}
			else
			{
				if (npc.velocity.Y == 0f)
				{
					if (TargetAbove)
					{
						npc.TargetClosest(true);
					}
					float num1412 = 1f;
					npc.velocity.X += (float)npc.direction * 0.1f;
					if (npc.velocity.X < 0f - num1412 || npc.velocity.X > num1412)
					{
						npc.velocity.X *= 0.95f;
					}
				}
				npc.velocity.Y += 0.3f;
				if (npc.velocity.Y > 10f)
				{
					npc.velocity.Y = 10f;
				}
				npc.ai[0] = 1f;
			}
			npc.rotation = npc.velocity.Y * (float)npc.direction * 0.1f;
			if (npc.rotation < -0.2f)
			{
				npc.rotation = -0.2f;
			}
			if (npc.rotation > 0.2f)
			{
				npc.rotation = 0.2f;
			}
			npc.velocity = Collision.AdvancedTileCollision(TileID.Sets.ForAdvancedCollision.ForSandshark, npc.position, npc.velocity, npc.width, npc.height, false, false, 1);
			base.AI();
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            base.OnHitPlayer(target, damage, crit);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            //if (npc.life > 0)
            //{
            //    int count = 0;
            //    while ((double)count < damage / (double)npc.lifeMax * 50.0)
            //    {
            //        Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Cloud, 0f, 0f, 0, new Color(5, 245, 150), 1.5f);
            //        dust.velocity *= 2f;
            //        dust.noGravity = true;
            //        count++;
            //    }
            //}
            //else
            //{
            //    for (int i = 0; i < 20; i++)
            //    {
            //        Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Cloud, 0f, 0f, 0, new Color(5, 245, 150), 1.5f);
            //        dust.velocity *= 2f;
            //        dust.noGravity = true;
            //    }

            //    Gore gore = Gore.NewGoreDirect(new Vector2(npc.position.X, npc.position.Y - 10f), new Vector2((float)hitDirection, 0f), mod.GetGoreSlot("Gores/MistPuff_1"), npc.scale);
            //    gore.velocity *= 0.3f;
            //    gore = Gore.NewGoreDirect(new Vector2(npc.position.X, npc.position.Y + (float)(npc.height / 2) - 15f), new Vector2((float)hitDirection, 0f), mod.GetGoreSlot("Gores/MistPuff_2"), npc.scale);
            //    gore.velocity *= 0.3f;
            //    gore = Gore.NewGoreDirect(new Vector2(npc.position.X, npc.position.Y + (float)npc.height - 20f), new Vector2((float)hitDirection, 0f), mod.GetGoreSlot("Gores/MistPuff_3"), npc.scale);
            //    gore.velocity *= 0.3f;
            //}
            //base.HitEffect(hitDirection, damage);
        }

        public override void NPCLoot()
        {
            base.NPCLoot();
        }
    }
}
