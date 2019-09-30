using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wizard101BazaarBot
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void AvailableBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string currItem = availableBox.GetItemText(availableBox.SelectedItem);
            availableBox.Items.RemoveAt(availableBox.SelectedIndex);
            selectedBox.Items.Add(currItem);
        }

        private void SelectedBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string currItem = selectedBox.SelectedItem.ToString();
            selectedBox.Items.RemoveAt(selectedBox.SelectedIndex);
            availableBox.Items.Add(currItem);
        }
    }
}
