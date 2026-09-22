using System.Windows;
using System.Windows.Controls;
using TVisionUpperDemp.Models;
using TVisionUpperDemp.Rendering;
namespace TVisionUpperDemp;
public partial class MainWindow : Window
{
    readonly DrawingDocument _document = new(); IRenderSurface? _surface;
    public MainWindow() { InitializeComponent(); CreateSurface(RendererKind.Canvas); }
    void Renderer_Click(object sender, RoutedEventArgs e) { if (sender is RadioButton { Tag: string name } && Enum.TryParse<RendererKind>(name, out var mode)) CreateSurface(mode); }
    void CoordinateMode_Changed(object sender, SelectionChangedEventArgs e) { if (_document?.Transform is not null) { _document.Transform.AxisMode = (AxisMode)(CoordinateModeBox?.SelectedIndex ?? 0); _surface?.Refresh(); UpdateStatus(); } }
    void CreateSurface(RendererKind mode) { _surface = RendererFactory.Create(mode, _document); SurfaceHost.Content = _surface.Element; RendererLabel.Text = mode.ToString(); _surface.Refresh(); UpdateStatus(); }
    void AddShape_Click(object sender, RoutedEventArgs e) { _document.Add((ShapeKind)(ShapeBox.SelectedIndex < 0 ? 0 : ShapeBox.SelectedIndex)); _surface?.Refresh(); UpdateStatus($"已添加 {_document.Shapes[^1].Tag}"); }
    void Clear_Click(object sender, RoutedEventArgs e) { _document.Shapes.Clear(); _surface?.Refresh(); UpdateStatus("已清空图形"); }
    void Reset_Click(object sender, RoutedEventArgs e) { _document.Transform.Reset(); _surface?.Refresh(); UpdateStatus("已重置视图"); }
    void UpdateStatus(string? message = null) => Status.Text = $"{message ?? "就绪"} · {RendererLabel.Text} · {CoordinateModeBox.SelectedItem} · 图形 {_document.Shapes.Count} · 缩放 { _document.Transform.Scale:0.00}x";
}
