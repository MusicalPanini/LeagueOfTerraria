using Microsoft.Xna.Framework;
using System.IO;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace TerraLeague.Projectiles
{
	public class EyeOfTheVoid_PlasmaSplit : ModProjectile
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
            Projectile.tileCollide = false;
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

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            target.AddBuff(BuffType<OrganicDeconstruction>(), 240);

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void AI()
        {
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

            if (Projectile.timeLeft < 10)
                Projectile.alpha += 26;
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (target.whoAmI == Projectile.ai[0])
                return false;
            else
                return base.CanHitNPC(target);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, 8, 8, 112, 0f, 0f, 255, new Color(59, 0, 255), 3.5f);
                dust.noGravity = true;
                dust.noLight = true;
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10; 
            return true;
        }
    }
}
