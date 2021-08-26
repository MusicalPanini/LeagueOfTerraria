using System;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class Crush : Passive
    {
        public override string Tooltip(Player player, ModItem modItem)
        {
            return TooltipName("CARVE") + LeagueTooltip.CreateColorString(PassiveSecondaryColor, "Melee attacks will reduce enemy defence by 6% stacking 6 times.")
                + "\n" + LeagueTooltip.CreateColorString(PassiveSecondaryColor, "You deal ") + LeagueTooltip.TooltipValue(0, false, "", new Tuple<int, ScaleType>(30, ScaleType.Melee)) 
                + LeagueTooltip.CreateColorString(PassiveSecondaryColor, " melee On Hit damage to enemies at max stacks.");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            base.UpdateAccessory(player, modItem);
        }

        public override void NPCHit(Item item, NPC target, ref int damage, ref float knockback, ref bool crit, ref int OnHitDamage, Player player, ModItem modItem)
        {
            int stacks = target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().CleavedStacks;

            if (stacks < 5)
            {
                target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().CleavedStacks++;

                if (stacks == 4)
                {
                    Microsoft.Xna.Framework.Vector2 position = new Microsoft.Xna.Framework.Vector2
                    (
                        target.position.X  - (target.width * 0.5f) - (System.Math.Min((System.Math.Min(target.width, target.height) / 28f) - 0.2f, 1.2f) / 2),
                        target.position.Y
                    );

                    for (int i = 0; i < 10; i++)
                    {
                        Dust.NewDustDirect(position, 4, 4, 10, -3, -0.5f, 0);
                        Dust dust = Dust.NewDustDirect(position, 4, 4, 169, -8, -0.5f, 0);
                    }

                    TerraLeague.PlaySoundWithPitch(target.Center, 13, 0, -0.5f);

                }

                if (Main.netMode == NetmodeID.MultiplayerClient)
                    TerraLeagueNPCsGLOBAL.PacketHandler.SendSyncStats(Main.LocalPlayer.whoAmI, -1, 2, target.whoAmI, target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().CleavedStacks);
            }
            else
            {
                PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
                OnHitDamage += (int)(modPlayer.MEL * 30 * 0.01);
            }

            target.AddBuff(BuffType<Cleaved>(), 360);

            base.NPCHit(item, target, ref damage, ref knockback, ref crit, ref OnHitDamage, player, modItem);
        }

        public override void NPCHitWithProjectile(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection, ref int OnHitDamage, Player player, ModItem modItem)
        {
            if (proj.DamageType == DamageClass.Melee)
            {
                int stacks = target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().CleavedStacks;

                if (stacks >= 5)
                {
                    PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
                    OnHitDamage += (int)(modPlayer.MEL * 30 * 0.01);
                }
            }

            base.NPCHitWithProjectile(proj, target, ref damage, ref knockback, ref crit, ref hitDirection, ref OnHitDamage, player, modItem);
        }

        public void DoThing(Player player, ModItem modItem)
        {

        }
    }
}
