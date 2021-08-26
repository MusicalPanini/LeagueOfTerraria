using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Items;
using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Prefixes
{
    public class Armor1Prefix : ModPrefix
    {
        public override PrefixCategory Category { get { return PrefixCategory.Accessory; } }
        public const byte Armor = 2;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Padded");

            base.SetStaticDefaults();
        }

        public override float RollChance(Item item)
        {
            return base.RollChance(item);
        }

        public override void ModifyValue(ref float valueMult)
        {
            valueMult += 1.21f;
        }

        public override void Apply(Item item)
        {
            item.GetGlobalItem<TerraLeaguePrefixGLOBAL>().Armor = Armor;
            item.rare += 1;
        }

    }
}
