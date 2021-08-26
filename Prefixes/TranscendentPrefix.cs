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
    public class TranscendentPrefix : ModPrefix
    {
        public override PrefixCategory Category { get { return PrefixCategory.Accessory; } }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Transcendent");
            base.SetStaticDefaults();
        }

        public TranscendentPrefix()
        {
        }

        public override void ModifyValue(ref float valueMult)
        {
            valueMult += 1.3225f;
        }

        public override void Apply(Item item)
        {
            item.GetGlobalItem<TerraLeaguePrefixGLOBAL>().Transedent = true;
            item.rare += 1;
        }
    }
}
