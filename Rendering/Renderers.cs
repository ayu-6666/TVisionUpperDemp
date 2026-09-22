using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SkiaSharp;
using SkiaSharp.Views.WPF;
using TVisionUpperDemp.Models;
namespace TVisionUpperDemp.Rendering;
public sealed class FrameworkRenderer(DrawingDocument d):RenderSurfaceBase(d){protected override void OnRender(DrawingContext dc)=>WpfDrawingTools.DrawScene(dc,Document,RenderSize);}
public sealed class DrawingVisualRenderer:RenderSurfaceBase
{readonly DrawingVisual _visual=new();public DrawingVisualRenderer(DrawingDocument d):base(d){AddVisualChild(_visual);AddLogicalChild(_visual);}protected override int VisualChildrenCount=>1;protected override Visual GetVisualChild(int i)=>_visual;protected override void OnRender(DrawingContext dc){using var c=_visual.RenderOpen();WpfDrawingTools.DrawScene(c,Document,RenderSize);}}
public sealed class CanvasRenderer:Canvas,IRenderSurface
{readonly DrawingDocument _doc;public UIElement Element=>this;public CanvasRenderer(DrawingDocument d){_doc=d;Background=new SolidColorBrush(Color.FromRgb(9,17,29));Focusable=true;MouseWheel+=(s,e)=>{_doc.Transform.ZoomAt(e.GetPosition(this),e.Delta>0?1.15:.87,RenderSize);Refresh();};}public void Refresh(){Children.Clear();var visual=new FrameworkRenderer(_doc);visual.Width=ActualWidth;visual.Height=ActualHeight;Children.Add(visual);}}
public sealed class SkiaRenderer:SKElement,IRenderSurface
{readonly DrawingDocument _doc;public UIElement Element=>this;public SkiaRenderer(DrawingDocument d){_doc=d;PaintSurface+=Paint;}public void Refresh()=>InvalidateVisual();void Paint(object? s,SKPaintSurfaceEventArgs e){e.Surface.Canvas.Clear(new SKColor(9,17,29));using var p=new SKPaint{Color=new SKColor(76,164,232),StrokeWidth=2,IsAntialias=true};var o=_doc.Transform.ToScreen(new(),new(e.Info.Width,e.Info.Height));e.Surface.Canvas.DrawLine(0,(float)o.Y,e.Info.Width,(float)o.Y,p);e.Surface.Canvas.DrawLine((float)o.X,0,(float)o.X,e.Info.Height,p);}}
public sealed class BitmapRenderer:Image,IRenderSurface
{readonly DrawingDocument _doc;public UIElement Element=>this;public BitmapRenderer(DrawingDocument d){_doc=d;Stretch=System.Windows.Media.Stretch.Fill;}public void Refresh(){if(ActualWidth<1||ActualHeight<1)return;var dv=new DrawingVisual();using(var c=dv.RenderOpen())WpfDrawingTools.DrawScene(c,_doc,new(ActualWidth,ActualHeight));var bmp=new RenderTargetBitmap((int)ActualWidth,(int)ActualHeight,96,96,PixelFormats.Pbgra32);bmp.Render(dv);Source=bmp;}}
public static class RendererFactory{public static IRenderSurface Create(RendererKind kind,DrawingDocument d)=>kind switch{RendererKind.Canvas=>new CanvasRenderer(d),RendererKind.DrawingVisual=>new DrawingVisualRenderer(d),RendererKind.SkiaSharp=>new SkiaRenderer(d),RendererKind.WriteableBitmap=>new BitmapRenderer(d),_=>new FrameworkRenderer(d)};}
