using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

public partial class frmAddCase : Form
{
    DatabaseHelper db = new DatabaseHelper(); // renamed and anonymized

    private string studentnumber, lastname, firstname, middlename, yearlevel, course, username;
    private int errorcount;
    private frmCaseManagement parentForm;

    public frmAddCase(frmCaseManagement parentForm, string studentnumber, string lastname, string firstname, string middlename, string yearlevel, string course, string username)
    {
        InitializeComponent();
        this.studentnumber = studentnumber;
        this.lastname = lastname;
        this.firstname = firstname;
        this.middlename = middlename;
        this.yearlevel = yearlevel;
        this.course = course;
        this.username = username;
        this.parentForm = parentForm;
        this.Draggable(true);
    }

    private void frmAddCase_Load(object sender, EventArgs e)
    {
        txtstudentid.Text = studentnumber;
        txtlastname.Text = lastname;
        txtfirstname.Text = firstname;
        txtmiddlename.Text = middlename;
        txtyearlevel.Text = yearlevel;
        txtcourse.Text = course;

        DataTable getviolations = db.GetData("SELECT violationcode FROM tblviolations");
        cmbviolation.Items.Clear();
        foreach (DataRow row in getviolations.Rows)
        {
            cmbviolation.Items.Add(row["violationcode"].ToString());
        }
    }

    private void cmbviolation_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbviolation.SelectedIndex != -1)
        {
            string code = cmbviolation.SelectedItem.ToString();
            DataTable result = db.GetData("SELECT description FROM tblviolations WHERE violationcode = '" + code + "';");
            if (result.Rows.Count > 0)
            {
                txtviolationdescription.Text = result.Rows[0]["description"].ToString();
            }
        }
    }

    private void btnclear_Click(object sender, EventArgs e)
    {
        errorProvider1.Clear();
        cmbviolation.SelectedIndex = -1;
        txtviolationdescription.Text = "";
        txtschoolyear.Clear();
        cmbconcernlevel.SelectedIndex = -1;
        txtrecommendation.Clear();
    }

    private void validateForm()
    {
        errorProvider1.Clear();
        errorcount = 0;

        if (cmbviolation.SelectedIndex < 0)
        {
            errorProvider1.SetError(cmbviolation, "Select a violation");
            errorcount++;
        }
        if (string.IsNullOrEmpty(txtschoolyear.Text))
        {
            errorProvider1.SetError(txtschoolyear, "School Year is empty");
            errorcount++;
        }
        if (cmbconcernlevel.SelectedIndex < 0)
        {
            errorProvider1.SetError(cmbconcernlevel, "Select a concern level");
            errorcount++;
        }
        if (string.IsNullOrEmpty(txtrecommendation.Text))
        {
            errorProvider1.SetError(txtrecommendation, "Recommendation is empty");
            errorcount++;
        }
    }

    private void btnsave_Click(object sender, EventArgs e)
    {
        validateForm();
        if (errorcount == 0)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to add this case?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    string caseID = DateTime.Now.ToString("yyyyMMddHHmmss");
                    string query = "SELECT studentID, violationID FROM tblcases WHERE violationID = '" + cmbviolation.Text + "' AND studentID = '" + studentnumber + "';";
                    DataTable existing = db.GetData(query);

                    string violationcount = "1st Offense";
                    if (existing.Rows.Count == 1)
                        violationcount = "2nd Offense";
                    else if (existing.Rows.Count > 1)
                        violationcount = "Repeat Offense";

                    db.ExecuteSQL("INSERT INTO tblcases (caseID, studentID, violationID, violationcount, status, resolution, createdby, datecreated, schoolyear, concernlevel, recommendation) " +
                                  "VALUES ('" + caseID + "', '" + studentnumber + "', '" + cmbviolation.Text + "', '" + violationcount + "', 'On-going', '', '" + username + "', '" +
                                  DateTime.Now.ToShortDateString() + "', '" + txtschoolyear.Text + "', '" + cmbconcernlevel.Text + "', '" + txtrecommendation.Text + "')");

                    if (db.RowAffected > 0)
                    {
                        db.ExecuteSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) VALUES ('" +
                            DateTime.Now.ToShortDateString() + "', '" + DateTime.Now.ToShortTimeString() +
                            "', 'Add', 'Case Management', '" + caseID + "', '" + username + "')");

                        MessageBox.Show("New case added", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        parentForm.LoadCases();
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error on save", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }

    // Minimize & Close
    private void btn_minimize_Click(object sender, EventArgs e)
    {
        this.WindowState = FormWindowState.Minimized;
    }

    private void btn_close_Click(object sender, EventArgs e)
    {
        this.Close();
    }

    // Hover Effects
    private void btn_minimize_MouseEnter(object sender, EventArgs e)
    {
        btn_minimize.BackColor = Color.Silver;
    }

    private void btn_minimize_MouseLeave(object sender, EventArgs e)
    {
        btn_minimize.BackColor = Color.FromArgb(150, 0, 52, 112);
    }

    private void btn_close_MouseEnter(object sender, EventArgs e)
    {
        btn_close.BackColor = Color.Salmon;
    }

    private void btn_close_MouseLeave(object sender, EventArgs e)
    {
        btn_close.BackColor = Color.FromArgb(150, 0, 52, 112);
    }
}
