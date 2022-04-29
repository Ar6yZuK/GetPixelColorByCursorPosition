using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace Получить_цвет_по_наведению_курсора
{
	public class ColorsNames
	{
		public static ColorsNames[] colors;
		static ColorsNames()
		{
			JsonColors j = new JsonColors();
			j.Collect(Properties.Resources.colors_russian);
			//new JsonColors();
		}

		public Color _color;
		public string _htmlColor;
		public string _name;
		public int _argb;

		//public Color Color
		//{
		//	get { return _color; }
		//}
		//public string HtmlColor
		//      {
		//	get { return _htmlColor; }
		//      }
		//public string Name
		//{
		//	get { return _name; }
		//}
		static public bool AddColor(Color color, string name, string htmlColor, int argb)
		{
			for (int i = 0; i < colors.Length; i++)
			{
				if(AddColorOfIndex(color, name, htmlColor, argb, i))
				{
					return true;
				}
			}
			return false;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="color"></param>
		/// <param name="name"></param>
		/// <param name="htmlColor"></param>
		/// <param name="index"></param>
		/// <returns>Возвращает true если успешно, иначе false если не null</returns>
		static public bool AddColorOfIndex(Color color, string name, string htmlColor, int argb, int index)
		{
			if (colors[index] == null)
			{
				colors[index] = new ColorsNames();
				colors[index]._color = color;
				colors[index]._name = name;
				colors[index]._htmlColor = htmlColor;
				colors[index]._argb = argb;
				return true;
			}
			return false;
		}

		public override string ToString()
		{
			return "{Name=" + _name + ",ARGB="+ $"{_color.A} {_color.R} {_color.G} {_color.B}" + ",Html=" + _htmlColor + "}";
		}

		public bool Equals(ColorsNames objA, ColorsNames objB)
		{
			return objA is ColorsNames namesA && objB is ColorsNames namesB && namesA._name == namesB._name && namesA._color == namesB._color && namesA._htmlColor == namesB._htmlColor && namesA._argb == namesB._argb;
		}
	}
	class JsonColors
	{
		public void Collect(byte[] Value)
		{
			DynamicJsonArray JsonArray = GetJsonFromFile(Value);
			ColorsNames.colors = new ColorsNames[JsonArray.Length];
			ToColorsNames(JsonArray, JsonArray.Length);
		}
		//public void Collect(string Value, ref ColorsNames[] colors)
		//{
		//	DynamicJsonArray JsonArray = GetJsonFromFile(Value);
		//	colors = new ColorsNames[JsonArray.Length]; // 1017
		//	ToColorsNames(JsonArray, ref colors);
		//}
		//public void Collect(FileInfo path, ref ColorsNames[] colors)
		//{
		//	DynamicJsonArray JsonArray = GetJsonFromFile(path.FullName);
		//	colors = new ColorsNames[JsonArray.Length]; // 1017
		//	ToColorsNames(JsonArray, ref colors);
		//}
		//private DynamicJsonArray GetJsonFromFile(string Value)
		//{
		//	return System.Web.Helpers.Json.Decode(Value);
		//}
		//private DynamicJsonArray GetJsonFromFile(FileInfo path)
		//{
		//	return System.Web.Helpers.Json.Decode(File.ReadAllText(path.FullName));
		//}
		private DynamicJsonArray GetJsonFromFile(byte[] Value)
		{
			return System.Web.Helpers.Json.Decode(Encoding.UTF8.GetString(Value));
		}
		// "[" +
		// "	[\"#000000\", \"Черный\"]" +
		// "]"
		/// <summary>
		/// Пример:
		/// [
		///		["#000000", "Черный"]
		/// ]
		/// </summary>
		/// <param name="dynamicJsonArray">Должно быть до двух подэлементов</param>
		/// <param name="colors"></param>
		private void ToColorsNames(DynamicJsonArray dynamicJsonArray, int lenght)
		{
			Color color;
			for (int i = 0; i < lenght; i++)
			{
				color = ColorTranslator.FromHtml(dynamicJsonArray[i][0]);
				ColorsNames.AddColorOfIndex(color:color , name: (string)dynamicJsonArray[i][1], htmlColor: (string)dynamicJsonArray[i][0], argb:color.ToArgb(), index:i);
			}
		}
	}
}
