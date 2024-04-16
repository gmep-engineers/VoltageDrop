using System.ComponentModel;
using System.Runtime.CompilerServices;

public class WPFViewModel : INotifyPropertyChanged
{
  private string _myText;

  public string MyText
  {
    get { return _myText; }
    set
    {
      if (_myText != value)
      {
        _myText = value;
        OnPropertyChanged();
      }
    }
  }

  public event PropertyChangedEventHandler PropertyChanged;

  protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
  {
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
  }

  public WPFViewModel()
  {
    MyText = "Hello, World!";
  }
}