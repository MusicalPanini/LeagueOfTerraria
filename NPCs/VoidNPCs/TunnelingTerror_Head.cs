using Microsoft.Xna.Framework;
using TerraLeague.Items;
using Terraria;
using Terraria.ID;
using TerraLeague.Gores;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System.IO;
using TerraLeague.Items.Banners;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
using TerraLeague.Biomes;
using TerraLeague.Items.Accessories;

namespace TerraLeague.NPCs.VoidNPCs
{
    public class TunnelingTerror_Head : WormClass
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tunneling Terror");

            var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            { // Influences how the NPC looks in the Bestiary
                CustomTexturePath = "TerraLeague/Textures/Bestiary/NPCs/TunnelingTerror",
                Position = new Vector2(60, 28),
                PortraitPositionXOverride = 16,
                PortraitPositionYOverride = 16
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);
        }
        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.DuneSplicerHead);
            NPC.lifeMax = 625;
            NPC.aiStyle = -1;
            minLength = 24;
            maxLength = 30;
            headType = NPCType<TunnelingTerror_Head>();
            bodyType = NPCType<TunnelingTerror_Body>();
            tailType = NPCType<TunnelingTerror_Tail>();
            speed = 16f;
            turnSpeed = 0.35f;

            //Banner = NPC.type;
            //BannerItem = ItemType<MistDevourerBanner>();
            base.SetDefaults();
        }

        public override bool PreAI()
        {
            return base.PreAI();
        }

        public override void Init()
        {
            head = true;
            base.Init();
        }

        private int attackCounter;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(attackCounter);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            attackCounter = reader.ReadInt32();
        }

        public override void CustomBehavior()
        {
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

                Gore.NewGore(NPC.position, NPC.velocity, GoreType<Gores.TerrorHead>());
            }
            base.HitEffect(hitDirection, damage);
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemType<VoidbornFlesh>(), 1, 1));
            npcLoot.Add(ItemDropRule.NormalvsExpert(ItemType<VoidStone>(), 300, 150));
        }

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            target.GetModPlayer<PLAYERGLOBAL>().AddVoidInfluence(100, false);

            base.ModifyHitPlayer(target, ref damage, ref crit);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
                ModContent.GetInstance<VoidBiome>().ModBiomeBestiaryInfoElement,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("A colossal worm reponsible for digging main tunnels for the Xer'Sai. This is only by happenstance though as its main goal is to consume living matter to add to its already large mass")
            });
        }
    }
}
