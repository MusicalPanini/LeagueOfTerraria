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
    public class Heal2Prefix : ModPrefix
    {
        public override PrefixCategory Category { get { return PrefixCategory.Accessory; } }
        public const byte Heal = 6;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Medicinal");

            base.SetStaticDefaults();
        }

        public override float RollChance(Item item)
        {
            return 1;
        }

        public override void ModifyValue(ref float valueMult)
        {
            valueMult += 1.3225f;
        }

        public override void Apply(Item item)
        {
            item.GetGlobalItem<TerraLeaguePrefixGLOBAL>().HealPower = Heal;
            item.rare += 1;
        }
    }
}
