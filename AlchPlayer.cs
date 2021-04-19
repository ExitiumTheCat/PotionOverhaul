using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using System.Linq;

namespace PotionOverhaul
{
	class AlchPlayer : ModPlayer
	{
		public List<Item> ItemsOnPotion = new List<Item>();
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
	}
}