using Microsoft.Xna.Framework;
using TerraLeague.Items;
using Terraria;
using Terraria.ID;
using TerraLeague.Gores;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using TerraLeague.Buffs;
using TerraLeague.Projectiles;
using Microsoft.Xna.Framework.Graphics;
using TerraLeague.Dusts;
using Terraria.Audio;
using Terraria.ModLoader.Utilities;

namespace TerraLeague.NPCs
{
    public class VoidPortal : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 14;
            DisplayName.SetDefault("Void Rift");
        }
        public override void SetDefaults()
        {
            NPC.width = 128;
            NPC.height = 128;
            NPC.defense = 0;
            NPC.lifeMax = 10;
            NPC.HitSound = new LegacySoundStyle(3, 5);
            NPC.DeathSound = new LegacySoundStyle(4, 7);
            NPC.value = 0;
            NPC.buffImmune[BuffType<TideCallerBubbled>()] = true;
            NPC.buffImmune[BuffType<Stunned>()] = true;
            NPC.knockBackResist = 0f;
            NPC.SpawnedFromStatue = true;
            NPC.noGravity = true;
            NPC.behindTiles = true;
            base.SetDefaults();
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPC.CountNPCS(NPC.type) != 0)
                return 0;
            float height = spawnInfo.player.position.Y;
            if (spawnInfo.player.ZoneUndergroundDesert )
                return SpawnCondition.DesertCave.Chance * 0.015f;
            else if (height > Main.maxTilesY * 8)
                return Math.Max(SpawnCondition.Cavern.Chance, SpawnCondition.Underground.Chance) * 0.01f;
            return 0;
        }

        public override bool PreAI()
        {
            Lighting.AddLight(NPC.Center, 1,0,1);
            return base.PreAI();
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
            if (NPC.life > 0)
            {
                int count = 0;
                while ((double)count < damage / (double)NPC.lifeMax * 50.0)
                {
                    Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.PortalBolt, 0f, 0f, 0, Color.Purple, 1.5f);
                    dust.noGravity = true;
                    count++;
                    break;
                }
            }
            else
            {
                for (int i = 0; i < 20; i++)
                {
                    Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.PortalBolt, 0f, 0f, 0, Color.Purple, 1.5f);
                    dust.velocity *= 2f;
                    dust.noGravity = true;
                }
            }

            base.HitEffect(hitDirection, damage);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            //Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            //spriteBatch.Draw
            //(
            //    texture,
            //    NPC.Center - Main.screenPosition,
            //    new Rectangle(0, 0, texture.Width, texture.Height),
            //    Color.White,
            //    NPC.rotation,
            //    texture.Size() * 0.5f,
            //    NPC.scale,
            //    SpriteEffects.None,
            //    0f
            //);
        }

        public override void FindFrame(int frameHeight)
        {
            if ((int)NPC.frameCounter > 13)
                NPC.frameCounter = -60;

            if ((int)NPC.frameCounter < 0)
                NPC.frame.Y = (((int)NPC.frameCounter * -1) % 4) * frameHeight;
            else
                NPC.frame.Y = (int)NPC.frameCounter * frameHeight;

            NPC.frameCounter += 0.25d;
        }
        
    }
}
