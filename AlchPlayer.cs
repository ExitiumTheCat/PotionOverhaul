using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using System.Linq;
using Terraria.ModLoader.IO;

namespace PotionOverhaul
{
	class AlchPlayer : ModPlayer
	{
		public List<Item> ItemsOnPotion = new List<Item>();
		public List<List<Item>> SavedPotions = new List<List<Item>>();
		public override void PreUpdate()
		{
			if (ItemsOnPotion.Count > 0 && !player.HasBuff(ModContent.BuffType<AlchBuff>()))
				ItemsOnPotion.Clear();
		}
		public override void PostUpdateEquips()
		{
			if (!player.HasBuff(ModContent.BuffType<AlchBuff>())) return;
			for (int i = 0; i < ItemsOnPotion.Count(); i++)
			{
				AlchEffects.AlchPostUpdateEquip(player, ItemsOnPotion[i].stack, ItemsOnPotion[i]);
			}
		}
		public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
		{
			if (!player.HasBuff(ModContent.BuffType<AlchBuff>())) return;
			for (int i = 0; i < ItemsOnPotion.Count(); i++)
			{
				AlchEffects.AlchOnMeleeHit(player, ItemsOnPotion[i].stack, ItemsOnPotion[i], target, damage, knockback, crit);
			}
		}
		public override TagCompound Save()
		{
			return new TagCompound
			{
				{"SavedPotions", SavedPotions},
				{"ItemsOnPotion", ItemsOnPotion}
			};
		}
		public override void Load(TagCompound tag)
		{
			if (tag.ContainsKey("SavedPotions"))
				SavedPotions = tag.GetList<List<Item>>("SavedPotions").ToList();
			if (tag.ContainsKey("ItemsOnPotion"))
				ItemsOnPotion = tag.GetList<Item>("ItemsOnPotion").ToList();
		}
	}
}