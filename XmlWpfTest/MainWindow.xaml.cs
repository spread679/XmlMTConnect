using System;
using System.Collections.Generic;
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
using System.Xml;

namespace XmlWpfTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.EndpointTextBox.Text = "http://mtconnect.mazakcorp.com:5609/";
        }

        public TreeViewItem GetXmlPath(XmlTextReader reader, TreeViewItem path)
        {
            while (reader.Read())
            {
                TreeViewItem newPath = new TreeViewItem();
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element: // The node is an element.
                        newPath.Header = reader.Name;
                        newPath.PreviewMouseDoubleClick += NewPath_PreviewMouseDoubleClick;

                        if (!reader.IsEmptyElement)
                            newPath = GetXmlPath(reader, newPath);

                        while (reader.MoveToNextAttribute()) // Read the attributes.
                        {
                            TreeViewItem attribute = new TreeViewItem()
                            {
                                Header = string.Concat("@", reader.Name, "=\"", reader.Value, "\"")
                            };
                            attribute.PreviewMouseDoubleClick += NewPath_PreviewMouseDoubleClick;
                            newPath.Items.Add(attribute);
                        }

                        path.Items.Add(newPath);

                        break;
                    case XmlNodeType.EndElement: //Display the end of the element.
                        return path;

                }
            }

            return path;
        }

        private void NewPath_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)sender;
            string header = item.Header.ToString();

            if (string.IsNullOrEmpty(this.NodeTextBox.Text))
            {
                this.NodeTextBox.Text = "/current?path=//";
            }

            if (header.StartsWith("@"))
            {
                if (this.NodeTextBox.Text.EndsWith("/"))
                    this.NodeTextBox.Text = this.NodeTextBox.Text.Substring(0, this.NodeTextBox.Text.Length - 1);
                this.NodeTextBox.Text += string.Concat("[", header, "]");
            }
            else
            {
                this.NodeTextBox.Text += string.Concat(item.Header, "/");
            }
        }

        private void GetEndpointButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                XmlTextReader reader = new XmlTextReader(this.EndpointTextBox.Text);
                this.RootTreeViewItem = GetXmlPath(reader, this.RootTreeViewItem);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
