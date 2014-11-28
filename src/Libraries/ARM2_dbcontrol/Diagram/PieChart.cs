using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace VirusBlokAda.CC.Diagram
{
    /// <summary>
    /// Класс PieChart
    /// Этот класс использует GDI+ для построения круговой диаграммы.
    /// </summary>
    public class PieChart : Chart
    {
        private const int _bufferSpace = 50;
        private ArrayList _chartItems;
        private int _perimeter;
        private Color _backgroundColor;
        private Color _borderColor;
        private float _total;
        private int _legendWidth;
        private int _legendHeight;
        private int _legendFontHeight;
        private string _legendFontStyle;
        private float _legendFontSize;

        public PieChart()
        {
            _chartItems = new ArrayList();
            _perimeter = 250;
            _backgroundColor = Color.White;
            _borderColor = Color.FromArgb(63, 63, 63);
            _legendFontSize = 9;
            _legendFontStyle = "Verdana";
        }

        public PieChart(Color bgColor)
        {
            _chartItems = new ArrayList();
            _perimeter = 250;
            _backgroundColor = bgColor;
            _borderColor = Color.FromArgb(63, 63, 63);
            _legendFontSize = 9;
            _legendFontStyle = "Verdana";
        }

        //*********************************************************************
        //
        // Этот метод собирает все точки данных и вычисляет все значения, необходимые
        // для рисования диаграммы.  Это первый метод, вызываемый перед вызовом метода Draw().
        //
        //*********************************************************************

        /// <summary>
        ///  Сбор и вычисление значений для гистограммы
        /// </summary>
        /// <param name="labels">значения х</param>
        /// <param name="values">значения у</param>
        public void CollectDataPoints(string[] xValues, string[] yValues)
        {
            _total = 0.0f;

            for (int i = 0; i < xValues.Length; i++)
            {
                float ftemp = Convert.ToSingle(yValues[i]);
                _chartItems.Add(new ChartItem(xValues[i], xValues.ToString(), ftemp, 0, 0, Color.AliceBlue));
                _total += ftemp;
            }

            float nextStartPos = 0.0f;
            int counter = 0;
            foreach (ChartItem item in _chartItems)
            {
                item.StartPos = nextStartPos;
                item.SweepSize = item.Value / _total * 360;
                nextStartPos = item.StartPos + item.SweepSize;
                item.ItemColor = GetColor(counter++);
            }

            CalculateLegendWidthHeight();
        }

        //*********************************************************************
        //
        // Этот метод возвращает растровый рисунок вызвавшей функции.  Именно этот метод
        // фактически рисует круговую диаграмму и ее легенду.
        //
        //*********************************************************************
        /// <summary>
        /// создание растрового рисунка диаграммы
        /// </summary>
        /// <returns></returns>
        public override Bitmap Draw()
        {
            int perimeter = _perimeter;
            Rectangle pieRect = new Rectangle(0, 0, perimeter, perimeter - 1);
            Bitmap bmp = new Bitmap(perimeter + _legendWidth, perimeter);
            Graphics grp = null;
            StringFormat sf = null;

            try
            {
                grp = Graphics.FromImage(bmp);
                sf = new StringFormat();

                // Рисование фона
                grp.FillRectangle(new SolidBrush(_backgroundColor), 0, 0, perimeter + _legendWidth, perimeter);

                // Выравнивание текста по правому краю
                sf.Alignment = StringAlignment.Far;

                // Рисование всех клиньев диаграммы и ее легенды
                for (int i = 0; i < _chartItems.Count; i++)
                {
                    ChartItem item = (ChartItem)_chartItems[i];
                    SolidBrush brs = null;
                    try
                    {
                        brs = new SolidBrush(item.ItemColor);
                        grp.FillPie(brs, pieRect, item.StartPos, item.SweepSize);
                        grp.FillRectangle(brs, perimeter + _bufferSpace, i * _legendFontHeight + 15, 10, 10);

                        grp.DrawString(item.Label, new Font(_legendFontStyle, _legendFontSize),
                            new SolidBrush(Color.Black), perimeter + _bufferSpace + 20, i * _legendFontHeight + 13);

                        grp.DrawString(item.Value.ToString(), new Font(_legendFontStyle, _legendFontSize),
                            new SolidBrush(Color.Black), perimeter + _bufferSpace + 300, i * _legendFontHeight + 13, sf);
                    }
                    finally
                    {
                        if (brs != null)
                            brs.Dispose();
                    }
                }

                //рисование границы около клина
                grp.DrawEllipse(new Pen(_borderColor, 2), pieRect);

                //рисование границы около легенды
                grp.DrawRectangle(new Pen(_borderColor, 1), perimeter + _bufferSpace - 10, 10, 320, _chartItems.Count * _legendFontHeight + 25);

                //рисование итога под легендой
                grp.DrawString("Total", new Font(_legendFontStyle, _legendFontSize, FontStyle.Bold),
                    new SolidBrush(Color.Black), perimeter + _bufferSpace + 30, (_chartItems.Count + 1) * _legendFontHeight, sf);
                grp.DrawString(_total.ToString(), new Font(_legendFontStyle, _legendFontSize, FontStyle.Bold),
                    new SolidBrush(Color.Black), perimeter + _bufferSpace + 300, (_chartItems.Count + 1) * _legendFontHeight, sf);
                grp.SmoothingMode = SmoothingMode.AntiAlias;
            }
            finally
            {
                if (sf != null) sf.Dispose();
                if (grp != null) grp.Dispose();
            }
            return bmp;
        }

        //*********************************************************************
        //
        //  Этот метод вычисляет пространство, необходимое для рисования легенды диаграммы.
        //
        //*********************************************************************
        /// <summary>
        /// вычисление пространства для рисования легенды диаграммы
        /// </summary>
        private void CalculateLegendWidthHeight()
        {
            Font fontLegend = new Font(_legendFontStyle, _legendFontSize);
            _legendFontHeight = fontLegend.Height + 5;
            _legendHeight = fontLegend.Height * (_chartItems.Count + 1);
            if (_legendHeight > _perimeter) _perimeter = _legendHeight;

            _legendWidth = _perimeter + _bufferSpace+100;
        }
    }
}
