﻿using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class ColdSteel : Passive
    {
        readonly int SlowDuration;
        readonly int radius;
        public ColdSteel(int slowDurationSeconds, int Radius)
        {
            SlowDuration = slowDurationSeconds;
            radius = Radius;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            return TooltipName("COLD STEEL") + LeagueTooltip.CreateColorString(PassiveSecondaryColor, "Chance to slow near by enemies upon taking damage");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            

            base.UpdateAccessory(player, modItem);
        }

        public override void OnHitByNPC(NPC npc, ref int damage, ref bool crit, Player player, ModItem modItem)
        {
            DoThing(player, modItem);
            base.OnHitByNPC(npc, ref damage, ref crit, player, modItem);
        }

        public override void OnHitByProjectile(NPC npc, ref int damage, ref bool crit, Player player, ModItem modItem)
        {
            DoThing(player, modItem);
            base.OnHitByProjectile(npc, ref damage, ref crit, player, modItem);
        }

        public override void OnHitByProjectile(Projectile proj, ref int damage, ref bool crit, Player player, ModItem modItem)
        {
            DoThing(player, modItem);
            base.OnHitByProjectile(proj, ref damage, ref crit, player, modItem);
        }

        public void DoThing(Player player, ModItem modItem)
        {
            if (Main.rand.Next(0, 5) == 0)
            {
                Efx(player);
                SendEfx(player, modItem);

                Targeting.GiveNPCsInRangeABuff(player.MountedCenter, radius, BuffType<Buffs.Slowed>(), SlowDuration * 60, true, true);
            }
        }

        override public void Efx(Player user)
        {
            Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 30), user.Center);

            for (int i = 0; i < 30; i++)
            {
                Dust dust = Dust.NewDustDirect(user.position, user.width, user.height, DustID.IceRod, 0, 0, 0, default, 2f);
                dust.noGravity = true;
                dust.velocity *= 5f;

                dust = Dust.NewDustDirect(new Vector2(user.position.X, user.position.Y), user.width, user.height, DustID.IceRod, 0f, 0f, 100, default, 1f);
                dust.velocity *= 3f;
            }

            TerraLeague.DustBorderRing(radius, user.MountedCenter, 67, default, 2f);
        }
    }
}
