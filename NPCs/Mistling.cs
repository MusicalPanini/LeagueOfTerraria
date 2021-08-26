using Microsoft.Xna.Framework;
using TerraLeague.Gores;
using TerraLeague.Items;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.NPCs
{
    public class Mistling : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mistling");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.DungeonSpirit];
        }
        public override void SetDefaults()
        {
            NPC.width = 24;
            NPC.height = 24;
            NPC.aiStyle = 2;
            NPC.damage = 35;
            NPC.defense = 10;
            NPC.lifeMax = 300;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.HitSound = SoundID.NPCHit54;
            NPC.DeathSound = SoundID.NPCHit52;
            NPC.aiStyle = NPCID.DemonEye;
            AnimationType = NPCID.DungeonSpirit;
            NPC.value = 100;
            NPC.knockBackResist = 0.2f;
            NPC.scale = 1f;
            NPC.buffImmune[20] = true;
            NPC.buffImmune[24] = true;
            NPC.buffImmune[39] = true;
            base.SetDefaults();
        }

        public override bool PreAI()
        {
            Lighting.AddLight(NPC.Center, new Color(5, 245, 150).ToVector3());

            for (int i = 0; i < 3; i++)
            {
                Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Cloud, 0f, 0f, 100, new Color(67, 248, 175), Main.rand.Next(1, 3));
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
            NPC.rotation = NPC.velocity.ToRotation() - MathHelper.PiOver2;
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

                Gore gore = Gore.NewGoreDirect(new Vector2(NPC.position.X, NPC.position.Y), new Vector2((float)hitDirection, 0f), GoreType<MistPuff_1>(), NPC.scale);
                gore.velocity *= 0.3f;
            }
            base.HitEffect(hitDirection, damage);
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemType<DamnedSoul>()));
            base.ModifyNPCLoot(npcLoot);
        }
    }
}
