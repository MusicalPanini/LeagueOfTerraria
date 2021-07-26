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

namespace TerraLeague.NPCs
{
    public class VoidPortal : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 14;
            DisplayName.SetDefault("Void Rift");
        }
        public override void SetDefaults()
        {
            npc.width = 128;
            npc.height = 128;
            npc.defense = 0;
            npc.lifeMax = 10;
            npc.HitSound = new LegacySoundStyle(3, 5);
            npc.DeathSound = new LegacySoundStyle(4, 7);
            npc.value = 0;
            npc.buffImmune[BuffType<TideCallerBubbled>()] = true;
            npc.buffImmune[BuffType<Stunned>()] = true;
            npc.knockBackResist = 0f;
            npc.SpawnedFromStatue = true;
            npc.noGravity = true;
            base.SetDefaults();
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPC.CountNPCS(npc.type) != 0)
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
            Lighting.AddLight(npc.Center, 1,0,1);
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
            if (npc.life > 0)
            {
                int count = 0;
                while ((double)count < damage / (double)npc.lifeMax * 50.0)
                {
                    Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.PortalBolt, 0f, 0f, 0, Color.Purple, 1.5f);
                    dust.noGravity = true;
                    count++;
                    break;
                }
            }
            else
            {
                for (int i = 0; i < 20; i++)
                {
                    Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.PortalBolt, 0f, 0f, 0, Color.Purple, 1.5f);
                    dust.velocity *= 2f;
                    dust.noGravity = true;
                }
            }

            base.HitEffect(hitDirection, damage);
        }

        public override void NPCLoot()
        {
            base.NPCLoot();
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            //Texture2D texture = Main.npcTexture[npc.type];
            //spriteBatch.Draw
            //(
            //    texture,
            //    npc.Center - Main.screenPosition,
            //    new Rectangle(0, 0, texture.Width, texture.Height),
            //    Color.White,
            //    npc.rotation,
            //    texture.Size() * 0.5f,
            //    npc.scale,
            //    SpriteEffects.None,
            //    0f
            //);
        }

        public override void FindFrame(int frameHeight)
        {
            if ((int)npc.frameCounter > 13)
                npc.frameCounter = -60;

            if ((int)npc.frameCounter < 0)
                npc.frame.Y = (((int)npc.frameCounter * -1) % 4) * frameHeight;
            else
                npc.frame.Y = (int)npc.frameCounter * frameHeight;

            npc.frameCounter += 0.25d;
        }
        
    }
}
