using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;
using System;
using System.Collections.Generic;
using Autodesk.AutoCAD.Windows;
using System.Windows.Forms;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;
using System.Windows.Forms.Integration;

namespace VoltageDrop
{
  public class Commands
  {
    [CommandMethod("VoltageDrop")]
    public void VoltageDropCommand()
    {
      Document doc = Application.DocumentManager.MdiActiveDocument;
      Editor ed = doc.Editor;

      // Prompt for voltage
      PromptDoubleResult voltageRes = ed.GetDouble("\nEnter voltage: ");
      if (voltageRes.Status != PromptStatus.OK) return;
      double voltage = voltageRes.Value;

      // Prompt for phase
      PromptStringOptions phaseOptions = new PromptStringOptions("\nEnter phase (Single/Three): ")
      {
        AllowSpaces = false
      };
      PromptResult phaseRes = ed.GetString(phaseOptions);
      if (phaseRes.Status != PromptStatus.OK) return;
      string phase = phaseRes.StringResult;

      // Prompt for wire type
      PromptStringOptions wireTypeOptions = new PromptStringOptions("\nEnter wire type: ")
      {
        AllowSpaces = false
      };
      PromptResult wireTypeRes = ed.GetString(wireTypeOptions);
      if (wireTypeRes.Status != PromptStatus.OK) return;
      string wireType = wireTypeRes.StringResult;

      // Prompt for size of wire
      PromptDoubleResult sizeOfWireRes = ed.GetDouble("\nEnter size of wire (AWG): ");
      if (sizeOfWireRes.Status != PromptStatus.OK) return;
      double sizeOfWire = sizeOfWireRes.Value;

      // Prompt for length of wire
      PromptDoubleResult lengthOfWireRes = ed.GetDouble("\nEnter length of wire (feet): ");
      if (lengthOfWireRes.Status != PromptStatus.OK) return;
      double lengthOfWire = lengthOfWireRes.Value;

      // Prompt for amperage
      PromptDoubleResult amperageRes = ed.GetDouble("\nEnter amperage: ");
      if (amperageRes.Status != PromptStatus.OK) return;
      double amperage = amperageRes.Value;

      // Prompt for number of parallel wires
      PromptIntegerOptions numParallelWiresOptions = new PromptIntegerOptions("\nEnter number of parallel wires: ")
      {
        AllowNegative = false,
        AllowZero = false
      };
      PromptIntegerResult numParallelWiresRes = ed.GetInteger(numParallelWiresOptions);
      if (numParallelWiresRes.Status != PromptStatus.OK) return;
      int numParallelWires = numParallelWiresRes.Value;

      // Calculate voltage drop
      double resistance = GetResistance(wireType, sizeOfWire);
      double voltageDrop = CalculateVoltageDrop(phase, resistance, lengthOfWire, amperage, numParallelWires);
      double voltageDropPercentage = (voltageDrop / voltage) * 100;

      ed.WriteMessage($"\nVoltage Drop: {voltageDrop:F2} volts ({voltageDropPercentage:F2}%)");
    }

    [CommandMethod("TestVoltageDrop")]
    public void TestVoltageDropCommand()
    {
      double voltage = 120;
      string phase = "Single";
      string wireType = "COPPER";
      double sizeOfWire = 4;
      double lengthOfWire = 100;
      double amperage = 20;
      int numParallelWires = 1;

      double resistance = GetResistance(wireType, sizeOfWire);
      double voltageDrop = CalculateVoltageDrop(phase, resistance, lengthOfWire, amperage, numParallelWires);
      double voltageDropPercentage = (voltageDrop / voltage) * 100;

      Application.ShowAlertDialog($"Resistance: {resistance:F3} ohms per 1000 feet");

      Application.ShowAlertDialog($"Voltage Drop: {voltageDrop:F2} volts ({voltageDropPercentage:F2}%)");
    }

    [CommandMethod("WPF")]
    public void WPF()
    {
      // Instantiate the Windows Form
      VoltageDropForm myForm = new VoltageDropForm
      {
        AutoScaleMode = AutoScaleMode.Dpi, // Set the AutoScaleMode to Dpi
        FormBorderStyle = FormBorderStyle.FixedSingle, // Set the border style to prevent resizing
        Size = new System.Drawing.Size(350, 700) // Set the desired form size
      };

      // Instantiate the WPF UserControl
      VoltageDrop myWPFUserControl = new VoltageDrop();

      // Create an ElementHost control
      ElementHost host = new ElementHost
      {
        Dock = DockStyle.Fill, // Fill the host in the Windows Form
        Child = myWPFUserControl // Set the WPF UserControl as the Child of the host
      };

      // Add the ElementHost to the Windows Form's Controls collection
      myForm.Controls.Add(host);

      // Show the form
      Application.ShowModalDialog(myForm);
    }

