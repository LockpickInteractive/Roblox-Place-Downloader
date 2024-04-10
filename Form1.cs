using System.IO.Compression;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace Roblox_Place_Downloader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string target = "https://github.com/qfoxb";
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo { FileName = target, UseShellExecute = true });

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            string target = "https://github.com/LockpickInteractive";
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo { FileName = target, UseShellExecute = true });
        }

        private void button1_Click(object sender, EventArgs e) // Download Single Place Version
        {
            long placeId = long.Parse(textBox1.Text);
            long versionId = long.Parse(textBox2.Text);
            string url = "https://assetdelivery.roblox.com/v1/asset/?id=" + placeId + "&versionId=" + versionId;

            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.AcceptEncoding] = "gzip";
                byte[] compressedBytes = client.DownloadData(url);

                using (GZipStream decompressionStream = new GZipStream(new MemoryStream(compressedBytes), CompressionMode.Decompress))
                {
                    const int blockSize = 1024;
                    byte[] buffer = new byte[blockSize];
                    using (MemoryStream memory = new MemoryStream())
                    {
                        int count = 0;
                        do
                        {
                            count = decompressionStream.Read(buffer, 0, blockSize);
                            if (count > 0)
                            {
                                memory.Write(buffer, 0, count);
                            }
                        }
                        while (count > 0);

                        byte[] decompressedBytes = memory.ToArray();

                        SaveFileDialog saveSingleFile = new SaveFileDialog();
                        saveSingleFile.Filter = "Roblox Place Files (*.rbxl)|*.rbxl";
                        saveSingleFile.Title = "Save Roblox Place File";

                        if (saveSingleFile.ShowDialog() == DialogResult.OK)
                        {
                            string filePath = saveSingleFile.FileName;
                            File.WriteAllBytes(filePath, decompressedBytes);
                        }
                    }
                }
            }


        }
    }
}
