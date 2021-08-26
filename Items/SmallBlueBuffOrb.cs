using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items
{
    public class SmallBlueBuffOrb : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));
            ItemID.Sets.ItemIconPulse[Item.type] = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            DisplayName.SetDefault("Crest of Insight");
            
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Blue;
            Item.width = 46;
            Item.height = 46;
            
            base.SetDefaults();
        }

        public override void GrabRange(Player player, ref int grabRange)
        {
            
        }

        public override bool GrabStyle(Player player)
        {
            Vector2 vectorItemToPlayer = player.Center - Item.Center;
            Vector2 movement = vectorItemToPlayer.SafeNormalize(default);
            Item.velocity = Item.velocity + movement;
            Item.velocity = Collision.TileCollision(Item.position, Item.velocity, Item.width, Item.height);
            return true;
        }

        public override void PostUpdate()
        {
            ItemID.Sets.ItemIconPulse[Item.type] = true;

            Lighting.AddLight(Item.Center, Color.Blue.ToVector3() * 0.55f * Main.essScale);
        }

        public override bool OnPickup(Player player)
        {
            player.AddBuff(BuffType<BlueBuff>(), 120 * 60);
            Terraria.Audio.SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 29), player.Center);
            for (int k = 0; k < 20; k++)
            {
                Dust.NewDust(player.position, player.width, player.height, DustID.BlueCrystalShard, 0, -2, 0, default, 1.2f);
            }
            CombatText.NewText(player.Hitbox, new Color(50, 50, 255), "Blue Buff");
            return false;
        }
    }
}
