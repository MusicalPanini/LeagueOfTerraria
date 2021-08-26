using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class DarkIceTome_IceShardSmallA : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ice Shard");
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.alpha = 0;
            Projectile.timeLeft = 35;
            Projectile.penetrate = 3;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
        }

        public override void AI()
        {
            Projectile.npcProj = false;
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

            if (Projectile.velocity.Y > 16f)
            {
                Projectile.velocity.Y = 16f;
            }

            if (Main.rand.Next(0, 3) == 0)
            {
                Dust dustIndex = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Ice, 0f, 0f, 100, default, 1f);
                dustIndex.noGravity = true;
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            target.AddBuff(BuffType<Buffs.Slowed>(), 120);
            target.GetGlobalNPC<NPCs.TerraLeagueNPCsGLOBAL>().slowed = true;
            target.GetGlobalNPC<NPCs.TerraLeagueNPCsGLOBAL>().icebornSubjugation = true;
            target.GetGlobalNPC<NPCs.TerraLeagueNPCsGLOBAL>().icebornSubjugationOwner = Projectile.owner;
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Terraria.Audio.SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 27), Projectile.position);

            return true;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust dustIndex = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Ice, Projectile.velocity.X / 1.5f, Projectile.velocity.Y / 1.5f, 100, default, 1f);
                dustIndex.noGravity = true;
            }
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (target.whoAmI == Projectile.ai[0])
                return false;
            else
                return base.CanHitNPC(target);
        }
    }
}
