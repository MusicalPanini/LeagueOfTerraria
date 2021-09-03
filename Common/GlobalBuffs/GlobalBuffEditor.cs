using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Common.GlobalBuffs
{
    public class GlobalBuffEditor : GlobalBuff
    {
        public override void ModifyBuffTip(int type, ref string tip, ref int rare)
        {
            if (type == BuffID.ManaSickness)
            {
                tip = "Heal Power nullified" +
                    "\nMagic and Summon damage reduced by ";
            }
            base.ModifyBuffTip(type, ref tip, ref rare);
        }

        public override void Update(int type, Player player, ref int buffIndex)
        {
            if (type == BuffID.ManaSickness)
            {
                player.GetModPlayer<PLAYERGLOBAL>().healPower = 1;
                player.GetDamage(DamageClass.Summon) *= 1f - player.manaSickReduction;
            }
            base.Update(type, player, ref buffIndex);
        }
    }
}
