using Microsoft.Xna.Framework;
using TerraLeague.Items;
using Terraria;
using TerraLeague.Gores;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TerraLeague.Items.Banners;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader.Utilities;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.Audio;
using System;
using Terraria.GameContent.Bestiary;
using TerraLeague.Biomes;
using Microsoft.Xna.Framework.Graphics;
using TerraLeague.Items.Accessories;

namespace TerraLeague.NPCs.VoidNPCs
{
    public class ZzRotFlyer : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zz'Rot Flyer");
            Main.npcFrameCount[NPC.type] = 3;

			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{ // Influences how the NPC looks in the Bestiary
				Velocity = 1f // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction

			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
		}
        public override void SetDefaults()
        {
            NPC.width = 40;
            NPC.height = 40;
            NPC.damage = 24;
            NPC.defense = 10;
            NPC.lifeMax = 40;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath2;
            NPC.value = 100f;
            NPC.scale = 1f;
			NPC.noGravity = true;
			NPC.aiStyle = -1;
            //Banner = NPC.type;
            //BannerItem = ItemType<UnleashedSpiritBanner>();
            base.SetDefaults();
		}

        public override bool PreAI()
        {
            //Lighting.AddLight(NPC.Center, new Color(5, 245, 150).ToVector3());

            return base.PreAI();
        }

		void Animation()
        {
			NPC.frameCounter++;
			if (NPC.frameCounter > 2)
            {
				if (NPC.frame.Y != 88)
					NPC.frame.Y += 44;
				else
					NPC.frame.Y = 0;

				NPC.frameCounter = 0;
			}
        }

