using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items
{
    public class RegenHeart : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemIconPulse[Item.type] = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            DisplayName.SetDefault("Rejuvenation");
            
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Orange;
            Item.width = 18;
            Item.height = 18;
            base.SetDefaults();
        }

        public override void GrabRange(Player player, ref int grabRange)
        {
            grabRange += 0;   
        }

        public override bool GrabStyle(Player player)
        {
            Vector2 vector4 = new Vector2(Item.position.X + (float)(Item.width / 2), Item.position.Y + (float)(Item.height / 2));
            float num15 = player.Center.X - vector4.X;
            float num16 = player.Center.Y - vector4.Y;
            float num17 = (float)System.Math.Sqrt((double)(num15 * num15 + num16 * num16));
            num17 = 12f / num17;
            num15 *= num17;
            num16 *= num17;
            int num18 = 5;
            Item.velocity.X = (Item.velocity.X * (float)(num18 - 1) + num15) / (float)num18;
            Item.velocity.Y = (Item.velocity.Y * (float)(num18 - 1) + num16) / (float)num18;
            return true;
        }

        public override bool ItemSpace(Player player)
        {
            return true;
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center, Color.Orange.ToVector3() * Main.essScale);
        }

        public override bool OnPickup(Player player)
        {
            Terraria.Audio.SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 29), player.Center);
            for (int k = 0; k < 20; k++)
            {
                Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.PortalBolt, 0, -6, 0, k % 2 == 0 ? new Color(248, 137, 89) : new Color(237, 137, 164), 3f);
                dust.noGravity = true;
                dust.noLight = true;
            }
            player.AddBuff(BuffType<Rejuvenation>(), 2 * 60);
            return false;
        }
    }
}
