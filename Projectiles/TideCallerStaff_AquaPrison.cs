using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    class TideCallerStaff_AquaPrison : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aqua Prison");
        }

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.alpha = 0;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            if (Projectile.velocity.X > 12)
                Projectile.velocity.X = 12;
            else if (Projectile.velocity.X < -12)
                Projectile.velocity.X = -12;

            Lighting.AddLight(Projectile.position, 0f, 0f, 0.5f);
            Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 211);
            dust.alpha = 0;
            dust.noLight = false;
            dust.noGravity = true;
            dust.scale = 1.4f;

            dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustType<Dusts.BubbledBubble>(), 0f, 0, 100, default, 2.5f);
            dust.noGravity = true;

            Projectile.velocity.Y += 0.4f;

            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.netUpdate = true;

            if (!NPCID.Sets.ShouldBeCountedAsBoss[target.type])
            {
                target.AddBuff(BuffType<TideCallerBubbled>(), 120);
                Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), target.Center, Vector2.Zero, ProjectileType<TideCallerStaff_BubbleVisual>(), 0, 0, Projectile.owner, target.whoAmI);
            }
            
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            if (Main.netMode != NetmodeID.Server)
                //Terraria.Audio.SoundEngine.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Sploosh").WithVolume(.7f), Projectile.position);

            for (int i = 0; i < 30; i++)
            {
                Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 211, 0f, -3f, 0, default, 2f);
            }
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustType<Dusts.BubbledBubble>(), -5 + i, 0, 100, default, 4f);
                dust.noGravity = true;
            }
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().bubbled)
                return false;
            else
                return base.CanHitNPC(target);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 20;
            return true;
        }
    }
}
