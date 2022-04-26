using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI.Model
{
    //public partial class Form1 : Form
    //{
    //    private void Form1_Load(object sender, EventArgs e)
    //    {
    //        //Определяем подключение
    //        OleDbConnection StrCon = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\sozon\Desktop\Binnance\BinanceAPI\BinanceAPI\Model;Extended Properties=text");
    //        //Строка для выборки данных
    //        string Select1 = "SELECT * FROM [Dictionary.txt]";
    //        //Создание объекта Command
    //        OleDbCommand comand1 = new OleDbCommand(Select1, StrCon);
    //        //Определяем объект Adapter для взаимодействия с источником данных
    //        OleDbDataAdapter adapter1 = new OleDbDataAdapter(comand1);
    //        //Определяем объект DataSet
    //        DataSet AllTables = new DataSet();
    //        //Открываем подключение
    //        StrCon.Open();
    //        //Заполняем DataSet таблицей из источника данных
    //        adapter1.Fill(AllTables);
    //        //Заполняем обект datagridview для отображения данных на форме
    //        StrCon.Close();
    //    }
    //    private void button1_Click(object sender, EventArgs e)
    //    {
    //        try
    //        {
    //            StreamWriter sw = new StreamWriter(@"D:\Exp\Dictionary1.txt", false, Encoding.Default);
    //            //Добавление имен столбцов
    //            for (int j = 0; j < dataGridView1.ColumnCount; j++)
    //            {
    //                sw.Write(dataGridView1.Columns[j].HeaderText);
    //                if (j < dataGridView1.ColumnCount - 1)
    //                    sw.Write(",");
    //            }
    //            sw.WriteLine();
    //            for (int i = 0; i < dataGridView1.RowCount; i++)
    //            {
    //                for (int j = 0; j < dataGridView1.ColumnCount; j++)
    //                {
    //                    sw.Write(dataGridView1.Rows[i].Cells[j].Value);
    //                    if (j < dataGridView1.ColumnCount - 1)
    //                        sw.Write(",");
    //                }
    //                sw.WriteLine();
    //            }
    //            sw.Flush();
    //            sw.Close();
    //        }
    //        catch (Exception ex)
    //        {
    //            MessageBox.Show(ex.Message);
    //        }
    //    }
    //}
}
