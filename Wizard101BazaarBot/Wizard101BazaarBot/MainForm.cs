using Google.Cloud.Vision.V1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
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
            LoadSettings();
            Control.CheckForIllegalCrossThreadCalls = false;
            new Thread(() => UpdateMouse()).Start();
        }

        private void LoadSettings()
        {
            if(File.Exists("config.cfg"))
            {
                RootObject obj = new JavaScriptSerializer().Deserialize<RootObject>(File.ReadAllText("config.cfg"));
                foreach(var reagent in obj.availableReagents)
                {
                    availableBox.Items.Add(reagent);
                }
                foreach(var reagent in obj.selectedReagents)
                {
                    selectedBox.Items.Add(reagent);
                }
                reagentXBox.Text = obj.reagentsX.ToString();
                reagentYBox.Text = obj.reagentsY.ToString();
            }else
            {
                RootObject root = new RootObject();
                root.availableReagents = new List<string>();
                root.selectedReagents = new List<string>();
                root.reagentsX = 507;
                root.reagentsY = 273;
                File.WriteAllText("config.cfg", new JavaScriptSerializer().Serialize(root));
            }
        }

        private void SaveSettings()
        {
            RootObject obj = new RootObject();
            obj.reagentsX = int.Parse(reagentXBox.Text);
            obj.reagentsY = int.Parse(reagentYBox.Text);
            obj.availableReagents = new List<string>();
            foreach(var item in availableBox.Items)
            {
                obj.availableReagents.Add(item.ToString());
            }
            obj.selectedReagents = new List<string>();
            foreach (var item in selectedBox.Items)
            {
                obj.selectedReagents.Add(item.ToString());
            }

            File.WriteAllText("config.cfg", new JavaScriptSerializer().Serialize(obj));
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

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void BuySelected()
        {

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
