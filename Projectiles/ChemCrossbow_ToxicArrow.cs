using Microsoft.Xna.Framework;
using System;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class ChemCrossbow_ToxicArrow : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Toxic Arrow");
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.alpha = 0;
            Projectile.timeLeft = 1200;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.aiStyle = 1;
            Projectile.arrow = true;
        }

        public override void AI()
        {
            Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Ice, 0f, 0f, 100, new Color(0,255,0));
            dust.noGravity = true;
            dust.noLight = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            int stacks = target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().DeadlyVenomStacks;

            if (stacks < 4)
            {
                target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().DeadlyVenomStacks++;

                if (Main.netMode == NetmodeID.MultiplayerClient)
                    TerraLeagueNPCsGLOBAL.PacketHandler.SendSyncStats(Main.LocalPlayer.whoAmI, -1, 3, target.whoAmI, target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().DeadlyVenomStacks);
            }

            target.AddBuff(BuffType<DeadlyVenom>(), 300);

            base.OnHitNPC(target, damage, knockback, false);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
            Projectile.Kill();
            return false;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Ice, 0f, 0f, 100, new Color(0, 255, 0), 0.7f);
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
