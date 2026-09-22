using System.Windows;
using System.Windows.Media;
namespace TVisionUpperDemp.Models;
public enum ShapeKind { Text, Rectangle, Triangle, Polygon, Line, Arrow, Arc, Ring, Ellipse }
public sealed record DrawShape(ShapeKind Kind, Point Position, string Tag, double Size = 55, Brush? Stroke = null, Brush? Fill = null);
