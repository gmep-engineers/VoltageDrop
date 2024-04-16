using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace VoltageDrop
{
  public partial class MainViewModel : ObservableObject
  {
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(VoltageDrop))]
    public string voltage;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(VoltageDrop))]
    public string percentage;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(VoltageDrop))]
    public string phase;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(VoltageDrop))]
    public string wireType;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(VoltageDrop))]
    public string wireSize;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(VoltageDrop))]
    public string length;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(VoltageDrop))]
    public string amperage;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(VoltageDrop))]
    public string numberOfParallelWires;

    public string VoltageDrop => voltage;

    private string UpdateVoltageDrop()
    {
      double resistance = GetResistance(wireType, wireSize);

      double parsedLength;
      if (!double.TryParse(length, out parsedLength))
      {
        return "Invalid length value";
      }

      double parsedAmperage;
      if (!double.TryParse(amperage, out parsedAmperage))
      {
        return "Invalid amperage value";
      }

      int parsedNumParallelWires;
      if (!int.TryParse(numberOfParallelWires, out parsedNumParallelWires))
      {
        return "Invalid number of parallel wires";
      }

      double voltageDrop = CalculateVoltageDrop(phase, resistance, parsedLength, parsedAmperage, parsedNumParallelWires);

      return $"{voltageDrop:F2}V";
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
  }
}