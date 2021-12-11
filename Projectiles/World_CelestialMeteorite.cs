using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using TerraLeague.Buffs;
using TerraLeague.Items;
using TerraLeague.Projectiles.Explosive;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    class World_CelestialMeteorite : ExplosiveProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Celestial Meteorite");
        }

        public override void SetDefaults()
        {
            ExplosionWidth = 1000;
            ExplosionHeight = 1000;

            Projectile.width = 36;
            Projectile.height = 36;
            Projectile.alpha = 0;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.aiStyle = 0;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, Color.OrangeRed.ToVector3());

            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 20 + Main.rand.Next(40);

                Vector2 position = Projectile.Center;
                //if (Projectile.Distance(Main.LocalPlayer.MountedCenter) > 2000)
                //{
                //    Vector2 dis = (Projectile.Center - Main.LocalPlayer.MountedCenter).SafeNormalize(Vector2.Zero);
                //    dis *= 2000;
                //    position += dis;
                //}
                SoundEffectInstance sound = TerraLeague.PlaySoundWithPitch(position, 2, 9, -1f);

                if (sound != null)
                {
                    if (sound.Volume * 3 > 1)
                        sound.Volume = 1;
                    else
                        sound.Volume *= 3;
                }
            }

            Projectile.rotation += Projectile.velocity.X * 0.05f;

            for (int i = 0; i < 3; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6, 0, 0, 0, default, 4f);
                dust.noGravity = true;

                dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6, 0, 3, 0, default, 1f);
            }


            base.AI();
        }

        public override void KillEffects()
        {
            Vector2 position = Main.LocalPlayer.MountedCenter;
            if (Projectile.Distance(Main.LocalPlayer.MountedCenter) > 1000)
            {
                Vector2 dis = (Projectile.Center - Main.LocalPlayer.MountedCenter).SafeNormalize(Vector2.Zero);
                dis *= 1000;
                position += dis;
            }
            TerraLeague.PlaySoundWithPitch(position, 2, 89, -1f);
            SoundEffectInstance sound = Terraria.Audio.SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, position);
            Main.NewText("A Celestial Comet has landed", new Color(0, 200, 255));
            if (sound != null)
            {
                sound.Pitch = -1;

                if (sound.Volume * 10 > 1)
                    sound.Volume = 1;
                else
                    sound.Volume *= 10;
            }

            Dust dust;
            for (int i = 0; i < 75; i++)
            {
                dust = Dust.NewDustDirect(Projectile.position + new Vector2(Projectile.width * 0.4f, Projectile.height * 0.4f), (int)(Projectile.width * 0.2), (int)(Projectile.height * 0.2), DustID.Smoke, 0f, 0f, 100, default, 2f);
                dust.velocity.X *= Main.rand.NextFloat(3f);
                dust.fadeIn = Main.rand.NextFloat(2, 4);
                dust = Dust.NewDustDirect(Projectile.position + new Vector2(Projectile.width * 0.4f, Projectile.height * 0.4f), (int)(Projectile.width * 0.2), (int)(Projectile.height * 0.2), DustID.Smoke, 0f, 0f, 100, default, 2f);
                dust.velocity.X *= Main.rand.NextFloat(6, 10);
                dust.velocity.Y *= 0.5f;
                dust.fadeIn = Main.rand.NextFloat(2, 4);
                dust = Dust.NewDustDirect(Projectile.position + new Vector2(Projectile.width * 0.4f, Projectile.height * 0.4f), (int)(Projectile.width * 0.2), (int)(Projectile.height * 0.2), DustID.Smoke, 0f, 0f, 100, default, 2f);
                dust.velocity.Y *= -Main.rand.NextFloat(6, 10);
                ///dust.velocity.Y *= 0.5f;
                dust.fadeIn = Main.rand.NextFloat(2, 4);
                dust = Dust.NewDustDirect(Projectile.position + new Vector2(Projectile.width * 0.4f, Projectile.height * 0.4f), (int)(Projectile.width * 0.2), (int)(Projectile.height * 0.2), DustID.BlueTorch, 0f, -3f, 100, default, 3f);
                dust.velocity.Y = -Main.rand.NextFloat(1, 4);
                dust = Dust.NewDustDirect(Projectile.position + new Vector2(Projectile.width * 0.4f, Projectile.height * 0.4f), (int)(Projectile.width * 0.2), (int)(Projectile.height * 0.2), DustID.Fireworks, 0f, 0f, 100, default, 2f);
                dust.noGravity = true;
                dust.fadeIn = 3;
            }
            for (int i = 0; i < 500; i++)
            {
                dust = Dust.NewDustDirect(Projectile.position + new Vector2(Projectile.width * 0.4f, Projectile.height * 0.4f), (int)(Projectile.width * 0.2), (int)(Projectile.height * 0.2), DustID.Fireworks, 0f, Main.rand.NextFloat(-10f, -3f), 100, default, 1f);
                //dust.noGravity = true;
                dust.velocity.X *= Main.rand.NextFloat(3f);
                dust.velocity *= 1.5f;
                dust.color = new Color(255, 0, 220);
                dust.fadeIn = Main.rand.NextFloat(1, 3);
            }

            //TerraLeague.DustBorderRing(Projectile.width / 2, Projectile.Center, 174, new Color(255, 0, 220), 3);
            Item.NewItem(Projectile.Hitbox, ItemType<FragmentOfTheAspect>());
        }

    }
}
