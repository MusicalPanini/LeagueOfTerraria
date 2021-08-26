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
using Terraria.GameContent;

namespace TerraLeague.NPCs.TargonBoss
{
    public class Star_Zoe : ModNPC
    {
        const int State_Charging = 0;
        const int State_Attack = 1;

        int CurrentState
        {
            get
            {
                return (int)NPC.ai[0];
            }

            set
            {
                NPC.ai[0] = value;
            }
        }
        int Charge
        {
            get
            {
                return (int)NPC.ai[1];
            }

            set
            {
                NPC.ai[1] = value;
            }
        }
        float AltScale
        {
            get
            {
                return NPC.localAI[0];
            }
            set
            {
                NPC.localAI[0] = value;
            }
        }
        int AltAlpha
        {
            get
            {
                if (NPC.localAI[1] <= 255 || NPC.localAI[1] >= 0)
                    return (int)NPC.localAI[1];
                else if (NPC.localAI[1] < 0)
                    return 0;
                else
                    return 255;
            }
            set
            {
                NPC.localAI[1] = value;
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shimmer of Twilight");
        }
        public override void SetDefaults()
        {
            NPC.width = 32;
            NPC.height = 32;
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
            NPC.alpha = 255;
            base.SetDefaults();
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0;
        }

        public override bool PreAI()
        {
            if (NPC.CountNPCS(NPCType<TargonBossNPC>()) <= 0)
            {
                NPC.active = false;
            }

            Lighting.AddLight(NPC.Center, TargonBossNPC.ZoeColor.ToVector3() * (AltAlpha / 255f) * (AltScale / 2f));
            return base.PreAI();
        }

        public override void AI()
        {
            if (CurrentState == State_Charging)
            {
                Charge++;
                AltScale = 2 * Charge / 300f;

                if (Charge < 51)
                {
                    AltAlpha += 5;
                }
                if (Charge == 60 * 4)
                {
                    NPC.TargetClosest();
                    NPC.netUpdate = true;
                }
                if (Charge == 60 * 5)
                {
                    CurrentState = State_Attack;
                }
            }
            else if (CurrentState == State_Attack)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(NPC.GetProjectileSpawnSource(), NPC.Center, new Vector2(8, 0).RotatedByRandom((double)MathHelper.TwoPi), ProjectileType<TargonBoss_PaddleStar>(), TargonBossNPC.ZoeDamage, 0);
                }
                NPC.active = false;
                TerraLeague.PlaySoundWithPitch(NPC.Center, 2, 27, 0);

                for (int i = 0; i < 10; i++)
                {
                    Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.PortalBolt, 0, 0, 150, TargonBossNPC.ZoeColor);
                    dust.noGravity = true;
                    dust.velocity *= 2;
                }
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
                    Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.PortalBolt, 0f, 0f, 0, TargonBossNPC.ZoeColor, 1.5f);
                    dust.noGravity = true;
                    count++;
                    break;
                }
            }
            else
            {
                for (int i = 0; i < 20; i++)
                {
                    Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.PortalBolt, 0f, 0f, 0, TargonBossNPC.ZoeColor, 1.5f);
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
                new Color(255, 255, 255, 255/*AltAlpha*/),
                NPC.rotation,
                texture.Size() * 0.5f,
                AltScale,
                SpriteEffects.None,
                0f
            );
        }
    }
}
