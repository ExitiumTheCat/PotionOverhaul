using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PotionOverhaul
{
	public class AlchEffects
	{
		public static float DiminishingReturns(float number, int amount, float amountToDiminish, float rate)
		{
			float f = 0f;
			for (int a = 0; a < amount; a++)
			{
				f += number - amountToDiminish * (a * rate);
			}
			return f;
		}
		public static string AlchGetPositiveTooltip(Item item, int amount)
		{
			switch (item.type)
			{
				case (ItemID.Daybloom):
					return "+ Slightly increases some stats during daytime";
				case (ItemID.Fireblossom):
					string Percentage = DiminishingReturns(0.25f, amount, 0.05f, 1f).ToString().Replace("0.", "").Replace("f", "");
					if (Percentage.Length == 1) Percentage += "0";
					return $"+ {Percentage}% of inflicting On fire! on hit";
					//Todo implement this
				case (ItemID.Shiverthorn):
					return "+ Repeatedly striking enemies makes them more brittle!";
				default:
					return "";
			}
		}
		public static string AlchGetNegativeTooltip(Item item, int amount)
		{
			switch (item.type)
			{
				case (ItemID.Shiverthorn):
					return "- You are more brittle!";
				default:
					return "";
			};
		}
		public static void AlchPostUpdateEquip(Player player, int amount, Item item)
		{
			switch (item.type)
			{
				case (ItemID.Daybloom):
					if (Main.dayTime)
					{
						player.statDefense += 2 + (1 * amount);
						player.meleeSpeed += 0.03f + (0.01f * amount + amount > 1 ? 0.01f : 0f);
						player.allDamage += 0.03f + (0.01f * amount + amount > 1 ? 0.01f : 0f);
						player.moveSpeed += 0.2f + (0.1f * amount);
					}
					break;
			}
		}
		public static void AlchOnMeleeHit(Player player, int amount, Item item, NPC target, int damage, float knockback, bool crit)
		{
			switch (item.type)
			{
				case (ItemID.Fireblossom):
					if (Main.rand.NextFloat() > DiminishingReturns(0.25f, amount, 0.05f, 1f))
					{
						target.AddBuff(BuffID.OnFire, 240);
					}
					break;
			}
		}
	}
}