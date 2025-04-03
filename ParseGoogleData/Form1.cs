using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using HtmlAgilityPack;

namespace ParseGoogleData
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string filePath = "C:\\test folder\\MyActivity.html"; // Change this to the actual file path
            string outputFilePath = "C:\\test folder\\ParsedLocations.txt";
            string coordinatesFilePath = "C:\\test folder\\Coordinates.txt";

            if (!File.Exists(filePath))
            {
                MessageBox.Show("File not found: " + filePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.Load(filePath, Encoding.UTF8);

            var links = doc.DocumentNode.SelectNodes("//a[@href]")
                            ?.Select(node => node.GetAttributeValue("href", ""));

            if (links != null && links.Any())
            {
                StringBuilder result = new StringBuilder();
                StringBuilder coordinates = new StringBuilder();
                var locationPattern = new Regex(@"/@([-0-9.]+),([-0-9.]+)");

                foreach (var link in links)
                {
                    if (link.Contains("/@"))
                    {
                        result.AppendLine(link);
                        var match = locationPattern.Match(link);
                        if (match.Success)
                        {
                            coordinates.AppendLine($"{match.Groups[1].Value}, {match.Groups[2].Value}");
                        }
                    }
                }

                if (result.Length > 0)
                {
                    File.WriteAllText(outputFilePath, result.ToString());
                    File.WriteAllText(coordinatesFilePath, coordinates.ToString());
                    MessageBox.Show("Location links and coordinates have been saved.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No location links found.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("No links found in the document.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}