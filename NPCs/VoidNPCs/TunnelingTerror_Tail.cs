using Microsoft.Xna.Framework;
using TerraLeague.Items;
using Terraria;
using Terraria.ID;
using TerraLeague.Gores;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TerraLeague.Items.Banners;

namespace TerraLeague.NPCs.VoidNPCs
{
    public class TunnelingTerror_Tail : WormClass
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tunneling Terror");
        }
        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.DuneSplicerTail);
            NPC.aiStyle = -1;
            headType = NPCType<TunnelingTerror_Head>();
            bodyType = NPCType<TunnelingTerror_Body>();
            tailType = NPCType<TunnelingTerror_Tail>();

            tail = true;

            //Banner = headType;
            //BannerItem = ItemType<MistDevourerBanner>();
            base.SetDefaults();
        }

        public override bool PreAI()
        {
            return base.PreAI();
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0;
        }

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            target.GetModPlayer<PLAYERGLOBAL>().AddVoidInfluence(100, false);

            base.ModifyHitPlayer(target, ref damage, ref crit);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                //for (int k = 0; k < 20; k++)
                //{
                //    Dust.NewDust(new Vector2(NPC.Center.X, NPC.Center.Y - 15), 1, 1, DustID.BlueCrystalShard, 0, 0, 0, default, 1.2f);
                //}

                Gore.NewGore(NPC.position, NPC.velocity, GoreType<TerrorTail>(), NPC.scale);
            }
            base.HitEffect(hitDirection, damage);
        }

    }
}
