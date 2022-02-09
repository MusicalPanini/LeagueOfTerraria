using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Biomes;
using TerraLeague.Items.Banners;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.NPCs
{
    public class MarbleSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Marble Slime");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.BlueSlime];

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            { // Influences how the NPC looks in the Bestiary
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }
        public override void SetDefaults()
        {
            NPC.width = 32;
            NPC.height = 24;
            NPC.aiStyle = 1;
            NPC.damage = 12;
            NPC.defense = 9;
            NPC.lifeMax = 55;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.05f;
            NPC.value = 25f;
            AIType = NPCID.BlueSlime;
            AnimationType = NPCID.BlueSlime;
            base.SetDefaults();
            NPC.scale = 1.3f;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<MarbleSlimeBanner>();
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            //if (spawnInfo.player.GetModPlayer<PLAYERGLOBAL>().zoneSurfaceMarble)
            //    return 1;

            return 0;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (Main.rand.Next(0, 12) == 0 && !Main.expertMode)
            {
                target.AddBuff(BuffID.Stoned, 60);
            }
            else if (Main.rand.Next(0, 6) == 0 && Main.expertMode)
            {
                target.AddBuff(BuffID.Stoned, 60);
            }
            base.OnHitPlayer(target, damage, crit);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            for (int k = 0; k < 60; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.t_Slime, hitDirection, -2, 150, new Color(50, 50, 50), 1f);
            }

            base.HitEffect(hitDirection, damage);
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Gel, 1, 1, 3));
            // Main.item[item].color = new Color(255, 255, 255);

            base.ModifyNPCLoot(npcLoot);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime,
                ModContent.GetInstance<SurfaceMarbleBiome>().ModBiomeBestiaryInfoElement,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("These Slimes being too goopy were unable to be petrified like the forest it resideds in. Instead it is able to petrify its prey before digesting it.")
            });
        }
    }
}
