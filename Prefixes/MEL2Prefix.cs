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
    public class MEL2Prefix : ModPrefix
    {
        public override PrefixCategory Category { get { return PrefixCategory.Accessory; } }
        public const byte MEL = 20;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Slayer's");

            base.SetStaticDefaults();
        }

        public override float RollChance(Item item)
        {
            return 0.75f;
        }

        public override void Apply(Item item)
        {
            item.GetGlobalItem<TerraLeaguePrefixGLOBAL>().MEL = MEL;
            item.rare += 2;
        }

        public override void ModifyValue(ref float valueMult)
        {
            valueMult += 1.44f;
        }
    }
}
