using System;
using System.Drawing;
using System.Collections;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace ARM2_dbcontrol.Diagram
{
    /// <summary>
    /// Класс BarGraph
    /// Этот класс использует GDI+ для расчета гистограммы.
    /// </summary>
    public class BarGraph : Chart
    {
        private const float _graphLegendSpacer = 15F;
        private const int _labelFontSize = 7;
        private const int _legendFontSize = 9;
        private const float _legendRectangleSize = 10F;
        private const float _spacer = 5F;

        // Общие компоненты
        private Color _backColor;
        private string _fontFamily;
        private string _longestTickValue = string.Empty;	// Используется для расчета максимального значения ширины
        private float _maxTickValueWidth;					// Используется для расчета смещения влево гистограммы
        private float _totalHeight;
        private float _totalWidth;

        // Компоненты диаграммы
        private float _barWidth;
        private float _bottomBuffer;	// Расстояние от нижней границы до оси x
        private bool _displayBarData;
        private Color _fontColor;
        private float _graphHeight;
        private float _graphWidth;
        private float _maxValue = 0.0f;	// = окончательное значение отметки * количество отметок
        private float _scaleFactor;        // = _maxValue / _graphHeight
        private float _spaceBtwBars;    // См. описание для _barWidth
        private float _topBuffer;        // Расстояние от верхней границы до вершины оси y
        private float _xOrigin;            // Координата начала рисунка по x
        private float _yOrigin;            // Координата начала рисунка по y 
        private string _yLabel;
        private int _yTickCount;
        private float _yTickValue;        // Значение каждой отметки = _maxValue/_yTickCount

        // Компоненты легенды
        private bool _displayLegend;
        private float _legendWidth;
        private string _longestLabel = string.Empty;	// Используется для расчета ширины легенды
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
        // Этот метод собирает и анализирует все точки данных и вычисляет все значения,
        // необходимые для построения гистограммы.  Этот метод вызывается перед вызовом метода Draw().
        // метки - значения x.
        // значения - значения y.
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

                    // Временное задание начальной позиции и размера проекции - 0.0
                    DataPoints.Add(new ChartItem(shortLbl, labels[i], temp, 0.0f, 0.0f, GetColor(i)));

                    // Поиск максимального значения данных _maxValue (значение временное)
                    if (_maxValue < temp) _maxValue = temp;

                    // Поиск самого длинного описания
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
        // Как и выше; вызывается, если пользователь не вводит значения x
        //
        //*********************************************************************

        public void CollectDataPoints(string[] values)
        {
            string[] labels = values;
            CollectDataPoints(labels, values);
        }

        //*********************************************************************
        //
        // Этот метод возвращает вызвавшей его функции растровый рисунок гистограммы.  Он вызывается после 
        // вычисления всех значений и точек данных.
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

                // Установка фона: нужно нарисовать на один пиксел больше, чем растровый
                // рисунок, чтобы покрыть всю область
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
        // Этот метод рисует все столбцы диаграммы.
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

                // Рисование столбцов и значений над каждым из них
                foreach (ChartItem item in DataPoints)
                {
                    using (SolidBrush barBrush = new SolidBrush(item.ItemColor))
                    {
                        float itemY = _yOrigin + _graphHeight - item.SweepSize;

                        // При рисовании все координаты вычисляются относительно (_xOrigin, _yOrigin)
                        graph.FillRectangle(barBrush, _xOrigin + item.StartPos, itemY, _barWidth, item.SweepSize);

                        // Рисование значений данных
                        if (_displayBarData)
                        {
                            float startX = _xOrigin + (i * (_barWidth + _spaceBtwBars));  // Эта строка выводит значение в центре столбца
                            float startY = itemY - 2f - valFont.Height;					  // Два пиксела от вершины столбца
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
        // Этот метод рисует метку y, деления, цифры делений и ось Y.
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

                // Рисование вертикальной метки в верхней части оси Y и размещение ее в середине над осью Y
                RectangleF recVLabel = new RectangleF(0f, _yOrigin - 2 * _spacer - lblFont.Height, _xOrigin * 2, lblFont.Height);
                sfVLabel.Alignment = StringAlignment.Center;
                graph.DrawString(_yLabel, lblFont, brs, recVLabel, sfVLabel);

                // Рисование всех делений и цифр делений
                for (int i = 0; i < _yTickCount; i++)
                {
                    float currentY = _topBuffer + (i * _yTickValue / _scaleFactor);	// Размещение знака отметки
                    float labelY = currentY - lblFont.Height / 2;						// Размещение отметки в середине метки
                    RectangleF lblRec = new RectangleF(_spacer, labelY, _maxTickValueWidth, lblFont.Height);

                    float currentTick = _maxValue - i * _yTickValue;					// Расчет значения отметки сверху вниз
                    graph.DrawString(currentTick.ToString("#"), lblFont, brs, lblRec, lblFormat);	// Рисование значения отметки
                    graph.DrawLine(pen, _xOrigin, currentY, _xOrigin - 4.0f, currentY);						// Рисование знака отметки
                }

                // Рисование оси y
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
        // Этот метод рисует ось x и все метки x
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

                // Рисование оси x
                graph.DrawLine(pen, _xOrigin, _yOrigin + _graphHeight, _xOrigin + _graphWidth, _yOrigin + _graphHeight);

                float currentX;
                float currentY = _yOrigin + _graphHeight + 2.0f;	// Все метки x нарисованы на 2 пиксела ниже оси x
                float labelWidth = _barWidth + _spaceBtwBars;		// Размещение точно под столбцом
                int i = 0;

                // Рисование меток x
                foreach (ChartItem item in DataPoints)
                {
                    currentX = _xOrigin + (i * labelWidth);
                    RectangleF recLbl = new RectangleF(currentX, currentY, labelWidth, lblFont.Height);
                    string lblString = _displayLegend ? item.Label : item.Description;	// Выбор отображения - кратко или полностью

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
        // Этот метод определяет место размещения поля легенды.
        // Он рисует границу легенды, описание и цветовой код.
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

                // Вычисление точки начала отрисовки легенды
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

                    // Рисование описания легенды
                    graph.DrawString(text, lblFont, brs, xLegendText, currentY, lblFormat);

                    // Рисование цветов
                    graph.FillRectangle(new SolidBrush(DataPoints[i].ItemColor), xColorCode, currentY + 3f, _legendRectangleSize, _legendRectangleSize);
                }

                // Рисование границы легенды
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
        // Этот метод вычисляет все необходимые для гистограммы значения на основе указанных точек данных
        //
        //*********************************************************************

        private void CalculateGraphDimension()
        {
            FindLongestTickValue();

            // Необходимо добавить еще один символ, это нужно не для рисования, а для расчетов
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
                rtOffset = _spacer;		// Помещение графа в середину

            _graphHeight = _totalHeight - _topBuffer - _bottomBuffer;	// Буферное пространство используется для печати меток
            _graphWidth = _totalWidth - leftOffset - rtOffset;
            _xOrigin = leftOffset;
            _yOrigin = _topBuffer;

            // после определения правильного _maxValue происходит вычисление _scaleFactor
            _scaleFactor = _maxValue / _graphHeight;
        }

        //*********************************************************************
        //
        // Этот метод определяет самое длинное значение отметки исходя из указанных точек данных.
        // Это необходимо для вычисления правильного размера графа.
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
        // Этот метод вычисляет ширину изображения в пикселах для указанного текста
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

                // Вычисление размера строки
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
        // Этот метод создает краткий текст из длинного описания, который используется при создании легенды
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
        // Этот метод вычисляет максимальное значение и значение каждого знака отметки для построения гистограммы.
        //
        //*********************************************************************

        private void CalculateTickAndMax()
        {
            float tempMax = 0.0f;

            // Добавляет место вверху гистограммы - около 10 % текущего максимального размера
            _maxValue *= 1.1f;

            if (_maxValue != 0.0f)
            {
                // Поиск округленного значения, наиболее близкого к текущему максимальному значению
                // Сначала вычисляется это максимальное значение, чтобы предоставить достаточно места для рисования значений каждого столбца
                double exp = Convert.ToDouble(Math.Floor(Math.Log10(_maxValue)));
                tempMax = Convert.ToSingle(Math.Ceiling(_maxValue / Math.Pow(10, exp)) * Math.Pow(10, exp));
            }
            else
                tempMax = 1.0f;

            // После вычисления максимального значения определяется значение отметки; оно должно быть целым числом
            _yTickValue = tempMax / _yTickCount;
            double expTick = Convert.ToDouble(Math.Floor(Math.Log10(_yTickValue)));
            _yTickValue = Convert.ToSingle(Math.Ceiling(_yTickValue / Math.Pow(10, expTick)) * Math.Pow(10, expTick));

            // Повторное вычисление максимального значения исходя из нового значения отметки
            _maxValue = _yTickValue * _yTickCount;
        }

        //*********************************************************************
        //
        // Этот метод вычисляет высоты каждого столбца в графе
        //
        //*********************************************************************

        private void CalculateSweepValues()
        {
            // Вызывается, когда все значения и шкалы известны
            // Все значения вычисляются относительно (_xOrigin, _yOrigin)
            int i = 0;
            foreach (ChartItem item in DataPoints)
            {
                // В этой реализации отрицательные значения не поддерживаются
                if (item.Value >= 0) item.SweepSize = item.Value / _scaleFactor;

                // (_spaceBtwBars/2) предоставляет наполовину белое пространство для первого столбца
                item.StartPos = (_spaceBtwBars / 2) + i * (_barWidth + _spaceBtwBars);
                i++;
            }
        }

        //*********************************************************************
        //
        // Этот метод вычисляет ширину каждого столбца в графе
        //
        //*********************************************************************

        private void CalculateBarWidth(int dataCount, float barGraphWidth)
        {
            // Белое пространство между столбцами имеет ту же ширину, что и сами столбцы
            _barWidth = barGraphWidth / (dataCount * 2);  // Такое пространство имеется у каждого столбца
            _spaceBtwBars = _barWidth;
        }

        //*********************************************************************
        //
        // Этот метод назначает значение по умолчанию для свойств гистограммы и вызывается только 
        // из конструкторов гистограмм
        //
        //*********************************************************************

        private void AssignDefaultSettings()
        {
            // значения по умолчанию
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
