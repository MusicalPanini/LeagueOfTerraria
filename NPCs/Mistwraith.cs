using Microsoft.Xna.Framework;
using TerraLeague.Items;
using Terraria;
using Terraria.ID;
using TerraLeague.Gores;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TerraLeague.Items.Banners;
using Terraria.ModLoader.Utilities;

namespace TerraLeague.NPCs
{
    public class Mistwraith : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mistwraith");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.CursedSkull];
        }
        public override void SetDefaults()
        {
            NPC.width = 44;
            NPC.height = 44;
            NPC.aiStyle = 10;
            NPC.damage = 50;
            NPC.defense = 20;
            NPC.lifeMax = 1000;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.HitSound = SoundID.NPCHit54;
            NPC.DeathSound = SoundID.NPCHit52;
            AnimationType = NPCID.CursedSkull;
            NPC.value = 500;
            NPC.knockBackResist = 0.3f;
            NPC.npcSlots = 3;
            NPC.scale = 1f;
            NPC.buffImmune[20] = true;
            NPC.buffImmune[24] = true;
            NPC.buffImmune[39] = true;
            Banner = NPC.type;
            BannerItem = ItemType<MistwraithBanner>();
            base.SetDefaults();
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.player.GetModPlayer<PLAYERGLOBAL>().zoneBlackMist && Main.hardMode)
                return SpawnCondition.OverworldNightMonster.Chance * 0.1f;
            return 0;
        }

        public override bool PreAI()
        {
            Lighting.AddLight(NPC.Center, new Color(5, 245, 150).ToVector3());
            for (int i = 0; i < 4; i++)
            {
                Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Cloud, 0f, 0f, 100, new Color(67, 248, 175), Main.rand.Next(1,5));
                dust.alpha = 200;
                dust.noGravity = true;
                dust.velocity *= 0.1f;
                dust.velocity += NPC.velocity * 0.1f;
                dust.position.X -= NPC.velocity.X / 3f * (float)i;
                dust.position.Y -= NPC.velocity.Y / 3f * (float)i;
            }


            return base.PreAI();
        }

        public override void AI()
        {
            base.AI();
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
                    Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Cloud, 0f, 0f, 50, new Color(5, 245, 150), 1.5f);
                    dust.velocity *= 2f;
                    dust.noGravity = true;
                    count++;
                }
            }
            else
            {
                for (int i = 0; i < 20; i++)
                {
                    Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Cloud, 0f, 0f, 50, new Color(5, 245, 150), 1.5f);
                    dust.velocity *= 2f;
                    dust.noGravity = true;
                }
                Gore gore = Gore.NewGoreDirect(new Vector2(NPC.position.X, NPC.position.Y - 10f), new Vector2((float)hitDirection, 0f), GoreType<MistPuff_1>(), NPC.scale * 1.5f);
                gore.velocity *= 0.3f;
                gore = Gore.NewGoreDirect(new Vector2(NPC.position.X, NPC.position.Y + (float)(NPC.height / 2) - 15f), new Vector2((float)hitDirection, 0f), GoreType<MistPuff_2>(), NPC.scale * 1.5f);
                gore.velocity *= 0.3f;
                gore = Gore.NewGoreDirect(new Vector2(NPC.position.X, NPC.position.Y + (float)NPC.height - 20f), new Vector2((float)hitDirection, 0f), GoreType<MistPuff_3>(), NPC.scale * 1.5f);
                gore.velocity *= 0.3f;
                gore = Gore.NewGoreDirect(new Vector2(NPC.position.X, NPC.position.Y - 10f), new Vector2((float)hitDirection, 0f), GoreType<MistPuff_1>(), NPC.scale);
                gore.velocity *= 1.2f;
                gore = Gore.NewGoreDirect(new Vector2(NPC.position.X, NPC.position.Y + (float)(NPC.height / 2) - 15f), new Vector2((float)hitDirection, 0f), GoreType<MistPuff_2>(), NPC.scale);
                gore.velocity *= 1.2f;
                gore = Gore.NewGoreDirect(new Vector2(NPC.position.X, NPC.position.Y + (float)NPC.height - 20f), new Vector2((float)hitDirection, 0f), GoreType<MistPuff_3>(), NPC.scale);
                gore.velocity *= 1.2f;

                for (int i = 0; i < 6; i++)
                {
                    int mistling = NPC.NewNPC((int)NPC.position.X + (i * 22), (int)NPC.Center.Y, NPCType<Mistling>());
                    Main.npc[mistling].velocity = new Vector2(0,-6).RotatedBy((MathHelper.Pi * (-2.5 + i))/6f);
                }
            }

            base.HitEffect(hitDirection, damage);
        }
    }
}
