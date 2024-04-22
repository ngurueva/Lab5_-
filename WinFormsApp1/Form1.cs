using WinFormsApp1.Objects;
using WinFormsApp1;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        List<BaseObject> objects = new();
        Player player;
        Marker marker;
        greenCircle circle1;
        greenCircle circle2;
        int score = 0;
        public Form1()
        {
            InitializeComponent();
            player = new Player(pbMain.Width / 2, pbMain.Height / 2, 0);
            marker = new Marker(pbMain.Width / 2 + 1, pbMain.Height / 2 + 1, 0);
            circle1 = new greenCircle(new Random().Next(10, 700), new Random().Next(10, 400), 0);
            circle2 = new greenCircle(new Random().Next(10, 700), new Random().Next(10, 400), 0);
            objects.Add(player);
            objects.Add(marker);
            objects.Add(circle1);
            objects.Add(circle2);
            textBox1.Text = "Очки: " + score;


            player.OnOverlap += (p, obj) =>
            {
                txtLog.Text = $"[{DateTime.Now:HH:mm:ss:ff}] Игрок пересекся с {obj}\n" + txtLog.Text;
               
                if (obj is greenCircle)
                {
                    objects.Remove((greenCircle)obj);
                    objects.Add(new greenCircle(new Random().Next(10, 700), new Random().Next(10, 400), 0));
                    score ++;
                    textBox1.Text = "Очки: " + score;
                }
            };

            player.OnMarkerOverlap += (m) =>
            {
                objects.Remove(m);
                marker = null;
            };
        }

        private void pbMain_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            g.Clear(Color.White);

            updatePlayer();

            foreach (var obj in objects.ToList())
            {
                if (obj != player)
                {
                    if (player.Overlaps(obj, g))
                    {
                        player.Overlap(obj);
                        obj.Overlap(player);
                    }

                    if (obj is greenCircle)
                    {
                        greenCircle green = (greenCircle)obj;
                        green.Shrink(); // Уменьшаем круг

                        if (green.ShouldRemove())
                        {
                            objects.Remove(green);
                            objects.Add(new greenCircle(new Random().Next(10, 700), new Random().Next(10, 400), 0));
                        }
                    }
                }
            }

            foreach (var obj in objects)
            {
                g.Transform = obj.GetTransform();
                obj.Render(g);
            }
        }

        private void updatePlayer()
        {
            if (marker != null)
            {
                float dx = marker.X - player.X;
                float dy = marker.Y - player.Y;
                float length = MathF.Sqrt(dx * dx + dy * dy);
                dx /= length;
                dy /= length;
                player.vX += dx * 0.5f;
                player.vY += dy * 0.5f;
                player.Angle = 90 - MathF.Atan2(player.vX, player.vY) * 180 / MathF.PI;
            }
            player.vX += -player.vX * 0.1f;
            player.vY += -player.vY * 0.1f;

            player.X += player.vX;
            player.Y += player.vY;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pbMain.Invalidate();
        }

        private void pbMain_MouseClick(object sender, MouseEventArgs e)
        {
            if (marker == null)
            {
                marker = new Marker(0, 0, 0);
                objects.Add(marker);
            }
            marker.X = e.X;
            marker.Y = e.Y;

        }
    }
}