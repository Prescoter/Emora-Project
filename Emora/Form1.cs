using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Emora
{
    public partial class Form1 : Form
    {
        private class WebsiteInfo
        {
            public string ErrorType { get; set; }
            public string ErrorMessage { get; set; }
            public string Url { get; set; }
        }

        private readonly Dictionary<string, WebsiteInfo> websites = new Dictionary<string, WebsiteInfo>
        {
            {"About.me", new WebsiteInfo {ErrorType = "status_code", Url = "https://about.me/{}"}},
            {"Chess", new WebsiteInfo {ErrorType = "status_code", Url = "https://www.chess.com/member/{}"}},
            {"DailyMotion", new WebsiteInfo {ErrorType = "status_code", Url = "https://www.dailymotion.com/{}"}},
            {"Docker Hub", new WebsiteInfo {ErrorType = "status_code", Url = "https://hub.docker.com/u/{}"}},
            {"Duolingo", new WebsiteInfo {ErrorType = "message", ErrorMessage = "Duolingo - Learn a language for free @duolingo", Url = "https://www.duolingo.com/profile/{}"}},
            {"Fiverr", new WebsiteInfo {ErrorType = "status_code", Url = "https://www.fiverr.com/{}"}},
            {"Flickr", new WebsiteInfo {ErrorType = "status_code", Url = "https://www.flickr.com/people/{}"}},
            {"GeeksforGeeks", new WebsiteInfo {ErrorType = "message", ErrorMessage = "Login GeeksforGeeks", Url = "https://auth.geeksforgeeks.org/user/{}"}},
            {"Genius (Artists)", new WebsiteInfo {ErrorType = "status_code", Url = "https://genius.com/artists/{}"}},
            {"Genius (Users)", new WebsiteInfo {ErrorType = "status_code", Url = "https://genius.com/{}"}},
            {"Giphy", new WebsiteInfo {ErrorType = "status_code", Url = "https://giphy.com/{}"}},
            {"GitHub", new WebsiteInfo {ErrorType = "status_code", Url = "https://www.github.com/{}"}},
            {"Imgur", new WebsiteInfo {ErrorType = "status_code", Url = "https://api.imgur.com/account/v1/accounts/{}?client_id=546c25a59c58ad7"}},
            {"Minecraft", new WebsiteInfo {ErrorType = "status_code", Url = "https://api.mojang.com/users/profiles/minecraft/{}"}},
            {"npm", new WebsiteInfo {ErrorType = "status_code", Url = "https://www.npmjs.com/~{}"}},
            {"Pastebin", new WebsiteInfo {ErrorType = "status_code", Url = "https://pastebin.com/u/{}"}},
            {"Patreon", new WebsiteInfo {ErrorType = "status_code", Url = "https://www.patreon.com/{}"}},
            {"PyPi", new WebsiteInfo {ErrorType = "status_code", Url = "https://pypi.org/user/{}"}},
            {"Reddit", new WebsiteInfo {ErrorType = "message", ErrorMessage = "\"error\": 404}", Url = "https://www.reddit.com/user/{}/about.json"}},
            {"Replit", new WebsiteInfo {ErrorType = "status_code", Url = "https://replit.com/@{}"}},
            {"Roblox", new WebsiteInfo {ErrorType = "status_code", Url = "https://www.roblox.com/user.aspx?username={}"}},
            {"RootMe", new WebsiteInfo {ErrorType = "status_code", Url = "https://www.root-me.org/{}"}},
            {"Scribd", new WebsiteInfo {ErrorType = "status_code", Url = "https://www.scribd.com/{}"}},
            {"Snapchat", new WebsiteInfo {ErrorType = "status_code", Url = "https://www.snapchat.com/add/{}"}},
            {"SoundCloud", new WebsiteInfo {ErrorType = "status_code", Url = "https://soundcloud.com/{}"}},
            {"SourceForge", new WebsiteInfo {ErrorType = "status_code", Url = "https://sourceforge.net/u/{}"}},
            {"Spotify", new WebsiteInfo {ErrorType = "status_code", Url = "https://open.spotify.com/user/{}"}},
            {"Steam", new WebsiteInfo {ErrorType = "message", ErrorMessage = "Steam Community :: Error", Url = "https://steamcommunity.com/id/{}"}},
            {"Telegram", new WebsiteInfo {ErrorType = "message", ErrorMessage = "<meta name=\"robots\" content=\"noindex, nofollow\">", Url = "https://t.me/{}"}},
            {"Tenor", new WebsiteInfo {ErrorType = "status_code", Url = "https://tenor.com/users/{}"}},
            {"TryHackMe", new WebsiteInfo {ErrorType = "message", ErrorMessage = "<title>TryHackMe</title>", Url = "https://tryhackme.com/p/{}"}},
            {"Vimeo", new WebsiteInfo {ErrorType = "status_code", Url = "https://vimeo.com/{}"}},
            {"Wattpad", new WebsiteInfo {ErrorType = "status_code", Url = "https://www.wattpad.com/user/{}"}},
            {"Wikipedia", new WebsiteInfo {ErrorType = "message", ErrorMessage = "(centralauth-admin-nonexistent:", Url = "https://en.wikipedia.org/wiki/Special:CentralAuth/{}?uselang=qqx"}},
            {"AllMyLinks", new WebsiteInfo {ErrorType = "status_code", Url = "https://allmylinks.com/{}"}},
            {"Buy Me a Coffee", new WebsiteInfo {ErrorType = "status_code", Url = "https://www.buymeacoffee.com/{}"}},
            {"BuzzFeed", new WebsiteInfo {ErrorType = "status_code", Url = "https://www.buzzfeed.com/{}"}},
            {"Cash APP", new WebsiteInfo {ErrorType = "status_code", Url = "https://cash.app/${}"}},
            {"Ebay", new WebsiteInfo {ErrorType = "message", Url = "https://www.ebay.com/usr/{}"}},
            {"Instagram", new WebsiteInfo {ErrorType = "status_code", Url = "https://www.picuki.com/profile/{}"}},
            {"JsFiddle", new WebsiteInfo {ErrorType = "status_code", Url = "https://jsfiddle.net/user/{}/"}},
            {"Linktree", new WebsiteInfo {ErrorType = "message", ErrorMessage = "\"statusCode\":404", Url = "https://linktr.ee/{}"}},
            {"Medium", new WebsiteInfo {ErrorType = "message", ErrorMessage = "<span class=\"fs\">404</span>", Url = "https://{}.medium.com/about"}},
            {"Pinterest", new WebsiteInfo {ErrorType = "message", ErrorMessage = "<title></title>", Url = "https://pinterest.com/{}/"}},
            {"Rapid API", new WebsiteInfo {ErrorType = "status_code", Url = "https://rapidapi.com/user/{}"}},
            {"TradingView", new WebsiteInfo {ErrorType = "status_code", Url = "https://www.tradingview.com/u/{}/"}},
        };
        private int Checked = 0;
        private int Results = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.GotFocus += RemoveText;
            textBox1.LostFocus += AddText;
            tableLayoutPanel1.HorizontalScroll.Maximum = 0;
            tableLayoutPanel1.AutoScroll = false;
            tableLayoutPanel1.VerticalScroll.Visible = false;
            tableLayoutPanel1.AutoScroll = true;
        }

        public void RemoveText(object sender, EventArgs e)
        {
            if (textBox1.Text == "Enter username here...")
                textBox1.Text = "";
        }

        public void AddText(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
                textBox1.Text = "Enter username here...";
        }

        private async void btn_search_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(username) || username == "Enter username here...")
            {
                MessageBox.Show("Please enter a username to check.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            btn_search.Enabled = false;
            textBox1.Enabled = false;
            label1.Text = $"Checking username {username}";
            this.Text = $"Emora | Checking username {username}";
            Checked = 0;
            Results = 0;
            tableLayoutPanel1.RowStyles.Clear();
            tableLayoutPanel1.Controls.Clear();
            Stopwatch stopwatch = Stopwatch.StartNew();

            using (var httpClient = new HttpClient())
            {
                httpClient.Timeout = TimeSpan.FromSeconds(8);
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:124.0) Gecko/20100101 Firefox/124.0");
                var tasks = new List<Task>();
                var semaphore = new SemaphoreSlim((int)numericUpDown1.Value);

                foreach (var website in websites)
                {
                    await semaphore.WaitAsync();
                    var task = Task.Run(async () =>
                    {
                        try
                        {
                            await CheckWebsite(username, website.Key, website.Value, httpClient);
                        }
                        finally
                        {
                            Checked++;
                            Invoke(new Action(() =>
                            {
                                this.Text = $"Emora | Checking username {username} [{Results} accounts found] [{Checked}/{websites.Count} websites checked]";
                            }));
                            semaphore.Release();
                        }
                    });
                    tasks.Add(task);
                }
                await Task.WhenAll(tasks);
                Text = $"Emora | {Results} accounts found for {username}";
                stopwatch.Stop();
                label1.Text = $"{Results} accounts found for {username} in {stopwatch.ElapsedMilliseconds / 1000.0} seconds";
                btn_search.Enabled = true;
                textBox1.Enabled = true;
            }
        }

        private async Task CheckWebsite(string username, string websiteName, WebsiteInfo websiteInfo, HttpClient httpClient)
        {
            try
            {
                var url = websiteInfo.Url.Replace("{}", username);
                var response = await httpClient.GetAsync(url);
                if (websiteInfo.ErrorType == "status_code")
                {
                    if (response.StatusCode >= HttpStatusCode.OK && response.StatusCode < HttpStatusCode.Redirect)
                    {
                        AddResult(websiteName, url);
                    }
                }
                else if (websiteInfo.ErrorType == "message")
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (!content.Contains(websiteInfo.ErrorMessage))
                    {
                        AddResult(websiteName, url);
                    }
                }
            }
            catch (Exception){}
        }

        private void AddResult(string websiteName, string url)
        {
            Results++;
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                Height = 80,
                BackColor = Color.FromArgb(66, 66, 66),
                BorderStyle = BorderStyle.None,
                TabIndex = tableLayoutPanel1.Controls.Count
            };

            PictureBox pictureBox = new PictureBox
            {
                Image = imageList1.Images[imageList1.Images.IndexOfKey(websiteName)],
                Size = new Size(64, 64),
                SizeMode = PictureBoxSizeMode.Zoom
            };
            pictureBox.Location = new Point((panel.Width - pictureBox.Width) / 8, (panel.Height - pictureBox.Height) / 2);

            var label = new Label
            {
                Text = websiteName,
                AutoSize = true
            };
            label.Location = new Point(pictureBox.Right + 10, (panel.Height - label.Height) / 3);
            label.Font = new Font(this.Font.FontFamily, 18);

            panel.Click += (sender, e) => Process.Start(url);
            panel.Cursor = Cursors.Hand;
            pictureBox.Click += (sender, e) => Process.Start(new Uri(url).GetLeftPart(UriPartial.Authority));
            pictureBox.Cursor = Cursors.Hand;
            label.Click += (sender, e) => Process.Start(new Uri(url).GetLeftPart(UriPartial.Authority));
            label.Cursor = Cursors.Hand;

            ToolTip toolTip = new ToolTip
            {
                AutoPopDelay = 5000,
                InitialDelay = 800,
                ReshowDelay = 500,
                ShowAlways = true
            };
            toolTip.SetToolTip(label, "Visit Website");
            toolTip.SetToolTip(pictureBox, "Visit Website");
            toolTip.SetToolTip(panel, "View User Profile");

            panel.Controls.Add(label);
            panel.Controls.Add(pictureBox);

            Invoke(new Action(() =>
            {
                if (tableLayoutPanel1.Controls.Count > 0)
                {
                    tableLayoutPanel1.Controls.Remove(tableLayoutPanel1.Controls[tableLayoutPanel1.Controls.Count - 1]);
                }

                tableLayoutPanel1.Controls.Add(panel);
                label1.Text = $"{Results} accounts found";
                tableLayoutPanel1.Controls.Add(new Label());
            }));
        }
    }
}
