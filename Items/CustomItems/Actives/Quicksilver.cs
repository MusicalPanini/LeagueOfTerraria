using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Actives
{
    public class Quicksilver : Active
    {
        readonly int effectDuration;

        public Quicksilver(int EffectDuration, int Cooldown)
        {
            effectDuration = EffectDuration;
            activeCooldown = Cooldown;
        }

        public override string Tooltip(Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            return TooltipName("QUICKSILVER") + LeagueTooltip.CreateColorString(ActiveSecondaryColor, "Become immune to most debuffs for " + effectDuration + " seconds") +
                 "\n" + LeagueTooltip.CreateColorString(ActiveSubColor, GetScaledCooldown(player) + " second cooldown");
        }

        public override void DoActive(Player player, LeagueItem modItem)
        {
            if (cooldownCount <= 0)
            {
                PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
                player.AddBuff(BuffType<GeneralCleanse>(), effectDuration * 60);
                SetCooldown(player);

                Efx(player);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    PacketHandler.SendActiveEfx(-1, player.whoAmI, player.whoAmI, modItem.Item.type);
            }
        }

        public override void PostPlayerUpdate(Player player)
        {
            base.PostPlayerUpdate(player);
        }

        override public void Efx(Player user)
        {
            Terraria.Audio.SoundEngine.PlaySound(new LegacySoundStyle(2, 29).WithPitchVariance(-0.3f), user.Center);
            for (int j = 0; j < 18; j++)
            {
                Dust dust = Dust.NewDustDirect(new Vector2(Main.rand.Next((int)user.position.X - 8, (int)user.position.X + 8), user.position.Y + 16), user.width, user.height, DustID.AncientLight, 0, -Main.rand.Next(6, 18), 0, new Color(255, 255, 255, 0), Main.rand.Next(Main.rand.Next(2, 3)));
                dust.noGravity = true;
            }
        }
    }
}

