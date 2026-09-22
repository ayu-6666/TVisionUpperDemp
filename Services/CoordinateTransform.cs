using System.Windows;
using System.Windows.Media;
using TVisionUpperDemp.Models;

namespace TVisionUpperDemp.Services;
public sealed class CoordinateTransform
{
    public double Scale { get; private set; } = 1;
    public Vector Offset { get; private set; }
    public Point ToScreen(Point world, Size size) => new(size.Width / 2 + Offset.X + world.X * Scale, size.Height / 2 + Offset.Y - world.Y * Scale);
    public Point ToWorld(Point screen, Size size) => new((screen.X - size.Width / 2 - Offset.X) / Scale, -(screen.Y - size.Height / 2 - Offset.Y) / Scale);
    public void ZoomAt(Point screen, double factor, Size size) { var before = ToWorld(screen, size); Scale = Math.Clamp(Scale * factor, .15, 8); var after = ToScreen(before, size); Offset += screen - after; }
    public void Pan(Vector delta) => Offset += delta;
    public void Reset() { Scale = 1; Offset = new(); }
}
