using Microsoft.Xna.Framework;
using TerraLeague.Items;
using Terraria;
using Terraria.ID;
using TerraLeague.Gores;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using TerraLeague.Buffs;
using TerraLeague.Items.BossBags;
using TerraLeague.NPCs.TargonBoss;
using Terraria.GameContent.Bestiary;
using TerraLeague.Biomes;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using System;

namespace TerraLeague.NPCs
{
    public class TargonSigil : ModNPC
    {
        int rerolls = 3;
        int currentBlessing = Main.rand.Next(0, 8);

        public override void Load()
        {
            IL.Terraria.GameContent.ShopHelper.IsNotReallyTownNPC += HookNotReallyTownNPC;
            //IL.Terraria.GameContent.TeleportPylonsSystem.IsPlayerNearAPylon += HookInteractionRange;
        }
        private static void HookNotReallyTownNPC(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (!c.TryGotoNext(i => i.MatchRet()))
            {
                return; // Patch unable to be applied
            }

            // Push the Int instance onto the stack
            c.Emit(OpCodes.Ldloc_0);
            // Call a delegate using the int and Player from the stack.
            c.EmitDelegate<Func<bool, int, bool>>((returnValue, type) => {
                // Regular c# code
                return type == Terraria.ModLoader.ModContent.NPCType<TargonSigil>();
            });
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Targon's Peak");
        }
        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.lifeMax = 400;
            NPC.defense = 0;
            NPC.damage = 0;
            NPC.width = 80;
            NPC.height = 100;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.value = 0f;
            NPC.npcSlots = 0f;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.dontTakeDamage = true;
            NPC.netAlways = true;
            NPC.dontTakeDamageFromHostiles = true;
            NPC.dontCountMe = true;
        }

        public override bool PreAI()
        {
            NPC.homeless = true;
            return base.PreAI();
        }

        public override void AI()
        {
            if (NPC.CountNPCS(NPCType<TargonSigil>()) > 1)
                NPC.active = false;

            if (Main.dayTime && Main.time == 0)
                rerolls = 3;

            if (Main.rand.Next(20) == 0)
            {
                Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.AncientLight, 0f, 0f, 150, new Color(0, 150, 255), 0.5f);
                dust.noGravity = true;
                dust.velocity *= 0.1f;
                dust.fadeIn = 1f;
            }

            NPC.spriteDirection = -1;
            NPC.Center = new Vector2(Common.ModSystems.WorldSystem.TargonCenterX * 16, 45 * 16);
            NPC.position.Y += (float)System.Math.Sin(Main.time * 0.1);

