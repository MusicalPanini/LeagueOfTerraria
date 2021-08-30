using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class MagicBolt : Passive
    {
        readonly int extraDamage;
        readonly int magicMinionScaling;

        public MagicBolt(int Damage, int MagicMinionScaling, int Cooldown, LeagueItem legItem) : base(legItem)
        {
            extraDamage = Damage;
            magicMinionScaling = MagicMinionScaling;
            passiveCooldown = Cooldown;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            string scaleText;
            if (modPlayer.SUM > modPlayer.MAG)
                scaleText = LeagueTooltip.TooltipValue(extraDamage, false, "", new Tuple<int, ScaleType>(magicMinionScaling, ScaleType.Summon));
            else
                scaleText = LeagueTooltip.TooltipValue(extraDamage, false, "", new Tuple<int, ScaleType>(magicMinionScaling, ScaleType.Magic));

            return TooltipName("Revved") + LeagueTooltip.CreateColorString(PassiveSecondaryColor, "Your next magic or minion attack will deal ") + scaleText + LeagueTooltip.CreateColorString(PassiveSecondaryColor, " extra damage") +
                "\n" + LeagueTooltip.CreateColorString(PassiveSubColor, GetScaledCooldown(player) + " second cooldown. Damage scales with the highest of either MAG or SUM");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            base.UpdateAccessory(player, modItem);
        }

        public override void NPCHitWithProjectile(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection, ref int OnHitDamage, Player player)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (cooldownCount <= 0 && (proj.DamageType == DamageClass.Magic || proj.DamageType == DamageClass.Summon))
            {
                damage += extraDamage + (int)(Math.Max(modPlayer.SUM, modPlayer.MAG) * magicMinionScaling / 100d);
                Efx(player, target);
                SendEfx(player, target, modItem);
                SetCooldown(player);
            }

            base.NPCHitWithProjectile(proj, target, ref damage, ref knockback, ref crit, ref hitDirection, ref OnHitDamage, player);
        }

        public override void PostPlayerUpdate(Player player)
        {
            base.PostPlayerUpdate(player);
        }

        override public void Efx(Player player, NPC HitNPC)
        {
            Terraria.Audio.SoundEngine.PlaySound(new LegacySoundStyle(2, 30).WithPitchVariance(0.3f), HitNPC.position);
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(HitNPC.position, HitNPC.width, HitNPC.height, DustID.YellowStarDust, 0, 0, 0, new Color(0, 0, 255, 150), 1.5f);
            }
        }
    }
}
