using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Xml;
using System.Xml.Linq;

using System.Windows.Forms;
using System.Management;
using System.Collections;
using Cores;

namespace devJace.Files
{
    public static class Xml
    {
        #region Xml_File
        public static void Save(string path, Xml_File dat)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(path))
                {
                    System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(Xml_File));
                    xs.Serialize(sw, dat);
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine($"{exc.Message}");
            }

        }

        public static bool Load(string path, ref Xml_File dat)
        {
            bool rtn = false;
            try
            {
                if (File.Exists(path))
                {

                    using (StreamReader sr = new StreamReader(path))
                    {
                        System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(Xml_File));
                        dat = (Xml_File)xs.Deserialize(sr);
                        rtn = true;
                    }
                }

            }
            catch (Exception exc)
            {
                Console.WriteLine($"{exc.Message}");
            }
            return rtn;
        }
        #endregion

        #region DIO_File
        public static void Save(string path, DIO_File dat)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(path))
                {
                    System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(DIO_File));
                    xs.Serialize(sw, dat);
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine($"{exc.Message}");
            }

        }

        public static bool Load(string path, ref DIO_File dat)
        {
            bool rtn = false;
            try
            {
                if (File.Exists(path))
                {

                    using (StreamReader sr = new StreamReader(path))
                    {
                        System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(DIO_File));
                        dat = (DIO_File)xs.Deserialize(sr);
                        rtn = true;
                    }
                }

            }
            catch (Exception exc)
            {
                Console.WriteLine($"{exc.Message}");
            }
            return rtn;
        }
        #endregion

        #region Obj_File
        public static void Save(string path, Obj_File dat)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(path))
                {
                    System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(Obj_File));
                    xs.Serialize(sw, dat);
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine($"{exc.Message}");
            }

        }

        public static bool Load(string path, ref Obj_File dat)
        {
            bool rtn = false;
            try
            {
                if (File.Exists(path))
                {

                    using (StreamReader sr = new StreamReader(path))
                    {
                        System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(Obj_File));
                        dat = (Obj_File)xs.Deserialize(sr);
                        rtn = true;
                    }
                }

            }
            catch (Exception exc)
            {
                Console.WriteLine($"{exc.Message}");
            }
            return rtn;
        }
        #endregion

        #region Pos_File
        public static void Save(string path, Pos_File dat)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(path))
                {
                    System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(Pos_File));
                    xs.Serialize(sw, dat);
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine($"{exc.Message}");
            }

        }

        public static bool Load(string path, ref Pos_File dat)
        {
            bool rtn = false;
            try
            {
                if (File.Exists(path))
                {

                    using (StreamReader sr = new StreamReader(path))
                    {
                        System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(Pos_File));
                        dat = (Pos_File)xs.Deserialize(sr);
                        rtn = true;
                    }
                }

            }
            catch (Exception exc)
            {
                Console.WriteLine($"{exc.Message}");
            }
            return rtn;
        }


        #endregion

        #region 두산 로봇 위치 레시피
        public static void Save(string path, Cobot_File dat)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(path))
                {
                    System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(Cobot_File));
                    xs.Serialize(sw, dat);
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine($"{exc.Message}");
            }

        }

        public static bool Load(string path, ref Cobot_File dat)
        {
            bool rtn = false;
            try
            {
                if (File.Exists(path))
                {

                    using (StreamReader sr = new StreamReader(path))
                    {
                        System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(Cobot_File));
                        dat = (Cobot_File)xs.Deserialize(sr);
                        rtn = true;
                    }
                }

            }
            catch (Exception exc)
            {
                Console.WriteLine($"{exc.Message}");
            }
            return rtn;
        }
        #endregion

        public static void Save(string path, List<exChiken> dat)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(path))
                {
                    System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(List<exChiken>));
                    xs.Serialize(sw, dat);
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine($"{exc.Message}");
            }

        }

        public static bool Load(string path, ref List<exChiken> dat)
        {
            bool rtn = false;
            try
            {
                if (File.Exists(path))
                {

                    using (StreamReader sr = new StreamReader(path))
                    {
                        System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(List<exChiken>));
                        dat = (List<exChiken>)xs.Deserialize(sr);
                        rtn = true;
                    }
                }

            }
            catch (Exception exc)
            {
                Console.WriteLine($"{exc.Message}");
            }
            return rtn;
        }

        public static void Save(string path, List<Core_Data.MainCounter> dat)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(path))
                {
                    System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(List<Core_Data.MainCounter>));
                    xs.Serialize(sw, dat);
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine($"{exc.Message}");
            }

        }

        public static bool Load(string path, ref List<Core_Data.MainCounter> dat)
        {
            bool rtn = false;
            try
            {
                if (File.Exists(path))
                {

                    using (StreamReader sr = new StreamReader(path))
                    {
                        System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(List<Core_Data.MainCounter>));
                        dat = (List<Core_Data.MainCounter>)xs.Deserialize(sr);
                        rtn = true;
                    }
                }

            }
            catch (Exception exc)
            {
                Console.WriteLine($"{exc.Message}");
            }
            return rtn;
        }

        public static void Save(string path, List<Etc_File> dat)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(path))
                {
                    System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(List<Etc_File>));
                    xs.Serialize(sw, dat);
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine($"{exc.Message}");
            }

        }

        public static bool Load(string path, ref List<Etc_File> dat)
        {
            bool rtn = false;
            try
            {
                if (File.Exists(path))
                {

                    using (StreamReader sr = new StreamReader(path))
                    {
                        System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(List<Etc_File>));
                        dat = (List<Etc_File>)xs.Deserialize(sr);
                        rtn = true;
                    }
                }

            }
            catch (Exception exc)
            {
                Console.WriteLine($"{exc.Message}");
            }
            return rtn;
        }

        public static void Save(string path, Gui_File dat)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(path))
                {
                    System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(Gui_File));
                    xs.Serialize(sw, dat);
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine($"{exc.Message}");
            }

        }

        public static bool Load(string path, ref Gui_File dat)
        {
            bool rtn = false;
            try
            {
                if (File.Exists(path))
                {

                    using (StreamReader sr = new StreamReader(path))
                    {
                        System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(Gui_File));
                        dat = (Gui_File)xs.Deserialize(sr);
                        rtn = true;
                    }
                }

            }
            catch (Exception exc)
            {
                Console.WriteLine($"{exc.Message}");
            }
            return rtn;
        }


    }
}
