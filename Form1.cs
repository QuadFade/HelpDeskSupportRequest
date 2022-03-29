//John Fade IV 2307363
//322, 349 Group Box dimensions
// 18, 12 Group Box Location
//800, 500 Form1 Dimensions
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace HelpDeskServiceRequest
{

    public partial class Form1 : Form
    {

        
        public DataTable dtSource = new DataTable();
        public DataView dvView = new DataView();
        public DataRow drSource;
        public DataColumn dcSource = new DataColumn();
        public int rowID;
        public Form1()
        {
            
            InitializeComponent();
            MakeDataTableAndDisplay();//creates the datatable and displays it on run
            createRequestGroupBox.Visible = true;//initializes the create service group box as visible
            editGroupBox.Visible = false;//hides the edit box
            dataGridView1.AllowUserToAddRows = false;
            

        }

        //closes the program
        private void ExitBTN_Click(object sender, EventArgs e)
        {
            DataTable dt = dvView.ToTable();
            string filename = @"ticket.json";

            JSONSerialize(dt, filename);
            this.Close();
        }

        //displays the create request groupbox
        private void createRequestBTN_Click(object sender, EventArgs e)
        {
            createRequestGroupBox.Visible = true;
            editGroupBox.Visible = false;
        }

     
        //simply calls the createDataBTN method 
        //displays a successful submission messagebox 
        public void createSubmitBTN_Click(object sender, EventArgs e)
        {
            createDataBTN();

            MessageBox.Show("Request Submitted Successfully!");
        }

        //places the clicked rowheader data into the edit groupbox 
        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            
            createRequestGroupBox.Visible = false;
            editGroupBox.Visible = true;
            
            //clears any text in the fields
            dateTimePicker1.ResetText();
            technicianAssignedTBox.Clear();
            requestCatagoryComboBox.ResetText();
            descriptionTextBox.Clear();
            notesBox.ResetText();

            //holds the currently clicked rowheader
            drSource = dtSource.Rows[e.RowIndex];
            //adds the data to the text fields for edit
            editDescriptionTextBox.Text = drSource["Description"].ToString();
            editRequestDateTimePicker.Text = drSource["Date"].ToString();
            editRequestCatagoryComboBox.Text = drSource["Category"].ToString();
            editTechnicianTextBox.Text = drSource["Technician Assigned"].ToString();
            editNotesTextBox.Text = drSource["Notes"].ToString();
        }

        //handles the creation of new row data for the fields
        public void createDataBTN()
        {
            
            
            //creates and adds data to a new row.
            drSource = dtSource.NewRow();
            drSource["ID"] = rowID++;
            drSource["Date"] = dateTimePicker1.Text;
            drSource["Technician Assigned"] = technicianAssignedTBox.Text;
            drSource["Category"] = requestCatagoryComboBox.Text;
            drSource["Description"] = descriptionTextBox.Text;
            drSource["Notes"] = notesBox.Text;
            drSource["Date Completed"] = "n/a";
            dtSource.Rows.Add(drSource);//adds new row data to the DataTable

            //Clears each box for a new entry
            dateTimePicker1.ResetText();
            technicianAssignedTBox.Clear();
            requestCatagoryComboBox.ResetText();
            descriptionTextBox.Clear();
            notesBox.Clear();
            createRequestGroupBox.Visible = true;

        }
        //handles deleting row data
        private void editDeleteTicketBTN_Click(object sender, EventArgs e)
        {
            //prompts the user for deletion confirmation
            if (MessageBox.Show("Are you sure you want to delete this row?", "Attention!!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                drSource.Delete();
                MessageBox.Show("Data Deleted", "Done", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                //this block will be executed when No/Cancel is selected
            }
            
        }

        //Method that creates the Data Table and displays it
        private void MakeDataTableAndDisplay()
        {
            //deserializes json data to the datatable
            dtSource = JSONDeserialize(@"ticket.json");
            //simple ID counter
            rowID = dtSource.Rows.Count;

            // Create new ID DataColumn, set DataType, ColumnName and add to DataTable.
            dcSource = new DataColumn();
            dcSource.DataType = System.Type.GetType("System.Int32");
            dcSource.ColumnName = "ID";

            // Create string columns with switch case
            for (int i = 1; i < 7; i++)
            {

                dcSource = new DataColumn();
                dcSource.DataType = Type.GetType("System.String");
                switch (i)
                {
                    case 1:
                        dcSource.ColumnName = ("Date");
                        
                        break;
                    case 2:
                        dcSource.ColumnName = ("Technician Assigned");
                       
                        break;
                    case 3:
                        dcSource.ColumnName = ("Category");
                        
                        break;
                    case 4:
                        dcSource.ColumnName = ("Description");
                       
                        break;
                    case 5:
                        dcSource.ColumnName = ("Notes");
                       
                        break;
                    case 6:
                        dcSource.ColumnName = ("Date Completed");
                      
                        break;

                }
               
            }
            

            // Create a DataView using the DataTable.
            dvView = new DataView(dtSource);

            // Set a DataGrid control's DataSource to the DataView.
            dataGridView1.DataSource = dvView;
        }
        //Json Serializer
        private void JSONSerialize(DataTable _dt, string _fileName)
        {
            // Serializaion                
            string JSONresult;
            JSONresult = JsonConvert.SerializeObject(_dt);
            File.WriteAllText(@_fileName, JSONresult);
        }
        //Json Deserializer
        private DataTable JSONDeserialize(string _fileName)
        {
            // DeSerializaion                
            string readText = File.ReadAllText(_fileName);
            dtSource = (DataTable)JsonConvert.DeserializeObject(readText, typeof(DataTable));  
            return dtSource;
            
        }

        //handles saving the json file even when closing with the windows exit button.
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            DataTable dt = dvView.ToTable();
            string filename = @"ticket.json";

            JSONSerialize(dt, filename);
            
        }
        //Handles changes made within the selected row header
        private void editSubmitBTN_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Confirm changes to this row?", "Attention!!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                drSource["Description"] = editDescriptionTextBox.Text;
                drSource["Date"] = editRequestDateTimePicker.Text;
                drSource["Category"] = editRequestCatagoryComboBox.Text;
                drSource["Technician Assigned"] = editTechnicianTextBox.Text;
                drSource["Notes"] = editNotesTextBox.Text;
                MessageBox.Show("Data Updated!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                //this block will be executed when No/Cancel is selected
            }
            
        }
        //"closes" the ticket by adding a timestamp to the date completed column
        //possibly swap out for checkbox...
        private void editCloseTicketBTN_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Confirm job complete?", "Attention!!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                drSource["Date Completed"] = DateTime.Now;
                
            }
            else
            {
                //this block will be executed when No/Cancel is selected
            }

        }
    }
}