            Lighting.AddLight(NPC.Center, 0, 0.3f, 1f);
            base.AI();
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            base.OnHitPlayer(target, damage, crit);
        }

        public override void HitEffect(int hitDirection, double damage)
        {

            base.HitEffect(hitDirection, damage);
        }

        public override void OnKill()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient && NPC.CountNPCS(NPCType<NPCs.TargonSigil>()) == 0)
                NPC.NewNPC(Common.ModSystems.WorldSystem.TargonCenterX * 16, 45 * 16, NPCType<NPCs.TargonSigil>());
            base.OnKill();
        }

        public override string GetChat()
        {
            string text = "From the greater beyond you can hear whispers in a language you do not know, but strangly can understand.";
            if (!Common.ModSystems.WorldSystem.TargonUnlocked)
            {
                return text + "\n\nYou are not worthy of their challenge just yet.";
            }
            else if (!Common.ModSystems.DownedBossSystem.downedTargonBoss)
            {
                if (NPC.CountNPCS(NPCType<TargonBossNPC>()) <= 0)
                {
                    return text + "\n\nThe whispers offer you a challenge with rewards of strength." +
                        "\n\nBring a " + GetModItem(ItemType<TargonMedallion>()).Name + " to the Arena to initiate the challenge";
                }
                else
                {
                    return text + "\n\nThe challenge is currently in progress.";
                }
            }
            else
            {
                string tip = Lang.GetBuffDescription(GetBuffID());
                int rare = 0;
                GetModBuff(GetBuffID()).ModifyBuffTip(ref tip, ref rare);
                text = "From the greater beyond you can hear whispers in a language you do not know, but strangly can understand.";
                if (Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().blessingCooldown <= 0)
                {
                    return text + "\n\nThe whipsers offer: The " + Lang.GetBuffName(GetBuffID())
                    + "\n" + tip;
                }
                else
                {
                    return text += "\n\nThe whipsers tell you it is not the right time." +
                        "\nThey will inform you when the next blessing is ready.";
                }
            }
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            if (Common.ModSystems.WorldSystem.TargonUnlocked && !Common.ModSystems.DownedBossSystem.downedTargonBoss)
            {
                if (NPC.CountNPCS(NPCType<TargonBossNPC>()) <= 0)
                    button = "Teleport to Arena";
            }
            else if (Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().blessingCooldown <= 0)
            {
                button = "Receive Blessing";
                button2 = "Reroll Blessing (" + rerolls + " remaining)";
            }
            else
            {
                string seconds = "" + Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().blessingCooldown % 3600 / 60;
                seconds = seconds.Length == 1 ? "0" + seconds : seconds;
                button = "Time Remaining: " + Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().blessingCooldown / 3600 + ":" + seconds;
                button2 = "Teleport to Arena";
            }
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (NPC.downedBoss1 && !Common.ModSystems.DownedBossSystem.downedTargonBoss)
            {
                if (firstButton)
                {
                    Vector2 teleportPos = new Vector2((Common.ModSystems.WorldSystem.TargonCenterX * 16) - 16, (60 * 16) + (float)(Main.worldSurface * 16));

                    Main.LocalPlayer.Teleport(teleportPos, 1, 0);
                    NetMessage.SendData(MessageID.Teleport, -1, -1, null, 0, Main.LocalPlayer.whoAmI, teleportPos.X, teleportPos.Y, 1, 0, 0);
                }
            }
            else if (Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().blessingCooldown <= 0)
            {
                if (firstButton)
                {
                    shop = false;
                    string text = "You gained the " + Lang.GetBuffName(GetBuffID());
                    int buffTime = 60 * 60 * 60; // Frames * seconds * minutes * gameDays = 60 MINUTES
                    Main.LocalPlayer.AddBuff(GetBuffID(), buffTime);
                    TerraLeague.PlaySoundWithPitch(NPC.Center, 2, 29, -1);
                    Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().blessingCooldown = 60 * 60 * 120;
                    Main.npcChatText = text;
                    GetRandomBlessing();
                }
                else
                {
                    if (rerolls > 0)
                    {
                        GetRandomBlessing();
                        rerolls--;
                        Main.npcChatText = GetChat();
                    }
                    else
                    {
                        Main.npcChatText = GetChat() + "\n\nCome back tommorrow for more rerolls";
                    }
                }
            }
            else
            {
                if (!firstButton)
                {
                    Vector2 teleportPos = new Vector2((Common.ModSystems.WorldSystem.TargonCenterX * 16) - 16, (60 * 16) + (float)(Main.worldSurface * 16));

                    Main.LocalPlayer.Teleport(teleportPos, 1, 0);
                    NetMessage.SendData(MessageID.Teleport, -1, -1, null, 0, Main.LocalPlayer.whoAmI, teleportPos.X, teleportPos.Y, 1, 0, 0);
                }
                else
                {
                    shop = false;
                    Main.npcChatText = GetChat();
                }
            }
        }

        int GetBuffID()
        {
            switch (currentBlessing)
            {
                case 0:
                    return BuffType<SunBlessing>();
                case 1:
                    return BuffType<MoonBlessing>();
                case 2:
                    return BuffType<StarBlessing>();
                case 3:
                    return BuffType<WarBlessing>();
                case 4:
                    return BuffType<ProtectorBlessing>();
                case 5:
                    return BuffType<TwilightBlessing>();
                case 6:
                    return BuffType<TravelerBlessing>();
                case 7:
                    return BuffType<JudicatorBlessing>();
                case 8:
                    return BuffType<ChargerBlessing>();
                case 9:
                    return BuffType<DestroyerBlessing>();
                case 10:
                    return BuffType<GreatBeyondBlessing>();
                case 11:
                    return BuffType<ImmortalFireBlessing>();
                case 12:
                    return BuffType<MessengerBlessing>();
                case 13:
                    return BuffType<ScourgeBlessing>();
                case 14:
                    return BuffType<SerpentBlessing>();
                case 15:
                    return BuffType<WandererBlessing>();
                default:
                    return BuffType<SunBlessing>();
            }
        }

        void GetRandomBlessing()
        {
            int newBuff;
            while (true)
            {
                newBuff = Main.rand.Next(0, 16);
                if (newBuff != currentBlessing)
                {
                    currentBlessing = newBuff;
                    break;
                }
            }
        }

        public override bool CheckActive()
        {
            return false;
        }

        public override bool CanTownNPCSpawn(int numTownNPCs, int money)
        {
            return false;
        }

        public override bool CheckConditions(int left, int right, int top, int bottom)
        {
            return false;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
                ModContent.GetInstance<TargonPeakBiome>().ModBiomeBestiaryInfoElement,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("At the peak of Mount Targon, the border of Man and God, rests a large slab of stardust infused Targon Granite. This Sigil acts as the physical link between the material realm and the stars, communicating the messages of The Celestials.")
            });
        }
    }
}
