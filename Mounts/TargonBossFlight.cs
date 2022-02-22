using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Mounts
{
    public class TargonBossFlight : ModMount
    {
        public override void Load()
        {
            IL.Terraria.Mount.DoesHoverIgnoresFatigue += HookDoesHoverIgnoresFatigure;
            base.Load();
        }

        private static void HookDoesHoverIgnoresFatigure(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (!c.TryGotoNext(i => i.MatchRet()))
            {
                return; // Patch unable to be applied
            }

            c.Emit(OpCodes.Ldarg_0);
            c.EmitDelegate<Func<bool, Mount, bool>>((returnValue, mount) => {
                if (mount.Type == MountType<TargonBossFlight>())
                {
                    return true;
                }
                else
                {
                    return returnValue;
                }
            });
        }

        public override void SetStaticDefaults()
        {
			//MountData.spawnDust = 226;
			//MountData.spawnDustNoGravity = true;
			MountData.buff = BuffType<InTargonArena>();
			MountData.heightBoost = 0;
			MountData.flightTimeMax = (int)Main.nightLength;
			MountData.fatigueMax = (int)Main.nightLength;
			MountData.fallDamage = 0f;
			MountData.usesHover = true;
			MountData.runSpeed = 8f;
			MountData.dashSpeed = 8f;
			MountData.acceleration = 0.16f;
			MountData.jumpHeight = 10;
			MountData.jumpSpeed = 4f;
			MountData.blockExtraJumps = true;

            //MountData.xOffset = 1;
            //MountData.bodyFrame = 3;
            //MountData.yOffset = 4;
            //MountData.playerHeadOffset = 18;
            MountData.totalFrames = 1; // Amount of animation frames for the mount
            MountData.playerYOffsets = Enumerable.Repeat(0, MountData.totalFrames).ToArray(); // Fills an array with values for less repeating code
            MountData.xOffset = 0;
            MountData.yOffset = 0;
            MountData.playerHeadOffset = 0;
            MountData.bodyFrame = 5;

            MountData.standingFrameCount = 1;
            MountData.standingFrameDelay = 4;
            MountData.standingFrameStart = 0;
            MountData.runningFrameCount = 1;
            MountData.runningFrameDelay = 4;
            MountData.runningFrameStart = 0;
            MountData.flyingFrameCount = 1;
            MountData.flyingFrameDelay = 4;
            MountData.flyingFrameStart = 0;
            MountData.inAirFrameCount = 1;
            MountData.inAirFrameDelay = 4;
            MountData.inAirFrameStart = 0;
            MountData.idleFrameCount = 1;
            MountData.idleFrameDelay = 12;
            MountData.idleFrameStart = 0;
            MountData.idleFrameLoop = true;
            MountData.swimFrameCount = 1;
            MountData.swimFrameDelay = 12;
            MountData.swimFrameStart = 0;

            if (!Main.dedServ)
			{
				MountData.textureWidth = MountData.backTexture.Width();
				MountData.textureHeight = MountData.backTexture.Height();
			}
		}

        public override void UpdateEffects(Player player)
        {
            for (int i = 0; i < player.velocity.Length() / 7f; i++)
            {
                Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.PortalBolt, 0, 0, 225, Microsoft.Xna.Framework.Color.Blue, player.velocity.Length() / 5f);
                dust.noLight = true;
                dust.noGravity = true;
                dust.velocity *= 0.1f;
            }
            base.UpdateEffects(player);
        }

        public override void SetMount(Player player, ref bool skipDust)
        {
            skipDust = true;

            base.SetMount(player, ref skipDust);
        }
    }
}
