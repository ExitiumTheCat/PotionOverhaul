using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace PotionOverhaul
{
	public class AlchPotion : ModItem
	{
		public override bool CloneNewInstances => true;
		public List<Item> ItemsOnPotion = new List<Item>();
		public bool Brewed;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Strange Potion");
			Tooltip.SetDefault("\n");
		}
		public override void SetDefaults()
		{
			item.width = 1;
			item.height = 1;
			item.useStyle = ItemUseStyleID.EatingUsing;
			item.useAnimation = 16;
			item.useTime = 16;
			item.useTurn = true;
			item.UseSound = SoundID.Item3;
			item.maxStack = 1;
			item.consumable = true;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			foreach (TooltipLine line in tooltips)
			{
				if (line.Name == "Tooltip0")
				{
					string positive = "";
					for (int i = 0; i < ItemsOnPotion.Count; i++)
					{
						positive += AlchEffects.AlchGetPositiveTooltip(ItemsOnPotion[i], ItemsOnPotion[i].stack) + (i == ItemsOnPotion.Count - 1 ? "" : "\n");
					}
					line.text = positive;
					line.overrideColor = Color.LightGreen;
				}
				if (line.Name == "Tooltip1")
				{
					string negative = "";
					for (int i = 0; i < ItemsOnPotion.Count; i++)
					{
						negative += AlchEffects.AlchGetNegativeTooltip(ItemsOnPotion[i], ItemsOnPotion[i].stack) + (AlchEffects.AlchGetNegativeTooltip(ItemsOnPotion[i], ItemsOnPotion[i].stack) == "" ? "" : "\n");
					}
					line.text = negative;
					line.overrideColor = Color.OrangeRed;
				}
			}
		}
		public override bool UseItem(Player player)
		{
			player.GetModPlayer<AlchPlayer>().ItemsOnPotion = ItemsOnPotion;
			player.AddBuff(ModContent.BuffType<AlchBuff>(), 120);
			return true;
		}
	}
}