using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace TerraLeague.Items.CustomItems.Actives
{
    public class PoltergeistsAscension : Active
    {
        readonly int effectDuration;

        public PoltergeistsAscension(int EffectDuration, int Cooldown)
        {
            effectDuration = EffectDuration;
            activeCooldown = Cooldown;
        }

        public override string Tooltip(Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            return TooltipName("Wraith step") + LeagueTooltip.CreateColorString(ActiveSecondaryColor, "Gain a burst of movement speed for " + effectDuration + " seconds") +
                 "\n" + LeagueTooltip.CreateColorString(ActiveSubColor, GetScaledCooldown(player) + " second cooldown");
        }

        public override void DoActive(Player player, LeagueItem modItem)
        {
            if (cooldownCount <= 0)
            {
                PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

                Efx(player);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    PacketHandler.SendActiveEfx(-1, player.whoAmI, player.whoAmI, modItem.Item.type);

                player.AddBuff(BuffID.Swiftness, effectDuration * 60);
                SetCooldown(player);
            }
        }

        public override void PostPlayerUpdate(Player player)
        {
            base.PostPlayerUpdate(player);
        }

        public override void Efx(Player user)
        {
            for (int i = 0; i < 10; i++)
            {
                float X = Main.rand.NextFloat(user.Left.X - 16, user.Right.X + 16);
                float Y = Main.rand.NextFloat(user.Top.Y - 16, user.Bottom.Y -16);
                Gore.NewGoreDirect(new Microsoft.Xna.Framework.Vector2(X, Y), new Microsoft.Xna.Framework.Vector2(Main.rand.NextFloat(-10, 10), 10), 1248);
                Gore.NewGoreDirect(new Microsoft.Xna.Framework.Vector2(X, Y), Microsoft.Xna.Framework.Vector2.Zero, GoreID.Smoke1);
            }
            TerraLeague.PlaySoundWithPitch(user.MountedCenter, 2, 117, 0.5f);
        }
    }
}

