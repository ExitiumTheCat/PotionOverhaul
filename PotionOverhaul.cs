using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.ID;
using PotionOverhaul.UI;
using System.Collections.Generic;

namespace PotionOverhaul
{
	public class PotionOverhaul : Mod
	{
		internal UserInterface InterfaceAlchUI;
		internal AlchUI AlchUI;
		private GameTime _lastUpdateUiGameTime;
		public override void Load()
		{
			if (!Main.dedServ)
			{
				InterfaceAlchUI = new UserInterface();
				AlchUI = new AlchUI();
				AlchUI.Activate();
				InterfaceAlchUI?.SetState(AlchUI);
			}
		}
		public override void Unload()
		{
			AlchUI = null;
		}
		public override void UpdateUI(GameTime gameTime)
		{
			_lastUpdateUiGameTime = gameTime;
			if (InterfaceAlchUI?.CurrentState != null)
			{
				InterfaceAlchUI.Update(gameTime);
			}
		}
		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
			if (mouseTextIndex != -1)
			{
				layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
					"PotionOverhaul: InterfaceAlchUI",
					delegate
					{
						if (_lastUpdateUiGameTime != null && InterfaceAlchUI?.CurrentState != null)
						{
							InterfaceAlchUI.Draw(Main.spriteBatch, _lastUpdateUiGameTime);
						}
						return true;
					},
					   InterfaceScaleType.UI));
			}
		}
		public static List<int> PotionIngredients = new List<int>()
		{
			ItemID.Daybloom,
			ItemID.Fireblossom,
			ItemID.Shiverthorn
		};
	}
}