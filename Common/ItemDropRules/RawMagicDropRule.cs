using Terraria;
using Terraria.GameContent.ItemDropRules;

namespace TerraLeague.Common.ItemDropRules
{
	public class RawMagicDropRule : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info)
		{
			if (!info.IsInSimulation)
			{
				return !info.npc.SpawnedFromStatue && !info.npc.townNPC && !info.npc.CountsAsACritter && info.npc.lifeMax > 5;
			}
			return false;
		}

		public bool CanShowItemDropInUI()
		{
			return false;
		}

		public string GetConditionDescription()
		{
			return "Drops from all hostile mobs";
		}
	}
}
