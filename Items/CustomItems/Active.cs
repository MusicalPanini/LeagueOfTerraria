using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems
{
    public abstract class Active
    {
        public delegate void On_PostPlayerUpdate(Player player);
        public delegate void On_NPCHit(Item item, NPC target, ref int damage, ref float knockback, ref bool crit, ref int onhitdamage, Player player);
        public delegate void On_NPCHitWithProjectile(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection, ref int onhitdamage, Player player);
        public delegate void On_OnHitByNPC(NPC npc, ref int damage, ref bool crit, Player player);
        public delegate void On_OnHitByProjectile(Projectile proj, ref int damage, ref bool crit, Player player);
        public delegate void On_OnHitByProjectileNPC(NPC npc, ref int damage, ref bool crit, Player player);

        public static On_PostPlayerUpdate del_PostPlayerUpdate;
        public static On_NPCHit del_NPCHit;
        public static On_NPCHitWithProjectile del_NPCHitWithProjectile;
        public static On_OnHitByNPC del_OnHitByNPC;
        public static On_OnHitByProjectile del_OnHitByProjectile;
        public static On_OnHitByProjectileNPC del_OnHitByProjectileNPC;

        //public delegate void On_OnKilledNPC(NPC npc, ref int damage, ref bool crit, Player player, LeagueItem leagueItem);

        public static string ActiveMainColor = "ff4d4d";
        public static string ActiveSecondaryColor = "ff8080";
        public static string ActiveSubColor = "cc0000";
        public bool currentlyActive = true;

        public float activeStat = 0;
        public int cooldownCount = 0;
        public int activeCooldown = 0;

        public abstract string Tooltip(Player player, LeagueItem modItem);
        internal ActivePacketHandler PacketHandler = new ActivePacketHandler(5);
        public abstract void DoActive(Player player, LeagueItem modItem);

        public string TooltipName(string name)
        {
            return LeagueTooltip.CreateColorString(ActiveMainColor, "Active: " + name.ToUpper()) + "\n";
        }

        public void AddStat(Player player, ModItem modItem, double maxStat, double statToAdd, bool cannotGoNegative = false)
        {
            int slot = TerraLeague.FindAccessorySlotOnPlayer(player, modItem);

            if (slot != -1)
            {
                player.GetModPlayer<PLAYERGLOBAL>().accessoryStat[slot] += statToAdd;
                if (player.GetModPlayer<PLAYERGLOBAL>().accessoryStat[slot] > maxStat)
                {
                    player.GetModPlayer<PLAYERGLOBAL>().accessoryStat[slot] = maxStat;
                }
                if (player.GetModPlayer<PLAYERGLOBAL>().accessoryStat[slot] < 0 && cannotGoNegative)
                {
                    player.GetModPlayer<PLAYERGLOBAL>().accessoryStat[slot] = 0;
                }
            }
        }

        virtual public void PostPlayerUpdate(Player player)
        {
            if (TerraLeague.noItemCooldowns)
                cooldownCount = 0;
            if (cooldownCount > 0)
                cooldownCount--;
        }

        virtual public void NPCHit(Item item, NPC target, ref int damage, ref float knockback, ref bool crit, ref int HitDamage, Player player)
        {

        }

        virtual public void NPCHitWithProjectile(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection, ref int OnHitDamage, Player player)
        {

        }

        virtual public void OnHitByNPC(NPC npc, ref int damage, ref bool crit, Player player)
        {

        }

        virtual public void OnHitByProjectile(Projectile proj, ref int damage, ref bool crit, Player player)
        {

        }

        virtual public void OnHitByProjectile(NPC npc, ref int damage, ref bool crit, Player player)
        {

        }

        virtual public void Efx(Player user)
        {

        }

        internal int GetScaledCooldown(Player player)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            return (int)(activeCooldown * modPlayer.ItemCdrLastStep);
        }

        internal void SetCooldown(Player player)
        {
            cooldownCount = GetScaledCooldown(player) * 60;
        }
    }
}
