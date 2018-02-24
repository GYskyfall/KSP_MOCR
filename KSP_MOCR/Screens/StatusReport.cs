﻿using System;
using System.Threading;

namespace KSP_MOCR
{
	public class StatusReport : MocrScreen
	{
		private String positionCode;
		
		public StatusReport(Screen form, String positionCode)
		{
			this.positionCode = positionCode;
			
			this.form = form;

			this.width = 27;
			this.height = 7;
		}
		
		public override void makeElements()
		{
			for (int i = 0; i < 1; i++) screenLabels.Add(null); // Initialize Labels
			for (int i = 0; i < 3; i++) screenButtons.Add(null); // Initialize Buttons

			for (int i = 0; i < 1; i++) screenInputs.Add(null); // Initialize Inputs
			screenInputs[0] = Helper.CreateInput(-2, -2, 1, 2); // Every page must have an input to capture keypresses on Unix

			screenLabels[0] = Helper.CreateLabel(0, 0, 27, 1, Helper.prtlen(positionCode.ToUpper() + " STATUS REPORT", 27, Helper.Align.CENTER));

			screenButtons[0] = Helper.CreateButton(0, 1, 9, 6, "");
			screenButtons[0].buttonStyle = MocrButton.style.LIGHT;
			screenButtons[0].setLightColor(MocrButton.color.RED);
			screenButtons[0].Click += (sender, e) => clickButton(sender, e, screenButtons[0]);
			
			screenButtons[1] = Helper.CreateButton(9, 1, 9, 6, "");
			screenButtons[1].buttonStyle = MocrButton.style.LIGHT;
			screenButtons[1].setLightColor(MocrButton.color.AMBER);
			screenButtons[1].Click += (sender, e) => clickButton(sender, e, screenButtons[1]);
			
			screenButtons[2] = Helper.CreateButton(18, 1, 9, 6, "");
			screenButtons[2].buttonStyle = MocrButton.style.LIGHT;
			screenButtons[2].setLightColor(MocrButton.color.GREEN);
			screenButtons[2].Click += (sender, e) => clickButton(sender, e, screenButtons[2]);

			form.dataStorage.Pull();
			Thread.Sleep(1000);
			setButtonColor(positionCode + "S");
		}
		
		public override void updateLocalElements(object sender, EventArgs e)
		{
			setButtonColor(positionCode + "S");
		}
		
		private void setButtonColor(String key)
		{
			String colorData = form.dataStorage.getData(key);
			switch (colorData)
			{
				case "RED":
					screenButtons[0].setLitState(true);
					screenButtons[1].setLitState(false);
					screenButtons[2].setLitState(false);
					break;
					
				case "AMBER":
					screenButtons[0].setLitState(false);
					screenButtons[1].setLitState(true);
					screenButtons[2].setLitState(false);
					break;
					
				case "GREEN":
					screenButtons[0].setLitState(false);
					screenButtons[1].setLitState(false);
					screenButtons[2].setLitState(true);
					break;
					
				case "BLANK":
					screenButtons[0].setLitState(false);
					screenButtons[1].setLitState(false);
					screenButtons[2].setLitState(false);
					break;
			}
		}

		private void clickButton(object sender, EventArgs e, MocrButton button)
		{
			bool lit = button.lit;

			// Reset all
			screenButtons[0].setLitState(false);
			screenButtons[1].setLitState(false);
			screenButtons[2].setLitState(false);

			if (!lit)
			{
				button.setLitState(true);
				form.dataStorage.setData(positionCode + "S", button.buttonColor.ToString());
			}
			else
			{
				form.dataStorage.setData(positionCode + "S", MocrButton.color.BLANK.ToString());
			}
		}
	}
}
