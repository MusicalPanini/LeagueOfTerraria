using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Gores;
using TerraLeague.Items;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.NPCs
{
    public class Sentry : ModNPC
    {

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 23;
            DisplayName.SetDefault("Blue Sentry");
        }

        private const int State_Idle = 0;
        private const int State_Chasing = 1;
        private const int State_Charging = 2;

        private const int AI_State_Slot = 0;
        private const int Attack_Timer_Slot = 1;



        public override void SetDefaults()
        {
            NPC.scale = 1.5f;
            NPC.rarity = 2;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.width = 36;
            NPC.height = 40;
            NPC.aiStyle = -1;
            NPC.npcSlots = 5;

            if (NPC.downedPlantBoss)
            {
                NPC.damage = 70;
                NPC.defense = 40;
                NPC.lifeMax = 2000;
            }
            else if (NPC.downedMechBossAny)
            {
                NPC.damage = 48;
                NPC.defense = 22;
                NPC.lifeMax = 500;
            }
            else
            {
                NPC.damage = 35;
                NPC.defense = 16;
                NPC.lifeMax = 250;
            }
            NPC.HitSound = SoundID.NPCHit7;
            NPC.DeathSound = SoundID.NPCDeath44;
            NPC.value = 12000;
            NPC.knockBackResist = 0;
            NPC.buffImmune[BuffID.OnFire] = true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            //if (spawnInfo.spawnTileX < Main.mapMaxX / 3 || spawnInfo.spawnTileX > (Main.mapMaxX * 2) / 3)   
            //    return SpawnCondition.OverworldDaySlime.Chance * 0.03f;
            //else
                return 0;
        }

        public float AI_State
        {
            get
            {
                return NPC.ai[AI_State_Slot];
            }
            set
            {
                NPC.ai[AI_State_Slot] = value;
            }
        }

        public float Attack_Timer
        {
            get
            {
                return NPC.ai[Attack_Timer_Slot];
            }
            set
            {
                NPC.ai[Attack_Timer_Slot] = value;
            }
        }

        public override void AI()
        {
            if (NPC.life < NPC.lifeMax && AI_State == State_Idle)
            {
                NPC.netUpdate = true;
                NPC.noTileCollide = true;
                AI_State = State_Chasing;
            }

            if (AI_State == State_Idle)
            {
                if ((int)(NPC.Center.Y / 16) == FindGround() + 2)
                {
                    NPC.velocity = Vector2.Zero;
                }
                else
                {
                    NPC.velocity = new Vector2(0, 1);
                }
            }

            if (AI_State == State_Chasing)
            {
                NPC.TargetClosest();

                if (NPC.Distance(Main.player[NPC.target].Center) > 450)
                {
                    float spd = NPC.velocity.Length();

                    if (spd == 0)
                        spd = 0.5f;
                    else if (spd > 3.5)
                        spd *= 0.98f;
                    else if (spd < 3.5)
                        spd *= 1.02f;

                    NPC.velocity = TerraLeague.CalcVelocityToPoint(NPC.Center, Main.player[NPC.target].Top, spd);
                }
                else if (NPC.Distance(Main.player[NPC.target].Center) > 200)
                {
                    float spd = NPC.velocity.Length();

                    if (spd == 0)
                        spd = 0.5f;
                    else if (spd > 2)
                        spd *= 0.98f;
                    else if (spd < 2)
                        spd *= 1.02f;

                    NPC.velocity = TerraLeague.CalcVelocityToPoint(NPC.Center, Main.player[NPC.target].Top, spd);
                }
                else
                {
                    float spd = NPC.velocity.Length();

                    if (spd != 0)
                        spd *= 0.98f;

                    NPC.velocity = TerraLeague.CalcVelocityToPoint(NPC.Center, Main.player[NPC.target].Top, spd);
                }

                if (NPC.Distance(Main.player[NPC.target].Center) < 450)
                {
                    Attack_Timer++;

                    if ((Attack_Timer == 60) || (Attack_Timer == 30 && Main.expertMode))
                    {
                        NPC.netUpdate = true;
                        Attack_Timer = 0;
                        AI_State = State_Charging;
                    }

                    if (Attack_Timer > 60)
                    {
                        Attack_Timer = 0;
                    }
                }
            }

            if (AI_State == State_Charging)
            {
                if (Math.Round(NPC.frameCounter, 1) <= 20)
                    Lighting.AddLight(NPC.Center, 0, 0, (float)((NPC.frameCounter - 12) / 10));

                float spd = NPC.velocity.Length();

                if (spd != 0)
                    spd *= 0.98f;

                NPC.velocity = TerraLeague.CalcVelocityToPoint(NPC.Center, Main.player[NPC.target].Top, spd);

                if (Math.Round(NPC.frameCounter,1) == 20)
                {
                    if (Main.hardMode)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            Projectile proj = Projectile.NewProjectileDirect(NPC.GetProjectileSpawnSource(), NPC.Center, TerraLeague.CalcVelocityToPoint(NPC.Center, Main.player[NPC.target].Center, (16 * (i + 1)) / 3), ProjectileID.SapphireBolt, NPC.damage / 4, 2);
                            proj.friendly = false;
                            proj.hostile = true;
                        }
                    }
                    else
                    {
                        Projectile proj = Projectile.NewProjectileDirect(NPC.GetProjectileSpawnSource(), NPC.Center, TerraLeague.CalcVelocityToPoint(NPC.Center, Main.player[NPC.target].Center, 16), ProjectileID.SapphireBolt, NPC.damage / 4, 2);
                        proj.friendly = false;
                        proj.hostile = true;
                    }
                    
                    Terraria.Audio.SoundEngine.PlaySound(new LegacySoundStyle(2, 10), NPC.Center);
                }

                if (NPC.frameCounter > 22.8)
                {
                    NPC.netUpdate = true;
                    AI_State = State_Chasing;
                }

            }

            NPC.rotation = (NPC.velocity.X / 8);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(new Vector2(NPC.Center.X, NPC.Center.Y - 15), 1, 1, DustID.BlueCrystalShard, 0, 0, 0, default, 1.2f);
                }

                Gore.NewGore(NPC.position, NPC.velocity, GoreType<SentryTop>(), 1.5f);
                Gore.NewGore(NPC.position, NPC.velocity, GoreType<SentryLeft>(), 1.5f);
                Gore.NewGore(NPC.position, NPC.velocity, GoreType<SentryRight>(), 1.5f);
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemType<SmallBlueBuffOrb>(), 1));
            base.ModifyNPCLoot(npcLoot);
        }

        private int FindGround()
        {
            for (int i = (int)(NPC.Center.Y/16); i < Main.mapMaxY; i++)
            {
                if (Main.tile[(int)(NPC.Center.X/16), i].IsActive)
                {
                    return i;
                }
            }

            return Main.mapMaxY;
        }

        public override void FindFrame(int frameHeight)
        {
            if (AI_State == State_Charging)
            {
                if ((int)NPC.frameCounter > 22 || (int)NPC.frameCounter < 12)
                    NPC.frameCounter = 12;
            }
            else
            {
                if ((int)NPC.frameCounter > 11 || (int)NPC.frameCounter < 0)
                    NPC.frameCounter = 0;
            }

            NPC.frame.Y = (int)NPC.frameCounter * frameHeight;
            NPC.frameCounter += 0.2d;
        }
    }
}
