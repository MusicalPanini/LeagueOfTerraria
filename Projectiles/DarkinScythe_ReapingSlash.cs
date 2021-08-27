using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    class DarkinScythe_ReapingSlash : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Reaping Slash");
        }

        public override void SetDefaults()
        {
            Projectile.width = 54;
            Projectile.height = 60;
            Projectile.alpha = 0;
            Projectile.timeLeft = 27;
            Projectile.penetrate = 1000;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().channelProjectile = true;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().playerInvincible = true;
            base.SetDefaults();
        }
        
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            player.GetModPlayer<PLAYERGLOBAL>().invincible = true;

            if (Projectile.soundDelay == 0)
            {
                player.ChangeDir(player.velocity.X > 0 ? 1 : -1);
                Projectile.spriteDirection = player.direction;

                TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 71, -1f);
            }
            Projectile.soundDelay = 100;

            player.direction = Projectile.spriteDirection;

            if (Projectile.timeLeft <= 26)
            {
                Projectile.friendly = true;
                if (Projectile.ai[0] == 1)
                {
                    Projectile.rotation += (2 * MathHelper.Pi) / 15;
                    Projectile.Center = Main.player[Projectile.owner].Center + new Vector2(33f, 40.5f).RotatedBy(Projectile.rotation);
                }
                else
                {
                    Projectile.spriteDirection = -1;
                    Projectile.rotation -= (2 * MathHelper.Pi) / 15;
                    Projectile.Center = Main.player[Projectile.owner].Center - new Vector2(33f, -40.5f).RotatedBy(Projectile.rotation);
                }
            }
            else
            {
                if (Projectile.ai[0] == 1)
                {
                    Projectile.rotation -= (2 * MathHelper.Pi) / 90;
                    Projectile.Center = Main.player[Projectile.owner].Center + new Vector2(33f, 40.5f).RotatedBy(Projectile.rotation);
                }
                else
                {
                    Projectile.spriteDirection = -1;
                    Projectile.rotation += (2 * MathHelper.Pi) / 90;
                    Projectile.Center = Main.player[Projectile.owner].Center - new Vector2(33f, -40.5f).RotatedBy(Projectile.rotation);
                }
            }

            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            PLAYERGLOBAL player = Main.player[Projectile.owner].GetModPlayer<PLAYERGLOBAL>();

            target.AddBuff(BuffType<UmbralTrespass>(), 300);

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = height = 10; 
            return true;
        }
    }
}
