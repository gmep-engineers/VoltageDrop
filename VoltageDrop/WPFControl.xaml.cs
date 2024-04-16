using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UserControl = System.Windows.Controls.UserControl;

namespace VoltageDrop
{
  /// <summary>
  /// Interaction logic for WPFControl.xaml
  /// </summary>
  public partial class WPFControl : UserControl
  {
    public string MyText
    {
      get { return (string)GetValue(MyTextProperty); }
      set { SetValue(MyTextProperty, value); }
    }

    public static readonly DependencyProperty MyTextProperty =
        DependencyProperty.Register("MyText", typeof(string), typeof(WPFControl), new PropertyMetadata(string.Empty));

    public WPFControl()
    {
      InitializeComponent();
      DataContext = new WPFViewModel();
    }
  }
}