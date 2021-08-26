using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class Item_Tempest : ModProjectile
    {
        readonly int spacing = 48;
        int number = 0;
        readonly int numberOfExtraStrikes = 6;
        public static int randSpread = 64;

        bool leftBlocks = true;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tempest");
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
            Projectile.timeLeft = 301;
            Projectile.extraUpdates = 35;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void AI()
        {
            if (Projectile.soundDelay == 0)
            {
                number = (int)Projectile.knockBack;
                Projectile.knockBack = 0;

                for (int i = 0; i < 3; i++)
                {
                    Gore gore = Gore.NewGoreDirect(Projectile.Center, default, Main.rand.Next(61, 64), 1f);
                    gore.velocity.Y = gore.velocity.Y + 1.5f;
                }
            }
            Projectile.soundDelay = 10;

            if (leftBlocks && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
            {
                leftBlocks = false;
                Projectile.netUpdate = true;
            }
            if (!leftBlocks)
            {
                Projectile.tileCollide = true;
            }

            for (int i = 0; i < 3; i++)
            {
                Vector2 dustBoxPosition = new Vector2(Projectile.position.X + 6, Projectile.position.Y + 6);
                int dustBoxWidth = Projectile.width - 12;
                int dustBoxHeight = Projectile.height - 12;
                Dust dust = Dust.NewDustDirect(dustBoxPosition, dustBoxWidth, dustBoxHeight, DustID.AncientLight, 0f, 0f, 100, new Color(0, 0, 255, 150), 1f);
                dust.noGravity = true;
                dust.velocity *= 0.1f;
                dust.velocity += Projectile.velocity * 0.1f;
                dust.position.X -= Projectile.velocity.X / 3f * (float)i;
                dust.position.Y -= Projectile.velocity.Y / 3f * (float)i;
            }

            Dust dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.AncientLight, 0, 0, 0, new Color(255, 106, 0, 150), 1f);
            dust2.noGravity = true;
            dust2.velocity *= 3f;
            Lighting.AddLight(Projectile.position, 0f, 0f, 1f);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.timeLeft > 2)
                Prime();

            return false;
        }

        public override void Kill(int timeLeft)
        {
            if (number < numberOfExtraStrikes && number > -numberOfExtraStrikes)
            {
                if (number == 0)
                {
                    Vector2 pos = new Vector2(Projectile.ai[0] + (spacing) + (Main.rand.Next(-randSpread, randSpread)), Projectile.ai[1] - 700);
                    Vector2 targetPos = new Vector2(Projectile.ai[0] + (spacing), Projectile.ai[1]);
                    Vector2 vel = TerraLeague.CalcVelocityToPoint(pos, targetPos, 4);
                    Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), pos, vel, Projectile.type, Projectile.damage, 1, Projectile.owner, targetPos.X, targetPos.Y);

                    pos = new Vector2(Projectile.ai[0] + (-spacing) + (Main.rand.Next(-randSpread, randSpread)), Projectile.ai[1] - 700);
                    targetPos = new Vector2(Projectile.ai[0] + (-spacing), Projectile.ai[1]);
                    vel = TerraLeague.CalcVelocityToPoint(pos, targetPos, 4);
                    Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), pos, vel, Projectile.type, Projectile.damage, -1, Projectile.owner, targetPos.X, targetPos.Y);
                }
                else
                {
                    Vector2 pos = new Vector2(Projectile.ai[0] + (spacing * number/Math.Abs(number)) + (Main.rand.Next(-randSpread, randSpread)), Projectile.ai[1] - 700);
                    Vector2 targetPos = new Vector2(Projectile.ai[0] + (spacing * number / Math.Abs(number)), Projectile.ai[1]);

                    Vector2 vel = TerraLeague.CalcVelocityToPoint(pos, targetPos, 4);

                    Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), pos, vel, Projectile.type, Projectile.damage, number > 0 ? number + 1 : number - 1, Projectile.owner, targetPos.X, targetPos.Y);
                }
            }

            Terraria.Audio.SoundEngine.PlaySound(new LegacySoundStyle(3, 53), Projectile.position);
            for (int i = 0; i < 50; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.AncientLight, 0, 0, 0, new Color(0, 0, 255, 150), 2f);
                dust.velocity *= 3f;
                dust.noGravity = true;
            }

            base.Kill(timeLeft);
        }

        public override bool? CanHitNPC(NPC target)
        {
            return base.CanHitNPC(target);
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public void Prime()
        {
            Projectile.tileCollide = false;
            Projectile.velocity = Vector2.Zero;
            Projectile.alpha = 255;
            Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            Projectile.timeLeft = 2;
        }
    }
}
