using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace PotionOverhaul
{
	public class AlchPotion : ModItem
	{
		public List<Item> ItemsOnPotion = new List<Item>();
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Strange Potion");
			Tooltip.SetDefault("");
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
		public override bool UseItem(Player player)
		{
			player.GetModPlayer<AlchPlayer>().ItemsOnPotion = ItemsOnPotion;
			player.AddBuff(ModContent.BuffType<AlchBuff>(), 120);
			return true;
		}
	}
}