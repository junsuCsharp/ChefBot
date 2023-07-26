using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using MetroFramework.Forms;

namespace Forms
{
    public partial class FormBase : Form
    {
        public FormBase()
        {
            InitializeComponent();
        }

        public void TEST()
        { 
            
        }

        List<Label> labelsOilHeaterNumber = new List<Label>();


        private void FormBase_Load(object sender, EventArgs e)
        {
            Console.WriteLine($"DEBUG ::: {System.Reflection.MethodBase.GetCurrentMethod().Name} | ");

            labelsOilHeaterNumber.Add(lbOil_Heater_Number0);
            labelsOilHeaterNumber.Add(lbOil_Heater_Number1);
            labelsOilHeaterNumber.Add(lbOil_Heater_Number2);
        }

        private void metroScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            //string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            //int iSelNumber = e.NewValue / 50;
            ////int iDisNumber = iSelNumber % labelsOilHeaterNumber.Count;
            ////Console.WriteLine($"DEBUG ::: {methodName} {e.OldValue} {e.NewValue} {iSelNumber} ");
            ////0 ~ 4 ::: 5
            ////0 : 0 1 2
            ////1 : 1 2 3
            ////2 : 2 3 4

            //switch (iSelNumber)
            //{
            //    case 0:
            //        labelsOilHeaterNumber[0].Text = $"#{1:00}";
            //        labelsOilHeaterNumber[1].Text = $"#{2:00}";
            //        labelsOilHeaterNumber[2].Text = $"#{3:00}";
            //        break;
                   
            //    case 1:
            //        labelsOilHeaterNumber[0].Text = $"#{2:00}";
            //        labelsOilHeaterNumber[1].Text = $"#{3:00}";
            //        labelsOilHeaterNumber[2].Text = $"#{4:00}";
            //        break;

            //    case 2:
            //        labelsOilHeaterNumber[0].Text = $"#{3:00}";
            //        labelsOilHeaterNumber[1].Text = $"#{4:00}";
            //        labelsOilHeaterNumber[2].Text = $"#{5:00}";
            //        break;
            //}  
        }

        public void Config_Changed()
        { 
            
        }
    }
}
