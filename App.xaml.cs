using System.Windows;
namespace TVisionUpperDemp;
public partial class App : Application { [STAThread] public static void Main() => new App().Run(new MainWindow()); }
