using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public sealed partial class Form1 : Form
    {
        private int _dy = 0;
        private int _dx = 0;
        private readonly List<int[,]> _steps;
        private int[,] _localField;
        private int _time = 0;
        private int _localPointer = 0;
        private Point _newZero;
        private Point _lastZero;
        private readonly bool _isPossible;
        readonly Font _font = new Font("Arial", 32);

        public Form1(List<int[,]> steps)
        {
            _steps = steps;
            InitializeComponent();
            Text = "Yahooo";
            var timer = new Timer();
            timer.Tick += OnTimer;
            timer.Start();
            _localField = _steps[0];
            _lastZero = Program.GetZeroLocation(_localField);
            _newZero = _lastZero;
            DoubleBuffered = true;
            Width = 319;
            Height = 348;
            if (_localField[0, 0] != _localField[0, 1])
                _isPossible = true;
        }

        private void OnTimer(object sender, EventArgs e)
        {
            _time = (_time + 1) % 10;
            if (_time == 0)
                NextField();
            Invalidate();
        }

        private void NextField()
        {
            _lastZero = Program.GetZeroLocation(_localField);
            _localPointer += 1;
            if (_localPointer == _steps.Count)
                _localPointer -= 1;
            _newZero = Program.GetZeroLocation(_steps[_localPointer]);
            _dx = _newZero.X - _lastZero.X;
            _dy = _newZero.Y - _lastZero.Y;
            _localField = _steps[_localPointer];
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            if (!_isPossible)
                g.DrawString("Impossible", _font, new SolidBrush(Color.Black), 10, 120);
            for (var i = 0; i < 3; i++) 
                for (var j = 0; j < 3; j++)
                    if (_localField[i, j] != 0)
                        if (new Point(j, i) == _lastZero)
                        {
                            g.FillRectangle(new SolidBrush(Color.Pink), (_newZero.X)*100 - _dx*_time*10,
                                (_newZero.Y)*100 - _dy*_time*10, 100, 100);
                            g.DrawRectangle(new Pen(Color.Red), (_newZero.X)*100 - _dx*_time*10,
                                (_newZero.Y)*100 - _dy*_time*10, 100, 100);
                            g.DrawString(_localField[i, j].ToString(), _font, new SolidBrush(Color.Red),
                                (_newZero.X)*100 - _dx*_time*10 + 25, (_newZero.Y)*100 - _dy*_time*10 + 20);
                        }
                        else
                        {
                            g.FillRectangle(new SolidBrush(Color.Aqua), j*100, i*100, 100, 100);
                            g.DrawRectangle(new Pen(Color.Red), j*100, i*100, 100, 100);
                            g.DrawString(_localField[i, j].ToString(), _font, new SolidBrush(Color.Red),
                                j*100 + 25, i*100 + 20);
                        }
        }
    }
}
