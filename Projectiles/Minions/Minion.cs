using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria;

namespace TerraLeague.Projectiles.Minions
{
    public class Minion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = Main.projFrames[ProjectileID.OneEyedPirate];
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            ProjectileID.Sets.CountsAsHoming[Projectile.type] = true;
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.aiStyle = 67;
            Projectile.width = 52;
            Projectile.height = 40;
            Projectile.timeLeft = 18000;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.minion = true;
            Projectile.minionSlots = 1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.scale = 1;
        }

        public override void AI()
        {
            Projectile.penetrate = -1;
            Main.projPet[Projectile.type] = true;
            Player player = Main.player[Projectile.owner];
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            if (player.dead)
            {
                modPlayer.minions = false;
            }
            if (modPlayer.minions)
            {
                Projectile.timeLeft = 2;
            }

            base.AI();
        }

        public override bool MinionContactDamage()
        {
            return true;
        }

    }
}
