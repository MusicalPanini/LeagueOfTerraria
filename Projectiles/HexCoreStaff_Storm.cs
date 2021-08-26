using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class HexCoreStaff_Storm : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chaos Storm");
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 100;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            if (Projectile.soundDelay == 0)
                Prime();
            Projectile.soundDelay = 100;
            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool PreKill(int timeLeft)
        {
           
            return base.PreKill(timeLeft);
        }

        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(new LegacySoundStyle(3, 53), Projectile.position);
            for (int i = 0; i < 30; i++)
            {
                Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - Projectile.width/4, Projectile.Center.Y - Projectile.height / 4), Projectile.width / 2, Projectile.height / 2, DustID.AncientLight, 0, 0, 50, new Color(0, 255, 255), 1.5f);
                dust.velocity *= 7f;
                dust.noGravity = true;
                dust.noLight = true;
                dust.fadeIn = 2;
                dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - Projectile.width / 4, Projectile.Center.Y - Projectile.height /4), Projectile.width / 2, Projectile.height / 2, DustID.Electric, 0, 0, 50, new Color(0, 255, 255), 0.5f);
                dust.velocity *= 5f;
            }
            

            TerraLeague.DustBorderRing(Projectile.width / 2, Projectile.Center, 261, new Color(0, 255, 255), 2);

            base.Kill(timeLeft);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public void Prime()
        {
            int size = 400;

            Projectile.tileCollide = false;
            Projectile.velocity = Vector2.Zero;
            Projectile.alpha = 255;
            Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
            Projectile.width = size;
            Projectile.height = size;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            Projectile.timeLeft = 2;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
