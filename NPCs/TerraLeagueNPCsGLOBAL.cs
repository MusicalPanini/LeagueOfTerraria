﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using TerraLeague.Buffs;
using TerraLeague.Projectiles;
using static Terraria.ModLoader.ModContent;
using Terraria.Localization;
using System;
using TerraLeague.Items.SummonerSpells;
using Terraria.ModLoader.Utilities;
using TerraLeague.NPCs.VoidNPCs;

namespace TerraLeague.NPCs
{
    public class TerraLeagueNPCsGLOBAL : GlobalNPC
    {
        internal static NPCPacketHandler PacketHandler = new NPCPacketHandler(2);
        public float initialSpeed = 1;

        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }
        int timer = 60;
        public int baseDefence = 0;
        public int requiemDamage = 0;
        public int requiemTime = 0;
        public int vesselTarget = -1;
        public int vesselTimer = 420;
        public bool requiem = false;
        public bool bubbled = false;
        public bool slowed = false;
        public bool umbralTrespass = false;
        public bool torment = false;
        public bool abyssalCurse = false;
        public bool grievousWounds = false;
        public bool ignited = false;
        public bool exhaused = false;
        public bool weakSunfire = false;
        public bool sunfire = false;
        public bool OrgDest = false;
        public bool essenFlux = false;
        public bool frozen = false;
        public bool ablaze = false;
        public bool illuminated = false;
        public bool seeded = false;
        public bool vessel = false;
        public bool harbingersInferno = false;
        public bool doomed = false;
        public bool maleficVisions = false;
        public bool calibrumMark = false;
        public bool gravitumMark = false;
        public bool infernumMark = false;
        public bool icebornSubjugation = false;
        public int icebornSubjugationOwner = -1;

        public bool snared = false;
        public bool stunned = false;

        public int CleavedStacks = 0;
        public bool cleaved = false;

        public int HemorrhageStacks = 0;
        public bool hemorrhage = false;

        public int DeadlyVenomStacks = 0;
        public bool deadlyVenom = false;

        public bool CausticWounds = false;
        public int CausticStacks = 0;

        public int PoxStacks = 0;
        public bool pox = false;


        public override void SetDefaults(NPC npc)
        {
            if (NPCID.Sets.ShouldBeCountedAsBoss[npc.type] || npc.boss)
            {
                npc.buffImmune[BuffType<Stunned>()] = true;
                //npc.buffImmune[BuffType<TideCallerBubbled>()] = true;

            }
            base.SetDefaults(npc);
        }

        public override void ResetEffects(NPC npc)
        {
            if (bubbled)
            {
                npc.velocity.Y = 0;
            }
            requiem = false;
            bubbled = false;
            if (!slowed)
                icebornSubjugation = false;
            slowed = false;
            umbralTrespass = false;
            torment = false;
            abyssalCurse = false;
            grievousWounds = false;
            ignited = false;
            exhaused = false;
            weakSunfire = false;
            sunfire = false;
            OrgDest = false;
            essenFlux = false;
            frozen = false;
            ablaze = false;
            illuminated = false;
            seeded = false;
            harbingersInferno = false;
            doomed = false;
            maleficVisions = false;
            calibrumMark = false;
            gravitumMark = false;
            infernumMark = false;

            snared = false;
            stunned = false;

            if (!cleaved)
                CleavedStacks = 0;
            cleaved = false;

            if (!hemorrhage)
                HemorrhageStacks = 0;
            hemorrhage = false;

            if (!deadlyVenom)
                DeadlyVenomStacks = 0;
            deadlyVenom = false;

            if (!CausticWounds)
                CausticStacks = 0;
            CausticWounds = false;

            if (!pox)
                PoxStacks = 0;
            pox = false;

            npc.defense = npc.defDefense;
            npc.damage = npc.defDamage;
        }


        public override bool PreAI(NPC npc)
        {
            // Dust Effects
            Dust dust;
            if (slowed)
            {
                if (Main.rand.Next(0, 8) == 0)
                {
                    dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 113, 0f, 0f, 100, default);
                    dust.velocity *= 0.2f;
                    dust.scale *= 1.2f;
                    dust.alpha *= 200;
                }
            }
            if (umbralTrespass)
            {
                int num = Main.rand.Next(0, 10);
                if (num == 0)
                {
                    dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 186, 0, -1, 150);
                    dust.velocity.X *= 0.3f;
                    dust.color = new Color(255, 0, 0);
                    dust.noGravity = false;
                }
                else if (num == 2)
                {
                    dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 186, 0, -1, 150);
                    dust.velocity.X *= 0.3f;

