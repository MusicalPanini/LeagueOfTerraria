using System;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class LastBreath_SteelTempest : ModProjectile
    {
        bool enemyHit = false;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("SteelTempest");
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.penetrate = -1;
            Projectile.alpha = 0;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.timeLeft = 20;
        }

        public float movementFactor
        {
            get
            {
                return Projectile.ai[0];
            }
            set
            {
                Projectile.ai[0] = value;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[Projectile.owner];

            if (!enemyHit)
            {
                if (!player.GetModPlayer<PLAYERGLOBAL>().gathering2 && !player.GetModPlayer<PLAYERGLOBAL>().gathering3)
                {
                    player.AddBuff(BuffType<LastBreath2>(), 360);
                }
                else
                {
                    player.AddBuff(BuffType<LastBreath3>(), 360);
                    player.ClearBuff(BuffType<LastBreath2>());
                }
            }

            enemyHit = true;

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void AI()
        {
            

            if (Projectile.timeLeft == 20)
            {
                if (Projectile.ai[1] != 0)
                {
                    Projectile.friendly = false;
                    Terraria.Audio.SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 117).WithPitchVariance(0.8f), Projectile.Center);
                }
                else
                {
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
                }
            }

            Player player = Main.player[Projectile.owner];

            player.itemTime = Projectile.timeLeft;
            Projectile.position.X = player.MountedCenter.X - (float)(Projectile.width / 2);
            Projectile.position.Y = player.MountedCenter.Y - (float)(Projectile.height / 2);
            player.direction = Projectile.direction;

            if (Projectile.velocity.X < 0)
                Projectile.spriteDirection = -1;
            if (!player.frozen)
            {
                if (movementFactor == 0f) 
                {
                    movementFactor = 5f; 
                    Projectile.netUpdate = true;
                }
                if (player.itemTime < 20 / 2) 
                {
                    movementFactor -= 3f;
                }
                else 
                {
                    movementFactor += 3f;
                }
            }
            Projectile.position += Projectile.velocity * movementFactor;

            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
        }
    }
}
