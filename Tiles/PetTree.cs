using Microsoft.Xna.Framework.Graphics;
using TerraLeague.Gores;
using TerraLeague.Items.PetrifiedWood;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Tiles
{
    public class PetTree : ModTree
    {
        private Mod mod
        {
            get
            {
                return GetInstance<TerraLeague>();
            }
        }

        public override int CreateDust()
        {
            return 51;
        }

        public override int GrowthFXGore()
        {
            return GoreType<PetLeaf>();
        }

        public override int DropWood()
        {
            return ItemType<PetWood>();
        }

        public override Texture2D GetTexture()
        {
            return ModContent.Request<Texture2D>("TerraLeague/Tiles/PetTree").Value;
        }

        public override Texture2D GetTopTextures(int i, int j, ref int frame, ref int frameWidth, ref int frameHeight, ref int xOffsetLeft, ref int yOffset)
        {
            return ModContent.Request<Texture2D>("TerraLeague/Tiles/PetTree_Tops").Value;
        }

        public override Texture2D GetBranchTextures(int i, int j, int trunkOffset, ref int frame)
        {
            return ModContent.Request<Texture2D>("TerraLeague/Tiles/PetTree_Branches").Value;
        }
    }
}
