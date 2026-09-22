using System.Windows;
namespace TVisionUpperDemp.Services;
public sealed class CoordinateTransform
{
    public double Scale { get; private set; } = 1; public Vector Offset { get; private set; }
    public Models.AxisMode AxisMode { get; set; } = Models.AxisMode.FourQuadrants;
    public Point ToScreen(Point world, Size size) { var ox=AxisMode==Models.AxisMode.FirstQuadrant?0:size.Width/2; var oy=AxisMode==Models.AxisMode.FirstQuadrant?size.Height:AxisMode==Models.AxisMode.SecondQuadrant?size.Height: size.Height/2; return new(ox+Offset.X+world.X*Scale,oy+Offset.Y-world.Y*Scale); }
    public Point ToWorld(Point screen, Size size) { var ox=AxisMode==Models.AxisMode.FirstQuadrant?0:size.Width/2; var oy=AxisMode==Models.AxisMode.FirstQuadrant?size.Height:AxisMode==Models.AxisMode.SecondQuadrant?size.Height:size.Height/2; return new((screen.X-ox-Offset.X)/Scale,-(screen.Y-oy-Offset.Y)/Scale); }
    public void ZoomAt(Point screen,double factor,Size size){var before=ToWorld(screen,size);Scale=Math.Clamp(Scale*factor,.15,8);Offset+=screen-ToScreen(before,size);}
    public void Pan(Vector delta)=>Offset+=delta; public void Reset(){Scale=1;Offset=new();}
}
