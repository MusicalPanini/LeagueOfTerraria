using Terraria;
using Terraria.GameContent.ItemDropRules;

namespace TerraLeague.Common.ItemDropRules
{
	public class KeyDropRule : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info)
		{
			if (!info.IsInSimulation)
			{
				return Main.hardMode && !info.npc.SpawnedFromStatue && !info.npc.townNPC && !info.npc.CountsAsACritter;
			}
			return false;
		}

		public bool CanShowItemDropInUI()
		{
			return false;
		}

		public string GetConditionDescription()
		{
			return "Drops in hardmode";
		}
	}
}
