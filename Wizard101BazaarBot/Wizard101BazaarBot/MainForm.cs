using AForge.Imaging;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using Wizard101BazaarBot.ImageRecognition;

namespace Wizard101BazaarBot
{
    public partial class MainForm : Form
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern short GetAsyncKeyState(int vKey);
        /*
         Buy more button
        X: 613 
        X Off: 106
        Y: 823
        Y Off: 550

        First click bar
        X: 1095
        X Off: 588
        Y: 616
        Y Off: 343

        Drag until
        X: 1150
        X Off: 643

        Buy button
        X: 820
        X Off: 313
        Y: 800
        Y Off: 527

        Ok button
        X: 1140
        Y: 620


        Scroll forward: 
        X: 1250 
        X Off: 633
        Y: 783
        Y Off: 510

        Topleft Reagentlist
        X: 780
        Y: 375

        Bottomright Reagentlist
        X: 1137
        Y: 750

            */


        private Thread mainThread;
        
        public MainForm()
        {
            InitializeComponent();
            LoadSettings();
            /*string[] availableReagents = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "BazaarItems"), "*.png");
            foreach (var reagent in availableReagents)
            {
                string[] splitter = reagent.Split('\\');
                root.availableReagents.Add(splitter[splitter.Length-1].Replace(".png", ""));
            }*/
            Control.CheckForIllegalCrossThreadCalls = false;
            new Thread(() => UpdateMouse()).Start();
            mainThread = new Thread(()=> Main());
            /*var positions = Recognize.GetPositions(new Bitmap(Path.Combine(Directory.GetCurrentDirectory(), "BazaarItems/steelnew.png")));
            if (positions.Count == 0)
                Log("Couldn't find ");
            else
            {
                Log("Found, attempting to buy");
                WINAPI.click(positions[0].X, positions[0].Y);
                Thread.Sleep(50);
                //BuySelected();
            }*/
        }

