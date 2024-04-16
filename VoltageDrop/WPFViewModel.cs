using CommunityToolkit.Mvvm.ComponentModel;

public class WPFViewModel : ObservableObject
{
  private string _myText;

  public string MyText
  {
    get { return _myText; }
    set { SetProperty(ref _myText, value); }
  }

  public WPFViewModel()
  {
    MyText = "Hello, World!";
  }
}