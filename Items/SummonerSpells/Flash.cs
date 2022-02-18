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
            int distance = 20;

            float xDis = Main.MouseWorld.X - player.position.X;
            float yDis = Main.MouseWorld.Y - player.position.Y;

            if (xDis < -distance * 16)
                xDis = -distance * 16;
            else if (xDis > distance * 16)
                xDis = distance * 16;

            if (yDis < -distance * 16)
                yDis = -distance * 16;
            else if (yDis > distance * 16)
                yDis = distance * 16;

            float newX = xDis + player.position.X;
            float newY = yDis + player.position.Y;

            Vector2 teleportPos = default;
            teleportPos.X = newX;
            if (player.gravDir == 1f)
            {
                teleportPos.Y = newY - (float)player.height;
            }
            else
            {
                teleportPos.Y = (float)Main.screenHeight - newY;
            }
            teleportPos.X -= (float)(player.width / 2);
            if (teleportPos.X > 50f && teleportPos.X < (float)(Main.maxTilesX * 16 - 50) && teleportPos.Y > 50f && teleportPos.Y < (float)(Main.maxTilesY * 16 - 50))
            {
                int blockX = (int)(teleportPos.X / 16f);
                int blockY = (int)(teleportPos.Y / 16f);
                if ((Main.tile[blockX, blockY].WallType != 87 || !((double)blockY > Main.worldSurface) || NPC.downedPlantBoss) && !Collision.SolidCollision(teleportPos, player.width, player.height))
                {
                    player.velocity = TerraLeague.CalcVelocityToPoint(player.MountedCenter, teleportPos, player.velocity.Length());
                    Efx(player.MountedCenter, teleportPos);
                    PacketHandler.SendFlash(-1, player.whoAmI, player.MountedCenter, teleportPos);
                    NetMessage.SendData(MessageID.Teleport, -1, -1, null, 0, (float)player.whoAmI, teleportPos.X, teleportPos.Y, 1, 0, 0);

                    player.Teleport(teleportPos, -1, 0);
                    SetCooldowns(player, spellSlot);
                }
            }

            //bool pathBlocked = false;
            //for (int x = (int)(Main.MouseWorld.X / 16) - 1; x < (int)((Main.mouseX + Main.screenPosition.X) / 16) + 1; x++)
            //{
            //    for (int y = (int)((Main.mouseY + Main.screenPosition.Y) / 16) - 1; y <= (int)((Main.mouseY + Main.screenPosition.Y) / 16) + 1; y++)
            //    {
            //        if (Main.tileSolid[Main.tile[x, y].TileType] && Main.tile[x, y].HasTile || Main.tile[x, y].WallType == WallID.LihzahrdBrickUnsafe && !NPC.downedPlantBoss)
            //        {
            //            pathBlocked = true;
            //            break;
            //        }
            //    }
            //}
            //if (!pathBlocked)
            //{
            //    Vector2 tp = new Vector2((int)(Main.mouseX + Main.screenPosition.X - 16), (int)(Main.mouseY + Main.screenPosition.Y - 24));

            //    Efx(player.MountedCenter, Main.MouseWorld);
            //    PacketHandler.SendFlash(-1, player.whoAmI, player.MountedCenter, Main.MouseWorld);

            //    player.Teleport(tp, -1, 0);
            //    NetMessage.SendData(MessageID.Teleport, -1, -1, null, 0, player.whoAmI, (int)(Main.mouseX + Main.screenPosition.X), (int)(Main.mouseY + Main.screenPosition.Y), 1, 0, 0);

            //    SetCooldowns(player, spellSlot);
            //}
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
