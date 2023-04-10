using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TelegramBot
{
    public partial class AuthForm : Form
    {
        MySQL mySQL;
        public AuthForm(MySQL mySQL)
        {
            this.mySQL = mySQL;
            InitializeComponent();
            authData.ContextMenuStrip = contextMenuStrip1;
            UpDateTable();
        }
        private void UpDateTable()
        {
            string SQLData = "SELECT auth.id, object.name, auth.step, auth.login, auth.password, auth.site, auth.status FROM auth, object WHERE auth.id_object = object.id";
            DataTable datatable = mySQL.GetDataTableSQL(SQLData);
            datatable.Columns["id"].ColumnName = "ID";
            datatable.Columns["name"].ColumnName = "Obj name";
            datatable.Columns["step"].ColumnName = "Step";
            datatable.Columns["login"].ColumnName = "Логин";
            datatable.Columns["password"].ColumnName = "Пароль";
            datatable.Columns["site"].ColumnName = "Сайт";
            datatable.Columns["status"].ColumnName = "Статус";
            authData.DataSource = datatable;
        }
        private void создатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AuthFormManager authFormManager = new AuthFormManager(mySQL);
            authFormManager.ShowDialog();
            UpDateTable();
        }

        private void изменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(authData.SelectedRows.Count == 1)
            {
                AuthFormManager authFormManager = new AuthFormManager(mySQL,authData.SelectedRows[0].Cells[0].Value.ToString());
                authFormManager.ShowDialog();
                UpDateTable();
            }
            else
            {
                MessageBox.Show("Выберите строку.");
            }
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (authData.SelectedRows.Count == 1)
            {
                mySQL.SendSQL("DELETE FROM admin WHERE id = " + authData.SelectedRows[0].Cells[0].Value.ToString());
                UpDateTable();
            }
            else
            {
                MessageBox.Show("Выберите строку.");
            }
        }
    }
}
