using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ManagerStoreBuilding
{
    public partial class FormDT : Form
    {
        public FormDT()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-7RAMVV0\\SQLEXPRESS; Initial Catalog = QLVatTu; Integrated Security = SSPI;");
            SqlCommand cmd = new SqlCommand("SELECT dbo.HoaDon.*, dbo.KhachHang.TenKH " +
                " from dbo.HoaDon INNER JOIN dbo.KhachHang ON dbo.HoaDon.MaKH = dbo.KhachHang.MaKH" +
                " where NgayTao between @DayStart and @DayEnd", conn);
            cmd.Parameters.AddWithValue("@DayStart", dateTimePicker1.Value);
            cmd.Parameters.AddWithValue("@DayEnd", dateTimePicker2.Value);

            try
            {
                conn.Open();
                DataTable tb = new DataTable();
                tb.Load(cmd.ExecuteReader());
                dataGridView1.DataSource = tb;
                conn.Close();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
            finally
            {
                conn.Close();
            }
            SqlCommand cmd1 = new SqlCommand("select Sum(ThanhTien) as DoanhThu " +
                " from HoaDon" +
                " where NgayTao between @DayStart and @DayEnd", conn);
            cmd1.Parameters.AddWithValue("@DayStart", dateTimePicker1.Value);
            cmd1.Parameters.AddWithValue("@DayEnd", dateTimePicker2.Value);

            try
            {
                conn.Open();
                SqlDataReader dr = cmd1.ExecuteReader();
                if (dr.Read())
                {
                    int ThanhTien = (int)dr.GetInt32(dr.GetOrdinal("DoanhThu"));
                    textBox1.Text = ThanhTien.ToString();
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.AllowUserToAddRows = false;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                //gọi hàm ToExcel() với tham số là dtg và filename từ SaveFileDialog
                ToExcel(dataGridView1, saveFileDialog.FileName);
            }
        }

        private void ToExcel(DataGridView dataGridView1, string fileName)
        {
            //khai báo thư viện hỗ trợ Microsoft.Office.Interop.Excel
            Microsoft.Office.Interop.Excel.Application excel;
            Microsoft.Office.Interop.Excel.Workbook workbook;
            Microsoft.Office.Interop.Excel.Worksheet worksheet;
            try
            {
                //Tạo đối tượng COM.
                excel = new Microsoft.Office.Interop.Excel.Application();
                excel.Visible = false;
                excel.DisplayAlerts = false;
                //tạo mới một Workbooks bằng phương thức add()
                workbook = excel.Workbooks.Add(Type.Missing);
                worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets["Sheet1"];
                //đặt tên cho sheet
                worksheet.Name = "Quản lý vật tư";

                // export header trong DataGridView
                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    worksheet.Cells[1, i + 1] = dataGridView1.Columns[i].HeaderText;
                }
                // export nội dung trong DataGridView
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    for (int j = 0; j < dataGridView1.ColumnCount; j++)
                    {
                        worksheet.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                    }
                }
                // sử dụng phương thức SaveAs() để lưu workbook với filename
                workbook.SaveAs(fileName);
                //đóng workbook
                workbook.Close();
                excel.Quit();
                MessageBox.Show("Xuất dữ liệu ra Excel thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                workbook = null;
                worksheet = null;
            }
        }
    }
}
