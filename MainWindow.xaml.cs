using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TVisionUpperDemp.Controls;
using TVisionUpperDemp.Models;

namespace TVisionUpperDemp;
public partial class MainWindow : Window
{
    readonly CoordinateSurface _surface = new();
    public MainWindow() { InitializeComponent(); Surface.Content = _surface; RendererBox.SelectionChanged += (_, _) => _surface.RenderMode = RendererBox.SelectedIndex; SizeChanged += (_, _) => _surface.InvalidateVisual(); }
    void AddShape_Click(object s, RoutedEventArgs e) { _surface.AddDemoShape((ShapeKind)ShapeBox.SelectedIndex); Status.Text = $"已添加 {ShapeBox.Text} · 共 {_surface.Shapes.Count} 个图形"; }
    void Clear_Click(object s, RoutedEventArgs e) { _surface.Shapes.Clear(); _surface.InvalidateVisual(); Status.Text = "已清空图形"; }
    void Reset_Click(object s, RoutedEventArgs e) { _surface.ResetView(); Status.Text = "已重置视图 · 视图比例 1.00"; }
}
