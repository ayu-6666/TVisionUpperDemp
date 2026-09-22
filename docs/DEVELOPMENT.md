# WPF 多渲染坐标绘图架构

主界面采用左侧渲染方式菜单、右侧 `ContentControl` 工作区。所有渲染器共享 `DrawingDocument`、`CoordinateTransform` 和图形模型，切换渲染方式不会丢失图形。

## 渲染器

- `CanvasRenderer`：以 `Canvas` 作为宿主，适合组合 WPF 子元素。
- `FrameworkRenderer`：重写 `OnRender`，使用 `DrawingContext` 即时绘制。
- `DrawingVisualRenderer`：使用独立 `DrawingVisual`，适合批量图形和缓存。
- `SkiaRenderer`：使用 `SKElement` 和 SkiaSharp 绘制，适合跨平台绘图能力扩展。
- `BitmapRenderer`：使用 `RenderTargetBitmap` 生成 `Image`，适合快照和像素输出场景。

## 配置和工具

`CoordinateTransform.AxisMode` 支持四象限、第一象限、第二象限；`WpfDrawingTools` 负责坐标轴、网格、刻度和全部图形几何。图形仅保存世界坐标，缩放与平移只改变变换配置。

## 运行

```bash
dotnet restore
dotnet run
```
