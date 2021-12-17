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
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Bestiary;
using TerraLeague.Biomes;
using TerraLeague.Items.Accessories;

namespace TerraLeague.NPCs.VoidNPCs
{
    public class XersaiBrute : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Xer'Sai Brute");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.SolarDrakomire];

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
            NPC.width = 60;
            NPC.height = 40;
            NPC.aiStyle = 3;
            NPC.damage = 24;
            NPC.defense = 10;
            NPC.lifeMax = 40;
            
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath2;
            NPC.value = 100f;
            AIType = NPCID.AnomuraFungus;
            //AnimationType = NPCID.SolarDrakomire;
            NPC.scale = 1f;
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
            int frameHeight = 56;

            if (NPC.ai[1] == 1)
            {
                Terraria.DataStructures.NPCAimedTarget targetData = NPC.GetTargetData(true);
                NPC.spriteDirection = (targetData.Center.X < NPC.Center.X ? -1 : 1);

                if (NPC.frame.Y < frameHeight * 8)
                    NPC.frame.Y = frameHeight * 8;

                NPC.frameCounter++;
                if (NPC.frameCounter > 5)
                {
                    if (NPC.frame.Y + frameHeight >= frameHeight * 10)
                        NPC.frame.Y = frameHeight * 8;
                    else
                        NPC.frame.Y += frameHeight;

                    NPC.frameCounter = 0;
                }
            }
            else
            {
                if (NPC.velocity.X > 0)
                    NPC.spriteDirection = 1;
                else
                    NPC.spriteDirection = -1;

                if (!NPC.collideY)
                {
                    NPC.frame.Y = frameHeight;
                    NPC.frameCounter = frameHeight;
                }
                else if (NPC.velocity.X != 0)
                {
                    if (NPC.frame.Y < frameHeight * 2)
                        NPC.frame.Y = frameHeight * 2;

                    NPC.frameCounter++;
                    if (NPC.frameCounter > 10)
                    {
                        if (NPC.frame.Y + frameHeight >= frameHeight * 8)
                            NPC.frame.Y = frameHeight * 2;
                        else
                            NPC.frame.Y += frameHeight;

                        NPC.frameCounter = 0;
                    }
                }
                else
                {
                    NPC.frame.Y = 0;
                    NPC.frameCounter = 0;
                }
            }
        }

        public override void AI()
        {
            Animation();

            NPC.ai[2]++;
            Terraria.DataStructures.NPCAimedTarget targetData = NPC.GetTargetData(true);
            if (NPC.ai[1] == 0)
            {
                if (NPC.ai[2] >= 180 && NPC.collideY)
                {
                    if (targetData.Type != 0 && Collision.CanHit(NPC, targetData))
                    {
                        NPC.ai[1] = 1;
                    }
                    NPC.ai[2] = 0;
                }
            }
            else if (NPC.ai[1] == 1)
            {
                NPC.ai[0] = 0;
                NPC.velocity.X = 0;
                if (NPC.velocity.Y < 0)
                    NPC.velocity.Y = 0;
                if (NPC.ai[2] == 1)
                {
                    int count = Main.rand.Next(8, 16);
                    for (int i = 0; i < count; i++)
                    {
                        Vector2 pos = NPC.spriteDirection == -1 ? NPC.Left : NPC.Right;
                        Projectile.NewProjectileDirect(NPC.GetProjectileSpawnSource(), pos, new Vector2(Main.rand.NextFloat(5,10), 0).RotatedBy(pos.AngleTo(targetData.Center) + Main.rand.NextFloat(-0.5f, 0.5f)), ProjectileType<Projectiles.XerSaiBrute_Spit>(), 24, 1);
                    }
                }
                else if (NPC.ai[2] >= 60)
                {
                    NPC.ai[1] = 0;
                    NPC.ai[2] = 0;
                    NPC.frameCounter = 0;
                    NPC.frame.Y = 0;
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
            if (NPC.life <= 0)
            {
                //for (int k = 0; k < 20; k++)
                //{
                //    Dust.NewDust(new Vector2(NPC.Center.X, NPC.Center.Y - 15), 1, 1, DustID.BlueCrystalShard, 0, 0, 0, default, 1.2f);
                //}

                Gore.NewGore(NPC.position, NPC.velocity, GoreType<XersaiBrute_1>(), NPC.scale);
                Gore.NewGore(NPC.position, NPC.velocity, GoreType<XersaiBrute_2>(), NPC.scale);
                Gore.NewGore(NPC.position, NPC.velocity, GoreType<XersaiBrute_3>(), NPC.scale);
                Gore.NewGore(NPC.position, NPC.velocity, GoreType<XersaiBrute_4>(), NPC.scale);
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
				new FlavorTextBestiaryInfoElement("A bulky void monster with nasty stomach contents. It is very ready to share it with all living things")
            });
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = null;
            TerraLeague.GetTextureIfNull(ref texture, "TerraLeague/NPCs/VoidNPCs/XersaiBrute_Glow");
            int frameHeight = (texture.Height / Main.npcFrameCount[NPC.type]);
            if (frameHeight <= 0)
                frameHeight = 1;

            Main.spriteBatch.Draw
            (
                texture,
                new Vector2
                (
                    NPC.position.X - Main.screenPosition.X + NPC.width * 0.5f,
                    NPC.position.Y - Main.screenPosition.Y + NPC.height * 0.5f + 20
                ),
                new Rectangle(0, (texture.Height / Main.npcFrameCount[NPC.type]) * (NPC.frame.Y / frameHeight), texture.Width, texture.Height / Main.npcFrameCount[NPC.type]),
                Color.White,
                NPC.rotation,
                new Vector2(texture.Width, texture.Width) * 0.5f,
                NPC.scale,
                NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                0f
            );
            base.PostDraw(spriteBatch, screenPos, drawColor);
        }
    }
}
