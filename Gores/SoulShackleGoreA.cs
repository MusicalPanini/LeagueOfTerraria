using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Gores
{
    public class SoulShackleGoreA : ModGore
    {
        float brightness = 1;

        public override void OnSpawn(Gore gore)
        {
            base.OnSpawn(gore);
        }

        public override bool Update(Gore gore)
        {
            if (brightness > 0)
                brightness -= 0.001f;
            if (brightness <= 0)
                brightness = 0;

            return base.Update(gore);
        }

        public override Color? GetAlpha(Gore gore, Color lightColor)
        {
            float glow = gore.timeLeft / 60f;

            float R = lightColor.R + ((255 - lightColor.R) * glow);
            float G = lightColor.G + ((255 - lightColor.G) * glow);
            float B = lightColor.B + ((255 - lightColor.B) * glow);
            Color color = new Color((int)R, (int)G, (int)B);

            return color;
        }
    }
}
