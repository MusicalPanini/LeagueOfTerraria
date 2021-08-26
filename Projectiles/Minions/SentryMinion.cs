using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;

namespace TerraLeague.Projectiles.Minions
{
    public abstract class SentryMinion : BaseMinion
    {
        protected float viewDist = 400f;
        protected int shootCool = 90;
        protected int shootCountdown = 90;
        protected NPC target = null;
        protected float shootSpeed;
        protected int shoot;
        protected double angle = 0;
        protected LegacySoundStyle shootSound;
        

        public virtual void CreateDust()
        {
        }

        public virtual void SelectFrame()
        {
        }

        public override void Behavior()
        {
            Player player = Main.player[Projectile.owner];
            float shortestDistance = viewDist;

            

            Projectile.velocity.X = 0f;
            Projectile.velocity.Y = Projectile.velocity.Y + 0.2f;
            if (Projectile.velocity.Y > 16f)
            {
                Projectile.velocity.Y = 16f;
            }


            if (player.HasMinionAttackTargetNPC)
            {
                NPC checkTarget = Main.npc[player.MinionAttackTargetNPC];

                float checkX = checkTarget.position.X + (float)checkTarget.width * 0.5f - Projectile.Center.X;
                float checkY = checkTarget.position.Y + (float)checkTarget.height * 0.5f - Projectile.Center.Y;
                float distance = (float)System.Math.Sqrt((double)(checkX * checkX + checkY * checkY));

                if (distance < shortestDistance && !checkTarget.friendly && checkTarget.life > 5 && Collision.CanHit(Projectile.position, Projectile.width, Projectile.height/2, checkTarget.position, checkTarget.width, checkTarget.height))
                {
                    shortestDistance = distance;
                    target = checkTarget;

                    double xDis = target.Center.X - Projectile.Center.X;
                    double yDis = target.Center.Y - Projectile.Center.Y;
                    angle = Math.Atan(yDis / xDis);
                }
            }

            if (target != null)
            {
                if (target.whoAmI == player.MinionAttackTargetNPC)
                {

                }
                else if (target != null && target.active && Collision.CanHit(Projectile.position, Projectile.width, Projectile.height / 2, target.position, target.width, target.height))
                {
                    if (Vector2.Distance(target.Center, Projectile.position) < viewDist)
                    {
                        double xDis = target.Center.X - Projectile.Center.X;
                        double yDis = target.Center.Y - Projectile.Center.Y;
                        angle = Math.Atan(yDis / xDis);
                    }
                    else
                    {
                        target = null;
                    }
                }
                else
                {
                    target = null;
                }
            }

            if (target == null)
            {
                for (int i = 0; i < Main.npc.Length; i++)
                {
                    NPC checkTarget = Main.npc[i];

                    float checkX = checkTarget.position.X + (float)checkTarget.width * 0.5f - Projectile.Center.X;
                    float checkY = checkTarget.position.Y + (float)checkTarget.height * 0.5f - Projectile.Center.Y;
                    float distance = (float)System.Math.Sqrt((double)(checkX * checkX + checkY * checkY));

                    if (distance < shortestDistance && !checkTarget.friendly && checkTarget.lifeMax > 5 && !checkTarget.immortal && checkTarget.active && Collision.CanHit(Projectile.position, Projectile.width, Projectile.height / 2, checkTarget.position, checkTarget.width, checkTarget.height))
                    {
                        shortestDistance = distance;
                        target = checkTarget;
                    }
                }
                if (target != null)
                {
                    double xDis = target.Center.X - Projectile.Center.X;
                    double yDis = target.Center.Y - Projectile.Center.Y;
                    angle = Math.Atan(yDis / xDis);
                }
            }

            if (target != null)
            {
                if (shootCountdown == 0 && Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, target.position, target.width, target.height))
                {
                    Vector2 velocity;
                    if (target.Center.X < Projectile.position.X)
                        velocity = new Vector2(-shootSpeed, 0).RotatedBy(angle);
                    else
                        velocity = new Vector2(shootSpeed, 0).RotatedBy(angle);

                    if (target.Center.Y - Projectile.Center.Y < 0)
                    {
                        velocity.Y = (float)-Math.Sqrt(Math.Pow(velocity.Y, 2));
                    }
                    else
                    {
                        velocity.Y = (float)Math.Sqrt(Math.Pow(velocity.Y, 2));
                    }

                    Projectile.NewProjectileDirect(Projectile.GetProjectileSource_FromThis(), Projectile.Center, velocity, shoot, (int)(Projectile.damage), Projectile.knockBack, Projectile.owner);
                    Terraria.Audio.SoundEngine.PlaySound(shootSound, Projectile.position);

                    shootCountdown = shootCool;
                }
            }
            else
            {
                angle = MathHelper.ToRadians(0);
            }
            if (shootCountdown > 0)
            {
                shootCountdown--;
            }
            SelectFrame();
            CreateDust();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public virtual void ChangeAnimation(double angle)
        {
        }
    }
}
