using System.Windows;
using System.Windows.Media;
using TVisionUpperDemp.Models;
namespace TVisionUpperDemp.Rendering;
public interface IRenderSurface { UIElement Element { get; } void Refresh(); }
public abstract class RenderSurfaceBase : FrameworkElement, IRenderSurface
{
    protected readonly DrawingDocument Document; Point? _drag; public UIElement Element=>this;
    protected RenderSurfaceBase(DrawingDocument document){Document=document;Focusable=true;ClipToBounds=true;}
    public void Refresh()=>InvalidateVisual();
    protected override void OnMouseWheel(System.Windows.Input.MouseWheelEventArgs e){Document.Transform.ZoomAt(e.GetPosition(this),e.Delta>0?1.15:.87,RenderSize);Refresh();e.Handled=true;}
    protected override void OnMouseLeftButtonDown(System.Windows.Input.MouseButtonEventArgs e){_drag=e.GetPosition(this);CaptureMouse();}
    protected override void OnMouseMove(System.Windows.Input.MouseEventArgs e){if(_drag is Point p){var now=e.GetPosition(this);Document.Transform.Pan(now-p);_drag=now;Refresh();}}
    protected override void OnMouseLeftButtonUp(System.Windows.Input.MouseButtonEventArgs e){_drag=null;ReleaseMouseCapture();}
    protected override void OnMouseRightButtonDown(System.Windows.Input.MouseButtonEventArgs e){var p=Document.Transform.ToWorld(e.GetPosition(this),RenderSize);Document.Shapes.Add(new(ShapeKind.Ellipse,p,$"Tag-{DateTime.Now:HHmmss}",42));Refresh();}
}
