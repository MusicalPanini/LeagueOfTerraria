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
    public class TaintedCavebat : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tainted Cave Bat");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.CaveBat];
        }
        public override void SetDefaults()
        {
			NPC.CloneDefaults(NPCID.CaveBat);
            NPC.damage = 24;
            NPC.defense = 10;
            NPC.lifeMax = 40;
            NPC.value = 100f;
            AnimationType = NPCID.CaveBat;
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
			
		}

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            base.OnHitPlayer(target, damage, crit);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            //if (NPC.life > 0)
            //{
            //    int count = 0;
            //    while ((double)count < damage / (double)NPC.lifeMax * 50.0)
            //    {
            //        Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Cloud, 0f, 0f, 0, new Color(5, 245, 150), 1.5f);
            //        dust.velocity *= 2f;
            //        dust.noGravity = true;
            //        count++;
            //    }
            //}
            //else
            //{
            //    for (int i = 0; i < 20; i++)
            //    {
            //        Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Cloud, 0f, 0f, 0, new Color(5, 245, 150), 1.5f);
            //        dust.velocity *= 2f;
            //        dust.noGravity = true;
            //    }

            //    Gore.NewGore(NPC.Top, NPC.velocity / 2, GoreType<UnleashedSpirit_1>(), 1f);

            //    Gore gore = Gore.NewGoreDirect(new Vector2(NPC.position.X, NPC.position.Y - 10f), new Vector2((float)hitDirection, 0f), GoreType<MistPuff_1>(), NPC.scale);
            //    gore.velocity *= 0.3f;
            //    gore = Gore.NewGoreDirect(new Vector2(NPC.position.X, NPC.position.Y + (float)(NPC.height / 2) - 15f), new Vector2((float)hitDirection, 0f), GoreType<MistPuff_2>(), NPC.scale);
            //    gore.velocity *= 0.3f;
            //    gore = Gore.NewGoreDirect(new Vector2(NPC.position.X, NPC.position.Y + (float)NPC.height - 20f), new Vector2((float)hitDirection, 0f), GoreType<MistPuff_3>(), NPC.scale);
            //    gore.velocity *= 0.3f;
            //}

            base.HitEffect(hitDirection, damage);
        }

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            target.GetModPlayer<PLAYERGLOBAL>().AddVoidInfluence(100, false);

            base.ModifyHitPlayer(target, ref damage, ref crit);
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
				new FlavorTextBestiaryInfoElement("Those that wander too close to the void will be corrupted by its visions. The fauna of the Underground is no exception")
			});
		}

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = null;
            TerraLeague.GetTextureIfNull(ref texture, "TerraLeague/NPCs/VoidNPCs/TaintedCavebat_Glow");
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
