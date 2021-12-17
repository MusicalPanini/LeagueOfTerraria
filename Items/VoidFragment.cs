using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items
{
    public class VoidFragment : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Void Matter");
            base.SetStaticDefaults();
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override bool OnPickup(Player player)
        {
            bool canTakeItem = false;

            for (int i = 0; i < 50; i++)
            {
                Item invItem = player.inventory[i];
                if ((invItem.type == Type && invItem.stack < invItem.maxStack) || invItem.type == ItemID.None)
                {
                    canTakeItem = true;
                    break;
                }
            }

            if (canTakeItem && player.GetModPlayer<PLAYERGLOBAL>().zoneVoid)
                player.GetModPlayer<PLAYERGLOBAL>().AddVoidInfluence(10 * Item.stack);

            return base.OnPickup(player);
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.width = 14;
            Item.height = 16;
            Item.rare = ItemRarityID.LightPurple;
            Item.value = Item.buyPrice(0, 0, 0, 50);
            
        }
        
        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            if (Item.timeSinceItemSpawned > 60 * 3)
            {
                Item.SetDefaults();

                for (int i = 0; i < 20; i++)
                {
                    Dust.NewDustDirect(Item.position, Item.width, Item.height, DustID.DemonTorch);
                }
            }
            base.Update(ref gravity, ref maxFallSpeed);
        }
    }
}
