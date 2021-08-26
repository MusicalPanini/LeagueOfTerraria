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
    public class Armor2Prefix : ModPrefix
    {
        public override PrefixCategory Category { get { return PrefixCategory.Accessory; } }
        public const byte Armor = 4;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Plated");

            base.SetStaticDefaults();
        }

        public override float RollChance(Item item)
        {
            return base.RollChance(item);
        }

        public override void ModifyValue(ref float valueMult)
        {
            valueMult += 1.3225f;
        }

        public override void Apply(Item item)
        {
            item.GetGlobalItem<TerraLeaguePrefixGLOBAL>().Armor = Armor;
            item.rare += 1;
        }

    }
}
