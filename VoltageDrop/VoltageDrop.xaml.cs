using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VoltageDrop
{
  public partial class VoltageDrop : UserControl
  {
    public VoltageDrop()
    {
      InitializeComponent();

      // Set default values for combo boxes
      VoltageComboBox.SelectedIndex = 1;
      PercentageComboBox.SelectedIndex = 2;
      PhaseComboBox.SelectedIndex = 1;
      WireTypeComboBox.SelectedIndex = 0;
      WireSizeComboBox.SelectedIndex = 10;

      // Set default values for text boxes
      LengthTextBox.Text = "100";
      AmperageTextBox.Text = "200";
      ParallelWiresTextBox.Text = "1";
    }

    private void UpdateVoltageDrop()
    {
      // Get selected voltage
      if (VoltageComboBox.SelectedItem == null) return;
      double voltage = double.Parse(((ComboBoxItem)VoltageComboBox.SelectedItem).Content.ToString().Replace("V", ""));

      // Get selected percentage
      if (PercentageComboBox.SelectedItem == null) return;
      double percentage = double.Parse(((ComboBoxItem)PercentageComboBox.SelectedItem).Content.ToString().Replace("%", ""));

      // Get selected phase
      if (PhaseComboBox.SelectedItem == null) return;
      string phase = ((ComboBoxItem)PhaseComboBox.SelectedItem).Content.ToString();

      // Get selected wire type
      if (WireTypeComboBox.SelectedItem == null) return;
      string wireType = ((ComboBoxItem)WireTypeComboBox.SelectedItem).Content.ToString().ToUpper();

      // Get selected wire size
      if (WireSizeComboBox.SelectedItem == null) return;
      string wireSize = ((ComboBoxItem)WireSizeComboBox.SelectedItem).Content.ToString();

      // Get length of wire
      if (string.IsNullOrWhiteSpace(LengthTextBox.Text)) return;
      if (!double.TryParse(LengthTextBox.Text, out double lengthOfWire))
      {
        MessageBox.Show("Invalid length of wire. Please enter a valid number.");
        return;
      }

      // Get amperage
      if (string.IsNullOrWhiteSpace(AmperageTextBox.Text)) return;
      if (!double.TryParse(AmperageTextBox.Text, out double amperage))
      {
        MessageBox.Show("Invalid amperage. Please enter a valid number.");
        return;
      }

      // Get number of parallel wires
      if (string.IsNullOrWhiteSpace(ParallelWiresTextBox.Text)) return;
      if (!int.TryParse(ParallelWiresTextBox.Text, out int numParallelWires))
      {
        MessageBox.Show("Invalid number of parallel wires. Please enter a valid integer.");
        return;
      }

      // Calculate voltage drop
      double resistance = GetResistance(wireType, wireSize);
      double voltageDrop = CalculateVoltageDrop(phase, resistance, lengthOfWire, amperage, numParallelWires);
      double voltageDropPercentage = (voltageDrop / voltage) * 100;

      // Display results
      VoltageDropTextBox.Text = $"{voltageDrop:F2}V";
      VoltageDropPercentageTextBox.Text = $"{voltageDropPercentage:F2}%";

      // Update the background color of VoltageDropPercentageTextBox
      UpdateVoltageDropPercentageBackground(voltageDropPercentage, percentage);

      // Calculate recommended wire size
      string recommendedWireSize = GetRecommendedWireSize(amperage);
      RecommendedWireSizeTextBox.Text = recommendedWireSize;
    }

    private void UpdateVoltageDropPercentageBackground(double voltageDropPercentage, double userSelectedPercentage)
    {
      if (voltageDropPercentage > userSelectedPercentage)
      {
        VoltageDropPercentageTextBox.Background = new SolidColorBrush(Colors.Red);
      }
      else
      {
        VoltageDropPercentageTextBox.Background = new SolidColorBrush(Colors.Green);
      }
    }

    private string GetRecommendedWireSize(double amperage)
    {
      // Get selected voltage
      double voltage = double.Parse(((ComboBoxItem)VoltageComboBox.SelectedItem).Content.ToString().Replace("V", ""));

      // Get selected percentage
      double percentage = double.Parse(((ComboBoxItem)PercentageComboBox.SelectedItem).Content.ToString().Replace("%", ""));

      // Get selected phase
      string phase = ((ComboBoxItem)PhaseComboBox.SelectedItem).Content.ToString();

      // Get selected wire type
      string wireType = ((ComboBoxItem)WireTypeComboBox.SelectedItem).Content.ToString().ToUpper();

      // Get length of wire
      double lengthOfWire = double.Parse(LengthTextBox.Text);

      int numParallelWires = 1;
      if (amperage >= 800)
        numParallelWires = 4;
      else if (amperage >= 400)
        numParallelWires = 2;

      double amperagePerWire = amperage / numParallelWires;
      string wireSize = GetWireSize(amperagePerWire);

      double resistance = GetResistance(wireType, wireSize);
      double voltageDrop = CalculateVoltageDrop(phase, resistance, lengthOfWire, amperagePerWire, 1);
      double voltageDropPercentage = (voltageDrop / voltage) * 100;

      while (voltageDropPercentage > percentage)
      {
        string nextWireSize = GetNextWireSize(wireSize);
        if (nextWireSize == wireSize)
          break;

        wireSize = nextWireSize;
        resistance = GetResistance(wireType, wireSize);
        voltageDrop = CalculateVoltageDrop(phase, resistance, lengthOfWire, amperagePerWire, 1);
        voltageDropPercentage = (voltageDrop / voltage) * 100;
      }

      if (numParallelWires > 1)
        return $"{numParallelWires} x {wireSize}";
      else
        return wireSize;
    }

    private void UpdateRecommendedWireSize(string recommendedWireSize)
    {
      string wireSize;
      int numParallelWires;

      if (recommendedWireSize.Contains("x"))
      {
        string[] parts = recommendedWireSize.Split('x');
        numParallelWires = int.Parse(parts[0].Trim());
        wireSize = parts[1].Trim();
      }
      else
      {
        numParallelWires = 1;
        wireSize = recommendedWireSize.Trim();
      }

      WireSizeComboBox.SelectedItem = WireSizeComboBox.Items.Cast<ComboBoxItem>()
          .FirstOrDefault(item => item.Content.ToString() == wireSize);

      ParallelWiresTextBox.Text = numParallelWires.ToString();
    }

    private string GetNextWireSize(string wireSize)
    {
      string[] wireSizes = { "12", "10", "8", "6", "4", "3", "2", "1", "1/0", "2/0", "3/0", "4/0", "250", "300", "350", "400", "500", "600", "700", "750", "800", "900", "1000", "1250", "1500", "1750", "2000" };

      int index = Array.IndexOf(wireSizes, wireSize);
      if (index == -1 || index == wireSizes.Length - 1)
        return wireSize;

      return wireSizes[index + 1];
    }

    private double GetResistance(string wireType, string sizeOfWire)
    {
      double resistance = 0.0;

      switch (sizeOfWire)
      {
        case "18":
          resistance = wireType.ToUpper() == "COPPER" ? 7.77 : 12.8;
          break;

        case "16":
          resistance = wireType.ToUpper() == "COPPER" ? 4.89 : 8.05;
          break;

        case "14":
          resistance = wireType.ToUpper() == "COPPER" ? 3.07 : 5.06;
          break;

        case "12":
          resistance = wireType.ToUpper() == "COPPER" ? 1.93 : 3.18;
          break;

        case "10":
          resistance = wireType.ToUpper() == "COPPER" ? 1.21 : 2.00;
          break;

        case "8":
          resistance = wireType.ToUpper() == "COPPER" ? 0.764 : 1.26;
          break;

        case "6":
          resistance = wireType.ToUpper() == "COPPER" ? 0.491 : 0.808;
          break;

        case "4":
          resistance = wireType.ToUpper() == "COPPER" ? 0.308 : 0.508;
          break;

        case "3":
          resistance = wireType.ToUpper() == "COPPER" ? 0.245 : 0.403;
          break;

        case "2":
          resistance = wireType.ToUpper() == "COPPER" ? 0.194 : 0.319;
          break;

        case "1":
          resistance = wireType.ToUpper() == "COPPER" ? 0.154 : 0.253;
          break;

        case "1/0":
          resistance = wireType.ToUpper() == "COPPER" ? 0.122 : 0.201;
          break;

        case "2/0":
          resistance = wireType.ToUpper() == "COPPER" ? 0.0967 : 0.159;
          break;

        case "3/0":
          resistance = wireType.ToUpper() == "COPPER" ? 0.0766 : 0.126;
          break;

        case "4/0":
          resistance = wireType.ToUpper() == "COPPER" ? 0.0608 : 0.100;
          break;

        case "250":
          resistance = wireType.ToUpper() == "COPPER" ? 0.0515 : 0.0847;
          break;

        case "300":
          resistance = wireType.ToUpper() == "COPPER" ? 0.0429 : 0.0707;
          break;

        case "350":
          resistance = wireType.ToUpper() == "COPPER" ? 0.0367 : 0.0605;
          break;

        case "400":
          resistance = wireType.ToUpper() == "COPPER" ? 0.0321 : 0.0529;
          break;

        case "500":
          resistance = wireType.ToUpper() == "COPPER" ? 0.0258 : 0.0424;
          break;

        case "600":
          resistance = wireType.ToUpper() == "COPPER" ? 0.0214 : 0.0353;
          break;

        case "700":
          resistance = wireType.ToUpper() == "COPPER" ? 0.0184 : 0.0303;
          break;

        case "750":
          resistance = wireType.ToUpper() == "COPPER" ? 0.0171 : 0.0282;
          break;

        case "800":
          resistance = wireType.ToUpper() == "COPPER" ? 0.0161 : 0.0265;
          break;

        case "900":
          resistance = wireType.ToUpper() == "COPPER" ? 0.0143 : 0.0235;
          break;

        case "1000":
          resistance = wireType.ToUpper() == "COPPER" ? 0.0129 : 0.0212;
          break;

        case "1250":
          resistance = wireType.ToUpper() == "COPPER" ? 0.0103 : 0.0169;
          break;

        case "1500":
          resistance = wireType.ToUpper() == "COPPER" ? 0.00858 : 0.0141;
          break;

        case "1750":
          resistance = wireType.ToUpper() == "COPPER" ? 0.00735 : 0.0121;
          break;

        case "2000":
          resistance = wireType.ToUpper() == "COPPER" ? 0.00643 : 0.0106;
          break;

        default:
          throw new ArgumentException("Invalid wire size.");
      }

      return resistance;
    }

    private double CalculateVoltageDrop(string phase, double resistance, double lengthOfWire, double amperage, int numParallelWires)
    {
      double voltageDrop;

      if (phase.Equals("Single Phase", StringComparison.OrdinalIgnoreCase))
      {
        voltageDrop = (2 * resistance * lengthOfWire * amperage) / (1000 * numParallelWires);
      }
      else if (phase.Equals("Three Phase", StringComparison.OrdinalIgnoreCase))
      {
        voltageDrop = (Math.Sqrt(3) * resistance * lengthOfWire * amperage) / (1000 * numParallelWires);
      }
      else
      {
        throw new ArgumentException("Invalid phase specified.");
      }

      return voltageDrop;
    }

    private string GetWireSize(double amperage)
    {
      switch (amperage)
      {
        case double n when n <= 25:
          return "12";

        case double n when n <= 30:
          return "10";

        case double n when n <= 40:
          return "8";

        case double n when n <= 55:
          return "6";

        case double n when n <= 70:
          return "4";

        case double n when n <= 85:
          return "3";

        case double n when n <= 95:
          return "2";

        case double n when n <= 110:
          return "1";

        case double n when n <= 150:
          return "1/0";

        case double n when n <= 175:
          return "2/0";

        case double n when n <= 200:
          return "3/0";

        case double n when n <= 230:
          return "4/0";

        case double n when n <= 255:
          return "250";

        case double n when n <= 285:
          return "300";

        case double n when n <= 310:
          return "350";

        case double n when n <= 335:
          return "400";

        case double n when n <= 380:
          return "500";

        case double n when n <= 420:
          return "600";

        case double n when n <= 460:
          return "700";

        case double n when n <= 475:
          return "750";

        case double n when n <= 490:
          return "800";

        case double n when n <= 520:
          return "900";

        case double n when n <= 545:
          return "1000";

        case double n when n <= 590:
          return "1250";

        case double n when n <= 625:
          return "1500";

        case double n when n <= 650:
          return "1750";

        case double n when n <= 665:
          return "2000";

        default:
          return "Wire size not found";
      }
    }

    private void VoltageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      UpdateVoltageDrop();
    }

    private void PercentageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      UpdateVoltageDrop();
    }

    private void PhaseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      UpdateVoltageDrop();
    }

    private void WireTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      UpdateVoltageDrop();
    }

    private void WireSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      UpdateVoltageDrop();
    }

    private void LengthTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
      UpdateVoltageDrop();
    }

    private void AmperageTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
      UpdateVoltageDrop();
    }

    private void ParallelWiresTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
      UpdateVoltageDrop();
    }
  }
}