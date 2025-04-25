/*
 *
 Drawing Centerizer-er: Mainly centers Tekla drawings, specifically NBG's flavor.
      Copyright (C) 2025  Eliza Oselskyi

      This program is free software: you can redistribute it and/or modify
      it under the terms of the GNU Lesser General Public License as published by
      the Free Software Foundation, either version 3 of the License, or
      (at your option) any later version.

      This program is distributed in the hope that it will be useful,
      but WITHOUT ANY WARRANTY; without even the implied warranty of
      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
      GNU Lesser General Public License for more details.

      You should have received a copy of the GNU Lesser General Public License
      along with this program.  If not, see <https://www.gnu.org/licenses/>.
 *
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Svg;
using System.Xml;
using Drawing.CenterView.Properties;

namespace Drawing.CenterView;

public class SvgIcon
{
    // ReSharper disable once MemberCanBePrivate.Global
    public SvgDocument SvgDoc { get; set; }
    public bool Clicked { get; set; }
    public Bitmap? Bmp { get; private set; }

    public SvgIcon(string svgFilePath)
    {
        try
        {
            GetBitmap(svgFilePath);
            SvgDoc = SvgDocument.Open(svgFilePath);
            Clicked = false;

            if (this.SvgDoc.Fill.ToString() == string.Empty)
                SvgDoc.Fill = new SvgColourServer(System.Drawing.Color.Black);
        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine(e);
            this.SvgDoc = new SvgDocument();
            SvgDoc.Width = new SvgUnit(10);
            SvgDoc.Height = new SvgUnit(10);
            var bmp = this.SvgDoc.Draw();
            this.Bmp = bmp;
        }
    }

    public SvgIcon(SvgIcon.Icon icon)
    {
        var sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        //string sPath = System.IO.Path.Combine(sCurrentDirectory + @"..\..\Resources\svgs\");
        var sPath = System.IO.Path.Combine(sCurrentDirectory + @"\svgs\");
        var iconPath = _iconPaths[icon];
        var combinedPath = System.IO.Path.Combine(sPath, iconPath);

        GetBitmap(combinedPath);
        this.SvgDoc = SvgDocument.Open(combinedPath);
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public Bitmap? GetBitmap(string svgFilePath)
    {
        var svgDoc = SvgDocument.Open<SvgDocument>(svgFilePath);
        var bmp = svgDoc.Draw();
        this.Bmp = bmp;
        return bmp;
    }

    public void Resize(int width, int height)
    {
        this.SvgDoc.Height = new SvgUnit(height);
        this.SvgDoc.Width = new SvgUnit(width);
        this.Bmp = this.SvgDoc.Draw();
    }

    public Bitmap ChangeIconColors(Color sourceColor, Color targetColor)
    {
        var svgDoc = this.SvgDoc;
        Console.WriteLine(svgDoc.Children.Count.ToString());
        foreach (var item in svgDoc.Children) ChangeFill(item, sourceColor, targetColor);

        var convertedImage = SvgDoc.Draw();
        this.Bmp = convertedImage;
        return convertedImage;
    }

    private static void ChangeFill(SvgElement element, Color sourceColor, Color targetColor)
    {
        var targetColorServer = new SvgColourServer(targetColor);

        if (element.Fill is SvgColourServer col && col.Colour.ToArgb() == sourceColor.ToArgb())
            element.Fill = targetColorServer;

        if (element.Children.Count <= 0) return;
        foreach (var item in element.Children) ChangeFill(item, sourceColor, targetColor);
    }

    public enum Icon
    {
        RightChevron,
        LeftChevron,
        TopChevron,
        BottomChevron,
        RightArrow,
        LeftArrow,
        TopArrow,
        BottomArrow,
        Paper,
        PaperAlt,
        Center
    }

    private readonly Dictionary<Enum, string> _iconPaths = new Dictionary<Enum, string>()
    {
        { Icon.RightChevron, "arrow-chevron-right.svg" },
        { Icon.LeftChevron, "arrow-chevron-left.svg" },
        { Icon.TopChevron, "arrow-chevron-up.svg" },
        { Icon.BottomChevron, "arrow-chevron-down.svg" },
        { Icon.RightArrow, "arrow-forward.svg" },
        { Icon.LeftArrow, "arrow-backward.svg" },
        { Icon.TopArrow, "arrow-upward.svg" },
        { Icon.BottomArrow, "arrow-downward.svg" },
        { Icon.Paper, "paper_orig.svg" },
        { Icon.PaperAlt, "paper_alt.svg" },
        { Icon.Center, "center.svg" }
    };
}