using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TerraLeague.Items.Weapons
{
    public class MercuryWeapon : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public int HammerPrefix = 0;
        public int CannonPrefix = 0;

        public override void LoadData(Item item, TagCompound tag)
        {
            HammerPrefix = tag.GetInt("Hammer");
            CannonPrefix = tag.GetInt("Cannon");
        }

        public override void SaveData(Item item, TagCompound tag)
        {
            tag.Add("Hammer", HammerPrefix);
            tag.Add("Cannon", CannonPrefix);
        }

        public override GlobalItem Clone(Item item, Item itemClone)
        {
            return base.Clone(item, itemClone);
        }
    }
}
