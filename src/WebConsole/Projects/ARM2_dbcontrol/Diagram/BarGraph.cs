using System;
using System.Drawing;
using System.Collections;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace ARM2_dbcontrol.Diagram
{
    /// <summary>
    /// ����� BarGraph
    /// ���� ����� ���������� GDI+ ��� ������� �����������.
    /// </summary>
    public class BarGraph : Chart
    {
        private const float _graphLegendSpacer = 15F;
        private const int _labelFontSize = 7;
        private const int _legendFontSize = 9;
        private const float _legendRectangleSize = 10F;
        private const float _spacer = 5F;

        // ����� ����������
        private Color _backColor;
        private string _fontFamily;
        private string _longestTickValue = string.Empty;	// ������������ ��� ������� ������������� �������� ������
        private float _maxTickValueWidth;					// ������������ ��� ������� �������� ����� �����������
        private float _totalHeight;
        private float _totalWidth;

        // ���������� ���������
        private float _barWidth;
        private float _bottomBuffer;	// ���������� �� ������ ������� �� ��� x
        private bool _displayBarData;
        private Color _fontColor;
        private float _graphHeight;
        private float _graphWidth;
        private float _maxValue = 0.0f;	// = ������������� �������� ������� * ���������� �������
        private float _scaleFactor;        // = _maxValue / _graphHeight
        private float _spaceBtwBars;    // ��. �������� ��� _barWidth
        private float _topBuffer;        // ���������� �� ������� ������� �� ������� ��� y
        private float _xOrigin;            // ���������� ������ ������� �� x
        private float _yOrigin;            // ���������� ������ ������� �� y 
        private string _yLabel;
        private int _yTickCount;
        private float _yTickValue;        // �������� ������ ������� = _maxValue/_yTickCount

        // ���������� �������
        private bool _displayLegend;
        private float _legendWidth;
        private string _longestLabel = string.Empty;	// ������������ ��� ������� ������ �������
        private float _maxLabelWidth = 0.0f;

        public string FontFamily
        {
            get { return _fontFamily; }
            set { _fontFamily = value; }
        }

        public Color BackgroundColor
        {
            set { _backColor = value; }
        }

        public int BottomBuffer
        {
            set { _bottomBuffer = Convert.ToSingle(value); }
        }

        public Color FontColor
        {
            set { _fontColor = value; }
        }

        public int Height
        {
            get { return Convert.ToInt32(_totalHeight); }
            set { _totalHeight = Convert.ToSingle(value); }
        }

        public int Width
        {
            get { return Convert.ToInt32(_totalWidth); }
            set { _totalWidth = Convert.ToSingle(value); }
        }

        public bool ShowLegend
        {
            get { return _displayLegend; }
            set { _displayLegend = value; }
        }

        public bool ShowData
        {
            get { return _displayBarData; }
            set { _displayBarData = value; }
        }
        public int TopBuffer
        {
            set { _topBuffer = Convert.ToSingle(value); }
        }

        public string VerticalLabel
        {
            get { return _yLabel; }
            set { _yLabel = value; }
        }

        public int VerticalTickCount
        {
            get { return _yTickCount; }
            set { _yTickCount = value; }
        }

        public BarGraph()
        {
            AssignDefaultSettings();
        }

        public BarGraph(Color bgColor)
        {
            AssignDefaultSettings();
            BackgroundColor = bgColor;
        }

        //*********************************************************************
        //
        // ���� ����� �������� � ����������� ��� ����� ������ � ��������� ��� ��������,
        // ����������� ��� ���������� �����������.  ���� ����� ���������� ����� ������� ������ Draw().
        // ����� - �������� x.
        // �������� - �������� y.
        //
        //*********************************************************************

        public void CollectDataPoints(string[] labels, string[] values)
        {
            if (labels.Length == values.Length)
            {
                for (int i = 0; i < labels.Length; i++)
                {
                    float temp = Convert.ToSingle(values[i]);
                    string shortLbl = MakeShortLabel(labels[i]);

                    // ��������� ������� ��������� ������� � ������� �������� - 0.0
                    DataPoints.Add(new ChartItem(shortLbl, labels[i], temp, 0.0f, 0.0f, GetColor(i)));

                    // ����� ������������� �������� ������ _maxValue (�������� ���������)
                    if (_maxValue < temp) _maxValue = temp;

                    // ����� ������ �������� ��������
                    if (_displayLegend)
                    {
                        string currentLbl = labels[i] + " (" + shortLbl + ")";
                        float currentWidth = CalculateImgFontWidth(currentLbl, _legendFontSize, FontFamily);
                        if (_maxLabelWidth < currentWidth)
                        {
                            _longestLabel = currentLbl;
                            _maxLabelWidth = currentWidth;
                        }
                    }
                }

                CalculateTickAndMax();
                CalculateGraphDimension();
                CalculateBarWidth(DataPoints.Count, _graphWidth);
                CalculateSweepValues();
            }
            else
                throw new Exception("X data count is different from Y data count");
        }

        //*********************************************************************
        //
        // ��� � ����; ����������, ���� ������������ �� ������ �������� x
        //
        //*********************************************************************

        public void CollectDataPoints(string[] values)
        {
            string[] labels = values;
            CollectDataPoints(labels, values);
        }

        //*********************************************************************
        //
        // ���� ����� ���������� ��������� ��� ������� ��������� ������� �����������.  �� ���������� ����� 
        // ���������� ���� �������� � ����� ������.
        //
        //*********************************************************************

        public override Bitmap Draw()
        {
            int height = Convert.ToInt32(_totalHeight);
            int width = Convert.ToInt32(_totalWidth);

            Bitmap bmp = new Bitmap(width, height);

            using (Graphics graph = Graphics.FromImage(bmp))
            {
                graph.CompositingQuality = CompositingQuality.HighQuality;
                graph.SmoothingMode = SmoothingMode.AntiAlias;

                // ��������� ����: ����� ���������� �� ���� ������ ������, ��� ���������
                // �������, ����� ������� ��� �������
                graph.FillRectangle(new SolidBrush(_backColor), -1, -1, bmp.Width + 1, bmp.Height + 1);

                DrawVerticalLabelArea(graph);
                DrawBars(graph);
                DrawXLabelArea(graph);
                if (_displayLegend) DrawLegend(graph);
            }

            return bmp;
        }

        //*********************************************************************
        //
        // ���� ����� ������ ��� ������� ���������.
        //
        //*********************************************************************

        private void DrawBars(Graphics graph)
        {
            SolidBrush brsFont = null;
            Font valFont = null;
            StringFormat sfFormat = null;

            try
            {
                brsFont = new SolidBrush(_fontColor);
                valFont = new Font(_fontFamily, _labelFontSize);
                sfFormat = new StringFormat();
                sfFormat.Alignment = StringAlignment.Center;
                int i = 0;

                // ��������� �������� � �������� ��� ������ �� ���
                foreach (ChartItem item in DataPoints)
                {
                    using (SolidBrush barBrush = new SolidBrush(item.ItemColor))
                    {
                        float itemY = _yOrigin + _graphHeight - item.SweepSize;

                        // ��� ��������� ��� ���������� ����������� ������������ (_xOrigin, _yOrigin)
                        graph.FillRectangle(barBrush, _xOrigin + item.StartPos, itemY, _barWidth, item.SweepSize);

                        // ��������� �������� ������
                        if (_displayBarData)
                        {
                            float startX = _xOrigin + (i * (_barWidth + _spaceBtwBars));  // ��� ������ ������� �������� � ������ �������
                            float startY = itemY - 2f - valFont.Height;					  // ��� ������� �� ������� �������
                            RectangleF recVal = new RectangleF(startX, startY, _barWidth + _spaceBtwBars, valFont.Height);
                            graph.DrawString(item.Value.ToString("#,###.##"), valFont, brsFont, recVal, sfFormat);
                        }
                        i++;
                    }
                }
            }
            finally
            {
                if (brsFont != null) brsFont.Dispose();
                if (valFont != null) valFont.Dispose();
                if (sfFormat != null) sfFormat.Dispose();
            }
        }

        //*********************************************************************
        //
        // ���� ����� ������ ����� y, �������, ����� ������� � ��� Y.
        //
        //*********************************************************************

        private void DrawVerticalLabelArea(Graphics graph)
        {
            Font lblFont = null;
            SolidBrush brs = null;
            StringFormat lblFormat = null;
            Pen pen = null;
            StringFormat sfVLabel = null;

            try
            {
                lblFont = new Font(_fontFamily, _labelFontSize);
                brs = new SolidBrush(_fontColor);
                lblFormat = new StringFormat();
                pen = new Pen(_fontColor);
                sfVLabel = new StringFormat();

                lblFormat.Alignment = StringAlignment.Near;

                // ��������� ������������ ����� � ������� ����� ��� Y � ���������� �� � �������� ��� ���� Y
                RectangleF recVLabel = new RectangleF(0f, _yOrigin - 2 * _spacer - lblFont.Height, _xOrigin * 2, lblFont.Height);
                sfVLabel.Alignment = StringAlignment.Center;
                graph.DrawString(_yLabel, lblFont, brs, recVLabel, sfVLabel);

                // ��������� ���� ������� � ���� �������
                for (int i = 0; i < _yTickCount; i++)
                {
                    float currentY = _topBuffer + (i * _yTickValue / _scaleFactor);	// ���������� ����� �������
                    float labelY = currentY - lblFont.Height / 2;						// ���������� ������� � �������� �����
                    RectangleF lblRec = new RectangleF(_spacer, labelY, _maxTickValueWidth, lblFont.Height);

                    float currentTick = _maxValue - i * _yTickValue;					// ������ �������� ������� ������ ����
                    graph.DrawString(currentTick.ToString("#"), lblFont, brs, lblRec, lblFormat);	// ��������� �������� �������
                    graph.DrawLine(pen, _xOrigin, currentY, _xOrigin - 4.0f, currentY);						// ��������� ����� �������
                }

                // ��������� ��� y
                graph.DrawLine(pen, _xOrigin, _yOrigin, _xOrigin, _yOrigin + _graphHeight);
            }
            finally
            {
                if (lblFont != null) lblFont.Dispose();
                if (brs != null) brs.Dispose();
                if (lblFormat != null) lblFormat.Dispose();
                if (pen != null) pen.Dispose();
                if (sfVLabel != null) sfVLabel.Dispose();
            }
        }

        //*********************************************************************
        //
        // ���� ����� ������ ��� x � ��� ����� x
        //
        //*********************************************************************

        private void DrawXLabelArea(Graphics graph)
        {
            Font lblFont = null;
            SolidBrush brs = null;
            StringFormat lblFormat = null;
            Pen pen = null;

            try
            {
                lblFont = new Font(_fontFamily, _labelFontSize);
                brs = new SolidBrush(_fontColor);
                lblFormat = new StringFormat();
                pen = new Pen(_fontColor);

                lblFormat.Alignment = StringAlignment.Center;

                // ��������� ��� x
                graph.DrawLine(pen, _xOrigin, _yOrigin + _graphHeight, _xOrigin + _graphWidth, _yOrigin + _graphHeight);

                float currentX;
                float currentY = _yOrigin + _graphHeight + 2.0f;	// ��� ����� x ���������� �� 2 ������� ���� ��� x
                float labelWidth = _barWidth + _spaceBtwBars;		// ���������� ����� ��� ��������
                int i = 0;

                // ��������� ����� x
                foreach (ChartItem item in DataPoints)
                {
                    currentX = _xOrigin + (i * labelWidth);
                    RectangleF recLbl = new RectangleF(currentX, currentY, labelWidth, lblFont.Height);
                    string lblString = _displayLegend ? item.Label : item.Description;	// ����� ����������� - ������ ��� ���������

                    graph.DrawString(lblString, lblFont, brs, recLbl, lblFormat);
                    i++;
                }
            }
            finally
            {
                if (lblFont != null) lblFont.Dispose();
                if (brs != null) brs.Dispose();
                if (lblFormat != null) lblFormat.Dispose();
                if (pen != null) pen.Dispose();
            }
        }

        //*********************************************************************
        //
        // ���� ����� ���������� ����� ���������� ���� �������.
        // �� ������ ������� �������, �������� � �������� ���.
        //
        //*********************************************************************

        private void DrawLegend(Graphics graph)
        {
            Font lblFont = null;
            SolidBrush brs = null;
            StringFormat lblFormat = null;
            Pen pen = null;

            try
            {
                lblFont = new Font(_fontFamily, _legendFontSize);
                brs = new SolidBrush(_fontColor);
                lblFormat = new StringFormat();
                pen = new Pen(_fontColor);
                lblFormat.Alignment = StringAlignment.Near;

                // ���������� ����� ������ ��������� �������
                float startX = _xOrigin + _graphWidth + _graphLegendSpacer;
                float startY = _yOrigin;

                float xColorCode = startX + _spacer;
                float xLegendText = xColorCode + _legendRectangleSize + _spacer;
                float legendHeight = 0.0f;
                for (int i = 0; i < DataPoints.Count; i++)
                {
                    ChartItem point = DataPoints[i];
                    string text = point.Description + " (" + point.Label + ")";
                    float currentY = startY + _spacer + (i * (lblFont.Height + _spacer));
                    legendHeight += lblFont.Height + _spacer;

                    // ��������� �������� �������
                    graph.DrawString(text, lblFont, brs, xLegendText, currentY, lblFormat);

                    // ��������� ������
                    graph.FillRectangle(new SolidBrush(DataPoints[i].ItemColor), xColorCode, currentY + 3f, _legendRectangleSize, _legendRectangleSize);
                }

                // ��������� ������� �������
                graph.DrawRectangle(pen, startX, startY, _legendWidth, legendHeight + _spacer);
            }
            finally
            {
                if (lblFont != null) lblFont.Dispose();
                if (brs != null) brs.Dispose();
                if (lblFormat != null) lblFormat.Dispose();
                if (pen != null) pen.Dispose();
            }
        }

        //*********************************************************************
        //
        // ���� ����� ��������� ��� ����������� ��� ����������� �������� �� ������ ��������� ����� ������
        //
        //*********************************************************************

        private void CalculateGraphDimension()
        {
            FindLongestTickValue();

            // ���������� �������� ��� ���� ������, ��� ����� �� ��� ���������, � ��� ��������
            _longestTickValue += "0";
            _maxTickValueWidth = CalculateImgFontWidth(_longestTickValue, _labelFontSize, FontFamily);
            float leftOffset = _spacer + _maxTickValueWidth;
            float rtOffset = 0.0f;

            if (_displayLegend)
            {
                _legendWidth = _spacer + _legendRectangleSize + _spacer + _maxLabelWidth + _spacer;
                rtOffset = _graphLegendSpacer + _legendWidth + _spacer;
            }
            else
                rtOffset = _spacer;		// ��������� ����� � ��������

            _graphHeight = _totalHeight - _topBuffer - _bottomBuffer;	// �������� ������������ ������������ ��� ������ �����
            _graphWidth = _totalWidth - leftOffset - rtOffset;
            _xOrigin = leftOffset;
            _yOrigin = _topBuffer;

            // ����� ����������� ����������� _maxValue ���������� ���������� _scaleFactor
            _scaleFactor = _maxValue / _graphHeight;
        }

        //*********************************************************************
        //
        // ���� ����� ���������� ����� ������� �������� ������� ������ �� ��������� ����� ������.
        // ��� ���������� ��� ���������� ����������� ������� �����.
        //
        //*********************************************************************

        private void FindLongestTickValue()
        {
            float currentTick;
            string tickString;
            for (int i = 0; i < _yTickCount; i++)
            {
                currentTick = _maxValue - i * _yTickValue;
                tickString = currentTick.ToString("#,###.##");
                if (_longestTickValue.Length < tickString.Length)
                    _longestTickValue = tickString;
            }
        }

        //*********************************************************************
        //
        // ���� ����� ��������� ������ ����������� � �������� ��� ���������� ������
        //
        //*********************************************************************

        private float CalculateImgFontWidth(string text, int size, string family)
        {
            Bitmap bmp = null;
            Graphics graph = null;
            Font font = null;

            try
            {
                font = new Font(family, size);

                // ���������� ������� ������
                bmp = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
                graph = Graphics.FromImage(bmp);
                SizeF oSize = graph.MeasureString(text, font);

                return oSize.Width;
            }
            finally
            {
                if (graph != null) graph.Dispose();
                if (bmp != null) bmp.Dispose();
                if (font != null) font.Dispose();
            }
        }

        //*********************************************************************
        //
        // ���� ����� ������� ������� ����� �� �������� ��������, ������� ������������ ��� �������� �������
        //
        //*********************************************************************

        private string MakeShortLabel(string text)
        {
            string label = text;
            if (text.Length > 2)
            {
                int midPostition = Convert.ToInt32(Math.Floor((double)text.Length / 2));
                label = text.Substring(0, 1) + text.Substring(midPostition, 1) + text.Substring(text.Length - 1, 1);
            }
            return label;
        }

        //*********************************************************************
        //
        // ���� ����� ��������� ������������ �������� � �������� ������� ����� ������� ��� ���������� �����������.
        //
        //*********************************************************************

        private void CalculateTickAndMax()
        {
            float tempMax = 0.0f;

            // ��������� ����� ������ ����������� - ����� 10 % �������� ������������� �������
            _maxValue *= 1.1f;

            if (_maxValue != 0.0f)
            {
                // ����� ������������ ��������, �������� �������� � �������� ������������� ��������
                // ������� ����������� ��� ������������ ��������, ����� ������������ ���������� ����� ��� ��������� �������� ������� �������
                double exp = Convert.ToDouble(Math.Floor(Math.Log10(_maxValue)));
                tempMax = Convert.ToSingle(Math.Ceiling(_maxValue / Math.Pow(10, exp)) * Math.Pow(10, exp));
            }
            else
                tempMax = 1.0f;

            // ����� ���������� ������������� �������� ������������ �������� �������; ��� ������ ���� ����� ������
            _yTickValue = tempMax / _yTickCount;
            double expTick = Convert.ToDouble(Math.Floor(Math.Log10(_yTickValue)));
            _yTickValue = Convert.ToSingle(Math.Ceiling(_yTickValue / Math.Pow(10, expTick)) * Math.Pow(10, expTick));

            // ��������� ���������� ������������� �������� ������ �� ������ �������� �������
            _maxValue = _yTickValue * _yTickCount;
        }

        //*********************************************************************
        //
        // ���� ����� ��������� ������ ������� ������� � �����
        //
        //*********************************************************************

        private void CalculateSweepValues()
        {
            // ����������, ����� ��� �������� � ����� ��������
            // ��� �������� ����������� ������������ (_xOrigin, _yOrigin)
            int i = 0;
            foreach (ChartItem item in DataPoints)
            {
                // � ���� ���������� ������������� �������� �� ��������������
                if (item.Value >= 0) item.SweepSize = item.Value / _scaleFactor;

                // (_spaceBtwBars/2) ������������� ���������� ����� ������������ ��� ������� �������
                item.StartPos = (_spaceBtwBars / 2) + i * (_barWidth + _spaceBtwBars);
                i++;
            }
        }

        //*********************************************************************
        //
        // ���� ����� ��������� ������ ������� ������� � �����
        //
        //*********************************************************************

        private void CalculateBarWidth(int dataCount, float barGraphWidth)
        {
            // ����� ������������ ����� ��������� ����� �� �� ������, ��� � ���� �������
            _barWidth = barGraphWidth / (dataCount * 2);  // ����� ������������ ������� � ������� �������
            _spaceBtwBars = _barWidth;
        }

        //*********************************************************************
        //
        // ���� ����� ��������� �������� �� ��������� ��� ������� ����������� � ���������� ������ 
        // �� ������������� ����������
        //
        //*********************************************************************

        private void AssignDefaultSettings()
        {
            // �������� �� ���������
            _totalWidth = 700f;
            _totalHeight = 450f;
            _fontFamily = "Verdana";
            _backColor = Color.White;
            _fontColor = Color.Black;
            _topBuffer = 30f;
            _bottomBuffer = 30f;
            _yTickCount = 2;
            _displayLegend = false;
            _displayBarData = false;
        }
    }
}
