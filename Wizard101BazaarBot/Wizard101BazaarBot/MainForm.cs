using Google.Cloud.Vision.V1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wizard101BazaarBot
{
    

    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            /*Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @"C:\Users\Jannik\Downloads\My First Project-5ac39e1eeea6.json");
            Google.Cloud.Vision.V1.Image img1 = Google.Cloud.Vision.V1.Image.FromFile(@"C:\Users\Jannik\Desktop\2019-10-01 18_36_21-Wizard101.png");
            ImageAnnotatorClient client = ImageAnnotatorClient.Create();
            IReadOnlyList<EntityAnnotation> textAnnotations = client.DetectLabels(img1);
            foreach (EntityAnnotation text in textAnnotations)
            {
                Console.WriteLine($"Description: {text.Description}");
            }*/
            Control.CheckForIllegalCrossThreadCalls = false;
            new Thread(() => UpdateMouse()).Start();
        }

        private void UpdateMouse()
        {
            while(true)
            {
                xLabel.Text = "X: "+Cursor.Position.X;
                yLabel.Text = "Y: "+Cursor.Position.Y;
                Thread.Sleep(10);
            }
        }

        private void AvailableBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(availableBox.GetItemText(availableBox.SelectedItem) != "")
            {
                string currItem = availableBox.GetItemText(availableBox.SelectedItem);
                availableBox.Items.RemoveAt(availableBox.SelectedIndex);
                selectedBox.Items.Add(currItem);
            }
            
        }

        private void SelectedBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(selectedBox.GetItemText(selectedBox.SelectedItem) != "")
            {
                string currItem = selectedBox.GetItemText(selectedBox.SelectedItem);
                selectedBox.Items.RemoveAt(selectedBox.SelectedIndex);
                availableBox.Items.Add(currItem);
            }
        }
    }

    public class RootObject
    {
        public List<string> availableReagents { get; set; }
        public List<string> selectedReagents { get; set; }
        public int reagentsX { get; set; }
        public int reagentsY { get; set; }
    }
}
