namespace WPFTesting
{
  public class WPFViewModelTests
  {
    [Fact]
    public void OnTextChanged_ValidInput_NoChanges()
    {
      // Arrange
      var viewModel = new WPFViewModel();
      viewModel.MyText = "ValidPassword123!";

      // Act
      viewModel.TextChangedCommand.Execute(null);

      // Assert
      Assert.Equal("ValidPassword123!", viewModel.MyText);
    }

    [Fact]
    public void OnTextChanged_InvalidInput_InvalidCharactersRemoved()
    {
      // Arrange
      var viewModel = new WPFViewModel();
      viewModel.MyText = "InvalidPassword123!@#$%^&*()";

      // Act
      viewModel.TextChangedCommand.Execute(null);

      // Assert
      Assert.Equal("InvalidPassword123", viewModel.MyText);
    }
  }
}