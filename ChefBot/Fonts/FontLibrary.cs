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
        public enum ENotoSans
        { 
            Normal, Black, ExtraBold, ExtraLight, Light, Medium, SemiBold, Thin
        }


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
            try
            {
                List<byte[]> fonts = new List<byte[]>();

                string path = Directory.GetCurrentDirectory();

                //privateFont.AddFontFile(Directory.GetCurrentDirectory() + @"\Fonts\NotoMono-Regular.ttf");              //00

                privateFont.AddFontFile(Directory.GetCurrentDirectory() + @"\Fonts\NotoSans-Black.ttf");                //00
                privateFont.AddFontFile(Directory.GetCurrentDirectory() + @"\Fonts\NotoSans-BlackItalic.ttf");          //01
                privateFont.AddFontFile(Directory.GetCurrentDirectory() + @"\Fonts\NotoSans-Bold.ttf");                 //02
                privateFont.AddFontFile(Directory.GetCurrentDirectory() + @"\Fonts\NotoSans-BoldItalic.ttf");           //03
                privateFont.AddFontFile(Directory.GetCurrentDirectory() + @"\Fonts\NotoSans-ExtraBold.ttf");            //04
                privateFont.AddFontFile(Directory.GetCurrentDirectory() + @"\Fonts\NotoSans-ExtraBoldItalic.ttf");      //05
                privateFont.AddFontFile(Directory.GetCurrentDirectory() + @"\Fonts\NotoSans-ExtraLight.ttf");           //06
                privateFont.AddFontFile(Directory.GetCurrentDirectory() + @"\Fonts\NotoSans-ExtraLightItalic.ttf");     //07
                privateFont.AddFontFile(Directory.GetCurrentDirectory() + @"\Fonts\NotoSans-Italic.ttf");               //08
                privateFont.AddFontFile(Directory.GetCurrentDirectory() + @"\Fonts\NotoSans-Light.ttf");                //09
                privateFont.AddFontFile(Directory.GetCurrentDirectory() + @"\Fonts\NotoSans-LightItalic.ttf");          //10
                privateFont.AddFontFile(Directory.GetCurrentDirectory() + @"\Fonts\NotoSans-Medium.ttf");               //11
                privateFont.AddFontFile(Directory.GetCurrentDirectory() + @"\Fonts\NotoSans-MediumItalic.ttf");         //12
                privateFont.AddFontFile(Directory.GetCurrentDirectory() + @"\Fonts\NotoSans-Regular.ttf");              //13
                privateFont.AddFontFile(Directory.GetCurrentDirectory() + @"\Fonts\NotoSans-SemiBold.ttf");             //14
                privateFont.AddFontFile(Directory.GetCurrentDirectory() + @"\Fonts\NotoSans-SemiBoldItalic.ttf");       //15
                privateFont.AddFontFile(Directory.GetCurrentDirectory() + @"\Fonts\NotoSans-Thin.ttf");                 //16
                privateFont.AddFontFile(Directory.GetCurrentDirectory() + @"\Fonts\NotoSans-ThinItalic.ttf");           //17

                foreach (byte[] font in fonts)
                {
                    IntPtr fontBuffer = Marshal.AllocCoTaskMem(font.Length);
                    Marshal.Copy(font, 0, fontBuffer, font.Length);
                    privateFont.AddMemoryFont(fontBuffer, font.Length);
                }
            }
            catch
            { 
                
            }           
        }

    }
}
