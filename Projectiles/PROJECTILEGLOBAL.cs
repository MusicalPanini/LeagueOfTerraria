using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;


namespace TerraLeague.Projectiles
{
    public class PROJECTILEGLOBAL : GlobalProjectile
    {
        internal ProjectilePacketHandler PacketHandler = new ProjectilePacketHandler(3);
        public bool summonAbility = false;
        public bool abilitySpell = false;
        public bool channelProjectile = false;
        public bool playerInvincible = false;
        public bool noOnHitEffects = false;

        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        public override void SetDefaults(Projectile projectile)
        {
            int type = projectile.type;
            //if (type == 195 || type == 433 || type == 374 || type == 376 || type == 389 || type == 408 || type == 379 || type == 309 || type == 642 || type == 644 || type == 680 || type == 664 || type == 666 || type == 668 || type == 694 || type == 695 || type == 696)
            //    projectile.minion = true;

            base.SetDefaults(projectile);
        }

        public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
        {
            PLAYERGLOBAL modPlayer = Main.player[projectile.owner].GetModPlayer<PLAYERGLOBAL>();

            if (modPlayer.pirateSet  && !target.immortal && !target.SpawnedFromStatue && target.lifeMax > 5 && projectile.DamageType == DamageClass.Ranged)
            {
                if(Main.rand.Next(0, 5) == 0)
                    Item.NewItem(target.getRect(), ItemID.CopperCoin);
                if (Main.rand.Next(0, 50) == 0)
                    Item.NewItem(target.getRect(), ItemID.SilverCoin);
                if (Main.rand.Next(0, 5000) == 0)
                    Item.NewItem(target.getRect(), ItemID.GoldCoin);
                if (Main.rand.Next(0, 500000) == 0)
                    Item.NewItem(target.getRect(), ItemID.PlatinumCoin);
            }

            base.OnHitNPC(projectile, target, damage, knockback, crit);
        }

        public override bool PreAI(Projectile projectile)
        {
            if (channelProjectile)
                Main.player[projectile.owner].GetModPlayer<PLAYERGLOBAL>().AbilityChannel = true;

            return base.PreAI(projectile);
        }

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (abilitySpell)
            {
                crit = (Main.player[projectile.owner].GetModPlayer<PLAYERGLOBAL>().arcanePrecision && Main.rand.Next(0, 100) < ArcanePrecision.critChance);
            }
            else if (projectile.minion)
            {
                crit = (Main.player[projectile.owner].GetModPlayer<PLAYERGLOBAL>().haunted && Main.rand.Next(0, 100) < Haunted.critChance);
            }
            else
            {
                base.ModifyHitNPC(projectile, target, ref damage, ref knockback, ref crit, ref hitDirection);
            }
        }

        public bool DoNotCountRangedDamage(Projectile proj)
        {
            if (proj.Name == "Crystal Shard"
                || proj.Name == "Hallow Star"
                )
                return true;
            else
                return false;
        }

        public override bool CanHitPlayer(Projectile projectile, Player target)
        {
            PLAYERGLOBAL modPlayer = target.GetModPlayer<PLAYERGLOBAL>();

            if (modPlayer.projectileDodge)
                return false;
            else
                return base.CanHitPlayer(projectile, target);
        }

        public void SyncProjectileKill(Projectile projectile)
        {
            if (Main.LocalPlayer.whoAmI != projectile.whoAmI)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    if (Main.npc[i].active && ((projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().CanHitNPC(projectile, Main.npc[i]) != null && projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().CanHitNPC(projectile, Main.npc[i]) == true)))
                    {
                        if (projectile.Hitbox.Intersects(Main.npc[i].Hitbox))
                            projectile.Kill();
                    }
                }
            }
        }

        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            Vector2 vector = projectile.position + oldVelocity;
            int num = (int)(projectile.position.X / 16f) - 1;
            int num2 = (int)((projectile.position.X + (float)projectile.width) / 16f) + 2;
            int num3 = (int)(projectile.position.Y / 16f) - 1;
            int num4 = (int)((projectile.position.Y + (float)projectile.height) / 16f) + 2;
            if (num < 0)
            {
                num = 0;
            }
            if (num2 > Main.maxTilesX)
            {
                num2 = Main.maxTilesX;
            }
            if (num3 < 0)
            {
                num3 = 0;
            }
            if (num4 > Main.maxTilesY)
            {
                num4 = Main.maxTilesY;
            }
            for (int i = num; i < num2; i++)
            {
                for (int j = num3; j < num4; j++)
                {
                    if (Main.tile[i, j].type == ModContent.TileType<Tiles.CrystalBomb>() && WorldGen.CanCutTile(i, j, Terraria.Enums.TileCuttingContext.AttackProjectile))
                    {
                        WorldGen.KillTile(i, j, false, false, false);
                    }
                }
            }

            return base.OnTileCollide(projectile, oldVelocity);
        }
    }
}
