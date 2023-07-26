using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Forms;
using System.Drawing.Text;
using System.Drawing;

namespace Fonts
{
    public class FontLibrary
    {
        private static FontLibrary inst = new FontLibrary();
        public PrivateFontCollection privateFont = new PrivateFontCollection();
        public static FontFamily[] Families
        {
            get
            {
                return inst.privateFont.Families;
            }
        }

        public FontLibrary()
        {
            AddFontFromMemory();
        }

        private void AddFontFromMemory()
        {
            List<byte[]> fonts = new List<byte[]>();

            privateFont.AddFontFile(Directory.GetCurrentDirectory() + @"\Fonts\NotoSans-Black.ttf");
            privateFont.AddFontFile(Directory.GetCurrentDirectory() + @"\Fonts\NotoSans-BlackItalic.ttf");
            privateFont.AddFontFile(Directory.GetCurrentDirectory() + @"\Fonts\NotoSans-Bold.ttf");
            privateFont.AddFontFile(Directory.GetCurrentDirectory() + @"\Fonts\NotoSans-BoldItalic.ttf");
            privateFont.AddFontFile(Directory.GetCurrentDirectory() + @"\Fonts\NotoSans-ExtraBold.ttf");
            privateFont.AddFontFile(Directory.GetCurrentDirectory() + @"\Fonts\NotoSans-ExtraBoldItalic.ttf");
            privateFont.AddFontFile(Directory.GetCurrentDirectory() + @"\Fonts\NotoSans-ExtraLight.ttf");
            privateFont.AddFontFile(Directory.GetCurrentDirectory() + @"\Fonts\NotoSans-ExtraLightItalic.ttf");
            privateFont.AddFontFile(Directory.GetCurrentDirectory() + @"\Fonts\NotoSans-Italic.ttf");
            privateFont.AddFontFile(Directory.GetCurrentDirectory() + @"\Fonts\NotoSans-Light.ttf");
            privateFont.AddFontFile(Directory.GetCurrentDirectory() + @"\Fonts\NotoSans-LightItalic.ttf");
            privateFont.AddFontFile(Directory.GetCurrentDirectory() + @"\Fonts\NotoSans-Medium.ttf");
            privateFont.AddFontFile(Directory.GetCurrentDirectory() + @"\Fonts\NotoSans-MediumItalic.ttf");
            privateFont.AddFontFile(Directory.GetCurrentDirectory() + @"\Fonts\NotoSans-Regular.ttf");
            privateFont.AddFontFile(Directory.GetCurrentDirectory() + @"\Fonts\NotoSans-SemiBold.ttf");
            privateFont.AddFontFile(Directory.GetCurrentDirectory() + @"\Fonts\NotoSans-SemiBoldItalic.ttf");
            privateFont.AddFontFile(Directory.GetCurrentDirectory() + @"\Fonts\NotoSans-Thin.ttf");
            privateFont.AddFontFile(Directory.GetCurrentDirectory() + @"\Fonts\NotoSans-ThinItalic.ttf");

            foreach (byte[] font in fonts)
            {
                IntPtr fontBuffer = Marshal.AllocCoTaskMem(font.Length);
                Marshal.Copy(font, 0, fontBuffer, font.Length);
                privateFont.AddMemoryFont(fontBuffer, font.Length);
            }
        }

    }
}
