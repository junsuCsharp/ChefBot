using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Forms
{
    public partial class FormLoading : Form
    {
        public FormLoading()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;

            label1.Text = null;
            this.label1.BackColor = Color.White;
            this.TopMost = true;
        }

        public FormLoading(bool IsChild)
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;
            if (IsChild)
            {
                this.StartPosition = FormStartPosition.CenterParent;
            }
            label1.Text = null;
            this.label1.BackColor = Color.White;
            this.TopMost = true;
        }

        public void updateTime(string msg)
        {
            label1.Text = msg;
        }

        //public void 
    }
}
