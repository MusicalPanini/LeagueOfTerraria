using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs.TargonBoss;
using static Terraria.ModLoader.ModContent;
using TerraLeague.Mounts;

namespace TerraLeague.Common.GlobalNPCs
{
    public class GLOBALTargonFight : GlobalNPC
    {
        public override bool? CanBeHitByItem(NPC npc, Player player, Item item)
        {
            if (npc.type == NPCType<TargonBossNPC>() ||
                npc.type == NPCType<TargonBoss_Gem>() ||
                npc.type == NPCType<Star_Diana>() ||
                npc.type == NPCType<Star_Kayle>() ||
                npc.type == NPCType<Star_Leona>() ||
                npc.type == NPCType<Star_Morg>() ||
                npc.type == NPCType<Star_Panth>() ||
                npc.type == NPCType<Star_Taric>() ||
                npc.type == NPCType<Star_Zoe>() ||
                npc.type == NPCType<KayleAttack>())
            {
                if ((player.mount.Type != MountType<TargonBossFlight>() || !player.mount.Active) && NPC.CountNPCS(NPCType<TargonBossNPC>()) > 0)
                {
                    return false;
                }

            }

            return base.CanBeHitByItem(npc, player, item);
        }

        public override bool? CanBeHitByProjectile(NPC npc, Projectile projectile)
        {
            if (npc.type == NPCType<TargonBossNPC>() ||
                npc.type == NPCType<TargonBoss_Gem>() ||
                npc.type == NPCType<Star_Diana>() ||
                npc.type == NPCType<Star_Kayle>() ||
                npc.type == NPCType<Star_Leona>() ||
                npc.type == NPCType<Star_Morg>() ||
                npc.type == NPCType<Star_Panth>() ||
                npc.type == NPCType<Star_Taric>() ||
                npc.type == NPCType<Star_Zoe>() ||
                npc.type == NPCType<KayleAttack>())
            {
                if ((Main.player[projectile.owner].mount.Type != MountType<TargonBossFlight>() || !Main.player[projectile.owner].mount.Active) && NPC.CountNPCS(NPCType<TargonBossNPC>()) > 0)
                {
                    return false;
                }

            }

            return base.CanBeHitByProjectile(npc, projectile);
        }
    }
}
