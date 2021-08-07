﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using TerraLeague.Buffs;
using TerraLeague.Items;
using TerraLeague.Items.BossBags;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.NPCs.TargonBoss
{
    [AutoloadBossHead]
    public class TargonBossNPC : ModNPC
    {
        public int StateTimer { get { if (npc.ai[0] < 0) { return 0; } else { return (int)npc.ai[0]; } } set { npc.ai[0] = value; } }
        public int State { get { return (int)npc.ai[1]; } set { npc.ai[1] = value; } }
        public float FloatyTimer { get { return npc.ai[2]; } set { npc.ai[2] = value; } }
        public int StarSpawnTimer { get { return (int)npc.ai[3]; } set { npc.ai[3] = value; } }
        public float RotationSpeed
        {
            get
            {
                if (State == State_FrenzyAnimation)
                {
                    return StateTimer / 60f;
                }
                else if (State == State_Frenzy)
                {
                    return 5;
                }
                else
                {
                    return 1;
                }
            }
        }

        public bool IsIdle { 
            get 
            { 
                return (
                    State == State_Idle ||
                    State == State_PanthIdle ||
                    State == State_MorgIdle ||
                    State == State_KayleIdle || 
                    State == State_LeonaIdle || 
                    State == State_DianaIdle ||
                    State == State_TaricIdle || 
                    State == State_ZoeIdle);
            }
        }
        public bool IsStars
        {
            get
            {
                return (
                    State == State_PanthStars ||
                    State == State_MorgStars ||
                    State == State_KayleStars ||
                    State == State_LeonaStars ||
                    State == State_DianaStars ||
                    State == State_TaricStars ||
                    State == State_ZoeStars);
            }
        }
        public bool IsAttacking
        {
            get
            {
                return (
                    State == State_PanthAttacking ||
                    State == State_MorgAttacking ||
                    State == State_KayleAttacking ||
                    State == State_LeonaAttacking ||
                    State == State_DianaAttacking ||
                    State == State_TaricAttacking ||
                    State == State_ZoeAttacking);
            }
        }

        public bool IsInAFrenzy
        {
            get { return npc.life / (float)npc.lifeMax < 0.33f; }
        }

        public static Color PanthColor { get { return new Color(255, 0, 0); } }
        public static Color MorgColor { get { return new Color(132, 0, 193); } }
        public static Color KayleColor { get { return new Color(255, 228, 96); } }
        public static Color LeonaColor { get { return new Color(255, 109, 0); } }
        public static Color DianaColor { get { return new Color(255, 255, 255); } }
        public static Color TaricColor { get { return new Color(0, 42, 255); } }
        public static Color ZoeColor { get { return new Color(246, 0, 255); } }

        const int State_Idle = 0;
        int State_FrenzyAnimation = 100;
        int State_Frenzy = 200;

        const int State_PanthAttacking = 10;
        const int State_PanthStars = 11;
        const int State_PanthIdle = 12;

        const int State_MorgAttacking = 20;
        const int State_MorgStars = 21;
        const int State_MorgIdle = 22;

        const int State_KayleAttacking = 30;
        const int State_KayleStars = 31;
        const int State_KayleIdle = 32;

        const int State_LeonaAttacking = 40;
        const int State_LeonaStars = 41;
        const int State_LeonaIdle = 42;

        const int State_DianaAttacking = 50;
        const int State_DianaStars = 51;
        const int State_DianaIdle = 52;

        const int State_TaricAttacking = 60;
        const int State_TaricStars = 61;
        const int State_TaricIdle = 62;

        const int State_ZoeAttacking = 70;
        const int State_ZoeStars = 71;
        const int State_ZoeIdle = 72;

        readonly int Timer_DrawStars = 80;
        readonly int Timer_PanthAttack = 120;
        readonly int Timer_PanthAttackInterval = 20;
        readonly int Timer_MorgAttack = 240;
        readonly int Timer_MorgAttackInterval = 60;
        readonly int Timer_KayleAttack = 60;
        readonly int Timer_KayleAttackInterval = 20;
        readonly int Timer_LeonaAttack = 270;
        readonly int Timer_DianaAttack = 150;
        readonly int Timer_TaricAttack = 60;
        readonly int Timer_ZoeAttack = 30;

        // Projectile deal 2x damage (4x in expert)
        public static int PanthDamage = 22 / 2;
        public static int MorgDamage = 25 / 2;
        public static int LeonaDamage = 24 / 2;
        public static int DianaDamage = 50 / 2;
        public static int ZoeDamage = 25 / 2;
        readonly int starGoreLifeExt = 30;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Celestial Gate Keeper");
            Main.npcFrameCount[npc.type] = 14;
        }
        public override void SetDefaults()
        {
            npc.width = 128;
            npc.height = 128;
            npc.damage = 20;
            npc.defense = 10;
            npc.lifeMax = 4400;
            npc.HitSound = new Terraria.Audio.LegacySoundStyle(3, 5);
            npc.DeathSound = new Terraria.Audio.LegacySoundStyle(4, 7);
            npc.scale = 1f;
            npc.noGravity = true;
            npc.boss = true;
            npc.knockBackResist = 0;
            npc.netAlways = true;
            bossBag = ItemType<TargonBossBag>();
            base.SetDefaults();
        }

        public override bool PreAI()
        {
            if (IsInAFrenzy && (State != State_FrenzyAnimation && State != State_Frenzy))
            {
                for (int i = 0; i < 20; i++)
                {
                    Dust dust = Dust.NewDustDirect(npc.Center + new Vector2(-32, -32), 4, 4, DustID.PortalBolt, -3, -3, 0, GetAttackColor, 1);
                }
                State = State_FrenzyAnimation;
                StateTimer = -60;
                npc.frame.Y += 7 * 130;
                TerraLeague.PlaySoundWithPitch(npc.Center, 13, 1, 0.5f);
            }
            else
            {
                GemInvincibilityCheck();
                if (State == State_Idle)
                    UpdateColor();
            }

            CheckForNearbyPlayers();
            Animations();

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                TerraLeagueWORLDGLOBAL.targonBossActive = true;
            }
            return base.PreAI();
        }

        public override void AI()
        {
            npc.spriteDirection = 1;

            if (State == State_Idle)
            {
                if (StateTimer == 0)
                {
                    TerraLeague.DustElipce(128, 128, 0, npc.Center, DustID.PortalBolt, DianaColor, 1, 180, true, -0.1f);
                }

                StateTimer++;
                if (StateTimer >= 120)
                    SetStarTimer();
            }
            else if (State == State_Frenzy)
            {
                UpdateColor();
                PlaceAttackStars();
            }
            else if (State == State_FrenzyAnimation)
            {
                UpdateColor();
                StateTimer++;
                if (StateTimer >= 300)
                {
                    State = State_Frenzy;
                }
            }
            else
            {
                if (StateTimer > 0)
                    StateTimer--;

                PlaceAttackStars();

                WhileIdle();
                WhileStars();
                WhileAttacking();
            }

            base.AI();
        }

        void CheckForNearbyPlayers()
        {
            if (!Main.player[npc.target].HasBuff(BuffType<InTargonArena>()))
            {
                npc.localAI[0]++;
                if (npc.localAI[0] > 10)
                    npc.active = false;
            }
            else
            {
                npc.localAI[0] = 0;
            }
        }

        void SetIdleTimer()
        {
            switch (State)
            {
                case State_PanthStars:
                case State_PanthAttacking:
                case State_PanthIdle:
                    State = State_PanthIdle;
                    break;
                case State_MorgStars:
                case State_MorgAttacking:
                case State_MorgIdle:
                    State = State_MorgIdle;
                    break;
                case State_KayleStars:
                case State_KayleAttacking:
                case State_KayleIdle:
                    State = State_KayleIdle;
                    break;
                case State_LeonaStars:
                case State_LeonaAttacking:
                case State_LeonaIdle:
                    State = State_LeonaIdle;
                    break;
                case State_DianaStars:
                case State_DianaAttacking:
                case State_DianaIdle:
                    State = State_DianaIdle;
                    break;
                case State_TaricStars:
                case State_TaricAttacking:
                case State_TaricIdle:
                    State = State_TaricIdle;
                    break;
                case State_ZoeStars:
                case State_ZoeAttacking:
                case State_ZoeIdle:
                    State = State_ZoeIdle;
                    break;
                default:
                    break;
            }


            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                float healthScale = ((Main.expertMode ? 1.25f : 1.5f) - (1 - (npc.life / (float)npc.lifeMax)));
                StateTimer = (int)(Main.rand.Next(90, 150) * healthScale);
                npc.netUpdate = true;
            }
        }
        void WhileIdle()
        {
            if (IsIdle)
            {
                if (StateTimer <= 0)
                {
                    SetStarTimer();
                }
            }
        }

        void SetStarTimer()
        {
            StateTimer = Timer_DrawStars;
            switch (Main.rand.Next(1, 8))
            {
                case 1:
                    State = State_PanthStars;
                    break;
                case 2:
                    State = State_MorgStars;
                    break;
                case 3:
                    State = State_KayleStars;
                    break;
                case 4:
                    State = State_LeonaStars;
                    break;
                case 5:
                    State = State_DianaStars;
                    break;
                case 6:
                    State = State_TaricStars;
                    break;
                case 7:
                    State = State_ZoeStars;
                    break;
                default:
                    break;
            }
        }
        void WhileStars()
        {
            if (IsStars)
            {
                switch (State)
                {
                    case State_PanthStars:
                        DrawPanthStars();
                        break;
                    case State_MorgStars:
                        DrawMorgStars();
                        break;
                    case State_KayleStars:
                        DrawKayleStars();
                        break;
                    case State_LeonaStars:
                        DrawLeonaStars();
                        break;
                    case State_DianaStars:
                        DrawDianaStars();
                        break;
                    case State_TaricStars:
                        DrawTaricStars();
                        break;
                    case State_ZoeStars:
                        DrawZoeStars();
                        break;
                    default:
                        break;
                }

                if (StateTimer == 0)
                {
                    SetAttackingTimer();
                }
            }
        }

        void SetAttackingTimer()
        {
            switch (State)
            {
                case State_PanthStars:
                case State_PanthAttacking:
                case State_PanthIdle:
                    StateTimer = Timer_PanthAttack;
                    State = State_PanthAttacking;
                    break;
                case State_MorgStars:
                case State_MorgAttacking:
                case State_MorgIdle:
                    StateTimer = Timer_MorgAttack;
                    State = State_MorgAttacking;
                    break;
                case State_KayleStars:
                case State_KayleAttacking:
                case State_KayleIdle:
                    StateTimer = Timer_KayleAttack;
                    State = State_KayleAttacking;
                    break;
                case State_LeonaStars:
                case State_LeonaAttacking:
                case State_LeonaIdle:
                    StateTimer = Timer_LeonaAttack;
                    State = State_LeonaAttacking;
                    break;
                case State_DianaStars:
                case State_DianaAttacking:
                case State_DianaIdle:
                    StateTimer = Timer_DianaAttack;
                    State = State_DianaAttacking;
                    break;
                case State_TaricStars:
                case State_TaricAttacking:
                case State_TaricIdle:
                    StateTimer = Timer_TaricAttack;
                    State = State_TaricAttacking;
                    break;
                case State_ZoeStars:
                case State_ZoeAttacking:
                case State_ZoeIdle:
                    StateTimer = Timer_ZoeAttack;
                    State = State_ZoeAttacking;
                    break;
                default:
                    break;
            }
        }
        void WhileAttacking()
        {
            if (IsAttacking)
            {
                switch (State)
                {
                    case State_PanthAttacking:
                        PanthAttack();
                        break;
                    case State_MorgAttacking:
                        MorgAttack();
                        break;
                    case State_KayleAttacking:
                        KayleAttack();
                        break;
                    case State_LeonaAttacking:
                        LeonaAttack();
                        break;
                    case State_DianaAttacking:
                        DianaAttack();
                        break;
                    case State_TaricAttacking:
                        TaricAttack();
                        break;
                    case State_ZoeAttacking:
                        ZoeAttack();
                        break;
                    default:
                        break;
                }

                if (StateTimer == 0)
                {
                    SetIdleTimer();
                }
            }
        }


        void GemInvincibilityCheck()
        {
            if (NPC.CountNPCS(NPCType<TargonBoss_Gem>()) > 0)
            {
                if (Main.time % 2 == 0)
                    TerraLeague.DustBorderRing(128, npc.Center, 263, TaricColor, 2);
                npc.dontTakeDamage = true;
            }
            else
            {
                npc.dontTakeDamage = false;
            }
        }

        void PlaceAttackStars()
        {
            if (StarSpawnTimer <= 0)
            {
                int npcType = -1;
                float starMultiplier = 1;

                if (State == State_Frenzy)
                {
                    int star = Main.rand.Next(100);

                    if (star < 16)
                        npcType = NPCType<Star_Panth>();
                    else if (star < 16 + 17)
                        npcType = NPCType<Star_Morg>();
                    else if (star < 16 + 16 + 17)
                        npcType = NPCType<Star_Kayle>();
                    else if (star < 16 + 16 + 17 + 17)
                        npcType = NPCType<Star_Leona>();
                    else if (star < 16 + 16 + 16 + 17 + 17)
                        npcType = NPCType<Star_Diana>();
                    else if (star < 16 + 16 + 16 + 17 + 17 + 17)
                        npcType = NPCType<Star_Zoe>();
                    else if (star < 100)
                        npcType = NPCType<Star_Taric>();
                    else
                        npcType = -1;
                }
                else
                {
                    switch (State)
                    {
                        case State_PanthStars:
                        case State_PanthAttacking:
                        case State_PanthIdle:
                            npcType = NPCType<Star_Panth>();
                            starMultiplier = 0.8f;
                            break;
                        case State_MorgStars:
                        case State_MorgAttacking:
                        case State_MorgIdle:
                            npcType = NPCType<Star_Morg>();
                            starMultiplier = 1.25f;
                            break;
                        case State_KayleStars:
                        case State_KayleAttacking:
                        case State_KayleIdle:
                            npcType = NPCType<Star_Kayle>();
                            starMultiplier = 1.5f;
                            break;
                        case State_LeonaStars:
                        case State_LeonaAttacking:
                        case State_LeonaIdle:
                            npcType = NPCType<Star_Leona>();
                            starMultiplier = 1.25f;
                            break;
                        case State_DianaStars:
                        case State_DianaAttacking:
                        case State_DianaIdle:
                            npcType = NPCType<Star_Diana>();
                            starMultiplier = 1f;
                            break;
                        case State_TaricStars:
                        case State_TaricAttacking:
                        case State_TaricIdle:
                            npcType = NPCType<Star_Taric>();
                            starMultiplier = 2.5f;
                            break;
                        case State_ZoeStars:
                        case State_ZoeAttacking:
                        case State_ZoeIdle:
                            npcType = NPCType<Star_Zoe>();
                            starMultiplier = 1;
                            break;
                        default:
                            npcType = -1;
                            starMultiplier = 1f;
                            break;
                    }
                }

                if (npcType != -1)
                {
                    int count = 1;

                    if (Main.netMode == NetmodeID.Server)
                    {
                        int numOfPlayers = 0;

                        for (int i = 0; i < 255; i++)
                        {
                            if (Main.player[i].active)
                            {
                                if (Main.player[i].HasBuff(BuffType<InTargonArena>()))
                                {
                                    numOfPlayers++;
                                }
                            }
                        }
                        count = Main.expertMode ? numOfPlayers : 1;
                    }

                    for (int i = 0; i < count; i++)
                    {
                        int X = Main.rand.NextBool() ? Main.rand.Next((int)npc.Center.X - 500, (int)npc.Center.X - 172) : Main.rand.Next((int)npc.Center.X + 172, (int)npc.Center.X + 500);
                        int Y = Main.rand.NextBool() ? Main.rand.Next((int)npc.Center.Y - 500, (int)npc.Center.Y - 172) : Main.rand.Next((int)npc.Center.Y + 172, (int)npc.Center.Y + 500);

                        NPC.NewNPC(X, Y, npcType);
                    }

                    if (State == State_Frenzy)
                    {
                        StarSpawnTimer = 20;
                    }
                    else
                    {
                        if (Main.expertMode)
                            StarSpawnTimer = (int)(30 + (int)((npc.life / (float)npc.lifeMax) * 90) * starMultiplier);
                        else
                            StarSpawnTimer = (int)(60 + (int)((npc.life / (float)npc.lifeMax) * 90) * starMultiplier);
                    }
                }
            }

            if (StarSpawnTimer > 0)
                StarSpawnTimer--;
        }

        void Animations()
        {
            if (TerraLeagueWORLDGLOBAL.TargonCenterX != 0)
            {
                FloatyTimer += (int)(1 * RotationSpeed);
                if (FloatyTimer > 360)
                {
                    FloatyTimer -= 360;
                    npc.netUpdate = true;
                }
                npc.Center = new Vector2(TerraLeagueWORLDGLOBAL.TargonCenterX * 16, (float)(Main.worldSurface + 50) * 16);
                npc.position.Y += 16 * (float)System.Math.Sin(MathHelper.ToRadians(FloatyTimer));
            }

            Lighting.AddLight(npc.Center, GetAttackColor.ToVector3());
        }

        void UpdateColor()
        {
            int frame = 4;

            if (State == State_Frenzy || State == State_FrenzyAnimation)
            {
                if (FloatyTimer % 45 >= System.Math.Truncate(RotationSpeed))
                    return;

                npc.frame.Y += 130;

                if (npc.frame.Y > 13 * 130)
                    npc.frame.Y = 130 * 7;

                TerraLeague.PlaySoundWithPitch(npc.Center, 2, 4, -1 + (RotationSpeed / 5f));
            }
            else
            {
                switch (State)
                {
                    case State_PanthStars:
                    case State_PanthAttacking:
                    case State_PanthIdle:
                        frame = 0;
                        break;
                    case State_MorgStars:
                    case State_MorgAttacking:
                    case State_MorgIdle:
                        frame = 1;
                        break;
                    case State_KayleStars:
                    case State_KayleAttacking:
                    case State_KayleIdle:
                        frame = 2;
                        break;
                    case State_LeonaStars:
                    case State_LeonaAttacking:
                    case State_LeonaIdle:
                        frame = 3;
                        break;
                    case State_DianaStars:
                    case State_DianaAttacking:
                    case State_DianaIdle:
                        frame = 4;
                        break;
                    case State_TaricStars:
                    case State_TaricAttacking:
                    case State_TaricIdle:
                        frame = 5;
                        break;
                    case State_ZoeStars:
                    case State_ZoeAttacking:
                    case State_ZoeIdle:
                        frame = 6;
                        break;
                }

                if (State != 0)
                    npc.frame.Y = (int)(frame) * 130;
                else
                    npc.frame.Y = 4 * 130;
            }
        }

        #region Attacks
        void PanthAttack()
        {
            if (StateTimer % Timer_PanthAttackInterval == 0)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectileDirect(npc.Center, TerraLeague.CalcVelocityToPoint(npc.Center, Main.player[npc.target].MountedCenter, 16), ProjectileType<TargonBoss_Spear>(), PanthDamage, 2);
                }
                TerraLeague.PlaySoundWithPitch(npc.Center, 2, 1, -0.5f);
            }
        }

        void MorgAttack()
        {
            if (StateTimer == Timer_MorgAttack)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        Vector2 vel = new Vector2(16, 0).RotatedBy(Main.rand.NextFloat(-MathHelper.Pi, MathHelper.Pi));

                        Projectile proj = Projectile.NewProjectileDirect(npc.Center, vel, ProjectileType<TargonBoss_SoulShackles>(), MorgDamage, 0, 255, npc.whoAmI, -1);
                        proj.ai[0] = npc.whoAmI;
                        proj.ai[1] = -1;
                    }
                    TerraLeague.PlaySoundWithPitch(npc.Center, 2, 1, -0.5f);
                }

                //for (int i = 0; i < Main.player.Length; i++)
                //{
                //    if (Main.player[i].active && !Main.player[i].dead)
                //    {
                //        if (Main.player[i].HasBuff(BuffType<Buffs.InTargonArena>()))
                //        {
                //            if (Main.netMode != NetmodeID.MultiplayerClient)
                //            {
                //                Vector2 vel = TerraLeague.CalcVelocityToPoint(npc.Center, Main.player[i].MountedCenter, 24);

                //                Projectile proj = Projectile.NewProjectileDirect(npc.Center, vel, ProjectileType<TargonBoss_SoulShackles>(), MorgDamage, 0, 255, npc.whoAmI, -1);
                //                proj.ai[0] = npc.whoAmI;
                //                proj.ai[1] = -1;
                //            }
                //            TerraLeague.PlaySoundWithPitch(npc.Center, 2, 1, -0.5f);
                //        }
                //    }
                //}
            }
        }

        void KayleAttack()
        {
            if (StateTimer % Timer_KayleAttackInterval == 0)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 pos = new Vector2((int)npc.position.X + Main.rand.Next(-40, 40), (int)npc.position.Y + Main.rand.Next(-40, 40));
                    NPC.NewNPC((int)pos.X, (int)pos.Y, NPCType<KayleAttack>());
                }
                TerraLeague.PlaySoundWithPitch(npc.Center, 2, 113, 0);
            }
        }

        void LeonaAttack()
        {
            if (StateTimer == Timer_LeonaAttack)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectileDirect(npc.Center, new Vector2(0, 1), ProjectileType<TargonBoss_LargeSolarBeam>(), LeonaDamage, 0);
                    Projectile.NewProjectileDirect(npc.Center, new Vector2(0, -1), ProjectileType<TargonBoss_LargeSolarBeam>(), LeonaDamage, 0);
                    Projectile.NewProjectileDirect(npc.Center, new Vector2(1, 0), ProjectileType<TargonBoss_LargeSolarBeam>(), LeonaDamage, 0);
                    Projectile.NewProjectileDirect(npc.Center, new Vector2(-1, 0), ProjectileType<TargonBoss_LargeSolarBeam>(), LeonaDamage, 0);
                }
                TerraLeague.PlaySoundWithPitch(npc.Center, 2, 34, 0);
            }
        }

        void DianaAttack()
        {
            if (StateTimer == Timer_DianaAttack)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectileDirect(npc.Center, Vector2.Zero, ProjectileType<TargonBoss_Moonfall>(), DianaDamage, 2);
                }
            }
        }

        void TaricAttack()
        {
            if (StateTimer == Timer_TaricAttack)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 pos = npc.Center + new Vector2(300, 0).RotatedBy(Main.rand.NextFloat(-MathHelper.Pi, MathHelper.Pi));

                    NPC.NewNPC((int)pos.X, (int)pos.Y, NPCType<TargonBoss_Gem>(), 0, 1);
                }
                TerraLeague.PlaySoundWithPitch(npc.Center, 2, 4, -0.5f);
            }
        }

        void ZoeAttack()
        {
            if (StateTimer == Timer_ZoeAttack)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        int speed = Main.rand.Next(6, 10);
                        Projectile.NewProjectileDirect(npc.Center, new Vector2(speed, 0).RotatedBy((0.5f + i) * MathHelper.TwoPi / 8f), ProjectileType<TargonBoss_PaddleStar>(), ZoeDamage, 0);
                    }
                }
                TerraLeague.PlaySoundWithPitch(npc.Center, 2, 9, 0f);
            }

        }
        #endregion

        #region Stars
        void DrawPanthStars()
        {
            int star1Time = Timer_DrawStars - 5;
            int star2Time = Timer_DrawStars - 17;
            int star3Time = Timer_DrawStars - 29;
            int star4Time = Timer_DrawStars - 41;
            int star5Time = Timer_DrawStars - 53;
            int lineTime = Timer_DrawStars - 65;

            Vector2 star1Pos = new Vector2(0, -128) + npc.Center;
            Vector2 star2Pos = new Vector2(0, 128) + npc.Center;
            Vector2 star3Pos = new Vector2(-32, -64) + npc.Center;
            Vector2 star4Pos = new Vector2(32, -64) + npc.Center;
            Vector2 star5Pos = new Vector2(0, -16) + npc.Center;

            int goreType = mod.GetGoreSlot("Gores/Star_1");

            if (StateTimer == star1Time)
            {
                Gore gore = Gore.NewGorePerfect(star1Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = StateTimer + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star1Pos, 2, 4, -0.4f);
            }
            else if (StateTimer == star2Time)
            {
                Gore gore = Gore.NewGorePerfect(star2Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = StateTimer + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star2Pos, 2, 4, -0.3f);
            }
            else if (StateTimer == star3Time)
            {
                Gore gore = Gore.NewGorePerfect(star3Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = StateTimer + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star3Pos, 2, 4, -0.2f);
            }
            else if (StateTimer == star4Time)
            {
                Gore gore = Gore.NewGorePerfect(star4Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = StateTimer + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star4Pos, 2, 4, -0.1f);
            }
            else if (StateTimer == star5Time)
            {
                Gore gore = Gore.NewGorePerfect(star5Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = StateTimer + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star4Pos, 2, 4, 0);
            }
            else if (StateTimer == lineTime)
            {
                TerraLeague.DustLine(star1Pos, star2Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star1Pos, star3Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star1Pos, star4Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star3Pos, star5Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star4Pos, star5Pos, 261, 0.5f, 2);
                TerraLeague.PlaySoundWithPitch(npc.Center, 2, 29, 0);

                UpdateColor();
            }
        }
        void DrawMorgStars()
        {
            int star1Time = Timer_DrawStars - 5;
            int star2Time = Timer_DrawStars - 13;
            int star3Time = Timer_DrawStars - 22;
            int star4Time = Timer_DrawStars - 30;
            int star5Time = Timer_DrawStars - 39;
            int star6Time = Timer_DrawStars - 47;
            int star7Time = Timer_DrawStars - 56;
            int lineTime = Timer_DrawStars - 65;

            Vector2 star1Pos = new Vector2(-96, 0) + npc.Center;
            Vector2 star2Pos = new Vector2(-48, -48) + npc.Center;
            Vector2 star3Pos = new Vector2(-48, 48) + npc.Center;
            Vector2 star4Pos = new Vector2(0, 0) + npc.Center;
            Vector2 star5Pos = new Vector2(48, 48) + npc.Center;
            Vector2 star6Pos = new Vector2(48, -48) + npc.Center;
            Vector2 star7Pos = new Vector2(96, 0) + npc.Center;

            int goreType = mod.GetGoreSlot("Gores/Star_2");

            if (StateTimer == star1Time)
            {
                Gore gore = Gore.NewGorePerfect(star1Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = StateTimer + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star1Pos, 2, 4, -0.6f);
            }
            else if (StateTimer == star2Time)
            {
                Gore gore = Gore.NewGorePerfect(star2Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = StateTimer + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star2Pos, 2, 4, -0.5f);
            }
            else if (StateTimer == star3Time)
            {
                Gore gore = Gore.NewGorePerfect(star3Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = StateTimer + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star3Pos, 2, 4, -0.4f);
            }
            else if (StateTimer == star4Time)
            {
                Gore gore = Gore.NewGorePerfect(star4Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = StateTimer + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star4Pos, 2, 4, -0.3f);
            }
            else if (StateTimer == star5Time)
            {
                Gore gore = Gore.NewGorePerfect(star5Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = StateTimer + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star5Pos, 2, 4, -0.2f);
            }
            else if (StateTimer == star6Time)
            {
                Gore gore = Gore.NewGorePerfect(star6Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = StateTimer + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star6Pos, 2, 4, -0.1f);
            }
            else if (StateTimer == star7Time)
            {
                Gore gore = Gore.NewGorePerfect(star7Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = StateTimer + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star7Pos, 2, 4, 0);
            }
            else if (StateTimer == lineTime)
            {
                TerraLeague.DustLine(star1Pos, star2Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star1Pos, star3Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star2Pos, star4Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star3Pos, star4Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star5Pos, star4Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star6Pos, star4Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star7Pos, star5Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star7Pos, star6Pos, 261, 0.5f, 2);
                TerraLeague.PlaySoundWithPitch(npc.Center, 2, 29, 0);

                UpdateColor();
            }
        }
        void DrawKayleStars()
        {
            int star1Time = Timer_DrawStars - 5;
            int star2Time = Timer_DrawStars - 15;
            int star3Time = Timer_DrawStars - 25;
            int star4Time = Timer_DrawStars - 35;
            int star5Time = Timer_DrawStars - 45;
            int star6Time = Timer_DrawStars - 55;
            int lineTime = Timer_DrawStars - 65;

            Vector2 star1Pos = new Vector2(0, -128) + npc.Center;
            Vector2 star2Pos = new Vector2(0, 128) + npc.Center;
            Vector2 star3Pos = new Vector2(-64, -64) + npc.Center;
            Vector2 star4Pos = new Vector2(64, -64) + npc.Center;
            Vector2 star5Pos = new Vector2(24, -64) + npc.Center;
            Vector2 star6Pos = new Vector2(-24, -64) + npc.Center;

            int goreType = mod.GetGoreSlot("Gores/Star_3");

            if (StateTimer == star1Time)
            {
                Gore gore = Gore.NewGorePerfect(star1Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = StateTimer + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star1Pos, 2, 4, -0.5f);
            }
            else if (StateTimer == star2Time)
            {
                Gore gore = Gore.NewGorePerfect(star2Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = StateTimer + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star2Pos, 2, 4, -0.4f);
            }
            else if (StateTimer == star3Time)
            {
                Gore gore = Gore.NewGorePerfect(star3Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = StateTimer + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star3Pos, 2, 4, -0.3f);
            }
            else if (StateTimer == star4Time)
            {
                Gore gore = Gore.NewGorePerfect(star4Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = StateTimer + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star4Pos, 2, 4, -0.2f);
            }
            else if (StateTimer == star5Time)
            {
                Gore gore = Gore.NewGorePerfect(star5Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = StateTimer + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star5Pos, 2, 4, -0.1f);
            }
            else if (StateTimer == star6Time)
            {
                Gore gore = Gore.NewGorePerfect(star6Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = StateTimer + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star6Pos, 2, 4, 0);
            }
            else if (StateTimer == lineTime)
            {
                TerraLeague.DustLine(star1Pos, star2Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star3Pos, star4Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star1Pos, star5Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star1Pos, star6Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star2Pos, star5Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star2Pos, star6Pos, 261, 0.5f, 2);
                TerraLeague.PlaySoundWithPitch(npc.Center, 2, 29, 0);

                UpdateColor();
            }
        }
        void DrawLeonaStars()
        {
            int star1Time = Timer_DrawStars - 5;
            int star2Time = Timer_DrawStars - 12;
            int star3Time = Timer_DrawStars - 20;
            int star4Time = Timer_DrawStars - 27;
            int star5Time = Timer_DrawStars - 35;
            int star6Time = Timer_DrawStars - 42;
            int star7Time = Timer_DrawStars - 50;
            int star8Time = Timer_DrawStars - 57;
            int lineTime = Timer_DrawStars - 65;

            Vector2 star1Pos = new Vector2(0, 96) + npc.Center;
            Vector2 star2Pos = new Vector2(0, 96).RotatedBy(MathHelper.TwoPi * 3 / 8f) + npc.Center;
            Vector2 star3Pos = new Vector2(0, 96).RotatedBy(MathHelper.TwoPi * 6 / 8f) + npc.Center;
            Vector2 star4Pos = new Vector2(0, 96).RotatedBy(MathHelper.TwoPi * 1 / 8f) + npc.Center;
            Vector2 star5Pos = new Vector2(0, 96).RotatedBy(MathHelper.TwoPi * 4 / 8f) + npc.Center;
            Vector2 star6Pos = new Vector2(0, 96).RotatedBy(MathHelper.TwoPi * 7 / 8f) + npc.Center;
            Vector2 star7Pos = new Vector2(0, 96).RotatedBy(MathHelper.TwoPi * 2 / 8f) + npc.Center;
            Vector2 star8Pos = new Vector2(0, 96).RotatedBy(MathHelper.TwoPi * 5 / 8f) + npc.Center;

            int goreType = mod.GetGoreSlot("Gores/Star_4");

            if (StateTimer == star1Time)
            {
                Gore gore = Gore.NewGorePerfect(star1Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = StateTimer + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star1Pos, 2, 4, -0.7f);
            }
            else if (StateTimer == star2Time)
            {
                Gore gore = Gore.NewGorePerfect(star2Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = StateTimer + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star2Pos, 2, 4, -0.6f);
            }
            else if (StateTimer == star3Time)
            {
                Gore gore = Gore.NewGorePerfect(star3Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = StateTimer + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star3Pos, 2, 4, -0.5f);
            }
            else if (StateTimer == star4Time)
            {
                Gore gore = Gore.NewGorePerfect(star4Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = StateTimer + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star4Pos, 2, 4, -0.4f);
            }
            else if (StateTimer == star5Time)
            {
                Gore gore = Gore.NewGorePerfect(star5Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = StateTimer + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star5Pos, 2, 4, -0.3f);
            }
            else if (StateTimer == star6Time)
            {
                Gore gore = Gore.NewGorePerfect(star6Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = StateTimer + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star6Pos, 2, 4, -0.2f);
            }
            else if (StateTimer == star7Time)
            {
                Gore gore = Gore.NewGorePerfect(star7Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = StateTimer + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star7Pos, 2, 4, -0.1f);
            }
            else if (StateTimer == star8Time)
            {
                Gore gore = Gore.NewGorePerfect(star8Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = StateTimer + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star8Pos, 2, 4, 0);
            }
            else if (StateTimer == lineTime)
            {
                TerraLeague.DustLine(star1Pos, star2Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star2Pos, star3Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star3Pos, star4Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star4Pos, star5Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star5Pos, star6Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star6Pos, star7Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star7Pos, star8Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star8Pos, star1Pos, 261, 0.5f, 2);
                TerraLeague.PlaySoundWithPitch(npc.Center, 2, 29, 0);

                UpdateColor();
            }
        }
        void DrawDianaStars()
        {
            int star1Time = Timer_DrawStars - 5;
            int star2Time = Timer_DrawStars - 12;
            int star3Time = Timer_DrawStars - 20;
            int star4Time = Timer_DrawStars - 27;
            int star5Time = Timer_DrawStars - 35;
            int star6Time = Timer_DrawStars - 42;
            int star7Time = Timer_DrawStars - 50;
            int star8Time = Timer_DrawStars - 57;
            int lineTime = Timer_DrawStars - 65;

            Vector2 star1Pos = new Vector2(0, -96) + npc.Center;
            Vector2 star2Pos = new Vector2(0, -96).RotatedBy(MathHelper.TwoPi * 1 / 8f) + npc.Center;
            Vector2 star3Pos = new Vector2(0, -96).RotatedBy(MathHelper.TwoPi * 2 / 8f) + npc.Center;
            Vector2 star4Pos = new Vector2(0, -96).RotatedBy(MathHelper.TwoPi * 3 / 8f) + npc.Center;
            Vector2 star5Pos = new Vector2(0, -96).RotatedBy(MathHelper.TwoPi * 4 / 8f) + npc.Center;
            Vector2 star6Pos = new Vector2(0, -96).RotatedBy(MathHelper.TwoPi * 5 / 8f) + npc.Center;
            Vector2 star7Pos = new Vector2(0, -32).RotatedBy(MathHelper.TwoPi * 4 / 8f) + npc.Center;
            Vector2 star8Pos = new Vector2(0, -32).RotatedBy(MathHelper.TwoPi * 1 / 8f) + npc.Center;

            int goreType = mod.GetGoreSlot("Gores/Star_5");

            if (StateTimer == star1Time)
            {
                Gore gore = Gore.NewGorePerfect(star1Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = StateTimer + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star1Pos, 2, 4, -0.7f);
            }
            else if (StateTimer == star2Time)
            {
                Gore gore = Gore.NewGorePerfect(star2Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = StateTimer + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star2Pos, 2, 4, -0.6f);
            }
            else if (StateTimer == star3Time)
            {
                Gore gore = Gore.NewGorePerfect(star3Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = StateTimer + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star3Pos, 2, 4, -0.5f);
            }
            else if (StateTimer == star4Time)
            {
                Gore gore = Gore.NewGorePerfect(star4Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = StateTimer + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star4Pos, 2, 4, -0.4f);
            }
            else if (StateTimer == star5Time)
            {
                Gore gore = Gore.NewGorePerfect(star5Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = StateTimer + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star5Pos, 2, 4, -0.3f);
            }
            else if (StateTimer == star6Time)
            {
                Gore gore = Gore.NewGorePerfect(star6Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = StateTimer + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star6Pos, 2, 4, -0.2f);
            }
            else if (StateTimer == star7Time)
            {
                Gore gore = Gore.NewGorePerfect(star7Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = StateTimer + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star7Pos, 2, 4, -0.1f);
            }
            else if (StateTimer == star8Time)
            {
                Gore gore = Gore.NewGorePerfect(star8Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = StateTimer + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star8Pos, 2, 4, 0);
            }
            else if (StateTimer == lineTime)
            {
                TerraLeague.DustLine(star1Pos, star2Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star2Pos, star3Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star3Pos, star4Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star4Pos, star5Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star5Pos, star6Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star6Pos, star7Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star7Pos, star8Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star8Pos, star1Pos, 261, 0.5f, 2);
                TerraLeague.PlaySoundWithPitch(npc.Center, 2, 29, 0);

                UpdateColor();
            }
        }
        void DrawTaricStars()
        {
            int star1Time = Timer_DrawStars - 5;
            int star2Time = Timer_DrawStars - 15;
            int star3Time = Timer_DrawStars - 25;
            int star4Time = Timer_DrawStars - 35;
            int star5Time = Timer_DrawStars - 45;
            int star6Time = Timer_DrawStars - 55;
            int lineTime = Timer_DrawStars - 65;

            Vector2 star1Pos = new Vector2(0, -96) + npc.Center;
            Vector2 star2Pos = new Vector2(80, -80) + npc.Center;
            Vector2 star3Pos = new Vector2(48, 80) + npc.Center;
            Vector2 star4Pos = new Vector2(0, 96) + npc.Center;
            Vector2 star5Pos = new Vector2(-48, 80) + npc.Center;
            Vector2 star6Pos = new Vector2(-80, -80) + npc.Center;

            int goreType = mod.GetGoreSlot("Gores/Star_6");

            if (StateTimer == star1Time)
            {
                Gore gore = Gore.NewGorePerfect(star1Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = StateTimer + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star1Pos, 2, 4, -0.5f);
            }
            else if (StateTimer == star2Time)
            {
                Gore gore = Gore.NewGorePerfect(star2Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = StateTimer + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star2Pos, 2, 4, -0.4f);
            }
            else if (StateTimer == star3Time)
            {
                Gore gore = Gore.NewGorePerfect(star3Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = StateTimer + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star3Pos, 2, 4, -0.3f);
            }
            else if (StateTimer == star4Time)
            {
                Gore gore = Gore.NewGorePerfect(star4Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = StateTimer + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star4Pos, 2, 4, -0.2f);
            }
            else if (StateTimer == star5Time)
            {
                Gore gore = Gore.NewGorePerfect(star5Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = StateTimer + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star5Pos, 2, 4, -0.1f);
            }
            else if (StateTimer == star6Time)
            {
                Gore gore = Gore.NewGorePerfect(star6Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = StateTimer + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star6Pos, 2, 4, 0);
            }
            else if (StateTimer == lineTime)
            {
                TerraLeague.DustLine(star1Pos, star2Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star2Pos, star3Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star3Pos, star4Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star4Pos, star5Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star5Pos, star6Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star6Pos, star1Pos, 261, 0.5f, 2);
                TerraLeague.PlaySoundWithPitch(npc.Center, 2, 29, 0);

                UpdateColor();
            }
        }
        void DrawZoeStars()
        {
            int star1Time = Timer_DrawStars - 5;
            int star2Time = Timer_DrawStars - 15;
            int star3Time = Timer_DrawStars - 25;
            int star4Time = Timer_DrawStars - 35;
            int star5Time = Timer_DrawStars - 45;
            int star6Time = Timer_DrawStars - 55;
            int lineTime = Timer_DrawStars - 65;

            Vector2 star1Pos = new Vector2(-96, 0) + npc.Center;
            Vector2 star2Pos = new Vector2(96, 0) + npc.Center;
            Vector2 star3Pos = new Vector2(-40, 0) + npc.Center;
            Vector2 star4Pos = new Vector2(0, -40) + npc.Center;
            Vector2 star5Pos = new Vector2(40, 0) + npc.Center;
            Vector2 star6Pos = new Vector2(0, 40) + npc.Center;

            int goreType = mod.GetGoreSlot("Gores/Star_7");

            if (StateTimer == star1Time)
            {
                Gore gore = Gore.NewGorePerfect(star1Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = StateTimer + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star1Pos, 2, 4, -0.5f);
            }
            else if (StateTimer == star2Time)
            {
                Gore gore = Gore.NewGorePerfect(star2Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = StateTimer + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star2Pos, 2, 4, -0.4f);
            }
            else if (StateTimer == star3Time)
            {
                Gore gore = Gore.NewGorePerfect(star3Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = StateTimer + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star3Pos, 2, 4, -0.3f);
            }
            else if (StateTimer == star4Time)
            {
                Gore gore = Gore.NewGorePerfect(star4Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = StateTimer + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star4Pos, 2, 4, -0.2f);
            }
            else if (StateTimer == star5Time)
            {
                Gore gore = Gore.NewGorePerfect(star5Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = StateTimer + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star5Pos, 2, 4, -0.1f);
            }
            else if (StateTimer == star6Time)
            {
                Gore gore = Gore.NewGorePerfect(star6Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = StateTimer + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star6Pos, 2, 4, 0);
            }
            else if (StateTimer == lineTime)
            {
                TerraLeague.DustLine(star1Pos, star2Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star3Pos, star4Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star4Pos, star5Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star5Pos, star6Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star6Pos, star3Pos, 261, 0.5f, 2);
                TerraLeague.PlaySoundWithPitch(npc.Center, 2, 29, 0);

                UpdateColor();
            }
        }
        #endregion

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life > 0)
            {
                int count = 0;
                while ((double)count < damage / (double)npc.lifeMax * 50.0)
                {
                    Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.PortalBolt, 0f, 0f, 0, GetAttackColor, 1.5f);
                    dust.velocity *= 2f;
                    dust.noGravity = true;
                    count++;
                }
            }
            else
            {
                for (int i = 0; i < 20; i++)
                {
                    Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.PortalBolt, 0f, 0f, 0, GetAttackColor, 1.5f);
                    dust.velocity *= 2f;
                    dust.noGravity = true;
                }
            }
            base.HitEffect(hitDirection, damage);
        }

        public override void NPCLoot()
        {
            int choice = Main.rand.Next(10);
            if (choice == 0)
                Item.NewItem(npc.getRect(), ItemType<Items.Placeable.TargonBossTrophy>());

            if (Main.expertMode)
            {
                npc.DropBossBags();
            }
            else
            {
                choice = Main.rand.Next(10);
                if (choice == 0)
                {
                    Item.NewItem(npc.getRect(), ItemType<Items.Placeable.TargonMonolith>());
                }
                Item.NewItem(npc.getRect(), ItemType<Items.CelestialBar>(), Main.rand.Next(2, 8));
            }

            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendData(MessageID.WorldData);
            base.NPCLoot();
        }

        Color GetAttackColor
        {
            get
            {
                switch (npc.frame.Y)
                {
                    case 0:
                        return PanthColor;
                    case 130:
                        return MorgColor;
                    case 130 * 2:
                        return KayleColor;
                    case 130 * 3:
                        return LeonaColor;
                    case 130 * 4:
                        return DianaColor;
                    case 130 * 5:
                        return TaricColor;
                    case 130 * 6:
                        return ZoeColor;
                    default:
                        return DianaColor;
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = Main.npcTexture[npc.type];
            spriteBatch.Draw
            (
                texture,
                npc.Center - Main.screenPosition,
                npc.frame,
                new Color(255, 255, 255, 255/*AltAlpha*/),
                MathHelper.ToRadians((float)FloatyTimer),
                npc.frame.Size() * 0.5f,
                npc.scale,
                SpriteEffects.None,
                0f
            );

            spriteBatch.Draw
            (
                texture,
                npc.Center - Main.screenPosition,
                npc.frame,
                new Color(255, 255, 255, 255/*AltAlpha*/),
                MathHelper.ToRadians((float)FloatyTimer) + (MathHelper.PiOver4 * 5),
                npc.frame.Size() * 0.5f,
                npc.scale,
                SpriteEffects.None,
                0f
            );

            return base.PreDraw(spriteBatch, drawColor);
        }

        public override Color? GetAlpha(Color drawColor)
        {
            return Color.White;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            if (Main.netMode == NetmodeID.Server)
                npc.life = (int)((double)npc.lifeMax * 0.75 * (double)numPlayers);
            else
                npc.life = (int)((double)npc.lifeMax * 0.75);
            base.ScaleExpertStats(numPlayers, bossLifeScale);
        }

        public override bool CheckActive()
        {
            return false;
        }

        public override bool CheckDead()
        {
            TerraLeagueWORLDGLOBAL.TargonArenaDefeated = true;

            return base.CheckDead();
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            base.BossLoot(ref name, ref potionType);
        }
    }
}