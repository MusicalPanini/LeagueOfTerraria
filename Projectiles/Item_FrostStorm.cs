using Microsoft.Xna.Framework.Audio;
using TerraLeague.NPCs;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    class Item_FrostStorm : ModProjectile
    {
        int framecount2 = 29;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frost Storm");
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 400;
            Projectile.height = 400;
            Projectile.timeLeft = 600;
            Projectile.penetrate = 1000;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.scale = 1;
            Projectile.alpha = 180;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            if ((int)Projectile.ai[0] >= 0)
            {
                TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 82, -0.7f);
                TerraLeague.PlaySoundWithPitch(Projectile.Center, 2, 45, -0.5f);
                Projectile.ai[0] = -1;
            }

            if (!Main.player[Projectile.owner].GetModPlayer<PLAYERGLOBAL>().frostHarbinger)
                Projectile.Kill();

            Projectile.Center = Main.player[Projectile.owner].Center;

            if (Projectile.timeLeft < 15)
            {
                Projectile.alpha += 5;
            }

            if (Projectile.timeLeft % 15 == 0)
            {
                Targeting.GiveNPCsInRangeABuff(Projectile.Center, Projectile.width / 2, BuffType<Buffs.Slowed>(), 15, true, true);
            }

            AnimateProjectile();
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().harbingersInferno)
            {
                if (target.townNPC)
                    return false;
                return Targeting.IsHitboxWithinRange(Projectile.Center, target.Hitbox, Projectile.width / 2f);
            }
            else
                return false;
        }

        public void AnimateProjectile()
        {
            Projectile.friendly = false;
            Projectile.frameCounter++;
            framecount2++;
            if (Projectile.frameCounter >= 5)
            {
                Projectile.frame++;
                Projectile.frame %= 4;
                Projectile.frameCounter = 0;
            }
            if (framecount2 >= 20)
            {
                Projectile.friendly = true;
                framecount2 = 0;
            }
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
