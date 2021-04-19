using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using PotionOverhaul.UI.CustomSlot;
using System;
using System.Linq;

namespace PotionOverhaul.UI
{
	internal class AlchUI : UIState
	{
		UIImage Background = new UIImage(ModContent.GetTexture("PotionOverhaul/UI/Background"));
		public static CustomItemSlot[] IngredientSlot = new CustomItemSlot[3];
		public static CustomItemSlot PotionSlot = new CustomItemSlot();
		public UIImageButton BrewButton = new UIImageButton(ModContent.GetTexture("PotionOverhaul/UI/BrewButton"));
		public int[] OldIngredients = new int[3];
		public Item OldPotion = new Item();
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
			OldIngredients = IngredientSlot.Select(e => e.Item.type).ToArray();
		}
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			//Overengineering, restart
			if (IngredientSlot.All(m => m.Item.IsAir) && !PotionSlot.Item.IsAir && !(PotionSlot.Item.modItem as AlchPotion).Brewed)
			{
				PotionSlot.Item.TurnToAir();
				return;
			}
			if (!Enumerable.SequenceEqual(OldIngredients, IngredientSlot.Select(e => e.Item.type)) || PotionSlot.Item.IsAir && IngredientSlot.Any(a => !a.Item.IsAir))
			{
				if (PotionSlot.Item.IsAir) PotionSlot.Item.netDefaults(ModContent.ItemType<AlchPotion>());
				AlchPotion Potion = PotionSlot.Item.modItem as AlchPotion;
				if (!Potion.Brewed)
				{
					Potion.ItemsOnPotion.Clear();
					for (int i = 0; i < IngredientSlot.Length; i++)
					{
						if (!IngredientSlot[i].Item.IsAir)
						{
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
				}
			}
			OldIngredients = IngredientSlot.Select(e => e.Item.type).ToArray();
		}
		private void OnBrewButton(UIMouseEvent evt, UIElement listeningElement)
		{
			if (!PotionSlot.Item.IsAir)
			{
				PotionSlot.Item.alpha = 0;
				(PotionSlot.Item.modItem as AlchPotion).Brewed = true;
				for (int i = 0; i < IngredientSlot.Length; i++)
				{
					IngredientSlot[i].Item.stack--;
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
	}
}