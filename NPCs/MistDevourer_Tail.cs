using Microsoft.Xna.Framework;
using TerraLeague.Items;
using Terraria;
using Terraria.ID;
using TerraLeague.Gores;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TerraLeague.Items.Banners;

namespace TerraLeague.NPCs
{
    public class MistDevourer_Tail
        : WormClass
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mist Devourer");
        }
        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.SeekerTail);
            NPC.aiStyle = -1;
            minLength = 24;
            maxLength = 30;
            headType = NPCType<MistDevourer_Head>();
            bodyType = NPCType<MistDevourer_Body>();
            tailType = NPCType<MistDevourer_Tail>();
            speed = 10f;
            turnSpeed = 0.06f;

            tail = true;

            Banner = headType;
            BannerItem = ItemType<MistDevourerBanner>();
            base.SetDefaults();
        }

        public override bool PreAI()
        {
            Lighting.AddLight(NPC.Center, new Color(5, 245, 150).ToVector3());

            return base.PreAI();
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (Main.rand.Next(0, 4) == 0)
                target.AddBuff(BuffID.Confused, 5*60);
            base.OnHitPlayer(target, damage, crit);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life > 0)
            {
                int count = 0;
                while ((double)count < damage / (double)NPC.lifeMax * 50.0)
                {
                    Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Cloud, 0f, 0f, 50, default, 1.5f);
                    dust.velocity *= 2f;
                    dust.noGravity = true;
                    count++;
                }
            }
            else
            {
                for (int i = 0; i < 20; i++)
                {
                    Dust dust;
                    if (i > 10)
                        dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Cloud, 0f, 0f, 50, default, 1.5f);
                    else
                        dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Cloud, 0f, 0f, 50, new Color(5, 245, 150), 1.5f);
                    dust.velocity *= 2f;
                    dust.noGravity = true;
                }

                Gore gore = Gore.NewGoreDirect(new Vector2(NPC.position.X, NPC.position.Y - 10f), new Vector2((float)hitDirection, 0f), GoreType<MistPuff_1>(), NPC.scale);
                gore.velocity *= 0.3f;
                gore = Gore.NewGoreDirect(new Vector2(NPC.position.X, NPC.position.Y + (float)(NPC.height / 2) - 15f), new Vector2((float)hitDirection, 0f), GoreType<MistPuff_2>(), NPC.scale);
                gore.velocity *= 0.3f;
                gore = Gore.NewGoreDirect(new Vector2(NPC.position.X, NPC.position.Y + (float)NPC.height - 20f), new Vector2((float)hitDirection, 0f), GoreType<MistPuff_3>(), NPC.scale);
                gore.velocity *= 0.3f;
            }
            base.HitEffect(hitDirection, damage);
        }

    }
}
