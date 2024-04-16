using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using System.Windows.Forms;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;
using System.Windows.Forms.Integration;

namespace VoltageDrop
{
  public class Commands
  {
    [CommandMethod("VD")]
    public void VD()
    {
      // Instantiate the Windows Form
      VoltageDropForm myForm = new VoltageDropForm
      {
        AutoScaleMode = AutoScaleMode.Dpi, // Set the AutoScaleMode to Dpi
        FormBorderStyle = FormBorderStyle.FixedSingle, // Set the border style to prevent resizing
        Size = new System.Drawing.Size(700, 755) // Set the desired form size
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
      Application.ShowModelessDialog(myForm);
    }

    [CommandMethod("WPF")]
    public void WPF()
    {
      // Create a new instance of the WPFControl
      WPFControl myWPFControl = new WPFControl();

      // Create a new Windows Form to host the WPFControl
      Form hostForm = new Form
      {
        Text = "WPF User Control",
        FormBorderStyle = FormBorderStyle.FixedSingle,
        MinimizeBox = false,
        MaximizeBox = false,
        StartPosition = FormStartPosition.CenterScreen,
        Width = 800,
        Height = 600
      };

      // Create an ElementHost to host the WPFControl
      ElementHost elementHost = new ElementHost
      {
        Dock = DockStyle.Fill,
        Child = myWPFControl
      };

      // Add the ElementHost to the form's controls collection
      hostForm.Controls.Add(elementHost);

      // Show the form as a modeless dialog
      Application.ShowModelessDialog(hostForm);
    }
  }
}