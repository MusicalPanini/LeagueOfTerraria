﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Biomes
{
    class SurfaceMarbleBackgroundStyle : ModSurfaceBackgroundStyle
	{
		// Use this to keep far Backgrounds like the mountains.

		public override void ModifyFarFades(float[] fades, float transitionSpeed)
		{
			for (int i = 0; i < fades.Length; i++)
			{
				if (i == Slot)
				{
					fades[i] += transitionSpeed;
					if (fades[i] > 1f)
					{
						fades[i] = 1f;
					}
				}
				else
				{
					fades[i] -= transitionSpeed;
					if (fades[i] < 0f)
					{
						fades[i] = 0f;
					}
				}
			}
		}

        public override int ChooseFarTexture()
        {
            return 7;
        }

        private static int SurfaceFrameCounter;
		private static int SurfaceFrame;
		public override int ChooseMiddleTexture()
		{
			return 8;

			//if (++SurfaceFrameCounter > 12)
			//{
			//	SurfaceFrame = (SurfaceFrame + 1) % 4;
			//	SurfaceFrameCounter = 0;
			//}
			//switch (SurfaceFrame)
			//{
			//	case 0:
			//		return BackgroundTextureLoader.GetBackgroundSlot("TerraLeague/Backgrounds/ExampleBiomeSurfaceMid0");
			//	case 1:
			//		return BackgroundTextureLoader.GetBackgroundSlot("TerraLeague/Backgrounds/ExampleBiomeSurfaceMid1");
			//	case 2:
			//		return BackgroundTextureLoader.GetBackgroundSlot("TerraLeague/Backgrounds/ExampleBiomeSurfaceMid2");
			//	case 3:
			//		return BackgroundTextureLoader.GetBackgroundSlot("TerraLeague/Backgrounds/ExampleBiomeSurfaceMid3");
			//	default:
			//		return -1;
			//}
		}

		public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b)
		{
			a += 400;
			scale = 1f;
			return BackgroundTextureLoader.GetBackgroundSlot("TerraLeague/Textures/Backgrounds/MarbleBiome_MidGround");
			//return BackgroundTextureLoader.GetBackgroundSlot("TerraLeague/Textures/Backgrounds/MarbleBiome_Foreground");
		}

    }
}