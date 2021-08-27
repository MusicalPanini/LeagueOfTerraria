using Microsoft.Xna.Framework;
using System.IO;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace TerraLeague.Projectiles
{
	public class EyeOfTheVoid_Plasma : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Plasma Fission");
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.alpha = 255;
            Projectile.timeLeft = 50;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.owner);
            writer.Write(Projectile.ai[0]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.owner = reader.ReadInt32();
            Projectile.ai[0] = reader.ReadInt32();
        }

        public override void AI()
        {
            if (Projectile.velocity.X < 0)
                Projectile.spriteDirection = -1;

            Projectile.rotation += (float)Projectile.direction * 0.1f;
            Lighting.AddLight(Projectile.position, 0.75f, 0f, 0.75f);
            for (int i = 0; i < 3; i++)
            {
                Vector2 dustBoxPosition = new Vector2(Projectile.position.X + 0, Projectile.position.Y + 0);
                int dustBoxWidth = Projectile.width - 8;
                int dustBoxHeight = Projectile.height - 8;
                Dust dust = Dust.NewDustDirect(dustBoxPosition, dustBoxWidth, dustBoxHeight, 112, 0f, 0f, 255, new Color(59, 0, 255), 2f);
                dust.noGravity = true;
                dust.velocity *= 0.1f;
                dust.position.X -= Projectile.velocity.X / 3f * (float)i;
                dust.position.Y -= Projectile.velocity.Y / 3f * (float)i;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffType<OrganicDeconstruction>(), 240);
            Projectile.position += Projectile.velocity * 1;
            Projectile.friendly = false;
            Projectile.ai[0] = target.whoAmI;
        }

        public override void Kill(int timeLeft)
        {
            Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), Projectile.position, Projectile.oldVelocity.RotatedBy(MathHelper.ToRadians(90)), ProjectileType<EyeOfTheVoid_PlasmaSplit>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Projectile.ai[0]);
            Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), Projectile.position, Projectile.oldVelocity.RotatedBy(MathHelper.ToRadians(-90)), ProjectileType<EyeOfTheVoid_PlasmaSplit>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Projectile.ai[0]);

            TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 45, 0.25f);
            TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 91, 0.5f);

            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, 8, 8, 112, 0f, 0f, 255, new Color(59, 0, 255), 3.5f);
                dust.noGravity = true;
                dust.noLight = true;
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = height = 10; 
            return true;
        }
    }
}
