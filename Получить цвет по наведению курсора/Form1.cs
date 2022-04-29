using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Markup;
using System.Web;
using System.Web.Helpers;
using System.Speech.Synthesis;
using System.Speech.Recognition;

namespace Получить_цвет_по_наведению_курсора
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}
		//static string GetColorNameRUSjson(System.Web.Helpers.DynamicJsonArray DecodedJson, in System.Drawing.Color c)
		//{
		//	for (int i = 0; i < DecodedJson.Length; i++)
		//	{
		//		if (((System.Drawing.Color)System.Drawing.ColorTranslator.FromHtml(DecodedJson[0][0])).ToArgb() == c.ToArgb())
		//		{
		//			return DecodedJson[i][1];
		//		}
		//	}
		//	return null;
		//}
		
		ScreenShoter SS;
		System.Speech.Synthesis.SpeechSynthesizer Speaker;
		bool DoWork;
		private void Form1_Load(object sender, EventArgs e)
		{
			CheckTopMost.Checked = Properties.Settings.Default.TopMost;
			CheckDoSpeak.Checked = Properties.Settings.Default.DoSpeak;
			TopMost = CheckTopMost.Checked;
			DoWork = CheckDoSpeak.Checked;

			//ColorsNames w = new ColorsNames(Color.AliceBlue, "");
			//new ColorsNames();
			//MessageBox.Show(ColorsNames.colors[0].ToString());
			Speaker = new System.Speech.Synthesis.SpeechSynthesizer();
			Speaker.SetOutputToDefaultAudioDevice();

			SS = new ScreenShoter();
			CursorPositionChanged();
			ScreenS();
		}
		async void ScreenS()
		{
			int i = 1;
			var colorsNames = new ColorsNames();
			await Task.Run(() =>
			{
				while (true)
				{
					SS.ScreenShot(CurretPosition.X, CurretPosition.Y);

					colorsNames = SS.GetPixelColor(0, 0);
					if (colorsNames?._name != null)
					{
						Invoke(new Action(() =>
						{
							pictureBox1.BackColor = colorsNames._color;
							textBox2.Text = colorsNames._name;
						}));
						if (DoWork)
						{
							if (Speaker.State == SynthesizerState.Speaking)
							{
								i++;
							}
							else if (Speaker.State == SynthesizerState.Ready)
							{
								Speaker.SpeakAsync(textBox2.Text/* + " " + i*/);
								i = 1;
							}
						}
					}
					else
					{
						Invoke(new Action(() => { textBox2.Text = "null"; }));
					}
					Thread.Sleep(16);
				}
			});
		}

		Point CurretPosition;
		async void CursorPositionChanged()
		{
			await Task.Run(() =>
			{
				Point LastPosition = Point.Empty;
				while (true)
				{
					CurretPosition = Cursor.Position;
					if (LastPosition != CurretPosition)
					{
						LastPosition = CurretPosition;

						Invoke(new Action(() => textBox1.Text = LastPosition.ToString()));
					}
					Thread.Sleep(16);
				}
			});
		}
		private void numericUpDown1_ValueChanged(object sender, EventArgs e)
		{
			Speaker.Rate = ((int)numericUpDown1.Value);
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			Properties.Settings.Default.TopMost = CheckTopMost.Checked;
			Properties.Settings.Default.DoSpeak = CheckDoSpeak.Checked;
			Properties.Settings.Default.Save();
			Application.Exit();
		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			base.TopMost = CheckTopMost.Checked;
		}

		private void checkBox2_CheckedChanged(object sender, EventArgs e)
		{
			DoWork = CheckDoSpeak.Checked;
		}
	}
}
