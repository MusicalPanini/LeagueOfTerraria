using Microsoft.Xna.Framework;
using TerraLeague.Items;
using Terraria;
using Terraria.ID;
using TerraLeague.Gores;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace TerraLeague.NPCs.TargonBoss
{
    public class KayleAttack : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starfire Spellblade");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.EnchantedSword];
        }
        public override void SetDefaults()
        {
            NPC.width = 48;
            NPC.height = 48;
            NPC.aiStyle = 23;
            NPC.damage = 20;
            NPC.defense = 0;
            NPC.lifeMax = 15;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.value = 0;
            NPC.buffImmune[20] = true;
            NPC.buffImmune[24] = true;
            NPC.buffImmune[39] = true;
            NPC.knockBackResist = 0.4f;
            AnimationType = NPCID.EnchantedSword;
            NPC.SpawnedFromStatue = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
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

            Lighting.AddLight(NPC.Center, TargonBossNPC.KayleColor.ToVector3());

            return base.PreAI();
        }

        public override void AI()
        {
            Lighting.AddLight(new Vector2((int)((NPC.position.X + (float)(NPC.width / 2)) / 16f), (int)((NPC.position.Y + (float)(NPC.height / 2)) / 16f)), 0.3f, 0.3f, 0.05f);

            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead)
            {
                NPC.TargetClosest(true);
            }
            if (NPC.ai[0] == 0f)
            {
                //float num298 = 9f;
                //Vector2 vector31 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
                //float num299 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - vector31.X;
                //float num300 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - vector31.Y;
                //float num301 = (float)Math.Sqrt((double)(num299 * num299 + num300 * num300));
                //float num302 = num301;
                //num301 = num298 / num301;
                //num299 *= num301;
                //num300 *= num301;
                //NPC.velocity.X = num299;
                //NPC.velocity.Y = num300;
                NPC.velocity = TerraLeague.CalcVelocityToPoint(NPC.Center, Main.player[NPC.target].MountedCenter, 16);
                NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver4;
                NPC.ai[0] = 1f;
                NPC.ai[1] = 0f;
                NPC.netUpdate = true;
            }
            else if (NPC.ai[0] == 1f)
            {
                NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver4;
                if (NPC.justHit)
                {
                    NPC.ai[0] = 2f;
                    NPC.ai[1] = 0f;
                }
                NPC.velocity *= 0.99f;
                NPC.ai[1] += 1f;

                if (NPC.ai[1] >= 200f)
                {
                    NPC.netUpdate = true;
                    NPC.ai[0] = 2f;
                    NPC.ai[1] = 0f;
                    NPC.velocity.X = 0f;
                    NPC.velocity.Y = 0f;
                }
            }
            else
            {
                if (NPC.justHit)
                {
                    NPC.ai[0] = 2f;
                    NPC.ai[1] = 0f;
                }
                NPC.velocity *= 0.99f;
                NPC.ai[1] += 1f;
                float num303 = NPC.ai[1] / 120f;
                num303 = 0.1f + num303 * 0.4f;
                NPC.rotation += num303 * (float)NPC.direction;
                if (NPC.ai[1] >= 120f)
                {
                    NPC.netUpdate = true;
                    NPC.ai[0] = 0f;
                    NPC.ai[1] = 0f;
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
                int num437 = 0;
                while ((double)num437 < damage / (double)NPC.lifeMax * 50.0)
                {
                    int num438 = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Smoke, 0f, 0f, 0, default, 1.5f);
                    Main.dust[num438].noGravity = true;
                    int num5 = num437;
                    num437 = num5 + 1;
                }
            }
            else
            {
                int num5;
                for (int num439 = 0; num439 < 20; num439 = num5 + 1)
                {
                    int num440 = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Smoke, 0f, 0f, 0, default, 1.5f);
                    Dust dust115 = Main.dust[num440];
                    Dust dust2 = dust115;
                    dust2.velocity *= 2f;
                    Main.dust[num440].noGravity = true;
                    num5 = num439;
                }
                int num441 = Gore.NewGore(new Vector2(NPC.position.X, NPC.position.Y + (float)(NPC.height / 2) - 10f), new Vector2((float)Main.rand.Next(-2, 3), (float)Main.rand.Next(-2, 3)), 61, NPC.scale);
                Gore gore5 = Main.gore[num441];
                Gore gore2 = gore5;
                gore2.velocity *= 0.5f;
                num441 = Gore.NewGore(new Vector2(NPC.position.X, NPC.position.Y + (float)(NPC.height / 2) - 10f), new Vector2((float)Main.rand.Next(-2, 3), (float)Main.rand.Next(-2, 3)), 61, NPC.scale);
                gore5 = Main.gore[num441];
                gore2 = gore5;
                gore2.velocity *= 0.5f;
                num441 = Gore.NewGore(new Vector2(NPC.position.X, NPC.position.Y + (float)(NPC.height / 2) - 10f), new Vector2((float)Main.rand.Next(-2, 3), (float)Main.rand.Next(-2, 3)), 61, NPC.scale);
                gore5 = Main.gore[num441];
                gore2 = gore5;
                gore2.velocity *= 0.5f;
            }

            base.HitEffect(hitDirection, damage);
        }
    }
}
