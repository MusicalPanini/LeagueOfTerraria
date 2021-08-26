using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ObjectData;
using TerraLeague.Items.PetrifiedWood;
using static Terraria.ModLoader.ModContent;
using Terraria.Localization;

namespace TerraLeague.Tiles.PetFurniture
{
    public class PetBed : ModTile
    {
        public override void SetStaticDefaults()
        {
            DustType = DustID.Cloud;

            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.HasOutlines[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style4x2); 
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Petrified Wood Bed");
            AddMapEntry(new Color(200, 200, 200), name);
            //TileID.Sets.DisableSmartCursor[Type] = true;
            AdjTiles = new int[] { TileID.Beds };
            //bed = true;
        }

        public override bool HasSmartInteract()
        {
            return true;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 64, 32, ItemType<PetBedItem>());
        }

        public override bool RightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;

            Tile tile = Main.tile[i, j];
            int spawnX = (i - (tile.frameX / 18)) + (tile.frameX >= 72 ? 5 : 2);
            int spawnY = j + 2;
            if (tile.frameY % 38 != 0)
            {
                spawnY--;
            }

            if (!Player.IsHoveringOverABottomSideOfABed(i, j))
            {
                if (player.IsWithinSnappngRangeToTile(i, j, 96))
                {
                    player.GamepadEnableGrappleCooldown();
                    player.sleeping.StartSleeping(player, i, j);
                }
            }
            else
            {
                player.FindSpawn();
                if (player.SpawnX == spawnX && player.SpawnY == spawnY)
                {
                    player.RemoveSpawn();
                    Main.NewText(Language.GetTextValue("Game.SpawnPointRemoved"), byte.MaxValue, 240, 20);
                }
                else if (Player.CheckSpawn(spawnX, spawnY))
                {
                    player.ChangeSpawn(spawnX, spawnY);
                    Main.NewText(Language.GetTextValue("Game.SpawnPointSet"), byte.MaxValue, 240, 20);
                }
            }

            return true;
            //Player player = Main.LocalPlayer;
            //Tile tile = Main.tile[i, j];
            //int spawnX = i - tile.frameX / 18;
            //int spawnY = j + 2;
            //spawnX += tile.frameX >= 72 ? 5 : 2;
            //if (tile.frameY % 38 != 0)
            //{
            //    spawnY--;
            //}
            //player.FindSpawn();
            //if (player.SpawnX == spawnX && player.SpawnY == spawnY)
            //{
            //    player.RemoveSpawn();
            //    Main.NewText("Spawn point removed!", 255, 240, 20, false);
            //}
            //else if (Player.CheckSpawn(spawnX, spawnY))
            //{
            //    player.ChangeSpawn(spawnX, spawnY);
            //    Main.NewText("Spawn point set!", 255, 240, 20, false);
            //}
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;

            if (!Player.IsHoveringOverABottomSideOfABed(i, j))
            {
                if (player.IsWithinSnappngRangeToTile(i, j, 96))
                {
                    player.noThrow = 2;
                    player.cursorItemIconEnabled = true;
                    player.cursorItemIconID = 5013;
                }
            }
            else
            {
                player.noThrow = 2;
                player.cursorItemIconEnabled = true;
                player.cursorItemIconID = ItemType<PetBedItem>();
            }

            //Player player = Main.LocalPlayer;
            //player.noThrow = 2;
            //player.itemIc = true;
            //player.showItemIcon2 = ItemType<PetBedItem>();
        }
    }
}