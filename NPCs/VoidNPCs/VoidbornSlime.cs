using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerraLeague.Items;
using TerraLeague.Items.Banners;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.NPCs.VoidNPCs
{
    public class VoidbornSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Voidborn Slime");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.BlueSlime];
        }
        public override void SetDefaults()
        {
            NPC.width = 44;
            NPC.height = 32;
            NPC.aiStyle = 1;
            NPC.damage = 18;
            NPC.defense = 6;
            NPC.lifeMax = 100;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.05f;
            NPC.value = 100f;
            NPC.alpha = 70;
            AIType = NPCID.BlueSlime;
            AnimationType = NPCID.BlueSlime;
            base.SetDefaults();
            //Banner = NPC.type;
            //BannerItem = ItemType<SoulBoundSlimeBanner>();
        }

        public override bool PreAI()
        {

            return base.PreAI();
        }

        public override void AI()
        {
            base.AI();
        }

        public override void OnKill()
        {
            Projectile.NewProjectileDirect(NPC.GetProjectileSpawnSource(), NPC.Center, NPC.velocity, ProjectileType<VoidbornSlime_CrystalBomb>(), 40, 2, NPC.lastInteraction);

            base.OnKill();
        }

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            target.GetModPlayer<PLAYERGLOBAL>().AddVoidInfluence(100, false);

            base.ModifyHitPlayer(target, ref damage, ref crit);
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            
            base.OnHitPlayer(target, damage, crit);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life > 0)
            {
                int num262 = 0;
                while ((double)num262 < damage / (double)NPC.lifeMax * 100.0)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.t_Slime, (float)hitDirection, -1f, NPC.alpha, Color.Purple, 1f);
                    int num5 = num262;
                    num262 = num5 + 1;
                }
            }
            else
            {
                for (int i = 0; i < 20; i++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.t_Slime, (float)hitDirection, -1f, NPC.alpha, Color.Purple, 1f);
                }
            }

            base.HitEffect(hitDirection, damage);
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemType<VoidFragment>(), 1));
            npcLoot.Add(ItemDropRule.Common(ItemID.Gel, 1, 1, 3));
            base.ModifyNPCLoot(npcLoot);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            int frameHeight = (texture.Height / Main.npcFrameCount[NPC.type]);
            if (frameHeight <= 0)
                frameHeight = 1;

            Main.spriteBatch.Draw
            (
                texture,
                new Vector2
                (
                    NPC.position.X - Main.screenPosition.X + NPC.width * 0.5f,
                    NPC.position.Y - Main.screenPosition.Y + NPC.height * 0.5f + 8
                ),
                new Rectangle(0, (texture.Height / Main.npcFrameCount[NPC.type]) * (NPC.frame.Y / frameHeight), texture.Width, texture.Height / Main.npcFrameCount[NPC.type]),
                Color.White,
                NPC.rotation,
                new Vector2(texture.Width, texture.Width) * 0.5f,
                NPC.scale,
                NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                0f
            );
        }
    }
}
