using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Terraria.GameContent;

namespace TerraLeague.Projectiles
{
    public class CelestialStaff_Starcall : ModProjectile
    {
        bool droppedStar = false;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starcall");
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 24;
            Projectile.timeLeft = 300;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            //Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            Projectile.rotation += Projectile.direction * 0.1f;

            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = Main.rand.Next(50, 71);
                Terraria.Audio.SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 9), Projectile.position);
            }

            if (Projectile.ai[1] == 0f && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
            {
                Projectile.ai[1] = 1f;
                Projectile.netUpdate = true;
            }
            if (Projectile.ai[1] != 0f)
            {
                Projectile.tileCollide = true;
            }

            for (int i = 0; i < 2; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.PortalBolt, 0, 0, 0, new Color(248, 137, 89), 1.5f);
                dust.velocity *= 0.3f;
                dust.noGravity = true;
                dust.noLight = true;

                dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.PortalBolt, 0, 0, 0, new Color(237, 137, 164), 1.5f);
                dust.velocity *= 0.3f;
                dust.noGravity = true;
                dust.noLight = true;
            }
            
            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Projectile.width == 22)
                Prime();
            if (!droppedStar)
            {
                if (!target.immortal)
                {
                    float chance = Items.Weapons.CelestialStaff.RejuvDropChance(Main.player[Projectile.owner]);

                    if (Main.rand.NextFloat() < chance)
                    {
                        droppedStar = true;
                        Item.NewItem(Projectile.Hitbox, ItemType<Items.RegenHeart>());
                    }
                }
            }
                //Projectile.NewProjectileDirect(target.Center, Vector2.Zero, ProjectileType<CelestialStaff_StarcallRejuv>(), 0, 0, Projectile.owner);

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            hitDirection = Projectile.Center.X > target.Center.X ? -1 : 1;

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(3, 3), Projectile.position);
            TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 4, -1f);

            Dust dust;
            for (int i = 0; i < 40; i++)
            {
                dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.PortalBolt, 0, 0, 0, new Color(237, 137, 164), 2f);
                dust.noGravity = true;
                dust.velocity *= 2f;

                dust = Dust.NewDustDirect(Projectile.Center, 1,1, DustID.PortalBolt, 0, 0, 0, new Color(248, 137, 89), 2f);
                dust.noGravity = true;
                dust.velocity *= 4f;
            }

            Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            
            base.Kill(timeLeft);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Prime();
            return false;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 10;
            height = 10;
            return true;
        }

        public void Prime()
        {
            Projectile.velocity = Vector2.Zero;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
            Projectile.width = 115;
            Projectile.height = 115;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            Projectile.timeLeft = 1;
        }

        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw
            (
                texture,
                new Vector2
                (
                    Projectile.position.X - Main.screenPosition.X + Projectile.width * 0.5f,
                    Projectile.position.Y - Main.screenPosition.Y + Projectile.height - texture.Height * 0.5f
                ),
                new Rectangle(0, 0, texture.Width, texture.Height),
                Color.White,
                Projectile.rotation,
                texture.Size() * 0.5f,
                Projectile.scale,
                SpriteEffects.None,
                0f
            );
        }
    }
}
