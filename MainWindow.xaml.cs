using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Win32;

namespace HTMLEmbeddedImage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string uril;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All Supported Images|*.jpg;*.jpeg;*.png|" + "JPEG(*.jpg;*.jpeg)|*.jpg;*.jpeg|" + "Portable Network Graphic(*.png)|*.png";
            if(op.ShowDialog() == true)
            {
                uril = op.FileName;
                notifier.Text = "Uploaded File : " + op.FileName;
            }
        }

        private void btnConvert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                byte[] bytes = FileToArray(uril);
                string url = "";
                string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                url = "data:image/png;base64, " + base64String;
                imageUrl.SelectAll();
                imageUrl.Cut();
                imageUrl.AppendText(url);
            }
            catch
            {
                notifier.Text = "No Image Selected";
            }
        }

        public byte[] FileToArray(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            Byte[] bytes = br.ReadBytes((Int32)fs.Length);
            br.Close();
            fs.Close();
            return bytes;
        }

        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            imageUrl.SelectAll();
            imageUrl.Copy();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            imageUrl.SelectAll();
            imageUrl.Cut();
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Main, "Embed Images directly into HTML Code, without using external images. Run your HTML Code Anywhere, Anytime. \n \n Designed By Edwards Moses","About Us");
        }

        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "HTML Document|*.html;*.htm";
            sf.FileName = "image.html";
            sf.Title = "HTML Embedded Image Example";
            if(sf.ShowDialog() == true)
            {
                try
                {
                    Clipboard.Clear();
                    imageUrl.SelectAll();
                    imageUrl.Copy();
                    FileStream fs = new FileStream(sf.FileName, FileMode.Create, FileAccess.Write);
                    StreamWriter bs = new StreamWriter(fs);
                    FlowDocument fr = imageUrl.Document;
                    string m = Clipboard.GetText();
                    bs.Write("<html><body><h1>HTML Embedded Image : </h1><img src='" + m + "'/><div><p>Designed by Edwards Moses<p></div></body></html>");
                    bs.Close();
                    fs.Close();
                    notifier.Text = "HTML Document Saved at: " + sf.FileName;
                }
                catch
                {
                    notifier.Text = "Error saving HTML Document.";
                }
            }
        }
    }
}
