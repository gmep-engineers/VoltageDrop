using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

public class WPFViewModel : ObservableObject
{
  private string _myText;

  [Required(ErrorMessage = "Please enter a value.")]
  [StringLength(10, ErrorMessage = "Maximum length is 10 characters.")]
  public string MyText
  {
    get { return _myText; }
    set
    {
      if (IsValidInput(value))
      {
        SetProperty(ref _myText, value);
      }
    }
  }

  public WPFViewModel()
  {
  }

  private bool IsValidInput(string input)
  {
    return !string.IsNullOrWhiteSpace(input) && input.Length <= 10;
  }
}