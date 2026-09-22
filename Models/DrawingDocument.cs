using System.Windows;
using System.Windows.Media;
namespace TVisionUpperDemp.Models;
public enum ShapeKind { Text, Rectangle, Triangle, Polygon, Line, Arrow, Arc, Ring, Ellipse }
public enum AxisMode { FourQuadrants, FirstQuadrant, SecondQuadrant }
public enum RendererKind { Canvas, FrameworkElement, DrawingVisual, SkiaSharp, WriteableBitmap }
public sealed record DrawShape(ShapeKind Kind, Point Position, string Tag, double Size = 55, Brush? Stroke = null, Brush? Fill = null);
public sealed class DrawingDocument
{
    public List<DrawShape> Shapes { get; } = new(); public Services.CoordinateTransform Transform { get; } = new();
    int _serial;
    public void Add(ShapeKind kind) { var n=Shapes.Count; Shapes.Add(new(kind, new Point(-180+n%5*85, 140-n/5*85), $"{kind}-{++_serial}", 38+n%3*10)); }
}
