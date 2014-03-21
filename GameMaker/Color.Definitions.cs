﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameMaker
{
	public partial struct Color
	{
		static Color()
		{
			Red = new Color(255, 0, 0);
			Green = new Color(0, 255, 0);
			Blue = new Color(0, 0, 255);
			Black = new Color(0, 0, 0);
			White = new Color(255, 255, 255);
			Gray = new Color(128, 128, 128);
			LightGray = new Color(196, 196, 196);
			Yellow = new Color(255, 255, 0);
			Lime = new Color(0, 255, 0);
			Aqua = new Color(0, 255, 255);
			Magenta = new Color(255, 0, 255);
			Silver = new Color(192, 192, 192);
			Maroon = new Color(128, 0, 0);
			Olive = new Color(128, 128, 0);
			Purple = new Color(128, 0, 128);
			Teal = new Color(0, 128, 128);
			Navy = new Color(0, 0, 128);
			DarkRed = new Color(139, 0, 0);
			Brown = new Color(165, 42, 42);
			FireBrick = new Color(178, 34, 34);
			Crimson = new Color(220, 20, 60);
			Tomato = new Color(255, 99, 71);
			Coral = new Color(255, 127, 80);
			IndianRed = new Color(205, 92, 92);
			LightCoral = new Color(240, 128, 128);
			DarkSalmon = new Color(233, 150, 122);
			Salmon = new Color(250, 128, 114);
			LightSalmon = new Color(255, 160, 122);
			OrangeRed = new Color(255, 69, 0);
			DarkOrange = new Color(255, 140, 0);
			Orange = new Color(255, 165, 0);
			Gold = new Color(255, 215, 0);
			DarkGoldenRod = new Color(184, 134, 11);
			GoldenRod = new Color(218, 165, 32);
			PaleGoldenRod = new Color(238, 232, 170);
			DarkKakhi = new Color(189, 183, 107);
			Kahki = new Color(240, 230, 140);
			YellowGreen = new Color(154, 205, 50);
			DarkOliveGreen = new Color(85, 107, 47);
			OliveDrab = new Color(107, 142, 35);
			LawnGreen = new Color(124, 252, 0);
			ChartReuse = new Color(127, 255, 0);
			GreenYellow = new Color(173, 255, 47);
			DarkGreen = new Color(0, 100, 0);
			ForestGreen = new Color(34, 139, 34);
			LimeGreen = new Color(50, 205, 50);
			LightGreen = new Color(144, 238, 144);
			PaleGreen = new Color(152, 251, 152);
			DarkSeaGreen = new Color(143, 188, 143);
			MediumSpringGreen = new Color(0, 250, 154);
			SpringGreen = new Color(0, 255, 127);
			SeaGreen = new Color(46, 139, 87);
			MediumAquaMarine = new Color(102, 205, 170);
			MediumSeaGreen = new Color(60, 179, 113);
			LightSeaGreen = new Color(32, 178, 170);
			DarkSiateGray = new Color(47, 79, 79);
			DarkCyan = new Color(0, 139, 139);
			LightCyan = new Color(224, 255, 255);
			DarkTurquoise = new Color(0, 206, 209);
			Turquoise = new Color(64, 224, 208);
			MediumTurquoise = new Color(72, 209, 204);
			PaleTurquoise = new Color(175, 238, 238);
			AquaMarine = new Color(127, 255, 212);
			PowderBlue = new Color(176, 224, 230);
			CadetBlue = new Color(95, 158, 160);
			SteelBlue = new Color(70, 130, 180);
			CornFlowerBlue = new Color(100, 149, 237);
			DeepSkyeBlue = new Color(0, 191, 255);
			DodgerBlue = new Color(30, 144, 255);
			LightBlue = new Color(173, 216, 230);
			SkyBlue = new Color(135, 206, 235);
			LightSkyBlue = new Color(135, 206, 250);
			MidnightBlue = new Color(25, 25, 112);
			DarkBlue = new Color(0, 0, 139);
			RoyalBlue = new Color(65, 105, 225);
			BlueViolet = new Color(138, 43, 226);
			Indigo = new Color(75, 0, 130);
			DarkSlateBlue = new Color(72, 61, 139);
			SlateBlue = new Color(106, 90, 205);
			MediumSlateBlue = new Color(123, 104, 238);
			MediumPurple = new Color(147, 112, 219);
			DarkMagenta = new Color(139, 0, 139);
			DarkViolet = new Color(148, 0, 211);
			DarkOrchid = new Color(153, 50, 204);
			MediumOrchid = new Color(186, 85, 211);
			Thistle = new Color(216, 191, 216);
			Plum = new Color(221, 160, 221);
			Violet = new Color(238, 130, 238);
			Orchid = new Color(218, 112, 214);
			MediumVioletRed = new Color(199, 21, 133);
			PaleVioletRed = new Color(219, 112, 147);
			DeepPink = new Color(255, 20, 147);
			HotPink = new Color(255, 105, 180);
			LightPink = new Color(255, 182, 193);
			Pink = new Color(255, 192, 203);
			AntiqueWhite = new Color(250, 235, 215);
			Beige = new Color(245, 245, 220);
			Bisque = new Color(255, 228, 196);
			BlanchedAlmond = new Color(255, 235, 205);
			Wheat = new Color(245, 222, 179);
			CornSilk = new Color(255, 248, 220);
			LemonChiffon = new Color(255, 250, 205);
			LightGoldenRodYellow = new Color(250, 250, 210);
			LightYellow = new Color(255, 255, 224);
			SaddleBrown = new Color(139, 69, 19);
			Sienna = new Color(160, 82, 45);
			Chocolate = new Color(210, 105, 30);
			Peru = new Color(205, 133, 63);
			SandyBrown = new Color(244, 164, 96);
			BurlyWood = new Color(222, 184, 135);
			Tan = new Color(210, 180, 140);
			RosyBrown = new Color(188, 143, 143);
			Moccasin = new Color(255, 228, 181);
			NavajoWhite = new Color(255, 222, 173);
			PeachPuff = new Color(255, 218, 185);
			MistyRose = new Color(255, 228, 225);
			LavenderBlush = new Color(255, 240, 245);
			Linen = new Color(250, 240, 230);
			OldLace = new Color(253, 245, 230);
			PapayaWhip = new Color(255, 239, 213);
			SeaShell = new Color(255, 245, 238);
			MintCream = new Color(245, 255, 250);
			SlateGray = new Color(112, 128, 144);
			LightSlateGray = new Color(119, 136, 153);
			LightSteelBlue = new Color(176, 196, 222);
			Lavender = new Color(230, 230, 250);
			FloralWhite = new Color(255, 250, 240);
			AliceBlue = new Color(240, 248, 255);
			GhostWhite = new Color(248, 248, 255);
			HoneyDew = new Color(240, 255, 240);
			Ivory = new Color(255, 255, 240);
			Azure = new Color(240, 255, 255);
			Snow = new Color(255, 250, 250);
			DimGray = new Color(105, 105, 105);
			DarkGray = new Color(169, 169, 169);
			Gainsboro = new Color(220, 220, 220);
			WhiteSmoke = new Color(245, 245, 245);
		}
		

		public static Color Red { get; private set; }

		public static Color Green { get; private set; }

		public static Color Blue { get; private set; }

		public static Color Black { get; private set; }

		public static Color White { get; private set; }

		public static Color Gray { get; private set; }

		public static Color LightGray { get; private set; }

		public static Color Yellow { get; private set; }

		public static Color Lime { get; private set; }

		public static Color Aqua { get; private set; }

		public static Color Magenta { get; private set; }

		public static Color Silver { get; private set; }

		public static Color Maroon { get; private set; }

		public static Color Olive { get; private set; }

		public static Color Purple { get; private set; }

		public static Color Teal { get; private set; }

		public static Color Navy { get; private set; }

		public static Color DarkRed { get; private set; }

		public static Color Brown { get; private set; }

		public static Color FireBrick { get; private set; }

		public static Color Crimson { get; private set; }

		public static Color Tomato { get; private set; }

		public static Color Coral { get; private set; }

		public static Color IndianRed { get; private set; }

		public static Color LightCoral { get; private set; }

		public static Color DarkSalmon { get; private set; }

		public static Color Salmon { get; private set; }

		public static Color LightSalmon { get; private set; }

		public static Color OrangeRed { get; private set; }

		public static Color DarkOrange { get; private set; }

		public static Color Orange { get; private set; }

		public static Color Gold { get; private set; }

		public static Color DarkGoldenRod { get; private set; }

		public static Color GoldenRod { get; private set; }

		public static Color PaleGoldenRod { get; private set; }

		public static Color DarkKakhi { get; private set; }

		public static Color Kahki { get; private set; }

		public static Color YellowGreen { get; private set; }

		public static Color DarkOliveGreen { get; private set; }

		public static Color OliveDrab { get; private set; }

		public static Color LawnGreen { get; private set; }

		public static Color ChartReuse { get; private set; }

		public static Color GreenYellow { get; private set; }

		public static Color DarkGreen { get; private set; }

		public static Color ForestGreen { get; private set; }

		public static Color LimeGreen { get; private set; }

		public static Color LightGreen { get; private set; }

		public static Color PaleGreen { get; private set; }

		public static Color DarkSeaGreen { get; private set; }

		public static Color MediumSpringGreen { get; private set; }

		public static Color SpringGreen { get; private set; }

		public static Color SeaGreen { get; private set; }

		public static Color MediumAquaMarine { get; private set; }

		public static Color MediumSeaGreen { get; private set; }

		public static Color LightSeaGreen { get; private set; }

		public static Color DarkSiateGray { get; private set; }

		public static Color DarkCyan { get; private set; }

		public static Color lightCyan { get; private set; }

		public static Color DarkTurquoise { get; private set; }

		public static Color Turquoise { get; private set; }

		public static Color LightCyan { get; private set; }

		public static Color MediumTurquoise { get; private set; }

		public static Color PaleTurquoise { get; private set; }

		public static Color AquaMarine { get; private set; }

		public static Color PowderBlue { get; private set; }

		public static Color CadetBlue { get; private set; }

		public static Color CornFlowerBlue { get; private set; }

		public static Color DeepSkyeBlue { get; private set; }

		public static Color DodgerBlue { get; private set; }

		public static Color LightBlue { get; private set; }

		public static Color SkyBlue { get; private set; }

		public static Color LightSkyBlue { get; private set; }

		public static Color MidnightBlue { get; private set; }

		public static Color DarkBlue { get; private set; }

		public static Color RoyalBlue { get; private set; }

		public static Color BlueViolet { get; private set; }

		public static Color Indigo { get; private set; }

		public static Color DarkSlateBlue { get; private set; }

		public static Color SlateBlue { get; private set; }

		public static Color MediumSlateBlue { get; private set; }

		public static Color MediumPurple { get; private set; }

		public static Color DarkMagenta { get; private set; }

		public static Color DarkViolet { get; private set; }

		public static Color DarkOrchid { get; private set; }

		public static Color MediumOrchid { get; private set; }

		public static Color Thistle { get; private set; }

		public static Color Plum { get; private set; }

		public static Color Violet { get; private set; }

		public static Color Orchid { get; private set; }

		public static Color MediumVioletRed { get; private set; }

		public static Color PaleVioletRed { get; private set; }

		public static Color DeepPink { get; private set; }

		public static Color HotPink { get; private set; }

		public static Color LightPink { get; private set; }

		public static Color Pink { get; private set; }

		public static Color AntiqueWhite { get; private set; }

		public static Color Beige { get; private set; }

		public static Color Bisque { get; private set; }

		public static Color BlanchedAlmond { get; private set; }

		public static Color Wheat { get; private set; }

		public static Color CornSilk { get; private set; }

		public static Color LemonChiffon { get; private set; }

		public static Color LightGoldenRodYellow { get; private set; }

		public static Color LightYellow { get; private set; }

		public static Color SaddleBrown { get; private set; }

		public static Color Sienna { get; private set; }

		public static Color Chocolate { get; private set; }

		public static Color Peru { get; private set; }

		public static Color SandyBrown { get; private set; }

		public static Color BurlyWood { get; private set; }

		public static Color Tan { get; private set; }

		public static Color RosyBrown { get; private set; }

		public static Color Moccasin { get; private set; }

		public static Color NavajoWhite { get; private set; }

		public static Color PeachPuff { get; private set; }

		public static Color MistyRose { get; private set; }

		public static Color LavenderBlush { get; private set; }

		public static Color Linen { get; private set; }

		public static Color OldLace { get; private set; }

		public static Color PapayaWhip { get; private set; }

		public static Color SeaShell { get; private set; }

		public static Color MintCream { get; private set; }

		public static Color SlateGray { get; private set; }

		public static Color LightSlateGray { get; private set; }

		public static Color LightSteelBlue { get; private set; }

		public static Color Lavender { get; private set; }

		public static Color FloralWhite { get; private set; }

		public static Color AliceBlue { get; private set; }

		public static Color GhostWhite { get; private set; }

		public static Color HoneyDew { get; private set; }

		public static Color Ivory { get; private set; }

		public static Color Azure { get; private set; }

		public static Color Snow { get; private set; }

		public static Color DimGray { get; private set; }

		public static Color DarkGray { get; private set; }

		public static Color Gainsboro { get; private set; }

		public static Color WhiteSmoke { get; private set; }

		public static Color SteelBlue { get; private set; }
	}
}
