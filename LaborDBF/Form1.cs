using System;
using System.Data;
using System.Windows.Forms;
using System.Data.OleDb;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;


namespace LaborDBF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
            if ( Convert.ToInt32(DateTime.Now.ToString("dd")) < 5 )
            {
                monthTextBox.Text = DateTime.Now.AddDays(-10).ToString("MMMMMMMMMMM");
            }
            else {
                 monthTextBox.Text = DateTime.Now.ToString("MMMMMMMMMMM");     
            }
        }
                

        OpenFileDialog ofd = new OpenFileDialog();

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string dirFile = textBox1.Text;
                string cadena = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + dirFile + ";Extended Properties=dBASE IV;User ID=Admin;Password=;";
                
                OleDbConnection con = new OleDbConnection(cadena);

                con.Open();
                String sqlComand = "Select * FROM " + textBox2.Text;

                OleDbDataAdapter adapter = new OleDbDataAdapter(sqlComand, con);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                con.Close();

                dataGridView1.DataSource = ds.Tables[0];
            }
            catch (OleDbException exp)
            {
                MessageBox.Show("Error: " + exp.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                int lenghtDir = ofd.FileName.Length - ofd.SafeFileName.Length;
                textBox1.Text = ofd.FileName.Substring(0, lenghtDir); 
                textBox2.Text = ofd.SafeFileName;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {                
                string dirFile = textBox1.Text;                
                string cadena = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + dirFile + ";Extended Properties=dBASE IV;User ID=Admin;Password=;";
                
                OleDbConnection con = new OleDbConnection(cadena);
                con.Open();                 
                
                String sqlComand = "SELECT SHTAT.PCODE, SHTAT.FAM + ' ' + LEFT(SHTAT.IM, 1) + '.' + LEFT(SHTAT.OT, 1) + '.' AS Doctor, COUNT(MU.COD_U) as Summa " +
                    "FROM MU LEFT JOIN SHTAT on MU.cod_u = SHTAT.PCODE group by PCODE, FAM, IM, OT ";
                
                OleDbDataAdapter adapter = new OleDbDataAdapter(sqlComand, con);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                con.Close();

                dataGridView1.DataSource = ds.Tables[0];
            }
            catch (OleDbException exp)
            {
                MessageBox.Show("Error: " + exp.Message);
            }
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            string dirFile = textBox1.Text;
            string cadena = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + dirFile + ";Extended Properties=dBASE IV;User ID=Admin;Password=;";

            OleDbConnection con = new OleDbConnection(cadena);
            con.Open();

            String sqlComand = "SELECT SHTAT.PCODE, SHTAT.FAM + ' ' + LEFT(SHTAT.IM, 1) + '.' + LEFT(SHTAT.OT, 1) + '.' AS Doctor, COUNT(MU.COD_U2) as Summa " +
                "FROM MU LEFT JOIN SHTAT on MU.cod_u2 = SHTAT.PCODE group by PCODE, FAM, IM, OT ";
            
            OleDbDataAdapter adapter = new OleDbDataAdapter(sqlComand, con);
            DataSet ds = new DataSet();
            adapter.Fill(ds);

            con.Close();

            dataGridView1.DataSource = ds.Tables[0];
        }

        private void exel_Click(object sender, EventArgs e)
        {            
            Excel.Application exApp = new Excel.Application();            
            exApp.Workbooks.Add();

            (exApp.Sheets[1]).Name = "Врачи";
            (exApp.Sheets[2]).Name = "Лаборанты";

            exApp.Columns.ColumnWidth = 22;
            exApp.Visible = true;

            Worksheet workSheet = (Worksheet)exApp.ActiveSheet;
            workSheet.Cells[1, 2] = monthTextBox.Text;
            (workSheet.Cells[1, 2] as Excel.Range).Font.Size = 16;
            (workSheet.Cells[1, 2] as Excel.Range).Font.Bold = true;
            (workSheet.Cells[1, 2] as Excel.Range).Font.Name = "Arial";
            //(workSheet.Cells[1, 1] as Excel.Range).Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlDash;


            workSheet.Cells[2, 2] = "C 01 по 31";
            (workSheet.Cells[2, 2] as Excel.Range).Font.Size = 16;

            workSheet.Cells[3, 1] = "Код врача";
            workSheet.Cells[3, 2] = "ФИО";
            workSheet.Cells[3, 3] = "Кол-во";

            # region edit workSheet Size, Bold, Name
            (workSheet.Cells[3, 1] as Excel.Range).Font.Size = 14;
            (workSheet.Cells[3, 2] as Excel.Range).Font.Size = 14;
            (workSheet.Cells[3, 3] as Excel.Range).Font.Size = 14;
            (workSheet.Cells[3, 1] as Excel.Range).Font.Bold = true;
            (workSheet.Cells[3, 2] as Excel.Range).Font.Bold = true;
            (workSheet.Cells[3, 3] as Excel.Range).Font.Bold = true;
            (workSheet.Cells[3, 1] as Excel.Range).Font.Name = "Arial";
            (workSheet.Cells[3, 2] as Excel.Range).Font.Name = "Arial";
            (workSheet.Cells[3, 3] as Excel.Range).Font.Name = "Arial";
            # endregion

            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                for (int j = 0; j < dataGridView1.RowCount; j++)
                {
                    try
                    {
                        exApp.Cells[j + 4, i + 1] = (dataGridView1[i, j].Value).ToString();
                    }
                    catch {   
                    }
                }
            }            
        }

        private void monthTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonStrangerPolikl_Click(object sender, EventArgs e)
        {
            string dirFile = textBox1.Text;
            string cadena = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dirFile + ";Extended Properties=dBASE IV;User ID=Admin;Password=;";//CharacterSet=1251;";

            OleDbConnection con = new OleDbConnection(cadena);

            con.Open();

            string sqlComand = "Select mu.Patient, po.PATIENT, po.S_pol, po.SN_pol, mu.cod, mu.K_U, ap.organ  FROM ( Select * from MU left JOIN po on mu.PATIENT = po.PATIENT) as TA left JOIN ap on TA.ap_id = ap.ap_id where ap.organ='1' ";//   where ap.organ='1' and  mu.cod = '26002'      where mu.cod = '26002' and ap.organ='1'                          where mu.cod = '26002' and ap.organ='1'";
            //string sqlComand = "Select pp.patient,pp.fam,pp.im,pp.ot,  mu.cod, mu.K_U, ap.organ  FROM ( Select * from MU left JOIN pp on mu.PATIENT = pp.PATIENT) as TA left JOIN ap on TA.ap_id = ap.ap_id where ap.organ='1' ";//   where ap.organ='1' and  mu.cod = '26002'      where mu.cod = '26002' and ap.organ='1'                          where mu.cod = '26002' and ap.organ='1'";

            OleDbDataAdapter adapter = new OleDbDataAdapter(sqlComand, con);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            
            con.Close();

            dataGridView1.DataSource = ds.Tables[0];
        }
    }
}
