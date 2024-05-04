using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WebsiteInfoApp
{
    public partial class MainWindow : Window
    {
        private readonly System.Net.Http.HttpClient httpClient;

        public MainWindow()
        {
            InitializeComponent();
            httpClient = new System.Net.Http.HttpClient();
        }

        private async void GetWebsiteInfoButton_Click(object sender, RoutedEventArgs e)
        {
            string url = WebsiteUrlTextBox.Text.Trim();
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string html = await response.Content.ReadAsStringAsync();
                string title = GetTitle(html);
                int paragraphCount = CountParagraphTags(html);
                string firstParagraph = GetFirstParagraphText(html);

                InfoTextBox.Text = $"Title: {title}\nParagraph Tags Count: {paragraphCount}\nFirst Paragraph Text: {firstParagraph}";
            }
            catch (HttpRequestException ex)
            {
                InfoTextBox.Text = $"Error: {ex.Message}";
            }
        }

        private string GetTitle(string html)
        {
           
            int startIndex = html.IndexOf("<title>", StringComparison.OrdinalIgnoreCase);
            int endIndex = html.IndexOf("</title>", StringComparison.OrdinalIgnoreCase);
            if (startIndex >= 0 && endIndex >= 0)
            {
                startIndex += "<title>".Length;
                return html.Substring(startIndex, endIndex - startIndex);
            }
            return "N/A";
        }

        private int CountParagraphTags(string html)
        {
            
            int count = 0;
            int index = 0;
            while ((index = html.IndexOf("<p>", index, StringComparison.OrdinalIgnoreCase)) != -1)
            {
                count++;
                index += "<p>".Length;
            }
            return count;
        }

        private string GetFirstParagraphText(string html)
        {
           
            int startIndex = html.IndexOf("<p>", StringComparison.OrdinalIgnoreCase);
            int endIndex = html.IndexOf("</p>", StringComparison.OrdinalIgnoreCase);
            if (startIndex >= 0 && endIndex >= 0)
            {
                startIndex += "<p>".Length;
                return StripTags(html.Substring(startIndex, endIndex - startIndex));
            }
            return "N/A";
        }

        private string StripTags(string input)
        {
           
            return System.Text.RegularExpressions.Regex.Replace(input, "<.*?>", String.Empty);
        }
    }
}
