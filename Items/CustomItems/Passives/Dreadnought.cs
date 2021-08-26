using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Buffs;
using TerraLeague.Items.CompleteItems;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class Dreadnought : Passive
    {
        readonly float statModifier;

        public Dreadnought(float StatIncreaseModifier)
        {
            statModifier = StatIncreaseModifier;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            return LeagueTooltip.CreateColorString(PassiveSubColor, "Moving will generate charge")
                + "\n" + TooltipName("Shipwrecker") + LeagueTooltip.CreateColorString(PassiveSecondaryColor, "Gain 0.25% movespeed per 1% charge")
                + "\n" + LeagueTooltip.CreateColorString(PassiveSecondaryColor, "At full charge your next attack will deal ") + LeagueTooltip.TooltipValue(0, false, "", new System.Tuple<int, ScaleType>(200, ScaleType.Armor)) + LeagueTooltip.CreateColorString(PassiveSecondaryColor, " extra damage");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            base.UpdateAccessory(player, modItem);
        }

        public override void NPCHit(Item item, NPC target, ref int damage, ref float knockback, ref bool crit, ref int OnHitDamage, Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (modPlayer.crushingBlows)
            {
                modPlayer.meleeFlatDamage += 2 * modPlayer.armorLastStep;

                Efx(player, target);
                SendEfx(player, target, modItem);

                passiveStat = 0;
            }

            base.NPCHit(item, target, ref damage, ref knockback, ref crit, ref OnHitDamage, player, modItem);
        }

        public override void NPCHitWithProjectile(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection, ref int OnHitDamage, Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (modPlayer.crushingBlows)
            {
                modPlayer.meleeFlatDamage += 2 * modPlayer.armorLastStep;
                modPlayer.rangedFlatDamage += 2 * modPlayer.armorLastStep;
                modPlayer.magicFlatDamage += 2 * modPlayer.armorLastStep;
                modPlayer.minionFlatDamage += 2 * modPlayer.armorLastStep;

                Efx(player, target);
                SendEfx(player, target, modItem);

                passiveStat = 0;
            }

            base.NPCHitWithProjectile(proj, target, ref damage, ref knockback, ref crit, ref hitDirection, ref OnHitDamage, player, modItem);
        }

        public override void PostPlayerUpdate(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            float stat;

            if (player.velocity.X < 0)
                stat = -player.velocity.X * statModifier;
            else
                stat = player.velocity.X * statModifier;

            AddStat(player, 100, stat);

            player.moveSpeed += passiveStat * 0.0025f;

            if (passiveStat >= 100)
            {
                if (player.velocity.Length() > 0)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        player.armorEffectDrawShadow = true;
                        Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.Blood, 0, 0, 0, new Color(255, 0, 0, 150), 0.2f * player.velocity.Length());
                        dust.velocity *= 0;
                        dust.noGravity = true;
                    }
                }
                player.AddBuff(BuffType<CrushingBlow>(), 2);
            }
            else
                player.ClearBuff(BuffType<CrushingBlow>());

            base.PostPlayerUpdate(player, modItem);
        }

        public override void Efx(Player user, NPC effectedNPC)
        {
            Terraria.Audio.SoundEngine.PlaySound(new LegacySoundStyle(2, 38).WithVolume(1.2f), effectedNPC.position);
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDustDirect(effectedNPC.position, effectedNPC.width, effectedNPC.height, DustID.Blood, 0, 0, 0, new Color(255, 0, 0, 150), 1.5f);
            }
        }
    }
}
