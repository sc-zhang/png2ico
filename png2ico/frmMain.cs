using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;


namespace png2ico
{
    public partial class frmMain : Form
    {
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }
        public frmMain()
        {
            InitializeComponent();
        }
        private void lblPicView_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        private void lblPicView_DragDrop(object sender, DragEventArgs e)
        {
            string imgPath = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            
            Image img = GetImage(imgPath);
            lblPicView.Image = resizeImage(img, new Size(lblPicView.Width, lblPicView.Height));
            if (img!=null)
            {
                try
                {
                    if (imgPath.EndsWith(".png"))
                    {
                        imgSize[] sizeList = {
                            new imgSize(16, 16),
                            new imgSize(32, 32),
                            new imgSize(48, 48),
                            new imgSize(64, 64),
                            new imgSize(96, 96),
                            new imgSize(128, 128),
                            new imgSize(256, 256)
                        };
                        //imgSize[] sizeList = { new imgSize(16, 16)};
                        Bitmap[] images = new Bitmap[sizeList.Length];
                        for(int i=0; i<sizeList.Length; i++)
                        {
                            images[i] = new Bitmap(resizeImage(img, new Size(sizeList[i].width, sizeList[i].height)));
                        }
                        string outImgPath = imgPath.Substring(0, imgPath.Length - 4) + ".ico";
                        using (var stream = new FileStream(outImgPath, FileMode.Create))
                        {
                            IconExporter.IconFactory.SavePngsToIcon(images, stream);
                        }
                        for(int i=0; i<sizeList.Length; i++)
                        {
                            images[i].Dispose();
                        }
                        images = null;
                        MessageBox.Show(string.Format("Image saved: {0}", outImgPath), "PNG2ICO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        string outImgPath = imgPath.Substring(0, imgPath.Length - 4) + ".png";
                        img.Save(outImgPath);
                        MessageBox.Show(string.Format("Image saved: {0}", outImgPath), "PNG2ICO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch(Exception except) {
                    MessageBox.Show(except.ToString(), "PNG2ICO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        public static Image GetImage(string imgPath)
        {
            if (!File.Exists(imgPath))
            {
                return null;
            }
            Bitmap bmp = null;
            if (imgPath.EndsWith(".png"))
            {
                FileStream fs = File.OpenRead(imgPath);
                int fileLength = 0;
                fileLength = (int)fs.Length;
                Byte[] image = new Byte[fileLength];
                fs.Read(image, 0, fileLength);
                Image result = Image.FromStream(fs);
                bmp = new Bitmap(result);
                fs.Close();
                result.Dispose();
            }
            else if (imgPath.EndsWith(".ico"))
            {
                Stream fs = File.OpenRead(imgPath);
                Icon result = new Icon(fs, new Size(256, 256));
                bmp = result.ToBitmap();
                result.Dispose();
                fs.Close();
            }
            else
            {
                MessageBox.Show("File format not support", "PNG2ICO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                bmp = null;
            }
            return bmp;
        }
        public static Image resizeImage(Image imgSrc, Size size)
        {
            return new Bitmap(imgSrc, size);
        }
        public class imgSize
        {
            public ushort width;
            public ushort height;
            public imgSize(ushort width, ushort height)
            {
                this.width = width;
                this.height = height;
            }
        }
    }
}

namespace IconExporter
{
    public class IconFactory
    {
        public const int maxWidth = 256;
        public const int maxHeigh = 256;

        private const ushort headerReserved = 0;
        private const ushort headerIconType = 1;
        private const byte headerLength = 6;
        private const byte entryReserved = 0;
        private const byte entryLength = 16;
        private const byte pngColorsInPalette = 0;
        private const ushort pngColorPlanes = 1;

        public static void SavePngsToIcon(Bitmap[] images, Stream stream)
        {
            if (images == null || stream == null)
            {
                MessageBox.Show("Error", "PNG2ICO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(IconFactory.headerReserved);
                writer.Write(IconFactory.headerIconType);
                writer.Write((ushort)images.Length);

                Dictionary<uint, byte[]> buffers = new Dictionary<uint, byte[]>();

                uint totalLenght = 0;
                uint baseOffset = (uint)(IconFactory.headerLength + IconFactory.entryLength * images.Length);
                for (int i=0; i< images.Length; i++)
                {
                    Bitmap img = images[i];
                    byte[] buffer = IconFactory.CreateImageBuffer(img);
                    uint offset = (baseOffset + totalLenght);

                    writer.Write((byte)(img.Width >= IconFactory.maxWidth ? 0 : img.Width));
                    writer.Write((byte)(img.Height >= IconFactory.maxHeigh ? 0 : img.Height));
                    writer.Write(IconFactory.pngColorsInPalette);
                    writer.Write(IconFactory.entryReserved);
                    writer.Write(IconFactory.pngColorPlanes);
                    writer.Write((ushort)Image.GetPixelFormatSize(img.PixelFormat));
                    writer.Write((uint)buffer.Length);
                    writer.Write(offset);

                    totalLenght += (uint)buffer.Length;

                    buffers.Add(offset, buffer);
                }
                foreach(var buffer in buffers)
                {
                    writer.BaseStream.Seek(buffer.Key, SeekOrigin.Begin);
                    writer.Write(buffer.Value);
                }
            }
        }
        private static byte[] CreateImageBuffer(Bitmap img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }
}
