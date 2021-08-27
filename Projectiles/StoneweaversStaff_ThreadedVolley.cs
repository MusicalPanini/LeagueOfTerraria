using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    class StoneweaversStaff_ThreadedVolley : ModProjectile
    {
        readonly Vector2[] stonePos = { new Vector2(-24,-24), new Vector2(24,-16), new Vector2(0,-8), new Vector2(16, -32), new Vector2(-8,-32) };

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Threaded Volley");
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.timeLeft = 1000;
            Projectile.penetrate = 1000;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.alpha = 150;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Prime();
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void AI()
        {
            if ((int)Projectile.localAI[1] == 0)
            {
                if (Projectile.alpha > 0)
                    Projectile.alpha -= 10;
                if (Projectile.alpha < 0)
                    Projectile.alpha = 0;

                if (Projectile.timeLeft == 1000)
                    Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

                Projectile.velocity = Vector2.Zero;

                Projectile.Center = new Vector2(Main.player[Projectile.owner].MountedCenter.X + stonePos[(int)Projectile.ai[0] % 5].X + Projectile.ai[0], Main.player[Projectile.owner].MountedCenter.Y + stonePos[(int)Projectile.ai[0] % 5].Y + Projectile.ai[0] /*+ (16 * (Projectile.alpha / 255f))*/);

                if (Projectile.timeLeft == 970 - ((int)Projectile.ai[0] * (int)Projectile.ai[1]))
                {
                    Projectile.localAI[1] = 1;
                    Projectile.velocity = new Vector2(0, -20).RotatedBy(Projectile.rotation);
                    Projectile.friendly = true;
                    Projectile.tileCollide = true;
                    TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 19, -1f);
                }
            }
            else
            {
                Dust.NewDustDirect(Projectile.position, 16, 16, DustID.t_Slime, 0f, 0f, 100, new Color(255, 125, 0), 0.7f);
            }
        }

        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item10, Projectile.position);

            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.t_Slime, 0f, 0f, 100, new Color(255, 125, 0), 0.7f);
                dust.velocity *= 1.5f;

                Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.t_Slime, 0f, 0f, 100, new Color(255, 125, 0), 1f);
            }
            
            base.Kill(timeLeft);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Prime();
            return false;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = height = 10;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public void Prime()
        {
            Projectile.tileCollide = false;
            Projectile.timeLeft = 3;
            Projectile.velocity = Vector2.Zero;
            Projectile.alpha = 255;
            Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
        }
    }
}
