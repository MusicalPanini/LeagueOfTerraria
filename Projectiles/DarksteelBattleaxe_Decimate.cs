using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    class DarksteelBattleaxe_Decimate : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
            DisplayName.SetDefault("Darksteel Battleaxe");
        }

        public override void SetDefaults()
        {
            Projectile.width = 66;
            Projectile.height = 81;
            Projectile.alpha = 0;
            Projectile.timeLeft = 71;
            Projectile.penetrate = 1000;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().channelProjectile = true;
            base.SetDefaults();
        }
        
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            //player.itemTime = 10;
            //player.noItems = true;

            if (Projectile.soundDelay == 0)
            {
                if (Projectile.ai[0] == 1)
                    Projectile.rotation = -MathHelper.PiOver2;
                else
                    Projectile.rotation = MathHelper.PiOver2;
            }
            Projectile.soundDelay = 100;

            if (Projectile.timeLeft == 26)
            {
                TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 71, -1f);
            }

            if (Projectile.timeLeft <= 26)
            {
                Projectile.friendly = true;
                if (Projectile.ai[0] == 1)
                {
                    Projectile.rotation += (2 * MathHelper.Pi) / 15;
                    Projectile.Center = Main.player[Projectile.owner].MountedCenter + new Vector2(33f, 40.5f).RotatedBy(Projectile.rotation);
                }
                else
                {
                    Projectile.spriteDirection = -1;
                    Projectile.rotation -= (2 * MathHelper.Pi) / 15;
                    Projectile.Center = Main.player[Projectile.owner].MountedCenter - new Vector2(33f, -40.5f).RotatedBy(Projectile.rotation);
                }
            }
            else
            {
                if (Projectile.ai[0] == 1)
                {
                    Projectile.rotation -= (2 * MathHelper.Pi) / 90;
                    Projectile.Center = Main.player[Projectile.owner].MountedCenter + new Vector2(33f, 40.5f).RotatedBy(Projectile.rotation);
                }
                else
                {
                    Projectile.spriteDirection = -1;
                    Projectile.rotation += (2 * MathHelper.Pi) / 90;
                    Projectile.Center = Main.player[Projectile.owner].MountedCenter - new Vector2(33f, -40.5f).RotatedBy(Projectile.rotation);
                }
            }

            player.direction = Projectile.spriteDirection;
            
            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            PLAYERGLOBAL player = Main.player[Projectile.owner].GetModPlayer<PLAYERGLOBAL>();

            player.lifeToHeal += 7;

            int stacks = target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().HemorrhageStacks;

            if (stacks < 4)
            {
                target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().HemorrhageStacks++;

                if (Main.netMode == NetmodeID.MultiplayerClient)
                    TerraLeagueNPCsGLOBAL.PacketHandler.SendSyncStats(Main.LocalPlayer.whoAmI, -1, 4, target.whoAmI, target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().HemorrhageStacks);
            }

            target.AddBuff(BuffType<Hemorrhage>(), 300);

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10;
            return true;
        }
    }
}