    private double GetResistance(string wireType, double sizeOfWire)
    {
      double resistance = 0.0;

      switch (sizeOfWire)
      {
        case 18:
          resistance = wireType.ToUpper() == "COPPER" ? 7.77 : 12.8;
          break;

        case 16:
          resistance = wireType.ToUpper() == "COPPER" ? 4.89 : 8.05;
          break;

        case 14:
          resistance = wireType.ToUpper() == "COPPER" ? 3.07 : 5.06;
          break;

        case 12:
          resistance = wireType.ToUpper() == "COPPER" ? 1.93 : 3.18;
          break;

        case 10:
          resistance = wireType.ToUpper() == "COPPER" ? 1.21 : 2.00;
          break;

        case 8:
          resistance = wireType.ToUpper() == "COPPER" ? 0.764 : 1.26;
          break;

        case 6:
          resistance = wireType.ToUpper() == "COPPER" ? 0.491 : 0.808;
          break;

        case 4:
          resistance = wireType.ToUpper() == "COPPER" ? 0.308 : 0.508;
          break;

        case 3:
          resistance = wireType.ToUpper() == "COPPER" ? 0.245 : 0.403;
          break;

        case 2:
          resistance = wireType.ToUpper() == "COPPER" ? 0.194 : 0.319;
          break;

        case 1:
          resistance = wireType.ToUpper() == "COPPER" ? 0.154 : 0.253;
          break;

        case 0:
          resistance = wireType.ToUpper() == "COPPER" ? 0.122 : 0.201;
          break;

        case -1:
          resistance = wireType.ToUpper() == "COPPER" ? 0.0967 : 0.159;
          break;

        case -2:
          resistance = wireType.ToUpper() == "COPPER" ? 0.0766 : 0.126;
          break;

        case -3:
          resistance = wireType.ToUpper() == "COPPER" ? 0.0608 : 0.100;
          break;

        case 250:
          resistance = wireType.ToUpper() == "COPPER" ? 0.0515 : 0.0847;
          break;

        case 300:
          resistance = wireType.ToUpper() == "COPPER" ? 0.0429 : 0.0707;
          break;

        case 350:
          resistance = wireType.ToUpper() == "COPPER" ? 0.0367 : 0.0605;
          break;

        case 400:
          resistance = wireType.ToUpper() == "COPPER" ? 0.0321 : 0.0529;
          break;

        case 500:
          resistance = wireType.ToUpper() == "COPPER" ? 0.0258 : 0.0424;
          break;

        case 600:
          resistance = wireType.ToUpper() == "COPPER" ? 0.0214 : 0.0353;
          break;

        case 700:
          resistance = wireType.ToUpper() == "COPPER" ? 0.0184 : 0.0303;
          break;

        case 750:
          resistance = wireType.ToUpper() == "COPPER" ? 0.0171 : 0.0282;
          break;

        case 800:
          resistance = wireType.ToUpper() == "COPPER" ? 0.0161 : 0.0265;
          break;

        case 900:
          resistance = wireType.ToUpper() == "COPPER" ? 0.0143 : 0.0235;
          break;

        case 1000:
          resistance = wireType.ToUpper() == "COPPER" ? 0.0129 : 0.0212;
          break;

        case 1250:
          resistance = wireType.ToUpper() == "COPPER" ? 0.0103 : 0.0169;
          break;

        case 1500:
          resistance = wireType.ToUpper() == "COPPER" ? 0.00858 : 0.0141;
          break;

        case 1750:
          resistance = wireType.ToUpper() == "COPPER" ? 0.00735 : 0.0121;
          break;

        case 2000:
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

      if (phase.Equals("Single", StringComparison.OrdinalIgnoreCase))
      {
        voltageDrop = (2 * resistance * lengthOfWire * amperage) / (1000 * numParallelWires);
      }
      else if (phase.Equals("Three", StringComparison.OrdinalIgnoreCase))
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