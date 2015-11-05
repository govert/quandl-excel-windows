﻿using System;
using System.Collections;
using System.Windows.Forms;

using Quandl.Shared;
using Newtonsoft.Json.Linq;

namespace Quandl.Excel.Addin
{
    using System.Linq;
    using Excel = Microsoft.Office.Interop.Excel;

    public partial class DataTaskPane : UserControl
    {
        private String databaseCode;
        private Excel.Range activeCells;

        public DataTaskPane(Excel.Range activeCells)
        {
            InitializeComponent();

            this.activeCells = activeCells;

            this.listBox1.DisplayMember = "Name";
            this.listBox1.ValueMember = "Value";
            this.listBox1.DoubleClick += ListBox1_DoubleClick;

            this.listBox2.DisplayMember = "Name";
            this.listBox2.ValueMember = "Value";
            this.listBox2.DoubleClick += ListBox2_DoubleClick;

            this.listBox3.DisplayMember = "Name";
            this.listBox3.ValueMember = "Value";

            if (this.activeCells == null)
            {
                this.textBox3.Text = "You have not selected a cell to populate data in.";
                this.button2.Enabled = false;
                this.button1.Enabled = false;
            }
            else
            {
                this.textBox3.Text = "";
                this.button2.Enabled = true;
                this.button1.Enabled = true;
            }
        }

        private void ListBox2_DoubleClick(object sender, EventArgs e)
        {
            ListBox box = (ListBox)sender;
            dynamic item = box.SelectedItem;
            if (this.listBox2.Items.Contains(item))
            {
                this.listBox2.Items.Remove(item);
                AvailableColumns_Recalculate();
            }
        }

        private void ListBox1_DoubleClick(object sender, EventArgs e)
        {
            ListBox box = (ListBox)sender;
            dynamic item = box.SelectedItem;
            if (!this.listBox2.Items.Contains(item))
            {
                this.listBox2.Items.Add(item);
                AvailableColumns_Recalculate();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            this.databaseCode = ((TextBox)sender).Text.ToUpper();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.listBox1.Items.Clear();
            this.button2.Enabled = false;

            String query = this.textBox1.Text;
            JObject data = TestFunctions.SearchDatasets(this.databaseCode, (string)query);
            foreach (JObject dataset in data["datasets"])
            {
                var columnNames = dataset["column_names"].ToObject<ArrayList>().ToArray().Select(x => ((string)x).ToUpper()).ToArray();
                this.listBox1.Items.Add(new { Value = this.databaseCode + '/' + dataset["dataset_code"].ToObject<String>(), Name = dataset["name"].ToObject<String>(), Extras = columnNames });
            }

            this.button2.Enabled = true;
        }

        private void AvailableColumns_Recalculate()
        {
            ArrayList newColumns = new ArrayList();
            foreach (dynamic item in this.listBox2.Items)
            {
                newColumns.AddRange(item.Extras);
            }

            foreach (String columnName in newColumns)
            {
                object columnSelection = new { Value = columnName, Name = columnName };

                if (!this.listBox3.Items.Contains(columnSelection))
                {
                    this.listBox3.Items.Add(columnSelection);
                }
            }

            ArrayList currentItems = new ArrayList(this.listBox3.Items);
            foreach (dynamic columnSelection in currentItems)
            {
                if (!newColumns.Contains(columnSelection.Value))
                {
                    this.listBox3.Items.Remove(columnSelection);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.listBox2.Items.Count == 0 || this.listBox3.Items.Count == 0 || this.listBox3.SelectedItems.Count == 0)
            {
                this.textBox3.Text = "You must select at least one Dataset and one Column To display.";
                return;
            }
            else if (this.activeCells == null) {
                this.textBox3.Text = "You have not selected a cell to populate data in.";
                return;
            }

            string[] quandlCodes = ConvertObjectCollectionToStringArray(this.listBox2.Items);
            string[] columnNames = new string[this.listBox3.SelectedItems.Count];

            int i = 0;
            foreach (dynamic columnName in this.listBox3.SelectedItems)
            {
                columnNames[i] = columnName.Value;
                i++;
            }

            TestFunctions.populateLatestStockData(quandlCodes, columnNames, this.activeCells);
        }

        private string[] ConvertObjectCollectionToStringArray(ListBox.ObjectCollection array)
        {
            string[] strArray = new string[array.Count];
            int i = 0;
            foreach (dynamic quandlCode in this.listBox2.Items) {
                strArray[i] = quandlCode.Value;
                i++;
            }
            return strArray;
        }
    }
}