        public override void AI()
        {
			Animation();

			if (NPC.target < 0 || NPC.target <= 255 || Main.player[NPC.target].dead)
			{
				NPC.TargetClosest(true);
			}
			
			NPCAimedTarget targetData = NPC.GetTargetData(true);
			bool playerDead = false;
			if (targetData.Type == NPCTargetType.Player)
			{
				playerDead = Main.player[NPC.target].dead;
			}
			float num = 6f;
			float num2 = 0.05f;
			//if (NPC.type == 42 )
			{
				num = 3.5f;
				num2 = 0.021f;

				num *= 1f ;
				num2 *= 1f ;
				//if ((double)(NPC.position.Y / 16f) < Main.worldSurface)
				{
					if (Main.player[NPC.target].position.Y - NPC.position.Y > 300f && NPC.velocity.Y < 0f)
					{
						NPC.velocity.Y *= 0.97f;
					}
					if (Main.player[NPC.target].position.Y - NPC.position.Y < 80f && NPC.velocity.Y > 0f)
					{
						NPC.velocity.Y *= 0.97f;
					}
				}
			}
			
			Vector2 vector = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
			float targetCenterX = targetData.Position.X + (float)(targetData.Width / 2);
			float targetCenterY = targetData.Position.Y + (float)(targetData.Height / 2);
			targetCenterX = (float)((int)(targetCenterX / 8f) * 8);
			targetCenterY = (float)((int)(targetCenterY / 8f) * 8);
			vector.X = (float)((int)(vector.X / 8f) * 8);
			vector.Y = (float)((int)(vector.Y / 8f) * 8);
			targetCenterX -= vector.X;
			targetCenterY -= vector.Y;
			float magnitudToTarget = (float)System.Math.Sqrt((double)(targetCenterX * targetCenterX + targetCenterY * targetCenterY));
			float num7 = magnitudToTarget;
			bool flag2 = false;
			if (magnitudToTarget > 600f)
			{
				flag2 = true;
			}
			if (magnitudToTarget == 0f)
			{
				targetCenterX = NPC.velocity.X;
				targetCenterY = NPC.velocity.Y;
			}
			else
			{
				magnitudToTarget = num / magnitudToTarget;
				targetCenterX *= magnitudToTarget;
				targetCenterY *= magnitudToTarget;
			}
			//bool num8 = NPC.type == 6 || NPC.type == 139 || NPC.type == 173 || NPC.type == 205;
			bool flag3 = true;//NPC.type == 42 || NPC.type == 94 || NPC.type == 619 || NPC.type == 176 || NPC.type == 210 || NPC.type == 211 || (NPC.type >= 231 && NPC.type <= 235);
			//bool flag4 = NPC.type != 173 && NPC.type != 6 && NPC.type != 42 && (NPC.type < 231 || NPC.type > 235) && NPC.type != 94 && NPC.type != 139 && NPC.type != 619;
			if (flag3)
			{
				if (num7 > 100f | flag3)
				{
					NPC.ai[0] += 1f;
					if (NPC.ai[0] > 0f)
					{
						NPC.velocity.Y += 0.023f;
					}
					else
					{
						NPC.velocity.Y -= 0.023f;
					}
					if (NPC.ai[0] < -100f || NPC.ai[0] > 100f)
					{
						NPC.velocity.X += 0.023f;
					}
					else
					{
						NPC.velocity.X -= 0.023f;
					}
					if (NPC.ai[0] > 200f)
					{
						NPC.ai[0] = -200f;
					}
				}
			}
			if (playerDead)
			{
				targetCenterX = (float)NPC.direction * num / 2f;
				targetCenterY = (0f - num) / 2f;
			}
			
			if (NPC.velocity.X < targetCenterX)
			{
				NPC.velocity.X += num2;
			}
			else if (NPC.velocity.X > targetCenterX)
			{
				NPC.velocity.X -= num2;
			}
			if (NPC.velocity.Y < targetCenterY)
			{
				NPC.velocity.Y += num2;
			}
			else if (NPC.velocity.Y > targetCenterY)
			{
				NPC.velocity.Y -= num2;
			}
			
			//else if (NPC.type == 42 || NPC.type == 176 || NPC.type == 205 || (NPC.type >= 231 && NPC.type <= 235))
			{
				if (NPC.velocity.X > 0f)
				{
					NPC.spriteDirection = 1;
				}
				if (NPC.velocity.X < 0f)
				{
					NPC.spriteDirection = -1;
				}
				NPC.rotation = NPC.velocity.X * 0.1f;
			}
			
			//if (NPC.type == 6 || NPC.type == 619 || NPC.type == 23 || NPC.type == 42 || NPC.type == 94 || NPC.type == 139 || NPC.type == 173 || NPC.type == 176 || NPC.type == 205 || NPC.type == 210 || NPC.type == 211 || (NPC.type >= 231 && NPC.type <= 235))
			{
				float num12 = 0.7f;
				
				if (NPC.collideX)
				{
					NPC.netUpdate = true;
					NPC.velocity.X = NPC.oldVelocity.X * (0f - num12);
					if (NPC.direction == -1 && NPC.velocity.X > 0f && NPC.velocity.X < 2f)
					{
						NPC.velocity.X = 2f;
					}
					if (NPC.direction == 1 && NPC.velocity.X < 0f && NPC.velocity.X > -2f)
					{
						NPC.velocity.X = -2f;
					}
				}
				if (NPC.collideY)
				{
					NPC.netUpdate = true;
					NPC.velocity.Y = NPC.oldVelocity.Y * (0f - num12);
					if (NPC.velocity.Y > 0f && (double)NPC.velocity.Y < 1.5)
					{
						NPC.velocity.Y = 2f;
					}
					if (NPC.velocity.Y < 0f && (double)NPC.velocity.Y > -1.5)
					{
						NPC.velocity.Y = -2f;
					}
				}
				//NPC.position += NPC.netOffset;
				
				//NPC.position -= NPC.netOffset;
			}
			
			float num17;
			Vector2 vector2;
			float num18;
			float num19;
			//if (NPC.type == 42 || NPC.type == 176 || (NPC.type >= 231 && NPC.type <= 235))
			{
				if (NPC.wet)
				{
					if (NPC.velocity.Y > 0f)
					{
						NPC.velocity.Y *= 0.95f;
					}
					NPC.velocity.Y -= 0.5f;
					if (NPC.velocity.Y < -4f)
					{
						NPC.velocity.Y = -4f;
					}
					NPC.TargetClosest(true);
				}
				if (NPC.ai[1] == 101f)
				{
					SoundEngine.PlaySound(SoundID.Item17, NPC.position);
					NPC.ai[1] = 0f;
				}
				if (Main.netMode != 1)
				{
					NPC.ai[1] += (float)Main.rand.Next(5, 20) * 0.1f * NPC.scale;
					if (NPC.type == 176)
					{
						NPC.ai[1] += (float)Main.rand.Next(5, 20) * 0.1f * NPC.scale;
					}
					if (targetData.Type == NPCTargetType.Player)
					{
						Player player = Main.player[NPC.target];
						if (player != null && player.stealth == 0f && player.itemAnimation == 0)
						{
							NPC.ai[1] = 0f;
						}
					}
					if (NPC.ai[1] >= 130f)
					{
						if (targetData.Type != 0 && Collision.CanHit(NPC, targetData))
						{
							num17 = 8f;
							vector2 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)(NPC.height / 2));
							num18 = targetData.Center.X - vector2.X + (float)Main.rand.Next(-20, 21);
							num19 = targetData.Center.Y - vector2.Y + (float)Main.rand.Next(-20, 21);
							if ((num18 < 0f && NPC.velocity.X < 0f) || (num18 > 0f && NPC.velocity.X > 0f))
							{
								float num20 = (float)Math.Sqrt((double)(num18 * num18 + num19 * num19));
								num20 = num17 / num20;
								num18 *= num20;
								num19 *= num20;
								int num21 = (int)(10f * NPC.scale);
								if (NPC.type == 176)
								{
									num21 = (int)(30f * NPC.scale);
								}
								int num22 = 55;
								int num23 = Projectile.NewProjectile(NPC.GetProjectileSpawnSource(), vector2.X, vector2.Y, num18, num19, ProjectileType<Projectiles.ZzRotFlyer_Shot>(), 24, 0f, Main.myPlayer, 0f, 0f);
								NPC.ai[1] = 101f;
								NPC.netUpdate = true;
							}

							NPC.ai[1] = 0f;
						}
						else if (NPC.ai[1] >= 240f && NPC.Center.Distance(targetData.Center) < 700)
						{
							num17 = 4f;
							vector2 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)(NPC.height / 2));
							num18 = targetData.Center.X - vector2.X + (float)Main.rand.Next(-20, 21);
							num19 = targetData.Center.Y - vector2.Y + (float)Main.rand.Next(-20, 21);
							if ((num18 < 0f && NPC.velocity.X < 0f) || (num18 > 0f && NPC.velocity.X > 0f))
							{
								float num20 = (float)Math.Sqrt((double)(num18 * num18 + num19 * num19));
								num20 = num17 / num20;
								num18 *= num20;
								num19 *= num20;
								int num21 = (int)(10f * NPC.scale);
								if (NPC.type == 176)
								{
									num21 = (int)(30f * NPC.scale);
								}
								int num22 = 55;
								int num23 = Projectile.NewProjectile(NPC.GetProjectileSpawnSource(), vector2.X, vector2.Y, num18, num19, ProjectileType<Projectiles.ZzRotFlyer_PShot>(), 16, 0f, Main.myPlayer, 0f, 0f);
								NPC.ai[1] = 101f;
								NPC.netUpdate = true;
							}

							NPC.ai[1] = 0f;
						}
					}
				}
			}
			
			if ((!(NPC.velocity.X > 0f) || !(NPC.oldVelocity.X < 0f)) && (!(NPC.velocity.X < 0f) || !(NPC.oldVelocity.X > 0f)) && (!(NPC.velocity.Y > 0f) || !(NPC.oldVelocity.Y < 0f)))
			{
				if (!(NPC.velocity.Y < 0f))
				{
					return;
				}
				if (!(NPC.oldVelocity.Y > 0f))
				{
					return;
				}
			}
			if (!NPC.justHit)
			{
				NPC.netUpdate = true;
			}
			return;
		}

		public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
		{
			target.GetModPlayer<PLAYERGLOBAL>().AddVoidInfluence(100, false);

			base.ModifyHitPlayer(target, ref damage, ref crit);
		}

		public override void HitEffect(int hitDirection, double damage)
        {
			if (NPC.life <= 0)
			{
				//for (int k = 0; k < 20; k++)
				//{
				//    Dust.NewDust(new Vector2(NPC.Center.X, NPC.Center.Y - 15), 1, 1, DustID.BlueCrystalShard, 0, 0, 0, default, 1.2f);
				//}

				Gore.NewGore(NPC.position, NPC.velocity, GoreType<ZzRotFlyer_1>(), NPC.scale);
				Gore.NewGore(NPC.position, NPC.velocity, GoreType<ZzRotFlyer_2>(), NPC.scale);
				Gore.NewGore(NPC.position, NPC.velocity, GoreType<ZzRotFlyer_3>(), NPC.scale);
			}

			base.HitEffect(hitDirection, damage);
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
				new FlavorTextBestiaryInfoElement("A varient of Zz'Rot that have adapted wings and a way of attacking from affar")
			});
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Texture2D texture = null;
			TerraLeague.GetTextureIfNull(ref texture, "TerraLeague/NPCs/VoidNPCs/ZzRotFlyer_Glow");
			int frameHeight = (texture.Height / Main.npcFrameCount[NPC.type]);
			if (frameHeight <= 0)
				frameHeight = 1;

			Main.spriteBatch.Draw
			(
				texture,
				new Vector2
				(
					NPC.position.X - Main.screenPosition.X + NPC.width * 0.5f,
					NPC.position.Y - Main.screenPosition.Y + NPC.height * 0.5f + 4
				),
				new Rectangle(0, (texture.Height / Main.npcFrameCount[NPC.type]) * (NPC.frame.Y / frameHeight), texture.Width, texture.Height / Main.npcFrameCount[NPC.type]),
				Color.White,
				NPC.rotation,
				new Vector2(texture.Width, texture.Width) * 0.5f,
				NPC.scale,
				NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
				0f
			);
		}
	}
}
