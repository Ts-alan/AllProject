using System;
using System.Drawing;
using System.Collections;

namespace ARM2_dbcontrol.Diagram
{
    /// <summary>
    /// Класс Chart
    /// Реализация базового класса для BarChart и PieChart
    /// </summary>
    abstract public class Chart
    {
        private const int _colorLimit = 12;

        private Color[] _color = 
			{ 
				Color.BlueViolet,
                Color.PaleVioletRed,
				Color.DeepSkyBlue,
				Color.Fuchsia,
				Color.RoyalBlue,
				Color.HotPink,
				Color.LightBlue,
				Color.Violet,
				Color.SlateBlue,
				Color.Salmon,
				Color.CornflowerBlue,
				Color.Crimson
			};

        // Представление набора всех точек данных для диаграммы
        private ChartItemsCollection _dataPoints = new ChartItemsCollection();

        // Этот метод реализуется производными классами
        public abstract Bitmap Draw();

        public ChartItemsCollection DataPoints
        {
            get { return _dataPoints; }
            set { _dataPoints = value; }
        }

        public void SetColor(int index, Color NewColor)
        {
            if (index < _colorLimit)
            {
                _color[index] = NewColor;
            }
            else
            {
                throw new Exception("Color Limit is " + _colorLimit);
            }
        }

        public Color GetColor(int index)
        {
            if (index < _colorLimit)
            {
                return _color[index];
            }
            else
            {
                throw new Exception("Color Limit is " + _colorLimit);
            }
        }
    }
}