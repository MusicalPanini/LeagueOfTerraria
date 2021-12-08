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
    public class LunariGun : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public int CalPrefix = 0;
        public int SevPrefix = 0;
        public int GravPrefix = 0;
        public int InfPrefix = 0;
        public int CrePrefix = 0;

        public override void LoadData(Item item, TagCompound tag)
        {
            CalPrefix = tag.GetInt("Calibrum");
            SevPrefix = tag.GetInt("Severum");
            GravPrefix = tag.GetInt("Gravitum");
            InfPrefix = tag.GetInt("Infernum");
            CrePrefix = tag.GetInt("Crescendum");
        }

        public override void SaveData(Item item, TagCompound tag)
        {
            tag.Add("Calibrum", CalPrefix);
            tag.Add("Severum", SevPrefix);
            tag.Add("Gravitum", GravPrefix);
            tag.Add("Infernum", InfPrefix);
            tag.Add("Crescendum", CrePrefix);
        }

        public override GlobalItem Clone(Item item, Item itemClone)
        {
            return base.Clone(item, itemClone);
        }
    }
}
