using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using PotionOverhaul.UI.CustomSlot;

namespace PotionOverhaul.UI
{
	internal class AlchUI : UIState
	{
		UIImage Background = new UIImage(ModContent.GetTexture("PotionOverhaul/UI/Background"));
		public static CustomItemSlot[] IngredientSlot = new CustomItemSlot[3];
		public override void OnInitialize()
		{
			Background.HAlign = 0.7f;
			Background.VAlign = 0.5f;
			Append(Background);

			CroppedTexture2D emptyTexture = new CroppedTexture2D(ModContent.GetTexture("PotionOverhaul/UI/Placeholder"), CustomItemSlot.DefaultColors.EmptyTexture);
			IngredientSlot[0] = new CustomItemSlot(0, 0.83f)
			{
				IsValidItem = item => item.IsAir || !item.IsAir && PotionOverhaul.PotionIngredients.Contains(item.type),
				EmptyTexture = emptyTexture,
				HoverText = "Ingredient"
			};
			IngredientSlot[0].HAlign = 0.30f;
			IngredientSlot[0].VAlign = 0.10f;
			Background.Append(IngredientSlot[0]);
			IngredientSlot[1] = new CustomItemSlot(0, 0.83f)
			{
				IsValidItem = item => item.IsAir || !item.IsAir && PotionOverhaul.PotionIngredients.Contains(item.type),
				EmptyTexture = emptyTexture,
				HoverText = "Ingredient"
			};
			IngredientSlot[1].HAlign = 0.50f;
			IngredientSlot[1].VAlign = 0.05f;
			Background.Append(IngredientSlot[1]);
			IngredientSlot[2] = new CustomItemSlot(0, 0.83f)
			{
				IsValidItem = item => item.IsAir || !item.IsAir && PotionOverhaul.PotionIngredients.Contains(item.type),
				EmptyTexture = emptyTexture,
				HoverText = "Ingredient"
			};
			IngredientSlot[2].HAlign = 0.70f;
			IngredientSlot[2].VAlign = 0.10f;
			Background.Append(IngredientSlot[2]);
		}
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}
	}
}