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
        /*
         Buy more button
        X: 613
        Y: 823

        First click bar
        X: 1095
        Y: 616

        Drag until
        X: 1150

        Buy button
        X: 820
        Y: 800

        Ok button
        X: 1140
        Y: 620


        Scroll forward: 
        X: 1250 
        Y: 783

        Topleft Reagentlist
        X: 780
        Y: 375

        Bottomright Reagentlist
        X: 1137
        Y: 750

            */
        public MainForm()
        {
            InitializeComponent();
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
