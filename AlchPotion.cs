using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader.IO;
using System.Linq;

namespace PotionOverhaul
{
	public class AlchPotion : ModItem
	{
		public override bool CloneNewInstances => true;
		public List<Item> ItemsOnPotion;
		public bool Brewed;
		public int Style = 1;
		public Color AverageColor = new Color(1, 1, 1, 1);
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Strange Potion");
			Tooltip.SetDefault("\n");
		}
		public override void SetDefaults()
		{
			ItemsOnPotion = new List<Item>();
			item.width = 26;
			item.height = 28;
			item.useStyle = ItemUseStyleID.EatingUsing;
			item.useAnimation = 8;
			item.useTime = 8;
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
						var p = AlchEffects.AlchGetPositiveTooltip(ItemsOnPotion[i], ItemsOnPotion[i].stack);
						if (p != "")
							positive += p + (i == ItemsOnPotion.Count - 1 ? "" : "\n");
					}
					line.text = positive;
					line.overrideColor = Color.LightGreen;
				}
				if (line.Name == "Tooltip1")
				{
					string negative = "";
					for (int i = 0; i < ItemsOnPotion.Count; i++)
					{
						var n = AlchEffects.AlchGetNegativeTooltip(ItemsOnPotion[i], ItemsOnPotion[i].stack);
						if (n != "")
							negative += n + (i == ItemsOnPotion.Count - 1 ? "" : "\n");
					}
					line.text = negative;
					line.overrideColor = Color.OrangeRed;
				}
			}
		}
		public override bool UseItem(Player player)
		{
			Texture2D texture = ModContent.GetTexture("PotionOverhaul/PotionStyles/Glass" + Style);
			Main.itemTexture[item.type] = texture;
			player.GetModPlayer<AlchPlayer>().ItemsOnPotion = ItemsOnPotion;
			player.AddBuff(ModContent.BuffType<AlchBuff>(), 111600);
			return true;
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			spriteBatch.Draw(ModContent.GetTexture("PotionOverhaul/PotionStyles/Glass" + Style), position, frame, drawColor, 0f, origin, scale, SpriteEffects.None, default);
			spriteBatch.Draw(ModContent.GetTexture("PotionOverhaul/PotionStyles/Liquid" + Style), position, frame, AverageColor, 0f, origin, scale, SpriteEffects.None, default);
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture = ModContent.GetTexture("PotionOverhaul/PotionStyles/Glass" + Style);
			spriteBatch.Draw(texture, item.position - Main.screenPosition + new Vector2(item.width / 2, item.height / 2), null, lightColor, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
			spriteBatch.Draw(ModContent.GetTexture("PotionOverhaul/PotionStyles/Liquid" + Style), item.position - Main.screenPosition + new Vector2(item.width / 2, item.height / 2), null, AverageColor, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
			return false;
		}
		public override TagCompound Save()
		{
			var averagecolorvalues = new int[4] { AverageColor.R, AverageColor.G, AverageColor.B, AverageColor.A };
			return new TagCompound 
			{
				{"ItemsOnPotion", ItemsOnPotion},
				{"Brewed", Brewed},
				{"Style", Style},
				{"AverageColor", averagecolorvalues},
			};
		}
		public override void Load(TagCompound tag)
		{
			if (tag.ContainsKey("ItemsOnPotion"))
				ItemsOnPotion = tag.GetList<Item>("ItemsOnPotion").ToList();
			if (tag.ContainsKey("Brewed"))
				Brewed = tag.GetBool("Brewed");
			if (tag.ContainsKey("Style"))
				Style = tag.GetInt("Style");
			if (tag.ContainsKey("AverageColor"))
				AverageColor = new Color(tag.GetIntArray("AverageColor")[0], tag.GetIntArray("AverageColor")[1], tag.GetIntArray("AverageColor")[2], tag.GetIntArray("AverageColor")[3]);
		}
	}
}
 