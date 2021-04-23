using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using PotionOverhaul.UI.CustomSlot;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework.Input;

namespace PotionOverhaul.UI
{
	//Overengineering, restart
	internal class AlchUI : UIState
	{
		public static CustomItemSlot[] IngredientSlot = new CustomItemSlot[3];
		public static CustomItemSlot PotionSlot = new CustomItemSlot();
		public UIImage Background = new UIImage(ModContent.GetTexture("PotionOverhaul/UI/Background"));
		public UIImageButton BrewButton = new UIImageButton(ModContent.GetTexture("PotionOverhaul/UI/BrewButton"));
		public UIImageButton SaveButton = new UIImageButton(ModContent.GetTexture("PotionOverhaul/UI/SaveButton"));
		public int[] OldIngredients = new int[3];
		public bool Blocked;
		Color OldAverageColor = new Color(1, 1, 1, 1);
		Color AverageColor = new Color(1, 1, 1, 1);
		CroppedTexture2D backgroundTexture = new CroppedTexture2D(ModContent.GetTexture("PotionOverhaul/UI/Slot"), Color.White);
		CroppedTexture2D iconTextureIngredient = new CroppedTexture2D(ModContent.GetTexture("PotionOverhaul/UI/IngredientSlotIcon"), Color.White);
		CroppedTexture2D iconTexturePotion = new CroppedTexture2D(ModContent.GetTexture("PotionOverhaul/UI/PotionSlotIcon"), Color.White);
		public override void OnInitialize()
		{
			Background.HAlign = 0.7f;
			Background.VAlign = 0.5f;
			Append(Background);

			IngredientSlot[0] = new CustomItemSlot(0, 0.9f)
			{
				IsValidItem = item => item.IsAir || !item.IsAir && PotionOverhaul.PotionIngredients.Contains(item.type),
				BackgroundTexture = backgroundTexture,
				EmptyTexture = iconTextureIngredient,
				HoverText = "Ingredient"
			};
			IngredientSlot[0].HAlign = 0.275f;
			IngredientSlot[0].VAlign = 0.125f;
			Background.Append(IngredientSlot[0]);
			IngredientSlot[1] = new CustomItemSlot(0, 0.9f)
			{
				IsValidItem = item => item.IsAir || !item.IsAir && PotionOverhaul.PotionIngredients.Contains(item.type),
				BackgroundTexture = backgroundTexture,
				EmptyTexture = iconTextureIngredient,
				HoverText = "Ingredient"
			};
			IngredientSlot[1].HAlign = 0.50f;
			IngredientSlot[1].VAlign = 0.075f;
			Background.Append(IngredientSlot[1]);
			IngredientSlot[2] = new CustomItemSlot(0, 0.9f)
			{
				IsValidItem = item => item.IsAir || !item.IsAir && PotionOverhaul.PotionIngredients.Contains(item.type),
				BackgroundTexture = backgroundTexture,
				EmptyTexture = iconTextureIngredient,
				HoverText = "Ingredient"
			};
			IngredientSlot[2].HAlign = 0.725f;
			IngredientSlot[2].VAlign = 0.125f;
			Background.Append(IngredientSlot[2]);

			PotionSlot = new CustomItemSlot(0, 0.9f)
			{
				IsValidItem = item => item.IsAir,
				BackgroundTexture = backgroundTexture,
				EmptyTexture = iconTexturePotion,
				HoverText = "Potion"
			};
			PotionSlot.HAlign = 0.50f;
			PotionSlot.VAlign = 0.85f;
			PotionSlot.OnClick += OnPotionSlot;
			Background.Append(PotionSlot);

			BrewButton.HAlign = 0.50f;
			BrewButton.VAlign = 0.45f;
			BrewButton.OnClick += OnBrewButton;
			Background.Append(BrewButton);
			SaveButton.HAlign = 0.030f;
			SaveButton.VAlign = 0.970f;
			SaveButton.OnClick += OnSaveButton;
			Background.Append(SaveButton);

			OldIngredients = IngredientSlot.Select(e => e.Item.type).ToArray();
		}
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			if (IngredientSlot.All(m => m.Item.IsAir) && !PotionSlot.Item.IsAir && !(PotionSlot.Item.modItem as AlchPotion).Brewed)
			{
				PotionSlot.Item.TurnToAir();
				return;
			}
			if (Blocked)
			{
				if (!PotionSlot.Item.IsAir && !(PotionSlot.Item.modItem as AlchPotion).Brewed)
				{
					Blocked = false;
				}
			}
			if (!Enumerable.SequenceEqual(OldIngredients, IngredientSlot.Select(e => e.Item.type)) || PotionSlot.Item.IsAir && IngredientSlot.Any(a => !a.Item.IsAir))
			{
				if (PotionSlot.Item.IsAir)
				{
					PotionSlot.Item = new Item();
					PotionSlot.Item.SetDefaults(ModContent.ItemType<AlchPotion>());
				}
				AlchPotion Potion = PotionSlot.Item.modItem as AlchPotion;
				if (!Potion.Brewed)
				{
					Potion.ItemsOnPotion.Clear();
					OldAverageColor = AverageColor;
					AverageColor = new Color();
					for (int i = 0; i < IngredientSlot.Length; i++)
					{
						if (!IngredientSlot[i].Item.IsAir)
						{
							if (AverageColor.R == 0) AverageColor = ColorsToArrayToAverage(Main.itemTexture[IngredientSlot[i].Item.type]);
							else AverageColor = MixColor(AverageColor, ColorsToArrayToAverage(Main.itemTexture[IngredientSlot[i].Item.type]));
							if (Potion.ItemsOnPotion.FindAll(t => t.type == IngredientSlot[i].Item.type).Count > 0)
							{
								Potion.ItemsOnPotion.Find(t => t.type == IngredientSlot[i].Item.type).stack++;
							}
							else
							{
								Item ingredient = IngredientSlot[i].Item.Clone();
								ingredient.stack = 1;
								Potion.ItemsOnPotion.Add(ingredient);
							}
						}
					}
					PotionSlot.Item.alpha = 155;
					Potion.AverageColor = AverageColor;
					Potion.Style = Main.rand.Next(1, 2);

					Texture2D texture = ModContent.GetTexture("PotionOverhaul/PotionStyles/Style" + Potion.Style);
					Color[] pixels = new Color[texture.Width * texture.Height];
					texture.GetData(pixels);
					for (int x = 0; x < texture.Width; x++)
					{
						for (int y = 0; y < texture.Height; y++)
						{
							if (pixels[x + y * texture.Width] == Color.White || pixels[x + y * texture.Width] == OldAverageColor)
							{
								pixels[x + y * texture.Width] = AverageColor;
							}
						}
					}
					texture.SetData(pixels);
					Main.itemTexture[PotionSlot.Item.type] = texture;
				}
				else
				{
					Blocked = true;
				}
			}
			OldIngredients = IngredientSlot.Select(e => e.Item.type).ToArray();
		}
		private void OnBrewButton(UIMouseEvent evt, UIElement listeningElement)
		{
			if (!PotionSlot.Item.IsAir)
			{
				if (Blocked)
				{
					return;
				}
				if ((PotionSlot.Item.modItem as AlchPotion).Brewed)
				{
					if (Enumerable.SequenceEqual(OldIngredients, IngredientSlot.Select(e => e.Item.type)) && !IngredientSlot.Any(e => e.Item.stack == 0))
					{
						PotionSlot.Item.stack++;
						for (int i = 0; i < IngredientSlot.Length; i++)
						{
							IngredientSlot[i].Item.stack--;
						}
					}
				}
				else
				{
					PotionSlot.Item.alpha = 0;
					(PotionSlot.Item.modItem as AlchPotion).Brewed = true;
					for (int i = 0; i < IngredientSlot.Length; i++)
					{
						IngredientSlot[i].Item.stack--;
					}
				}
			}
		}
		private void OnPotionSlot(UIMouseEvent evt, UIElement listeningElement)
		{
			Item item = Main.LocalPlayer.trashItem;
			if (!item.IsAir && item.type == ModContent.ItemType<AlchPotion>() && !(item.modItem as AlchPotion).Brewed)
			{
				item.alpha = 0;
				(item.modItem as AlchPotion).Brewed = true;
				for (int i2 = 0; i2 < IngredientSlot.Length; i2++)
				{
					IngredientSlot[i2].Item.stack--;
				}
				return;
			}
			Item itemm = Main.mouseItem;
			if (!itemm.IsAir && itemm.type == ModContent.ItemType<AlchPotion>() && !(itemm.modItem as AlchPotion).Brewed)
			{
				itemm.alpha = 0;
				(itemm.modItem as AlchPotion).Brewed = true;
				for (int i2 = 0; i2 < IngredientSlot.Length; i2++)
				{
					IngredientSlot[i2].Item.stack--;
				}
			}
		}
		private void OnSaveButton(UIMouseEvent evt, UIElement listeningElement)
		{
			if (!IngredientSlot.All(i => i.Item.IsAir))
			{
				List<Item> ingredientlist = new List<Item>();
				for (int i = 0; i < IngredientSlot.Length; i++)
				{
					if (!IngredientSlot[i].Item.IsAir)
					{
						Item ingredient = IngredientSlot[i].Item.Clone();
						ingredient.stack = 1;
						ingredientlist.Add(ingredient);
					}
				}
				Main.LocalPlayer.GetModPlayer<AlchPlayer>().SavedPotions.Add(ingredientlist);
			}
		}
		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
			if (SaveButton.IsMouseHovering)
				Main.hoverItemName = "Save Recipe";
		}
		private Color ColorsToArrayToAverage(Texture2D texture)
		{
			Color[] colors = new Color[texture.Width * texture.Height];
			texture.GetData(colors);
			Color averagecolor = new Color();
			for (int x = 0; x < texture.Width; x++)
			{
				for (int y = 0; y < texture.Height; y++)
				{
					if (colors[x + y * texture.Width].A != 0)
					averagecolor = MixColor(averagecolor, colors[x + y * texture.Width]);
				}
			}
			return averagecolor;
		}
		private Color MixColor(Color color, Color color2)
		{
			int r = (color.R + color2.R) / 2;
			int g = (color.G + color2.G) / 2;
			int b = (color.B + color2.B) / 2;
			return new Color(Convert.ToByte(r), Convert.ToByte(g), Convert.ToByte(b));
		}
	}
}