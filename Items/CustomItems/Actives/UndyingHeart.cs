using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace TerraLeague.Items.CustomItems.Actives
{
    public class UndyingHeart : Active
    {
        readonly int MaxLifeScaling;

        public UndyingHeart(int maxlifeScaling, int Cooldown)
        {
            MaxLifeScaling = maxlifeScaling.GetIfLower(1);
            activeCooldown = Cooldown;
        }

        public override string Tooltip(Player player, LeagueItem modItem)
        {
            return TooltipName("Undying Heart") + LeagueTooltip.CreateColorString(ActiveSecondaryColor, "Reduce your life to 1 and gain invinciblity for ") 
                + LeagueTooltip.TooltipValue(0, false, "", new System.Tuple<int, ScaleType>(MaxLifeScaling, ScaleType.MaxLife)) + LeagueTooltip.CreateColorString(ActiveSecondaryColor, " seconds") +
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
                player.statLife = 1 + modPlayer.GetTotalShield(); 
                player.AddBuff(BuffID.ShadowDodge, (int)(60 * modPlayer.GetRealHeathWithoutShield(true) * MaxLifeScaling * 0.01f));
                SetCooldown(player);
            }
        }

        public override void PostPlayerUpdate(Player player)
        {
            base.PostPlayerUpdate(player);
        }

        public override void Efx(Player user)
        {
            //for (int i = 0; i < 10; i++)
            //{
            //    float X = Main.rand.NextFloat(user.Left.X - 16, user.Right.X + 16);
            //    float Y = Main.rand.NextFloat(user.Top.Y - 16, user.Bottom.Y -16);
            //    Gore.NewGoreDirect(new Microsoft.Xna.Framework.Vector2(X, Y), new Microsoft.Xna.Framework.Vector2(Main.rand.NextFloat(-10, 10), 10), 1248);
            //    Gore.NewGoreDirect(new Microsoft.Xna.Framework.Vector2(X, Y), Microsoft.Xna.Framework.Vector2.Zero, GoreID.Smoke1);
            //}
            TerraLeague.PlaySoundWithPitch(user.MountedCenter, 2, 117, 0.5f);
        }
    }
}

