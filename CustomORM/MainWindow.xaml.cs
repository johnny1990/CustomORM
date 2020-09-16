using CustomORM.DbConnection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CustomORM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnectionStringBuilder connbuilder = new SqlConnectionStringBuilder();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            txtResult.Clear();
            txtResult2.Clear();
            txtResult3.Clear();
            txtResInsert.Clear();

            try
            {
                SqlDb.MainConnectionString = connbuilder.ConnectionString;
                IDataReader reader = SqlDb.Get(new List<SqlParameter>(), txtSp.Text);

                txtResult3.Text = "List<SqlParameter> collection = new List<SqlParameter>();" + Environment.NewLine;


                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string field = reader.GetName(i);
                    string type = "string";
                    string conv = ".ToString();";
                    if (field.Contains("ID"))
                    {
                        type = "int?";
                        conv = ".ToInt();";
                    }
                    else if (field.Contains("Date"))
                    {
                        type = "DateTime?";
                        conv = ".ToDate();";
                    }
                    txtResult.Text += "public " + type + " " + field + " {get;set;}\r\n";

                    txtResult2.Text += "record." + field + "=reader[\"" + field + "\"]" + conv + Environment.NewLine;
                    if (!field.Contains("ID"))
                        txtResult3.Text += "collection.Add(new SqlParameter(\"@" + field + "\", obj." + field + "));" + Environment.NewLine;
                }
                txtResult2.Text += "result.Add(record);";
                txtResult3.Text += "SqlParameter par = new SqlParameter(\"@ID\", SqlDbType.BigInt, sizeof(long));" + Environment.NewLine;
                txtResult3.Text += "par.Direction = System.Data.ParameterDirection.Output; " + Environment.NewLine;
                txtResult3.Text += "collection.Add(par);" + Environment.NewLine;
                txtResult3.Text += "obj.ID = SQLDB.ExecuteInsert(collection, " + txtSp.Text + ").ToLong().Value;" + Environment.NewLine;
                txtResult3.Text += "return obj; ";
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message); 
            }
        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            connbuilder.DataSource = tb1.Text;
            connbuilder.InitialCatalog = tb4.Text;
            connbuilder.UserID = tb3.Text;
            connbuilder.Password = tb2.Text;

            try
            {
                SqlConnection conn = new SqlConnection(connbuilder.ConnectionString);
                conn.Open();
                MessageBox.Show("OK");
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn3_Click(object sender, RoutedEventArgs e)
        {
            byte[] bytes = File.ReadAllBytes(@"D:\Test\Test.xls");
            string s = "new List<byte>() {";
            foreach (byte b in bytes)
                s += b.ToString() + ",";
            s = s.Substring(0, s.Length - 1);
            s += "};";
            tb5.Text = s;
        }
    }
}
