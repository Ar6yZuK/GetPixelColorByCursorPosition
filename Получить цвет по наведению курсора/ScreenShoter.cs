using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Получить_цвет_по_наведению_курсора
{
	public class ScreenShoter
	{ 
		// NEED:
		// DpiAware = true;.

		private static Graphics g;
		static public int Width;
		static public int Height;
		static public Size SizeScreen;
		static public Bitmap ScreenShotImage;

		static public Bitmap Screenshot => ScreenShotImage;
		public Bitmap GetScreenShot => ScreenShotImage;

		public ScreenShoter()
		{
			//ScreenShotImage = new Bitmap((int)Width, (int)Height);
			g = Graphics.FromImage(ScreenShotImage);
		}
		static ScreenShoter()
		{
			ScreenShotImage = new Bitmap(1, 1);
			Width = (int)(Screen.PrimaryScreen.Bounds.Width);
			Height = (int)(Screen.PrimaryScreen.Bounds.Height);
			SizeScreen = new Size(Width, Height);
		}
		public ColorsNames GetPixelColor(ushort x, ushort y)
		{
			for (int i = 0; i < ColorsNames.colors.Length; i++)
			{
				if(ColorsNames.colors[i]._argb == ScreenShotImage.GetPixel(x, y).ToArgb())
				{
					return ColorsNames.colors[i];
				}
			}
			return null;
		}
		public ColorsNames GetPixelColor(Point point)
		{
			return GetPixelColor((ushort)point.X, (ushort)point.Y);
		}
		public void ScreenShotFull()
		{
			g.CopyFromScreen(0, 0, 0, 0, SizeScreen);
		}
		public void ScreenShot(int sourceX, int sourceY)
		{
			//ScreenShotImage = new Bitmap(1, 1);
			g = Graphics.FromImage(ScreenShotImage);

			g.CopyFromScreen(sourceX, sourceY, 0, 0, new Size(1, 1));
		}
	}
}
