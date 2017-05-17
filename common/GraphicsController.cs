using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ttpim.gamemodule.common
{
    public struct DrawingTextParams
    {
        private Image _startImage; 
        private String _leftText;
        private String _rightText; 
        private Font _leftFont;
        private Font _rightFont; 
        private Color _textLeftColor;
        private Color _textRightColor;
        private Color _backColor;
        private StringFormat leftStrFormat;
        private StringFormat rightStrFormat;
        private int _cornerRadius;

        public Image StartImage
        {
            get 
            {
                return _startImage;
            }
            set 
            {
                _startImage = value;
            }
        }

        public String LeftText
        {
            get 
            {
                return _leftText;
            }
            set 
            {
                _leftText = value;
            }
        }

        public String RightText
        {
            get 
            {
                return _rightText;
            }
            set 
            {
                _rightText = value;
            }
        }

        public Font LeftFont
        {
            get 
            {
                return _leftFont;
            }
            set 
            {
                _leftFont = value;
            }
        }

        public Font RightFont
        {
            get 
            {
                return _rightFont;
            }
            set 
            {
                _rightFont = value;
            }
        }

        public Color LeftTextColor
        {
            get 
            {
                return _textLeftColor;
            }
            set 
            {
                _textLeftColor = value;
            }
        }

        public Color RightTextColor
        {
            get 
            {
                return _textRightColor;
            }
            set 
            {
                _textRightColor = value;
            }
        }

        public Color BackColor
        {
            get
            {
                return _backColor;
            }
            set
            {
                _backColor = value;
            }
        }

        public StringFormat LeftStringFormat
        {
            get
            {
                return leftStrFormat;
            }
            set
            {
                leftStrFormat = value;
            }
        }

        public StringFormat RightStringFormat
        {
            get
            {
                return rightStrFormat;
            }
            set
            {
                rightStrFormat = value;
            }
        }

        public int CornerRadius
        {
            get
            {
                return _cornerRadius;
            }
            set
            {
                _cornerRadius = value;
            }
        }
    }
    public static class GraphicsController
    {
        private static Image RoundCorners(Image StartImage, int CornerRadius, Color BackgroundColor)
        {
            CornerRadius *= 2;
            Bitmap RoundedImage = new Bitmap(StartImage.Width, StartImage.Height);
            Graphics g = Graphics.FromImage(RoundedImage);
            g.Clear(BackgroundColor);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Brush brush = new TextureBrush(StartImage);
            GraphicsPath gp = new GraphicsPath();
            gp.AddArc(0, 0, CornerRadius, CornerRadius, 180, 90);
            gp.AddArc(0 + RoundedImage.Width - CornerRadius, 0, CornerRadius, CornerRadius, 270, 90);
            gp.AddArc(0 + RoundedImage.Width - CornerRadius, 0 + RoundedImage.Height - CornerRadius, CornerRadius, CornerRadius, 0, 90);
            gp.AddArc(0, 0 + RoundedImage.Height - CornerRadius, CornerRadius, CornerRadius, 90, 90);
            g.FillPath(brush, gp);
            return RoundedImage;
        }

        public static Image DrawTextToImage(DrawingTextParams drawParams, int x, int y)
        {
            SizeF leftTextSize = new SizeF();
            SizeF rightTextSize = new SizeF();

            Image img = drawParams.StartImage;
            Graphics g = Graphics.FromImage(img);
            if ((drawParams.LeftText != null) && (drawParams.LeftFont != null))
                leftTextSize = g.MeasureString(drawParams.LeftText, drawParams.LeftFont);
            if ((drawParams.RightText != null) && (drawParams.RightFont != null))
                rightTextSize = g.MeasureString(drawParams.RightText, drawParams.RightFont);
            int bHeight = img.Height;
            int bWidth = img.Width;
            img.Dispose();
            g.Dispose();

            img = new Bitmap(bWidth, bHeight);

            g = Graphics.FromImage(img);
            g.Clear(drawParams.BackColor);
            Brush leftTextBrush = new SolidBrush(drawParams.LeftTextColor);
            Brush rightTextBrush = new SolidBrush(drawParams.RightTextColor);

            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g.DrawString(drawParams.LeftText, drawParams.LeftFont, leftTextBrush, new RectangleF(img.Width / 2 - (int)leftTextSize.Width/2, (int)leftTextSize.Height / 2+3, (int)leftTextSize.Width + 20, (int)leftTextSize.Height), drawParams.LeftStringFormat);
            g.DrawString(drawParams.RightText, drawParams.RightFont, rightTextBrush, new RectangleF(img.Width - (int)rightTextSize.Width - x, (int)leftTextSize.Height / 2, (int)rightTextSize.Width + 20, (int)rightTextSize.Height), drawParams.RightStringFormat);
            g.Save();

            leftTextBrush.Dispose();
            rightTextBrush.Dispose();
            g.Dispose();

            img = RoundCorners(img, drawParams.CornerRadius, System.Drawing.Color.Transparent);
            return img;
        }

        public static Image DrawTextToImage(DrawingTextParams drawParams)
        {
            SizeF leftTextSize = new SizeF();
            SizeF rightTextSize = new SizeF();

            Image img = drawParams.StartImage;
            Graphics g = Graphics.FromImage(img);
            if ((drawParams.LeftText != null) && (drawParams.LeftFont != null))
                leftTextSize = g.MeasureString(drawParams.LeftText, drawParams.LeftFont);
            if ((drawParams.RightText != null) && (drawParams.RightFont != null))
                rightTextSize = g.MeasureString(drawParams.RightText, drawParams.RightFont);
            int bHeight = img.Height;
            int bWidth = img.Width;
            img.Dispose();
            g.Dispose();

            img = new Bitmap(bWidth, bHeight);
     
            g = Graphics.FromImage(img);
            g.Clear(drawParams.BackColor);
            Brush leftTextBrush = new SolidBrush(drawParams.LeftTextColor);
            Brush rightTextBrush = new SolidBrush(drawParams.RightTextColor);

            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g.DrawString(drawParams.LeftText, drawParams.LeftFont, leftTextBrush, new RectangleF(35, 7, (int)leftTextSize.Width + 20, (int)leftTextSize.Height), drawParams.LeftStringFormat);
            g.DrawString(drawParams.RightText, drawParams.RightFont, rightTextBrush, new RectangleF(img.Width - (int)rightTextSize.Width - 35, 7, (int)rightTextSize.Width + 20, (int)rightTextSize.Height), drawParams.RightStringFormat);
            g.Save();

            leftTextBrush.Dispose();
            rightTextBrush.Dispose();
            g.Dispose();

            img = RoundCorners(img, drawParams.CornerRadius, System.Drawing.Color.Transparent);

            return img;

        }
    }
}
