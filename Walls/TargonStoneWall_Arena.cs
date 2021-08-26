using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.NPCs.TargonBoss;
using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Walls
{
    public class TargonStoneWall_Arena : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = false;
            AddMapEntry(new Color(120, 119, 110));
            //drop = mod.WallType("PetWall");
        }

        public override void KillWall(int i, int j, ref bool fail)
        {
            fail = Common.ModSystems.DownedBossSystem.downedTargonBoss;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            if (NPC.CountNPCS(ModContent.NPCType<TargonBossNPC>()) > 0)
            {
                r = 0.2f;
                g = 0.1f;
                b = 0.5f;
            }
            else
            {
                r = g = b = 0;
            }

            base.ModifyLight(i, j, ref r, ref g, ref b);
        }
    }
}
