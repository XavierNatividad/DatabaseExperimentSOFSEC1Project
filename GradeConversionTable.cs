using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DatabaseExperimentSOFSEC1Project
{
    public partial class GradeConversionTable : Form
    {
        private DataGridView conversionTableGrid;

        public GradeConversionTable()
        {
            InitializeComponent();
            this.Load += GradeConversionTable_Load;  // Attach the Load event
        }

        private void GradeConversionTable_Load(object sender, EventArgs e)
        {
            SetupConversionTable();  // Call SetupConversionTable when the form loads
        }

        private void SetupConversionTable()
        {
            conversionTableGrid = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeColumns = false,
                AllowUserToResizeRows = false,
                SelectionMode = DataGridViewSelectionMode.CellSelect,
                MultiSelect = false,
                BackgroundColor = Color.White,
                RowTemplate = { Height = 37 },
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Font = new Font("Inter", 12),
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    SelectionBackColor = Color.White,
                    SelectionForeColor = Color.Black
                },
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    Font = new Font("Inter", 14, FontStyle.Bold),
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                },
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                ColumnHeadersHeight = 40
            };

            conversionTableGrid.Columns.Add("Percentage", "Percentage");
            conversionTableGrid.Columns.Add("Grade", "Grade");
            conversionTableGrid.Columns.Add("Description", "Description");

            foreach (DataGridViewColumn col in conversionTableGrid.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.ReadOnly = true;
            }

            string[,] gradeData = {
                { "97-100", "4.000", "Excellent" },
                { "93-96", "3.500", "Superior" },
                { "89-92", "3.000", "Very Good" },
                { "85-88", "2.500", "Good" },
                { "80-84", "2.000", "Satisfactory" },
                { "75-79", "1.500", "Fair" },
                { "70-74", "1.000", "Pass" },
                { "Below 70", "R", "Repeat" },
                { "", "W", "Authorized Withdrawal" },
                { "", "A", "Audit" },
                { "", "9.9", "Deferred Grade" }
            };

            for (int i = 0; i < gradeData.GetLength(0); i++)
            {
                conversionTableGrid.Rows.Add(gradeData[i, 0], gradeData[i, 1], gradeData[i, 2]);
            }

            Controls.Add(conversionTableGrid);
        }
    }
}
