using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace PotionOverhaul
{
	public class AlchBuff : ModBuff
	{
		public List<Item> ItemsOnPotion = new List<Item>();
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Potion Effects!");
		}
		public override void ModifyBuffTip(ref string tip, ref int rare)
		{
			var positive = "";
			var negative = "";
			for (int i = 0; i < ItemsOnPotion.Count; i++)
			{
				var p = AlchEffects.AlchGetPositiveTooltip(ItemsOnPotion[i], ItemsOnPotion[i].stack);
				if (p != "")
					positive += p + (i == ItemsOnPotion.Count - 1 ? "" : "\n");
				var n = AlchEffects.AlchGetNegativeTooltip(ItemsOnPotion[i], ItemsOnPotion[i].stack);
				if (n != "")
					negative += n + (i == ItemsOnPotion.Count - 1 ? "" : "\n");
			}
			tip = positive + "\n" + negative;
		}
	}
}