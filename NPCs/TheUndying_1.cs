using Microsoft.Xna.Framework;
using TerraLeague.Items;
using Terraria;
using Terraria.ID;
using TerraLeague.Gores;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TerraLeague.Items.Accessories;
using TerraLeague.Items.Banners;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.Bestiary;
using TerraLeague.Biomes;

namespace TerraLeague.NPCs
{
    public class TheUndying_1 : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Undying");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.BlueArmoredBonesMace];
        }
        public override void SetDefaults()
        {
            NPC.width = 18;
            NPC.height = 40;
            NPC.aiStyle = 3;
            NPC.damage = 28;
            NPC.defense = 9;
            NPC.lifeMax = 40;
            NPC.HitSound = SoundID.NPCHit2;
            NPC.DeathSound = SoundID.NPCDeath2;
            NPC.value = 100f;
            AIType = NPCID.BlueArmoredBonesMace;
            AnimationType = NPCID.BlueArmoredBonesMace;
            NPC.scale = 1f;
            base.SetDefaults();
            Banner = NPC.type;
            BannerItem = ItemType<UndyingBanner>();
        }

        public override bool PreAI()
        {
            Lighting.AddLight(NPC.Center, new Color(5, 245, 150).ToVector3());

            return base.PreAI();
        }

        public override void AI()
        {
            base.AI();
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.player.GetModPlayer<PLAYERGLOBAL>().zoneBlackMist)
                return SpawnCondition.OverworldNightMonster.Chance;
            return 0;
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
                    Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Cloud, 0f, 0f, 0, new Color(5, 245, 150), 1.5f);
                    dust.velocity *= 2f;
                    dust.noGravity = true;
                    count++;
                }
            }
            else
            {
                for (int i = 0; i < 20; i++)
                {
                    Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Cloud, 0f, 0f, 0, new Color(5, 245, 150), 1.5f);
                    dust.velocity *= 2f;
                    dust.noGravity = true;
                }

                Gore.NewGore(NPC.Center, NPC.velocity / 2, GoreType<TheUndying_1_1>(), 1f);
                Gore.NewGore(NPC.Top, NPC.velocity / 2, GoreType<TheUndying_1_2>(), 1f);
                Gore.NewGore(NPC.Bottom, NPC.velocity / 2, GoreType<TheUndying_1_3>(), 1f);

                Gore gore = Gore.NewGoreDirect(new Vector2(NPC.position.X, NPC.position.Y - 10f), new Vector2((float)hitDirection, 0f), GoreType<MistPuff_1>(), NPC.scale);
                gore.velocity *= 0.3f;
                gore = Gore.NewGoreDirect(new Vector2(NPC.position.X, NPC.position.Y + (float)(NPC.height / 2) - 15f), new Vector2((float)hitDirection, 0f), GoreType<MistPuff_2>(), NPC.scale);
                gore.velocity *= 0.3f;
                gore = Gore.NewGoreDirect(new Vector2(NPC.position.X, NPC.position.Y + (float)NPC.height - 20f), new Vector2((float)hitDirection, 0f), GoreType<MistPuff_3>(), NPC.scale);
                gore.velocity *= 0.3f;
            }
            base.HitEffect(hitDirection, damage);
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemType<DamnedSoul>(), 1));
            npcLoot.Add(ItemDropRule.NormalvsExpert(ItemType<Nightbloom>(), 250, 125));
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime,
                ModContent.GetInstance<BlackMistBiome>().ModBiomeBestiaryInfoElement,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("The old martyr remembered rallying his comrades to their doom. But it had been years since he could remember why...")
            });
        }
    }
}
