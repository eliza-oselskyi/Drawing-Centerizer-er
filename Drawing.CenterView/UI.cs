using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Drawing.CenterView;

public partial class PluginForm
{
        private SvgIcon _rightArrow;
        private SvgIcon _leftArrow;
        private SvgIcon _topArrow;
        private SvgIcon _bottomArrow;
        private SvgIcon _rightChevron;
        private SvgIcon _leftChevron;
        private SvgIcon _topChevron;
        private SvgIcon _bottomChevron;
        private SvgIcon _paper;
        private SvgIcon _center;
        private Dictionary<string, SvgIcon> _svgIcons  = new Dictionary<string, SvgIcon>();
        private void UI()
        {
            try
            {
                _rightArrow = new SvgIcon(SvgIcon.Icon.RightArrow);
                _rightArrow.ChangeIconColors(Color.Black, Color.White);
                _rightArrow.Resize(25, 25);
                _svgIcons.Add(rightArrowImage.Name.ToString(), _rightArrow);
                rightArrowImage.Image = _rightArrow.Bmp;
                
                _leftArrow = new SvgIcon(SvgIcon.Icon.LeftArrow);
                _leftArrow.ChangeIconColors(Color.Black, Color.White);
                _leftArrow.Resize(25, 25);
                _svgIcons.Add(leftArrowImage.Name.ToString(), _leftArrow);
                leftArrowImage.Image = _leftArrow.Bmp;
                
                _topArrow = new SvgIcon(SvgIcon.Icon.TopArrow);
                _topArrow.ChangeIconColors(Color.Black, Color.White);
                _topArrow.Resize(25, 25);
                _svgIcons.Add(topArrowImage.Name.ToString(), _topArrow);
                topArrowImage.Image = _topArrow.Bmp;
                
                _bottomArrow = new SvgIcon(SvgIcon.Icon.BottomArrow);
                _bottomArrow.ChangeIconColors(Color.Black, Color.White);
                _bottomArrow.Resize(25, 25);
                _svgIcons.Add(bottomArrowImage.Name.ToString(),_bottomArrow);
                bottomArrowImage.Image = _bottomArrow.Bmp;
                
                _rightChevron = new SvgIcon(SvgIcon.Icon.RightChevron);
                _rightChevron.ChangeIconColors(Color.Black, Color.White);
                _rightChevron.Resize(25, 25);
                _svgIcons.Add(rightChevronImage.Name.ToString(), _rightChevron);
                rightChevronImage.Image = _rightChevron.Bmp;
                
                _leftChevron = new SvgIcon(SvgIcon.Icon.LeftChevron);
                _leftChevron.ChangeIconColors(Color.Black, Color.White);
                _leftChevron.Resize(25, 25);
                _svgIcons.Add(leftChevronImage.Name.ToString(),  _leftChevron);
                leftChevronImage.Image = _leftChevron.Bmp;
                
                _topChevron = new SvgIcon(SvgIcon.Icon.TopChevron);
                _topChevron.ChangeIconColors(Color.Black, Color.White);
                _topChevron.Resize(25, 25);
                _svgIcons.Add(topChevronImage.Name.ToString(),  _topChevron);
                topChevronImage.Image = _topChevron.Bmp;
                
                _bottomChevron = new  SvgIcon(SvgIcon.Icon.BottomChevron);
                _bottomChevron.ChangeIconColors(Color.Black, Color.White);
                _bottomChevron.Resize(25, 25);
                _svgIcons.Add(bottomChevronImage.Name.ToString(),  _bottomChevron);
                bottomChevronImage.Image =  _bottomChevron.Bmp;
                
                _paper = new SvgIcon(SvgIcon.Icon.Paper);
                _paper.Resize(125,125);
                _paper.ChangeIconColors(Color.Black, Color.White);
                _paper.Bmp.RotateFlip(RotateFlipType.Rotate90FlipY);
                _svgIcons.Add(paperImage.Name.ToString(), _paper);
                paperImage.Image = _paper.Bmp;

                _center = new SvgIcon(SvgIcon.Icon.Center);
                _center.Resize(25, 25);
                _center.ChangeIconColors(Color.Black, Color.White);
                _svgIcons.Add(centerImage.Name.ToString(),  _center);
                centerImage.Image = _center.Bmp;
                
                Refresh();
            }
            catch (Exception ex)
            {
                InfoBox.OnError(infoBox, ex);
            }
        }
}

public static class InfoBox
{
    private static readonly Color BackColorDefault = Color.FromArgb(40, 57, 56);
    private static readonly Color BackColorOnDisplay = Color.Honeydew;
    private static readonly Color ForeColorOnDisplay = Color.Black;
    private const BorderStyle BorderStyleOnDisplay = System.Windows.Forms.BorderStyle.Fixed3D;
    private const BorderStyle BorderStyleOnDefault = System.Windows.Forms.BorderStyle.None;

    public static void OnError(Label errorBox, Exception ex)
    {
        Console.WriteLine(ex);
        errorBox.BackColor = BackColorOnDisplay;
        errorBox.ForeColor = ForeColorOnDisplay;
        errorBox.BorderStyle = BorderStyleOnDisplay;
        // ReSharper disable once LocalizableElement
        errorBox.Text = $"{ex.Message}\n\n{ex}";
    }

    public static void ToDefault(Label errorBox)
    {
        errorBox.Text = string.Empty;
        errorBox.BorderStyle = BorderStyleOnDefault;
        errorBox.BackColor = BackColorDefault;
    }

    public static void OnInfo(Label errorBox, string info)
    {
        errorBox.BorderStyle = BorderStyleOnDisplay;
        errorBox.BackColor = BackColorOnDisplay;
        errorBox.ForeColor = ForeColorOnDisplay;
        errorBox.Text = info;
    }
}
