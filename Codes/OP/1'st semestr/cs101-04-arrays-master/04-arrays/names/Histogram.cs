using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace names
{
	class Histogram
	{
		public static void Show(string title, int[] xValues, int[] yValues)
		{
			// Подробности о том, как использовать класс Chart, можно найти тут: http://msdn.microsoft.com/ru-ru/library/dd456632.aspx
			// Но не бойтесь экспериментировать с кодом самостоятельно!

			var chart = new Chart();
			var series = new Series();
			series.ChartType = SeriesChartType.Column;
			for (int i = 0; i < xValues.Length; i++)
				series.Points.Add(new DataPoint(xValues[i], yValues[i]));
			chart.Series.Add(series);
			chart.ChartAreas.Add(new ChartArea());
			chart.Dock = DockStyle.Fill;
			if (!string.IsNullOrEmpty(title))
				chart.Titles.Add(title);


			// Form — это привычное нам окно программы. Это одна из главных частей подсистемы под названием Windows Forms http://msdn.microsoft.com/ru-ru/library/ms229601.aspx
			var form = new Form();
			form.Text = title;
			form.Size = new Size(800, 600);
			form.Controls.Add(chart);
			form.ShowDialog();
		}
	}
}