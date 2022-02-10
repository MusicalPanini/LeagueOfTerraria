using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.SummonerSpells
{
    public class FlashRune : SummonerSpell
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flash Rune");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override string GetIconTexturePath()
        {
            return "TerraLeague/Items/SummonerSpells/Flash";
        }

        public override string GetSpellName()
        {
            return "Flash";
        }

        public override int GetRawCooldown()
        {
            return 90;
        }
        public override string GetTooltip()
        {
            return "Blink to your cursor";
        }

        public override void DoEffect(Player player, int spellSlot)
        {
            bool pathBlocked = false;
            for (int x = (int)(Main.MouseWorld.X / 16) - 1; x < (int)((Main.mouseX + Main.screenPosition.X) / 16) + 1; x++)
            {
                for (int y = (int)((Main.mouseY + Main.screenPosition.Y) / 16) - 1; y <= (int)((Main.mouseY + Main.screenPosition.Y) / 16) + 1; y++)
                {
                    if (Main.tileSolid[Main.tile[x, y].TileType] && Main.tile[x, y].HasTile || Main.tile[x, y].WallType == WallID.LihzahrdBrickUnsafe && !NPC.downedPlantBoss)
                    {
                        pathBlocked = true;
                        break;
                    }
                }
            }
            if (!pathBlocked)
            {
                Vector2 tp = new Vector2((int)(Main.mouseX + Main.screenPosition.X - 16), (int)(Main.mouseY + Main.screenPosition.Y - 24));

                Efx(player.MountedCenter, Main.MouseWorld);
                PacketHandler.SendFlash(-1, player.whoAmI, player.MountedCenter, Main.MouseWorld);

                player.Teleport(tp, -1, 0);
                NetMessage.SendData(MessageID.Teleport, -1, -1, null, 0, player.whoAmI, (int)(Main.mouseX + Main.screenPosition.X), (int)(Main.mouseY + Main.screenPosition.Y), 1, 0, 0);

                SetCooldowns(player, spellSlot);
            }
        }

        static public void Efx(Vector2 startPoint, Vector2 teleportPoint)
        {
            TerraLeague.DustLine(startPoint, teleportPoint, 228, 1, 2);
            TerraLeague.PlaySoundWithPitch(teleportPoint, 2, 72, 0.5f);
            for (int i = 0; i < 20; i++)
            {
                Dust dust = Dust.NewDustDirect(teleportPoint - (Vector2.One * 16), 32, 32, DustID.GoldFlame, 0, 0, 0, default, 4);
                dust.noGravity = true;
                dust.noLight = true;
                dust.velocity *= 2;
            }
        }
    }
}
