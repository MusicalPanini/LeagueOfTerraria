using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class DarksteelDagger_DroppedDagger : ModProjectile
    {
        int attackRange = 256;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dagger");
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 22;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 60 * 10;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;
            Projectile.hide = true;
        }

        public override void AI()
        {
            switch ((int)Projectile.ai[0])
            {
                case 0:
                    Projectile.rotation = -1.57f;
                    break;
                case 1:
                    break;
                case 2:
                    Projectile.rotation = 1.57f;
                    Projectile.spriteDirection = -1;
                    break;
                default:
                    Projectile.rotation = 3.14f;
                    break;
            }

            if (Main.LocalPlayer.whoAmI == Projectile.owner)
            {
                Player player = Main.player[Projectile.owner];

                if (Targeting.IsHitboxWithinRange(Projectile.Center, player.Hitbox, 32))
                {
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Grab, Projectile.position);

                    var npcs = Targeting.GetAllNPCsInRange(player.MountedCenter, 256, true);

                    for (int i = 0; i < npcs.Count; i++)
                    {
                        NPC npc = Main.npc[npcs[i]];

                        float X = Main.rand.NextFloat(npc.Left.X - (npc.width / 2), npc.Right.X + (npc.width / 2));
                        float Y = Main.rand.NextFloat(npc.Top.Y - (npc.height / 2), npc.Bottom.Y + (npc.height / 2));
                        Vector2 pos = new Vector2(X, Y);
                        Vector2 vel = (npc.Center - pos).SafeNormalize(Vector2.One);

                        Projectile.NewProjectileDirect(player.GetProjectileSource_Item(new Items.Weapons.Severum().Item), pos, vel, ModContent.ProjectileType<Projectiles.Severum_Onslaught>(), Projectile.damage, 0, player.whoAmI, npc.whoAmI);
                    }

                    Projectile.ai[1] = 1;
                    Projectile.netUpdate = true;
                    Projectile.Kill();
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void Kill(int timeLeft)
        {
            if (Projectile.ai[1] != 0)
            {
                TerraLeague.PlaySoundWithPitch(Main.player[Projectile.owner].Center, 2, 71, Main.rand.NextFloat(0.3f, 0.55f));
                TerraLeague.DustRing(261, Main.player[Projectile.owner], Color.Gray);
            }

            base.Kill(timeLeft);
        }

        public void AnimateProjectile()
        {

        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 offset = Vector2.Zero;
            switch ((int)Projectile.ai[0])
            {
                case 0:
                    offset.X -= 16;
                    break;
                case 1:
                    offset.Y -= 16;
                    break;
                case 2:
                    offset.X += 16;
                    break;
                default:
                    offset.X += 16;
                    break;
            }

            lightColor = Lighting.GetColor((Projectile.Center + offset).ToTileCoordinates());

            return base.PreDraw(ref lightColor);
        }

        public override void PostDraw(Color lightColor)
        {
            if (Main.LocalPlayer.whoAmI == Projectile.owner)
            {
                Player player = Main.player[Projectile.owner];
                float distance = Projectile.Center.Distance(player.MountedCenter);

                if (attackRange + 255 >= distance)
                {
                    float alpha = 0.75f;
                    if (distance > attackRange)
                        alpha -= ((distance - attackRange) / 255f) * 0.75f;

                    if (alpha < 0)
                        alpha = 0;

                    TerraLeague.DrawCircle(Projectile.Center, attackRange, Color.Gray * alpha);
                }
            }

            base.PostDraw(lightColor);
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCsAndTiles.Add(index);
            base.DrawBehind(index, behindNPCsAndTiles, behindNPCs, behindProjectiles, overPlayers, overWiresUI);
        }
    }
}
