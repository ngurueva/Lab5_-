using System;
using System.Collections.Generic;
using System.Timers;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace WinFormsApp1.Objects
{
    class greenCircle : BaseObject
    {
        private float radius = 20; // Радиус круга
        private bool shrink = false; // Флаг для отслеживания уменьшения

        public greenCircle(float x, float y, float angle) : base(x, y, angle)
        {
        }

        public void Shrink()
        {
            if (radius > 5)
            {
                radius -= 0.1f; // Уменьшаем радиус на 1 каждый тик
            }
            else
            {
                // Можно пометить объект для удаления
                shrink = true;
            }
        }

        public override void Render(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.LimeGreen), -15, -15, 2 * radius, 2 * radius);
            
        }

        public bool ShouldRemove()
        {
            return shrink;
        }
        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(-3, -3, 6, 6);
            return path;
        }
    }
}