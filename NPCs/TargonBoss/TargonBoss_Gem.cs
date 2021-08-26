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
using System.Linq;
using Terraria.GameContent;

namespace TerraLeague.NPCs.TargonBoss
{
    public class TargonBoss_Gem : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Protector's Gem");
        }
        public override void SetDefaults()
        {
            NPC.width = 20;
            NPC.height = 42;
            NPC.defense = 0;
            NPC.lifeMax = 10;

            NPC.HitSound = new LegacySoundStyle(3, 5);
            NPC.DeathSound = new LegacySoundStyle(2, 27);
            NPC.value = 0;
            NPC.buffImmune[BuffType<TideCallerBubbled>()] = true;
            NPC.buffImmune[BuffType<Stunned>()] = true;
            NPC.knockBackResist = 0f;
            NPC.SpawnedFromStatue = true;
            NPC.noGravity = true;
            NPC.alpha = 255;
            base.SetDefaults();
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0;
        }

        public override bool PreAI()
        {
            if ((int)NPC.ai[0] == 1)
            {
                NPC.width = 40;
                NPC.height = 84;
                NPC.defense = 20;
                NPC.lifeMax = 50 * (Main.expertMode ? 2 : 1);
                NPC.life = NPC.lifeMax;
                NPC.netUpdate = true;
                NPC.ai[0] = 2;
            }

            if (NPC.CountNPCS(NPCType<TargonBossNPC>()) <= 0)
            {
                NPC.active = false;
            }

            Lighting.AddLight(NPC.Center, TargonBossNPC.TaricColor.ToVector3());


            return base.PreAI();
        }

        public override void AI()
        {
            NPC bossNPC = Main.npc.First(x => x.type == NPCType<TargonBossNPC>());

            if (Main.time % 2 == 0)
            {
                if (NPC.lifeMax > 10)
                    TerraLeague.DustLine(bossNPC.Center + TerraLeague.CalcVelocityToPoint(bossNPC.Center, NPC.Center, 128), NPC.Center, 263, Main.rand.NextFloat(0.08f, 0.2f), 1f, TargonBossNPC.TaricColor);
                else
                    TerraLeague.DustLine(bossNPC.Center + TerraLeague.CalcVelocityToPoint(bossNPC.Center, NPC.Center, 128), NPC.Center, 263, Main.rand.NextFloat(0.08f, 0.1f), 1, TargonBossNPC.TaricColor);
            }
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
                    Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.PortalBolt, 0f, 0f, 0, TargonBossNPC.TaricColor, 1.5f);
                    dust.noGravity = true;
                    count++;
                    break;
                }
            }
            else
            {
                for (int i = 0; i < 20; i++)
                {
                    Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.PortalBolt, 0f, 0f, 0, TargonBossNPC.TaricColor, 1.5f);
                    dust.velocity *= 2f;
                    dust.noGravity = true;
                }
            }

            base.HitEffect(hitDirection, damage);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            Main.spriteBatch.Draw
            (
                texture,
                NPC.Center - Main.screenPosition,
                new Rectangle(0, 0, texture.Width, texture.Height),
                new Color(255, 255, 255, 255),
                NPC.rotation,
                texture.Size() * 0.5f,
                NPC.lifeMax > 10 ? 2 : 1,
                SpriteEffects.None,
                0f
            );
        }
    }
}
