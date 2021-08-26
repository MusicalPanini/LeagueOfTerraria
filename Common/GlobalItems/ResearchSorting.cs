using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Common.GlobalItems
{
    public class ResearchSorting : GlobalItem
    {
        public override void SetStaticDefaults()
        {
            //base.SetStaticDefaults();

            //for (int i = 0; i < ItemLoader.ItemCount; i++)
            //{
            //    ModItem moditem = ModContent.GetModItem(i);
            //    if (moditem != null)
            //    {
            //        if (moditem.Mod == Mod)
            //        {
            //            if (CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[i] != 0)
            //            {
            //                CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[i] = moditem.Item.maxStack;
            //            }
            //        }
            //    }
            //}
        }
	}
}