                    dust.noGravity = false;
                }

            }
            if (requiem)
            {
                Color color = Main.rand.NextBool() ? new Color(0, 255, 140) : new Color(0, 255, 0);
                dust = Dust.NewDustDirect(new Vector2(npc.Center.X - 4, npc.position.Y - 320 + npc.height / 3f), 1, 300 + npc.height / 2, 186, 0f, 2f, 197, color, 2f);
                dust.noGravity = true;
                dust.velocity.X *= 0.1f;
                dust.fadeIn = 2.6f;
            }
            if (torment)
            {
                if (Main.rand.Next(0, 8) == 0)
                {
                    Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 6, 0f, 0f, 100, default);
                }
            }
            if (abyssalCurse)
            {
                if (Main.rand.Next(0, 4) == 0)
                {
                    dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height / 2, 14, 0f, 0f, 100, default);
                    dust.color = new Color(255, 0, 255);
                    dust.alpha = 150;
                    dust.scale = 1f;
                    dust.velocity.X = 0;
                    dust.velocity.Y = -0.5f;
                }
            }
            if (OrgDest)
            {
                if (Main.rand.Next(0, 3) == 0)
                {
                    dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 112, 0f, 0f, 255, new Color(59, 0, 255));
                    dust.alpha = 150;
                    dust.noGravity = true;
                    dust.noLight = true;
                }
            }
            if (essenFlux)
            {
                for (int i = 0; i < 2; i++)
                {
                    dust = Dust.NewDustDirect(new Vector2(npc.position.X - 3, npc.position.Y + (npc.height / 2)), npc.width + 6, 4, 159, 0f, 0f, 50, default);
                    dust.noGravity = true;
                }
            }
            if (grievousWounds)
            {
                if (Main.rand.Next(0, 8) == 0)
                {
                    dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 14, 0f, 0f, 100, default, 1f);
                    dust.color = new Color(255, 0, 0);
                    dust.velocity.X = 0;
                    dust.velocity.Y = 0.5f;
                }
            }
            if (ignited)
            {
                if (Main.rand.Next(0, 2) == 0)
                {
                    dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 6, 0f, -12f, 100, default, 6);
                    dust.noGravity = true;
                }
            }
            if (sunfire || weakSunfire)
            {
                if (Main.rand.Next(0, 2) == 0)
                {
                    dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 6, 0f, 0f, 100, default, 2f);
                    dust.noGravity = true;
                }

                if (Main.rand.Next(0, 4) == 0)
                {
                    dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 6, 0f, 0f, 100, default, 1f);
                    dust.velocity.Y = -Math.Abs(dust.velocity.Y * 2f);
                }
            }
            if (CausticWounds)
            {
                if (Main.rand.Next(0, 8) == 0)
                {
                    dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 112, 0f, 0f, 100, new Color(59, 0, 255));
                    dust.noGravity = true;
                    dust.velocity *= 0;
                }
            }
            if (ablaze)
            {
                if (Main.rand.Next(0, 2) == 0)
                {
                    dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 6, 0f, 0f, 0, new Color(255, 0, 0), 4);
                    dust.noGravity = true;
                    Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 6, 0f, 0f, 0, new Color(255, 0, 0), 1);
                }
            }
            if (infernumMark)
            {
                if (Main.rand.Next(0, 2) == 0)
                {
                    dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.GemSapphire, 0f, -2f, 0, default, 2);
                    dust.noGravity = true;
                    dust.velocity.X *= 0.1f;
                    dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.GemSapphire, 0f, -3f, 0, default, 0.5f);
                    dust.velocity.X *= 0.2f;
                }
            }
            if (doomed)
            {
                if (Main.rand.Next(0, 2) == 0)
                {
                    dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 110, 0f, 0f, 0, new Color(0, 255, 201), 2);
                    dust.noGravity = true;
                    dust.velocity *= 0;

                    dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 110, 0f, 0f, 0, new Color(0, 255, 201), 1);
                    dust.noGravity = true;
                    dust.velocity *= 0;
                }
            }
            if (harbingersInferno)
            {
                dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 6, 0f, 0f, 0, default, 3);
                dust.noGravity = true;
                Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 6, 0f, 0f, 0, default, 1);
            }
            if (cleaved)
            {
                int num = Main.rand.Next(0, 8);
                if (num == 0)
                {
                    dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 186, 0, 1, 200, new Color(50, 0, 20));
                    dust.velocity.X *= 0.1f;
                    dust.velocity.Y -= 3f;
                    dust.noGravity = false;
                }
            }
            if (deadlyVenom)
            {
                int num = Main.rand.Next(0, 4);
                if (num == 0)
                {
                    dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 167, 0f, -1f, 125, new Color(0, 192, 255), 1f);
                    dust.velocity.X *= 0.1f;
                    dust.velocity.Y = -System.Math.Abs(dust.velocity.Y);
                    dust.noGravity = true;
                    dust.fadeIn = 1.5f;
                }
            }
            if (hemorrhage)
            {
                int num = Main.rand.Next(0, 4);
                if (num == 0)
                {
                    dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.Blood, 0, 0, 0, default, 1.25f);
                    dust.velocity.X *= 0f;
                    dust.velocity.Y = System.Math.Abs(dust.velocity.Y);
                }
            }
            if (pox)
            {
                int num = Main.rand.Next(0, 4);
                if (num == 0)
                {
                    dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 167, 0, 0, 0, default, 1.25f);
                    dust.velocity.X *= 0f;
                    dust.velocity.Y = System.Math.Abs(dust.velocity.Y);
                    dust.noGravity = true;
                }
            }
            if (illuminated)
            {
                dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.GoldFlame, 0f, 0f, 0, default, 1);
                dust.noGravity = true;
                dust.velocity *= 1.3f;
            }
            if (vessel)
            {
                if (Main.rand.Next(0, 2) == 0)
                {
                    dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.BlueTorch, 0f, -2f, 200, new Color(0, 255, 201), 3f);
                    dust.noGravity = true;
                    dust.velocity.Y -= 2;

                    dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.BlueTorch, 0f, -1f, 200, new Color(0, 255, 201), 3f);
                    dust.noGravity = true;
                    dust.velocity.Y -= 2;
                }
            }
            if (calibrumMark)
            {
                dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 111, 0, -2, 100, default, 1f);
                dust.noGravity = true;
                dust.velocity.X *= 0;
            }
            if (gravitumMark)
            {
                dust = Dust.NewDustDirect(new Vector2(npc.Center.X - 16, npc.position.Y - 16), 32, 32, DustID.HallowedTorch, 0f, 0f, 100, new Color(0, 0, 0), 1f);
                dust.noGravity = true;
                dust.fadeIn = 1.2f;
                dust.velocity = (dust.position - npc.Center) * -0.05f;
            }

            npc.defense = npc.defDefense;
            npc.damage = npc.defDamage;

            if (bubbled)
            {
                npc.velocity.Y = 0;
                npc.position.X = npc.oldPosition.X;
                npc.position.Y = npc.oldPosition.Y - 1;
            }
            if (slowed)
            {
                npc.damage = (int)(npc.damage * 0.7);
                npc.position = new Vector2((npc.oldPosition.X + npc.position.X) / 2, (npc.oldPosition.Y + npc.position.Y) / 2);
            }
            if (cleaved)
                npc.defense -= (int)(npc.defense * 0.06 * (CleavedStacks + 1));


            timer--;
            if (timer <= 0)
                timer = 60;

            if (vessel)
            {
                vesselTimer--;

                if (vesselTimer <= 0 || !Main.npc[vesselTarget].active)
                {
                    npc.life = 0;
                }
            }

            if (stunned || bubbled || vessel)
            {
                npc.frameCounter = 0;
                npc.velocity = Vector2.Zero;
                if (vessel)
                {
                    npc.position = vesselTimer == 419 ? npc.position : npc.oldPosition;
                    npc.SpawnedFromStatue = true;
                }

                return false;
            }

            return base.PreAI(npc);
        }
        public override void AI(NPC npc)
        {
            
        }

        public override void OnHitByItem(NPC npc, Player player, Item item, int damage, float knockback, bool crit)
        {
            
        }

        public override void OnHitByProjectile(NPC npc, Projectile projectile, int damage, float knockback, bool crit)
        {
            
        }

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            PLAYERGLOBAL modPlayer = spawnInfo.player.GetModPlayer<PLAYERGLOBAL>();

            if (modPlayer.zoneSurfaceMarble)
            {
                pool.Remove(NPCID.GreenSlime);
                pool.Remove(NPCID.BlueSlime);

                pool.Add(NPCType<MarbleSlime>(), SpawnCondition.OverworldDaySlime.Chance);

                if (!Main.dayTime)
                    pool.Add(NPCID.GreekSkeleton, SpawnCondition.OverworldNightMonster.Chance);

                if (Main.hardMode)
                    pool.Add(NPCID.Medusa, 0.2f);

            }
            
            if (modPlayer.zoneVoid)
            {
                pool.Clear();

                pool.Add(NPCType<TaintedCavebat>(), 1);
                pool.Add(NPCType<TaintedSkeleton>(), 1);
                pool.Add(NPCType<ZzRotFlyer>(), 1);
                pool.Add(NPCType<XersaiBrute>(), 0.75f);

                if (Main.hardMode)
                {
                    pool.Add(NPCType<VoidbornSlime>(), 0.5f);

                    if (NPC.CountNPCS(NPCType<TunnelingTerror_Head>()) < 1)
                        pool.Add(NPCType<TunnelingTerror_Head>(), 0.25f);

                    if (NPC.CountNPCS(NPCType<XersaiStoneSwimmer>()) < 1)
                        pool.Add(NPCType<XersaiStoneSwimmer>(), 0.25f);
                }
            }

            base.EditSpawnPool(pool, spawnInfo);
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (deadlyVenom)
            {
                npc.lifeRegen -= (int)(2 * (DeadlyVenomStacks + 1));

                if (damage < (DeadlyVenomStacks + 1))
                {
                    damage = (DeadlyVenomStacks + 1);
                }
            }
            if (hemorrhage)
            {
                npc.lifeRegen -= (int)(4 * (HemorrhageStacks + 1));

                if (damage < (HemorrhageStacks + 1))
                {
                    damage = (HemorrhageStacks + 1);
                }
            }
            if (harbingersInferno)
            {
                npc.lifeRegen -= 40;

                if (damage < 2)
                {
                    damage = 2;
                }
            }
            if (ablaze)
            {
                npc.lifeRegen -= 24;

                if (damage < 3)
                {
                    damage = 3;
                }
            }
            if (infernumMark)
            {
                npc.lifeRegen -= 300;

                if (damage < 100)
                {
                    damage = 100;
                }
            }
            if (sunfire)
            {
                npc.lifeRegen -= 100;

                if (damage < 50)
                {
                    damage = 50;
                }
            }
            else if (weakSunfire)
            {
                npc.lifeRegen -= 20;

                if (damage < 10)
                {
                    damage = 10;
                }
            }
            if (ignited)
            {
                npc.lifeRegen -= IgniteRune.GetDOTDamage();
                if (damage < 10)
                    damage = 10;
                //int regen;
                //if (NPC.downedGolemBoss)
                //{
                //    regen = 144;
                //    if (damage < 50)
                //        damage = 50;
                //}
                //else if (NPC.downedPlantBoss)
                //{
                //    regen = 64;
                //    if (damage < 25)
                //        damage = 25;
                //}
                //else if (NPC.downedMechBossAny)
                //{
                //    regen = 38;
                //    if (damage < 10)
                //        damage = 10;
                //}
                //else if (Main.hardMode)
                //{
                //    regen = 24;
                //    if (damage < 5)
                //        damage = 5;
                //}
                //else if (NPC.downedBoss2)
                //{
                //    regen = 12;
                //    if (damage < 2)
                //        damage = 2;
                //}
                //else
                //{
                //    regen = 4;
                //    if (damage < 1)
                //        damage = 1;
                //}
                //npc.lifeRegen -= regen * 4;
            }
            if (torment)
            {
                int regen;
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }

                if (NPCID.Sets.ShouldBeCountedAsBoss[npc.type])
                {
                    regen = 500;
                }
                else
                {
                    regen = (int)(npc.lifeMax * 0.1f);
                    if (regen > 500)
                    {
                        regen = 500;
                    }
                }

                if (slowed)
                {
                    regen *= 2;
                }

                if (regen < 30)
                {
                    regen = 30;
                }

                npc.lifeRegen -= regen;

                if (damage < 10)
                {
                    damage = 10;
                }
            }
            if (maleficVisions)
            {
                npc.lifeRegen -= 20;

                if (damage < 2)
                {
                    damage = 2;
                }
            }

            if (grievousWounds && npc.lifeRegen < 0)
            {
                npc.lifeRegen = (int)(npc.lifeRegen * 1.5);
            }
        }

        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            if (modPlayer.targonArena)
            {
                maxSpawns = 0;
            }
            if (modPlayer.zoneBlackMist)
            {
                maxSpawns =(int)(maxSpawns * 2);
                if (spawnRate > 300)
                {
                    spawnRate = 300;
                }
                    spawnRate = (int)(spawnRate * 0.35);
            }
            if (modPlayer.Disruption)
            {
                spawnRate = (int)(spawnRate * 2);
            }
        }

        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            if (calibrumMark)
            {
                damage = (int)(damage * 1.5);
            }

            VesselStriked(player.whoAmI, damage, crit);
        }

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (calibrumMark)
            {
                damage = (int)(damage * 1.5);
            }

            if (OrgDest)
                damage = (int)(damage * 1.1);

            if (projectile.owner != -1 || projectile.owner != 255)
                VesselStriked(projectile.owner, damage, crit);

            if (projectile.type == ProjectileType<EyeofGod_TestofSpirit>())
            {
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    PacketHandler.SendCreateVessel(-1, projectile.owner, npc.whoAmI, projectile.owner);
                }
                else
                {
                    Player player = Main.player[projectile.owner];
                    int vessel = NPC.NewNPC((int)player.Bottom.X + (64 * player.direction), (int)player.Bottom.Y, npc.type);
                    Main.npc[vessel].life = npc.life;
                    Main.npc[vessel].SpawnedFromStatue = true;
                    Main.npc[vessel].GetGlobalNPC<TerraLeagueNPCsGLOBAL>().vesselTarget = npc.whoAmI;
                    Main.npc[vessel].GetGlobalNPC<TerraLeagueNPCsGLOBAL>().vessel = true;
                    Main.npc[vessel].GetGlobalNPC<TerraLeagueNPCsGLOBAL>().vesselTimer = 420;
                }
                //Main.npc[vessel].AddBuff(ModContent.BuffType<Vessel>(), 60 * 7);
            }

            base.ModifyHitByProjectile(npc, projectile, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override bool CheckDead(NPC npc)
        {
            return base.CheckDead(npc);
        }

        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if (type == NPCID.ArmsDealer && NPC.downedBoss2)
            {
                shop.item[nextSlot].SetDefaults(ItemID.Revolver);
                nextSlot++;
            }

            if (type == NPCID.Dryad)
            {
                shop.item[nextSlot].SetDefaults(ItemType<Items.Placeable.PetSeeds>());
                nextSlot++;
            }
        }

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            //if (vessel)
            //    return false;
            base.ModifyNPCLoot(npc, npcLoot);
        }

        public override void OnKill(NPC npc)
        {
            if (icebornSubjugation)
            {
                for (int i = 0; i < 20; i++)
                {
                    int type;
                    switch (Main.rand.Next(0, 3))
                    {
                        case 0:
                            type = ProjectileType<DarkIceTome_IceShardSmallA>();
                            break;
                        case 1:
                            type = ProjectileType<DarkIceTome_IceShardSmallB>();
                            break;
                        default:
                            type = ProjectileType<DarkIceTome_IceShardSmallC>();
                            break;
                    }

                    Projectile.NewProjectileDirect(Main.player[icebornSubjugationOwner].GetProjectileSource_Item(new Items.Weapons.DarkIceTome().Item), npc.Center, new Vector2(0, Main.rand.NextFloat(9, 12)).RotatedByRandom(MathHelper.TwoPi), type, npc.lifeMax / 5, 1, icebornSubjugationOwner, npc.whoAmI);
                }
            }
            if (seeded)
            {
                if (Main.rand.Next(0, 4) == 0)
                {
                    int x = Main.rand.Next(-5, 4);
                    x *= Main.rand.Next(0, 2) == 0 ? 1 : -1;
                    Projectile.NewProjectile(null, npc.Center, new Vector2(x, -8), ProjectileType<StrangleThornsTome_Seed>(), 0, 0, 255);
                }
            }

            base.OnKill(npc);
        }

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (ignited || ablaze)
            {
                drawColor = new Color(150, 0, 0);
            }
            if (exhaused)
            {
                drawColor = new Color(199, 160, 14);
            }
            if (deadlyVenom)
            {
                drawColor = new Color(0, 160, 0);
            }
            if (stunned && !frozen)
            {
                drawColor = new Color(255, 255, 0);
            }
            if (OrgDest)
            {
                drawColor = new Color(255, 0, 255);
            }
            if (vessel)
            {
                drawColor = new Color(0, 255, 144);
            }
            if (icebornSubjugation)
            {
                drawColor = new Color(0, 144, 255);
            }

            base.DrawEffects(npc, ref drawColor);
        }

        public double OnHitDamage(NPC npc, Player player, int Damage, float knockBack = 0, int hitDirection = 0, bool crit = false)
        {
            bool flag = Main.netMode == 0;
            double damage;
            if (npc.active && npc.life > 0)
            {
                damage = (double)Damage;
                int defence
                    = npc.defense;
                if (npc.ichor)
                {
                    defence -= 15;
                }
                if (npc.betsysCurse)
                {
                    defence -= 40;
                }
                if (defence < 0)
                {
                    defence = 0;
                }
                damage = Main.CalculateDamageNPCsTake((int)damage, defence);
                if (crit)
                {
                    damage *= 2.0;
                }
                if (npc.takenDamageMultiplier > 1f)
                {
                    damage *= (double)npc.takenDamageMultiplier;
                }
                if ((npc.takenDamageMultiplier > 1f || Damage != 9999) && npc.lifeMax > 1)
                {
                    if (npc.friendly)
                    {
                        Color color = crit ? CombatText.DamagedFriendlyCrit : CombatText.DamagedFriendly;
                        CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), color, (int)damage, crit, false);
                    }
                    else
                    {
                        Color color2 = crit ? Color.Purple : Color.MediumPurple;
                        CombatText.NewText(new Rectangle((int)npc.position.X + 32, (int)npc.position.Y, npc.width, npc.height), color2, (int)damage, crit, false);
                    }
                }
                if (damage >= 1.0)
                {
                    if (!npc.immortal)
                    {
                        if (npc.realLife >= 0)
                        {
                            Main.npc[npc.realLife].life -= (int)damage;
                            npc.life = Main.npc[npc.realLife].life;
                            npc.lifeMax = Main.npc[npc.realLife].lifeMax;
                        }
                        else
                        {
                            npc.life -= (int)damage;
                        }
                    }
                    npc.HitEffect(hitDirection, damage);
                    if (npc.realLife >= 0)
                    {
                        Main.npc[npc.realLife].checkDead();
                    }
                    else
                    {
                        npc.checkDead();
                    }
                    return damage;
                }
                return 0.0;
            }
            return 0.0;
            
            //if (!npc.active || NPC.life <= 0)
            //{
            //    return 0.0;
            //}
            //double num = (double)Damage;
            //int num2 = NPC.defense;
            //if (npc.ichor)
            //{
            //    num2 -= 20;
            //}
            //if (npc.betsysCurse)
            //{
            //    num2 -= 40;
            //}
            //if (num2 < 0)
            //{
            //    num2 = 0;
            //}
            //if (NPCLoader.StrikeNPC(npc, ref num, num2, ref knockBack, hitDirection, ref crit))
            //{
            //    num = Main.CalculateDamageNPCsTake((int)num, num2);
            //    if (crit)
            //    {
            //        num *= 1.5;
            //    }
            //    if (npc.takenDamageMultiplier > 1f)
            //    {
            //        num *= (double)npc.takenDamageMultiplier;
            //    }
            //}
            //if ((npc.takenDamageMultiplier > 1f || Damage != 9999) && NPC.lifeMax > 1)
            //{
            //    if (npc.friendly)
            //    {
            //        Color color = crit ? Color.Purple : Color.MediumPurple;
            //        CombatText.NewText(new Rectangle((int)NPC.position.X, (int)NPC.position.Y + 40, NPC.width, NPC.height), color, (int)num, false, false);
            //    }
            //    else
            //    {
            //        Color color2 = crit ? Color.Purple : Color.MediumPurple;

            //        CombatText.NewText(new Rectangle((int)NPC.position.X + 32, (int)NPC.position.Y, NPC.width, NPC.height), color2, (int)num, false, false);
            //    }
            //    if (Main.netMode == NetmodeID.MultiplayerClient)
            //    {
            //        PacketHandler.SendBattleText(-1, player.whoAmI, (int)num, npc.whoAmI, crit);
            //    }
            //}
            //if (num >= 1.0)
            //{
            //    if (!npc.immortal)
            //    {
            //        if (npc.realLife >= 0)
            //        {
            //            Main.npc[npc.realLife].life -= (int)num;
            //            NPC.life = Main.npc[npc.realLife].life;
            //            NPC.lifeMax = Main.npc[npc.realLife].lifeMax;
            //        }
            //        else
            //        {
            //            NPC.life -= (int)num;
            //        }
            //    }
            //    if (npc.realLife >= 0)
            //    {
            //        Main.npc[npc.realLife].checkDead();
            //    }
            //    else
            //    {
            //        npc.HitEffect(hitDirection, num);
            //        npc.checkDead();
            //    }
            //    return num;
            //}
            //return 0.0;
        }

        public override bool CanHitPlayer(NPC npc, Player target, ref int cooldownSlot)
        {
            if (target.HasBuff(BuffType<UmbralTrespassing>()) || vessel || stunned)
                return false;
            else
                return base.CanHitPlayer(npc, target, ref cooldownSlot);
        }

        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (vessel)
            {
                Texture2D texture = Request<Texture2D>("TerraLeague/Gores/VesselLink").Value;

                Vector2 position = npc.Center + new Vector2(0, 6);
                Vector2 center = Main.npc[vesselTarget].Center;
                float num1 = (float)texture.Height;
                Vector2 vector2_4 = center - position;
                bool flag = true;
                if (float.IsNaN(position.X) && float.IsNaN(position.Y))
                    flag = false;
                if (float.IsNaN(vector2_4.X) && float.IsNaN(vector2_4.Y))
                    flag = false;
                while (flag)
                {
                    if ((double)vector2_4.Length() < (double)num1 + 1.0)
                    {
                        flag = false;
                    }
                    else
                    {
                        Vector2 vector2_1 = vector2_4;
                        vector2_1.Normalize();
                        position += vector2_1 * num1;
                        vector2_4 = center - position;

                        if (Main.rand.Next(0, 6) == 0)
                        {
                            Dust dust = Dust.NewDustPerfect(position, 59, null, 200, new Color(0, 255, 201), 3f);
                            dust.noGravity = true;
                        }

                    }
                }
            }

            return base.PreDraw(npc, spriteBatch, screenPos, drawColor);
        }

        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (frozen)
            {
                Texture2D texture = Request<Texture2D>("TerraLeague/Gores/FrozenEffect").Value;
                Color color = Color.White;
                color.A = 200;
                Main.spriteBatch.Draw
                (
                    texture,
                    new Vector2
                    (
                        npc.position.X - Main.screenPosition.X + npc.width * 0.5f,
                        npc.position.Y - Main.screenPosition.Y + npc.height * 0.5f
                    ),
                    new Rectangle(0, 0, texture.Width, texture.Height),
                    color,
                    0,
                    texture.Size() * 0.5f,
                    ((npc.width > npc.height ? npc.width : npc.height) / 56f) + 0.2f,
                    SpriteEffects.None,
                    0f
                );
            }

            if (CausticWounds)
            {
                Texture2D texture;
                int timeLeft = 0;
                if (npc.HasBuff(BuffType<CausticWounds>()))
                    timeLeft = npc.buffTime[npc.FindBuffIndex(BuffType<CausticWounds>())];

                switch (CausticStacks)
                {
                    case 1:
                        texture = Request<Texture2D>("TerraLeague/Gores/Caustic1").Value;
                        break;
                    case 2:
                        texture = Request<Texture2D>("TerraLeague/Gores/Caustic2").Value;
                        break;
                    case 3:
                        texture = Request<Texture2D>("TerraLeague/Gores/Caustic3").Value;
                        break;
                    case 4:
                        texture = Request<Texture2D>("TerraLeague/Gores/Caustic4").Value;
                        break;
                    case 5:
                        texture = Request<Texture2D>("TerraLeague/Gores/Caustic5").Value;
                        break;
                    default:
                        texture = Request<Texture2D>("TerraLeague/Gores/Caustic5").Value;
                        break;
                }

                if (CausticStacks < 5 || (CausticStacks >= 5 && timeLeft > 210))
                {
                    Main.spriteBatch.Draw
                    (
                        texture,
                        new Vector2
                        (
                            npc.position.X - Main.screenPosition.X + npc.width * 0.5f,
                            npc.position.Y - Main.screenPosition.Y + npc.height * 1.25f
                        ),
                        new Rectangle(0, 0, texture.Width, texture.Height),
                        new Color(255, 0, 255, 255),
                        0,
                        texture.Size() * 0.5f,
                        (npc.width / 128f) + (CausticStacks >= 5 ? (timeLeft > 210 ? ((timeLeft - 210) / 30f) : 0f) : 0.25f),
                        SpriteEffects.None,
                        0f
                    ); ;
                }
            }

            if (deadlyVenom)
            {
                Texture2D texture;

                switch (DeadlyVenomStacks)
                {
                    case 0:
                        texture = Request<Texture2D>("TerraLeague/Gores/DeadlyVenom1").Value;
                        break;
                    case 1:
                        texture = Request<Texture2D>("TerraLeague/Gores/DeadlyVenom2").Value;
                        break;
                    case 2:
                        texture = Request<Texture2D>("TerraLeague/Gores/DeadlyVenom3").Value;
                        break;
                    case 3:
                        texture = Request<Texture2D>("TerraLeague/Gores/DeadlyVenom4").Value;
                        break;
                    case 4:
                        texture = Request<Texture2D>("TerraLeague/Gores/DeadlyVenom5").Value;
                        break;
                    default:
                        texture = Request<Texture2D>("TerraLeague/Gores/DeadlyVenom1").Value;
                        break;
                }

                Main.spriteBatch.Draw
                (
                    texture,
                    new Vector2
                    (
                        npc.position.X - Main.screenPosition.X + npc.width * 0.5f,
                        npc.position.Y - Main.screenPosition.Y - npc.height * 0.8f
                    ),
                    new Rectangle(0, 0, texture.Width, texture.Height),
                    Color.White,
                    0,
                    texture.Size() * 0.5f,
                    (npc.width / 128f) + 0.5f,
                    SpriteEffects.None,
                    0f
                );
            }

            if (pox)
            {
                Texture2D texture = Request<Texture2D>("TerraLeague/Gores/Pox").Value;

                for (int i = 0; i < PoxStacks + 1; i++)
                {
                    Main.spriteBatch.Draw
                    (
                        texture,
                        new Vector2
                        (
                            npc.position.X - Main.screenPosition.X + (npc.width * 1.5f) + 4,
                            npc.position.Y - Main.screenPosition.Y + (npc.height * 1.2f) * (i * 0.2f)
                        ),
                        new Rectangle(0, 0, texture.Width, texture.Height),
                        Color.White,
                        0,
                        texture.Size() * 0.5f,
                        0.4f,
                        SpriteEffects.None,
                        0f
                    );
                }
            }

            if (hemorrhage)
            {
                Texture2D texture = Request<Texture2D>("TerraLeague/Gores/Hemorrhage").Value;

                for (int i = 0; i < HemorrhageStacks + 1; i++)
                {
                    Main.spriteBatch.Draw
                    (
                        texture,
                        new Vector2
                        (
                            npc.position.X - Main.screenPosition.X + npc.width * 1.5f,
                            npc.position.Y - Main.screenPosition.Y + (npc.height * 1.2f) * (i * 0.2f)
                        ),
                        new Rectangle(0, 0, texture.Width, texture.Height),
                        Color.White,
                        0,
                        texture.Size() * 0.5f,
                        0.4f,
                        SpriteEffects.None,
                        0f
                    );
                }
            }

            if (cleaved)
            {
                Texture2D texture;

                switch (CleavedStacks)
                {
                    case 0:
                        texture = Request<Texture2D>("TerraLeague/Gores/Cleaved1").Value;
                        break;
                    case 1:
                        texture = Request<Texture2D>("TerraLeague/Gores/Cleaved2").Value;
                        break;
                    case 2:
                        texture = Request<Texture2D>("TerraLeague/Gores/Cleaved3").Value;
                        break;
                    case 3:
                        texture = Request<Texture2D>("TerraLeague/Gores/Cleaved4").Value;
                        break;
                    case 4:
                        texture = Request<Texture2D>("TerraLeague/Gores/Cleaved5").Value;
                        break;
                    case 5:
                        texture = Request<Texture2D>("TerraLeague/Gores/Cleaved6").Value;
                        break;
                    default:
                        texture = Request<Texture2D>("TerraLeague/Gores/Cleaved1").Value;
                        break;
                }

                Main.spriteBatch.Draw
                (
                    texture,
                    new Vector2
                    (
                        npc.position.X - Main.screenPosition.X - (npc.width * 0.5f) - (System.Math.Min((System.Math.Min(npc.width, npc.height) / 28f) - 0.2f, 1.2f) / 2),
                        npc.position.Y - Main.screenPosition.Y
                    ),
                    new Rectangle(0, 0, texture.Width, texture.Height),
                    Color.White,
                    0,
                    texture.Size() * 0.5f,
                    Math.Max(Math.Min((System.Math.Min(npc.width, npc.height) / 28f) - 0.2f, 1.2f), 0.5f),
                    SpriteEffects.None,
                    0f
                );
            }

            if (grievousWounds)
            {
                Texture2D texture = Request<Texture2D>("TerraLeague/Gores/GrievousWounds").Value;

                Main.spriteBatch.Draw
                (
                    texture,
                    new Vector2
                    (
                        npc.position.X - Main.screenPosition.X - (npc.width * 0.5f) - (System.Math.Min((System.Math.Min(npc.width, npc.height) / 28f) - 0.3f, 1.2f) / 2),
                        npc.position.Y - Main.screenPosition.Y + npc.height * 0.7f
                    ),
                    new Rectangle(0, 0, texture.Width, texture.Height),
                    Color.White,
                    0,
                    texture.Size() * 0.5f,
                    System.Math.Min((System.Math.Min(npc.width, npc.height) / 28f) - 0.1f, 1.2f),
                    SpriteEffects.None,
                    0f
                );
            }

            base.PostDraw(npc, spriteBatch, screenPos, drawColor);
        }

        public void VesselStriked(int attacker, int damage, bool crit)
        {
            if (vessel && vesselTarget != -1)
            {
                Main.player[attacker].ApplyDamageToNPC(Main.npc[vesselTarget], (int)(damage * 0.5), 0, 0, crit);
            }
        }

        public override bool? DrawHealthBar(NPC npc, byte hbPosition, ref float scale, ref Vector2 position)
        {
            if (CausticWounds)
            {
                position = new Vector2(position.X, (npc.position.Y + npc.height * 1.25f) + (50 * ((npc.width / 128f) + 0.25f) ));
                return true;
            }

            return base.DrawHealthBar(npc, hbPosition, ref scale, ref position);
        }
    }
}
