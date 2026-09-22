using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using TVisionUpperDemp.Models;
using TVisionUpperDemp.Services;
namespace TVisionUpperDemp.Controls;
public sealed class CoordinateSurface : FrameworkElement
{
    public List<DrawShape> Shapes { get; } = new();
    public int RenderMode { get; set; }
    readonly CoordinateTransform T = new(); Point? drag; int serial;
    public CoordinateSurface() { Focusable = true; ClipToBounds = true; }
    public void AddDemoShape(ShapeKind kind) { var n = Shapes.Count; Shapes.Add(new(kind, new Point(-220 + n % 6 * 85, 150 - n / 6 * 90), kind.ToString(), 42 + n % 3 * 12)); InvalidateVisual(); }
    public void ResetView() { T.Reset(); InvalidateVisual(); }
    protected override void OnRender(DrawingContext dc) { base.OnRender(dc); dc.DrawRectangle(new SolidColorBrush(Color.FromRgb(11,17,27)), null, new Rect(RenderSize)); DrawGrid(dc); foreach (var s in Shapes) DrawShape(dc, s); }
    void DrawGrid(DrawingContext dc) { var pen = new Pen(new SolidColorBrush(Color.FromRgb(29,44,63)), 1); double step = 50 * T.Scale; if (step < 18) step *= 2; if (step > 90) step /= 2; var origin = T.ToScreen(new(), RenderSize); for (double x=origin.X%step;x<ActualWidth;x+=step) dc.DrawLine(pen,new Point(x,0),new Point(x,ActualHeight)); for(double y=origin.Y%step;y<ActualHeight;y+=step) dc.DrawLine(pen,new Point(0,y),new Point(ActualWidth,y)); var axis=new Pen(new SolidColorBrush(Color.FromRgb(107,160,218)),2); dc.DrawLine(axis,new Point(0,origin.Y),new Point(ActualWidth,origin.Y)); dc.DrawLine(axis,new Point(origin.X,0),new Point(origin.X,ActualHeight)); for(int i=-20;i<=20;i++){ var px=T.ToScreen(new Point(i*50,0),RenderSize).X; if(px>=0&&px<ActualWidth){dc.DrawLine(pen,new Point(px,origin.Y-4),new Point(px,origin.Y+4)); if(i!=0) dc.DrawText(new FormattedText((i*50).ToString(),System.Globalization.CultureInfo.InvariantCulture,FlowDirection.LeftToRight,new Typeface("Segoe UI"),10,Brushes.Gray,1),new Point(px+3,origin.Y+5));}} }
    void DrawShape(DrawingContext dc, DrawShape s) { var p=T.ToScreen(s.Position,RenderSize); var stroke=new Pen(s.Stroke ?? new SolidColorBrush(Color.FromRgb(74,194,180)),2); var fill=s.Fill ?? new SolidColorBrush(Color.FromArgb(40,74,194,180)); double z=s.Size*T.Scale; switch(s.Kind){case ShapeKind.Text: dc.DrawText(new FormattedText($"{s.Tag}\n({s.Position.X:0},{s.Position.Y:0})",System.Globalization.CultureInfo.InvariantCulture,FlowDirection.LeftToRight,new Typeface("Segoe UI"),14,Brushes.White,1),p);break;case ShapeKind.Rectangle:dc.DrawRoundedRectangle(fill,stroke,new Rect(p.X-z,p.Y-z/2,2*z,z),5,5);break;case ShapeKind.Triangle:dc.DrawGeometry(fill,stroke,Geo(new[]{new Point(p.X,p.Y-z),new Point(p.X-z,p.Y+z),new Point(p.X+z,p.Y+z)}));break;case ShapeKind.Polygon:dc.DrawGeometry(fill,stroke,Geo(new[]{new Point(p.X,p.Y-z),new Point(p.X+z,p.Y-z/3),new Point(p.X+z/2,p.Y+z),new Point(p.X-z/2,p.Y+z),new Point(p.X-z,p.Y-z/3)}));break;case ShapeKind.Line:case ShapeKind.Arrow:var end=new Point(p.X+z*1.8,p.Y-z*.7);dc.DrawLine(stroke,p,end);if(s.Kind==ShapeKind.Arrow){dc.DrawLine(stroke,end,new Point(end.X-z*.25,end.Y-z*.1));dc.DrawLine(stroke,end,new Point(end.X-z*.08,end.Y+z*.25));}break;case ShapeKind.Arc:var g=new StreamGeometry();using(var c=g.Open()){c.BeginFigure(new Point(p.X-z,p.Y),false,false);c.ArcTo(new Point(p.X+z,p.Y),new Size(z,z),0,false,SweepDirection.Clockwise,true);}dc.DrawGeometry(null,stroke,g);break;case ShapeKind.Ring:dc.DrawEllipse(null,new Pen(stroke.Brush,8*T.Scale),p,z,z);break;case ShapeKind.Ellipse:dc.DrawEllipse(fill,stroke,p,z*1.2,z*.7);break;} }
    StreamGeometry Geo(IEnumerable<Point> pts){var g=new StreamGeometry();using var c=g.Open();c.BeginFigure(pts.First(),true,true);c.PolyLineTo(pts.Skip(1),true,true);return g;}
    protected override void OnMouseWheel(MouseWheelEventArgs e){T.ZoomAt(e.GetPosition(this),e.Delta>0?1.15:.87,RenderSize); InvalidateVisual(); e.Handled=true;}
    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e){drag=e.GetPosition(this);CaptureMouse();}
    protected override void OnMouseMove(MouseEventArgs e){if(drag is Point p){var now=e.GetPosition(this);T.Pan(now-p);drag=now;InvalidateVisual();}}
    protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e){drag=null;ReleaseMouseCapture();}
    protected override void OnMouseRightButtonDown(MouseButtonEventArgs e){var p=T.ToWorld(e.GetPosition(this),RenderSize);Shapes.Add(new DrawShape(ShapeKind.Ellipse,p,$"Tag-{++serial}"));InvalidateVisual();}
}
