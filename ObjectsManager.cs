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
    public partial class ObjectsManager : Form
    {
        MySQL mySQL;
        public ObjectsManager(MySQL mySQL)
        {
            this.mySQL = mySQL;
            InitializeComponent();
            authData.ContextMenuStrip = contextMenuStrip1;
            objectsData.ContextMenuStrip = contextMenuStripObjects;
            stepSettingData.ContextMenuStrip = contextMenuStripStep;
            UpDateTable();
            UpDateTableObject();
            UpDateTableStep();
        }
        private void UpDateTable()
        {
            string SQLData = "SELECT auth.id, object.name, auth.step, auth.site, auth.login, auth.password, auth.status FROM auth, object WHERE auth.id_object = object.id";
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
        private void UpDateTableObject()
        {
            string SQLData = "SELECT * FROM object";
            DataTable datatable = mySQL.GetDataTableSQL(SQLData);
            datatable.Columns["id"].ColumnName = "ID";
            datatable.Columns["name"].ColumnName = "Имя";
            datatable.Columns["ip"].ColumnName = "IP";
            datatable.Columns["port"].ColumnName = "Port";
            datatable.Columns["status"].ColumnName = "Статус";
            objectsData.DataSource = datatable;
        }
        private void UpDateTableStep()
        {
            string SQLData = "SELECT * FROM step";
            DataTable datatable = mySQL.GetDataTableSQL(SQLData);
            datatable.Columns["step"].ColumnName = "Step";
            datatable.Columns["id_object"].ColumnName = "Object";
            datatable.Columns["user_agent"].ColumnName = "User agent";
            datatable.Columns["language"].ColumnName = "Language";
            stepSettingData.DataSource = datatable;
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
        private void создатьToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            ObjectForm objectForm = new ObjectForm(mySQL);
            objectForm.ShowDialog();
            UpDateTableObject();
        }
        private void изменитьToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            if (objectsData.SelectedRows.Count != 0)
            {
                ObjectForm objectForm = new ObjectForm(mySQL, objectsData.SelectedRows[0].Cells[0].Value.ToString());
                objectForm.ShowDialog();
                UpDateTableObject();
            }
        }
        private void удалитьToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            if (objectsData.SelectedRows.Count == 1)
            {
                mySQL.SendSQL("DELETE FROM object WHERE id = " + objectsData.SelectedRows[0].Cells[0].Value.ToString());
                UpDateTableObject();
            }
        }
        private void создатьToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            StepForm stepForm = new StepForm(mySQL);
            stepForm.ShowDialog();
            UpDateTableStep();
        }
        private void изменитьToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if(stepSettingData.SelectedRows.Count > 0)
            {
                StepForm stepForm = new StepForm(mySQL, stepSettingData.SelectedRows[0].Cells[0].Value.ToString());
                stepForm.ShowDialog();
                UpDateTableStep();
            }
        }
        private void удалитьToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if(stepSettingData.SelectedRows.Count == 1)
            {
                mySQL.SendSQL("DELETE FROM step WHERE id = " + objectsData.SelectedRows[0].Cells[0].Value.ToString());
                UpDateTableStep();
            }
        }
        private void Status_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(authData.SelectedRows.Count > 0)
            {
                ToolStripMenuItem? menuItem = sender as ToolStripMenuItem;
                if (menuItem != null) {
                    ChangeStatus("auth",authData.SelectedRows[0].Cells[0].Value.ToString(),menuItem.Text);
                }
                else
                {
                    MessageBox.Show("Error toolStrip");
                }
                UpDateTable();
            }
        }
        private void ChangeStatus(string bd, string id, string status)
        {
            mySQL.SendSQL("UPDATE " + bd +" SET status = '" + status + "' WHERE id = " + id);
        }
    }
}
