using System;
using System.Xml;
using System.Xml.XPath;

namespace XmlTest
{
    class Program
    {
        static void Main(string[] args)
        {
            String URLString = "http://mtconnect.mazakcorp.com:5609/";
            //String URLString = "file:///C:/Users/riccardo/Desktop/test.xml";
            XmlTextReader reader = new XmlTextReader(URLString);
            var xmlTree = GetXmlPath(reader, new System.Collections.Generic.Dictionary<string, object>());
            System.Collections.Generic.List<string> keys = new System.Collections.Generic.List<string>();
            bool quit = false;

            var docNav = new XPathDocument(URLString);
            var nav = docNav.CreateNavigator();
            do
            {
                if (xmlTree.Count > 0)
                {
                    Console.Write("[+] Select between ( ");

                    foreach (var item in xmlTree)
                    {
                        Console.Write($"{item.Key} ");
                    }

                    Console.WriteLine($"): ");
                    Console.Write($" > ");
                    string key = Console.ReadLine();

                    if (xmlTree.ContainsKey(key))
                    {
                        xmlTree = (System.Collections.Generic.Dictionary<string, object>)xmlTree[key];
                        keys.Add(key);
                    }
                    else
                    {
                        Console.WriteLine("[+] Quitting..");
                        quit = true;
                    }
                }
                else
                {
                    quit = true;
                    Console.WriteLine("[-] No elements...");
                }
            } while (!quit);
            

            Console.WriteLine("");
            Console.WriteLine("");
            Console.Write("/current?path=//");
            foreach (var item in keys)
                Console.Write(string.Concat(item, "/"));

            Console.WriteLine("");
            Console.WriteLine("");
        }

        public static System.Collections.Generic.Dictionary<string, object> GetXmlPath(XmlTextReader reader, System.Collections.Generic.Dictionary<string, object> path)
        {
            while (reader.Read())
            {
                System.Collections.Generic.Dictionary<string, object> newPath = new System.Collections.Generic.Dictionary<string, object>();
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element: // The node is an element.
                        string name = reader.Name;
                        if (!reader.IsEmptyElement)
                            newPath = GetXmlPath(reader, newPath);
                        path[name] = newPath;

                        break;
                    case XmlNodeType.EndElement: //Display the end of the element.
                        return path;
                        
                }
            }

            return path;
        }

        private static void Print(XmlTextReader reader)
        {
            int counter = 0;
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element: // The node is an element.
                        Console.Write("<" + reader.Name);

                        while (reader.MoveToNextAttribute()) // Read the attributes.
                            Console.Write(" " + reader.Name + "='" + reader.Value + "'");
                        Console.Write(">");
                        Console.WriteLine(">");
                        break;
                    case XmlNodeType.Text: //Display the text in each element.
                        Console.WriteLine(reader.Value);
                        break;
                    case XmlNodeType.EndElement: //Display the end of the element.
                        Console.Write("</" + reader.Name);
                        Console.WriteLine(">");
                        break;
                }
            }
        }

        //TreeViewItem newItem = new TreeViewItem();
        //newItem.Header = string.Format("{0}({1})",
        //        node.Attribute(OpcAttribute.DisplayName).Value,
        //        node.NodeId);

        //treeItem.Items.Add(newItem);

        //    foreach (var childNode in node.Children())
        //        BrowseOpcUaServer(childNode, newItem);
    }
}