        private bool ContainsLoading(Bitmap bitmapToSearchFor)
        {
            Rectangle rect = new Rectangle(new Point(0, 0), bitmapToSearchFor.Size);
            Bitmap formattedImage = bitmapToSearchFor.Clone(rect, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            if (Recognize.FindBitmapsEntry(Recognize.makeScreen2(true), formattedImage).Count > 0)
            {
                formattedImage.Dispose();
                return true;
            }
            else
            {
                formattedImage.Dispose();
                return false;
            }
        }

        private bool ContainsNext(Bitmap bitmapToSearchFor)
        {
            Rectangle rect = new Rectangle(new Point(0, 0), bitmapToSearchFor.Size);
            Bitmap formattedImage = bitmapToSearchFor.Clone(rect, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            if (Recognize.FindBitmapsEntry(Recognize.makeScreen2(), formattedImage).Count > 0)
            {
                formattedImage.Dispose();
                return true;
            }
            else
            {
                formattedImage.Dispose();
                return false;
            }
        }

        private void Main()
        {
            /*Image<Bgr, byte> Image1 = new Image<Bgr, byte>(Recognize.makeScreen(true)); //Your first image
            Image<Bgr, byte> Image2 = new Image<Bgr, byte>(new Bitmap(Path.Combine(Directory.GetCurrentDirectory(), "BazaarItems/titanium.png")).ConvertToFormat(System.Drawing.Imaging.PixelFormat.Format24bppRgb)); //Your second image

            double Threshold = 0.9; //set it to a decimal value between 0 and 1.00, 1.00 meaning that the images must be identical

            Image<Gray, float> Matches = Image1.MatchTemplate(Image2, TemplateMatchingType.CcoeffNormed);

            for (int y = 0; y < Matches.Data.GetLength(0); y++)
            {
                for (int x = 0; x < Matches.Data.GetLength(1); x++)
                {
                    if (Matches.Data[y, x, 0] >= Threshold) //Check if its a valid match
                    {

                        Log("Hey I found titanium!");
                    }
                }
            }*/
            while (true)//Main loop
            {
                int counter = 1;
                //Refresh layer
                WINAPI.click(int.Parse(reagentXBox.Text), int.Parse(reagentYBox.Text));
                Cursor.Position = new Point(0,0);
                Thread.Sleep(50);
                while (ContainsLoading(new Bitmap(Path.Combine(Directory.GetCurrentDirectory(), "BazaarUtils/isloading.png"))))
                {
                    Thread.Sleep(1);
                }

                bool runAgain = true;
                while(runAgain)
                {
                    //Log("Scanning page " + counter);
                    counter++;
                    foreach (var item in selectedBox.Items)
                    {
                        Image<Bgr, byte> source = new Image<Bgr, byte>(Recognize.makeScreen(true)); // Image B
                        Image<Bgr, byte> template = new Image<Bgr, byte>(new Bitmap(Path.Combine(Directory.GetCurrentDirectory(), "BazaarItems/" + item + ".png")).ConvertToFormat(System.Drawing.Imaging.PixelFormat.Format24bppRgb)); // Image A

                        using (Image<Gray, float> result = source.MatchTemplate(template, TemplateMatchingType.CcoeffNormed))
                        {
                            double[] minValues, maxValues;
                            Point[] minLocations, maxLocations;
                            result.MinMax(out minValues, out maxValues, out minLocations, out maxLocations);

                            if (maxValues[0] > 0.9)
                            {
                                // This is a match. Do something with it, for example draw a rectangle around it.
                                Rectangle match = new Rectangle(maxLocations[0], template.Size);
                                Log("Found " + item);
                                WINAPI.click(match.X + 800, match.Y + 380);
                                Thread.Sleep(50);
                                BuySelected();
                            }
                        }

                        source.Dispose();
                        template.Dispose();
                    }
                    Thread.Sleep(100);
                    if (ContainsNext(new Bitmap(Path.Combine(Directory.GetCurrentDirectory(), "BazaarUtils/nextpagegray.png"))))
                    {
                        runAgain = false;
                    }else
                    {
                        WINAPI.click(1250, 780);
                        Cursor.Position = new Point(0, 0);
                        Thread.Sleep(50);
                    }
                }
            }
        }

        private void LoadSettings()
        {
            if(File.Exists("config.cfg"))
            {
                List<string> foundReagents = new List<string>();
                string[] availableReagents = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "BazaarItems"), "*.png");
                foreach (var reagent in availableReagents)
                {
                    string[] splitter = reagent.Split('\\');
                    foundReagents.Add(splitter[splitter.Length - 1].Replace(".png", ""));
                }

                RootObject obj = new JavaScriptSerializer().Deserialize<RootObject>(File.ReadAllText("config.cfg"));
                foreach(var reagent in obj.availableReagents)
                {
                    if(File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "BazaarItems", reagent+".png")))
                    {
                        availableBox.Items.Add(reagent);
                    }else
                    {
                        Log("File not found: "+ Path.Combine(Directory.GetCurrentDirectory(), "BazaarItems", reagent + ".png"));
                    }
                }
                foreach(var reagent in obj.selectedReagents)
                {
                    if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "BazaarItems", reagent + ".png")))
                    {
                        selectedBox.Items.Add(reagent);
                    }
                    else
                    {
                        Log("File not found: " + Path.Combine(Directory.GetCurrentDirectory(), "BazaarItems", reagent + ".png"));
                    }
                }
                reagentXBox.Text = obj.reagentsX.ToString();
                reagentYBox.Text = obj.reagentsY.ToString();
                foreach (var reagent in foundReagents)
                {
                    if(!obj.availableReagents.Contains(reagent) && !obj.selectedReagents.Contains(reagent))
                    {
                        availableBox.Items.Add(reagent);
                        Log("Found new item: "+reagent);
                        SaveSettings();
                    }
                }
                
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
                if(this.WindowState == FormWindowState.Normal)
                {
                    xLabel.Text = "X: " + Cursor.Position.X;
                    yLabel.Text = "Y: " + Cursor.Position.Y;
                    Thread.Sleep(10);
                }

                if (GetAsyncKeyState(0x70) != 0)
                {
                    mainThread.Abort();
                }
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

        private void Log(string message)
        {
            logBox.Text += "\n[" + DateTime.Now.ToLongTimeString() + "]  " + message;
            logBox.SelectionStart = logBox.Text.Length;
            logBox.ScrollToCaret();
        }

        private bool NextPage()
        {
            WINAPI.click(int.Parse(reagentXBox.Text) + 633, int.Parse(reagentYBox.Text) + 510);
            Thread.Sleep(300);
            Cursor.Position = new Point(0, 0);
            if (Recognize.Contains(new Bitmap(Path.Combine(Directory.GetCurrentDirectory(), "BazaarUtils/nextpage.png"))))
            {
                return true;
            }else
            {
                return false;
            }
        }

        private void BuySelected()
        {
            Log("Attempting to buy");
            WINAPI.click(int.Parse(reagentXBox.Text) + 106, int.Parse(reagentYBox.Text) + 550);
            Thread.Sleep(300);
            WINAPI.mDown(int.Parse(reagentXBox.Text) + 588, int.Parse(reagentYBox.Text) + 343);
            Thread.Sleep(300);
            WINAPI.mDragTo(int.Parse(reagentXBox.Text) + 643, int.Parse(reagentYBox.Text) + 343);
            Thread.Sleep(300);
            WINAPI.click(int.Parse(reagentXBox.Text) + 313, int.Parse(reagentYBox.Text) + 527);
            Cursor.Position = new Point(0,0);
            Thread.Sleep(500);
            Log("Clicked buy");
            WINAPI.click(int.Parse(reagentXBox.Text) + 313, int.Parse(reagentYBox.Text) + 527);
            Cursor.Position = new Point(0, 0);
            Recognize.makeScreen().Save(Path.Combine(Directory.GetCurrentDirectory(), $"Purchases/{DateTime.Now.ToBinary()}.png"));
            Image<Bgr, byte> source = new Image<Bgr, byte>(Recognize.makeScreen()); // Image B
            Image<Bgr, byte> template = new Image<Bgr, byte>(new Bitmap(Path.Combine(Directory.GetCurrentDirectory(), "BazaarUtils/ok.png"))); // Image A

            using (Image<Gray, float> result = source.MatchTemplate(template, TemplateMatchingType.CcoeffNormed))
            {
                double[] minValues, maxValues;
                Point[] minLocations, maxLocations;
                result.MinMax(out minValues, out maxValues, out minLocations, out maxLocations);

                // You can try different values of the threshold. I guess somewhere between 0.75 and 0.95 would be good.
                if (maxValues[0] > 0.85)
                {
                    Log("Found ok button at X: "+maxLocations[0].X + " Y: "+maxLocations[0].Y);
                    // This is a match. Do something with it, for example draw a rectangle around it.
                    Rectangle match = new Rectangle(maxLocations[0], template.Size);
                    WINAPI.click(match.X, match.Y);
                    Thread.Sleep(50);
                    Log("Clicked ok button");
                }
                else
                {
                    Log("I couldn't find the ok button!");
                }
            }

            WINAPI.click(1250, 780);
            Cursor.Position = new Point(0, 0);
            Thread.Sleep(50);
            WINAPI.click(1140, 620);
            Cursor.Position = new Point(0, 0);
            Thread.Sleep(50);
            source.Dispose();
            template.Dispose();
        }

        private void StopBtn_Click(object sender, EventArgs e)
        {
            Log("this is a log message!");
            //BuySelected();  
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            if(mainThread.IsAlive)
            {
                MessageBox.Show("Please stop the current task first.");
            }else
            {
                mainThread.Start();
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
