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
    public partial class FormAlarm : Form
    {
        public FormAlarm()
        {
            InitializeComponent();

            devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
                $"{System.Reflection.MethodBase.GetCurrentMethod().Name}");
        }

        private void FormAlarm_Load(object sender, EventArgs e)
        {
            Alarm_Init_DatagridView(ref dataGridViewAlarm);
        }

        void Alarm_Init_DatagridView(ref DataGridView dgv)
        {
            dgv.ReadOnly = true;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // row 로 선택하기
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None; //row size 막기
            dgv.AllowUserToResizeRows = false; //row size 막기
            dgv.AllowUserToResizeColumns = false; //column size 막기
            dgv.MultiSelect = false;
            dgv.RowHeadersVisible = true;
            dgv.RowHeadersWidth = 15;
            dgv.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgv.AllowUserToAddRows = false;


            dgv.Rows.Clear();
            dgv.Columns.Clear();

            dgv.ColumnCount = 5;
            dgv.RowCount = 17;
            dgv.ColumnHeadersVisible = true;

            dgv.Columns[0].HeaderText = $"날짜";
            dgv.Columns[0].Width = 150;
            dgv.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dgv.Columns[1].HeaderText = $"시간";
            dgv.Columns[1].Width = 150;

            dgv.Columns[2].HeaderText = $"이름";
            dgv.Columns[2].Width = 300;

            dgv.Columns[3].HeaderText = $"내용";
            dgv.Columns[3].Width = 150;

            dgv.Columns[4].HeaderText = $"레벨";
            dgv.Columns[4].Width = 150;
            //
            //dgv.Columns[5].HeaderText = $"해제";
            //dgv.Columns[5].Width = 150;
            //
            //dgv.Columns[6].HeaderText = $"시간";
            //dgv.Columns[6].Width = 150;

            for (int ix = 0; ix < dgv.ColumnCount; ix++)
            {
                if (ix == 3)
                {   
                    dgv.Columns[ix].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgv.Columns[ix].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                dgv.Columns[ix].Resizable = DataGridViewTriState.False;
                dgv.Columns[ix].SortMode = DataGridViewColumnSortMode.NotSortable;

            }

            for (int ux = 0; ux < dgv.RowCount; ux++)
            {
                //dgv.Rows[ux].Cells[0].Value = treeNodes[ux].Text;
                if (ux % 2 == 0)
                {
                    //strNodeHeader = treeNodes[ux].Text;
                    dgv.Rows[ux].HeaderCell.Style.BackColor = Color.DarkGray;
                    dgv.Rows[ux].Cells[0].Style.BackColor = Color.White;
                }
                else
                {
                    dgv.Rows[ux].HeaderCell.Style.BackColor = Color.DarkGray;
                    dgv.Rows[ux].Cells[0].Style.BackColor = Color.AliceBlue;
                }

                dgv.Rows[ux].Height = 50;

                int fontSize = 11;
                for (int col = 0; col < dgv.Columns.Count; col++)
                {
                    dgv.Rows[ux].Cells[col].Style.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
                }
            }

         
            dgv.ClearSelection();

        }
        
        public void Update_Alarm(string name, string desc, int level)
        {
            DataGridView dgv = dataGridViewAlarm;

            List<List<string>> alarmBuff = new List<List<string>>();
            for (int row = 0; row < dgv.RowCount -1; row++)
            {
                List<string> colBuff = new List<string>();
                for (int col = 0; col < dgv.ColumnCount; col++)
                {
                    if (dgv.Rows[row].Cells[col].Value == null)
                    {
                        colBuff.Add(null);
                    }
                    else
                    {
                        colBuff.Add(dgv.Rows[row].Cells[col].Value.ToString());
                    }

                }
                alarmBuff.Add(colBuff);
            }

            List<string> colData = new List<string>();
            colData.Add(DateTime.Now.ToString("yyyy-MM-dd"));
            colData.Add(DateTime.Now.ToString("HH:mm:ss.fff"));
            colData.Add(name);
            colData.Add(desc);
            switch (level)
            {
                case 1:
                    colData.Add("Warning");                    
                    break;
                case 2:
                    colData.Add("Error");                    
                    break;

                case 3:
                    colData.Add("Fatal");                    
                    break;

                default:
                    colData.Add("Unkown");
                    break;
            }
            alarmBuff.Insert(0, colData);
            //Console.WriteLine();
            for (int row = 0; row < dgv.RowCount; row++)
            {
                
                for (int col = 0; col < dgv.ColumnCount; col++)
                {
                    dgv.Rows[row].Cells[col].Value = alarmBuff[row][col];
                }
                
            }
        }
             
             
    }
}
