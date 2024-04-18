using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace VoltageDrop.Tests
{
  [TestClass]
  public class VoltageDrop_Tests
  {
    [TestMethod]
    public void GetResistance_ValidCopperWireSize_ReturnsCorrectResistance()
    {
      // Arrange
      string wireType = "COPPER";
      string wireSize = "14";
      double expected = 3.07;

      // Act
      double actual = VoltageDrop.GetResistance(wireType, wireSize);

      // Assert
      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GetResistance_ValidAluminumWireSize_ReturnsCorrectResistance()
    {
      // Arrange
      string wireType = "ALUMINUM";
      string wireSize = "10";
      double expected = 2.00;

      // Act
      double actual = VoltageDrop.GetResistance(wireType, wireSize);

      // Assert
      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GetResistance_WireTypeCaseInsensitive_ReturnsCorrectResistance()
    {
      // Arrange
      string wireType = "cOpPeR";
      string wireSize = "8";
      double expected = 0.764;

      // Act
      double actual = VoltageDrop.GetResistance(wireType, wireSize);

      // Assert
      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GetResistance_InvalidWireType_ReturnsAluminumResistance()
    {
      // Arrange
      string wireType = "INVALID";
      string wireSize = "14";
      double expected = 5.06; // Assuming it defaults to aluminum resistance

      // Act
      double actual = VoltageDrop.GetResistance(wireType, wireSize);

      // Assert
      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GetResistance_InvalidWireSize_ThrowsArgumentException()
    {
      // Arrange
      string wireType = "COPPER";
      string wireSize = "INVALID";

      // Act & Assert
      Assert.ThrowsException<ArgumentException>(() => VoltageDrop.GetResistance(wireType, wireSize));
    }
  }
}