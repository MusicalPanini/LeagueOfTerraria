using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TerraLeague.Projectiles.Homing;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class Item_EnergizedBolt : RichochetProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Energized Bolt");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.timeLeft = 300;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
            Projectile.extraUpdates = 8;
            Projectile.netImportant = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 5;

            Projectile.penetrate = (int)Projectile.ai[1] == 1 ? 9 : 6;
            CanOnlyHitTarget = true;
            NPC_CanTargetDummy = true;
            NPC_CanTargetCritters = true;
            MaxVelocity = 6;
        }

        public override void AI()
        {
            if (Projectile.soundDelay == 0)
            {
                Projectile.penetrate = (int)Projectile.ai[1] == 1 ? 9 : 6;
                SetHomingDefaults(true, 480, 301);
            }
            Projectile.soundDelay = 100;

            if (Projectile.timeLeft == 300)
            {
                Terraria.Audio.SoundEngine.PlaySound(new LegacySoundStyle(3, 53), Projectile.position);
            }

            for (int i = 0; i < 4; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.AncientLight, 0, 0, 0, new Color(255, 255, 0, 150), 1f);
                dust.velocity *= i == 3? 3 : 0.3f;
                dust.scale = i == 4 ? 1.25f : 1;
                dust.noGravity = true;
            }

            Lighting.AddLight(Projectile.position, 1f, 1f, 0f);
            base.AI();
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }
    }
}
