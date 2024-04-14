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
    [CommandMethod("WPF")]
    public void WPF()
    {
      // Instantiate the Windows Form
      VoltageDropForm myForm = new VoltageDropForm
      {
        AutoScaleMode = AutoScaleMode.Dpi, // Set the AutoScaleMode to Dpi
        FormBorderStyle = FormBorderStyle.FixedSingle, // Set the border style to prevent resizing
        Size = new System.Drawing.Size(700, 900) // Set the desired form size
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
  }
}