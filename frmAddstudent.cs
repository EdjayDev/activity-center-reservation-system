using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

public partial class frmAddstudent : Form
{
    private string username;
    private int errorcount;
    private Form frmStudents_load;

    // SQL connection helper, anonymous connection string (replace with your config)
    Class1 addstudent = new Class1("SERVER_NAME", "DATABASE_NAME", "USERNAME", "PASSWORD");

    Dictionary<string, int> strandsOptions = new Dictionary<string, int>
    {
        { "ACADEMIC TRACK - General Academic Strand (GAS)", 0 },
        { "ACADEMIC TRACK - Humanities and Social Sciences (HUMSS)", 1 },
        { "ACADEMIC TRACK - Accountancy, Business and Management (ABM)", 2 },
        { "ACADEMIC TRACK - Science, Technology, Engineering and Mathematics (STEM)", 3 },
        { "ARTS AND DESIGN TRACK - Performing Arts", 4 },
        { "SPORTS TRACK - Coaching and Sports", 5 },
        { "SPORTS TRACK - Officiating", 6 },
        { "TECHNICAL VOCATIONAL LIVELIHOOD TRACK - Food and Beverage Services", 7 },
        { "TECHNICAL VOCATIONAL LIVELIHOOD TRACK - Bread and Pastry Production", 8 },
        { "TECHNICAL VOCATIONAL LIVELIHOOD TRACK - Housekeeping", 9 },
        { "TECHNICAL VOCATIONAL LIVELIHOOD TRACK - Cookery", 10 },
        { "TECHNICAL VOCATIONAL LIVELIHOOD TRACK - Caregiving", 11 },
        { "TECHNICAL VOCATIONAL LIVELIHOOD TRACK - Tour Guiding Services", 12 },
        { "TECHNICAL VOCATIONAL LIVELIHOOD TRACK - Bartending", 13 },
        { "TECHNICAL VOCATIONAL LIVELIHOOD TRACK - Tourism Promotion Services", 14 },
        { "TECHNICAL VOCATIONAL LIVELIHOOD TRACK - Computer Programming", 15 },
        { "TECHNICAL VOCATIONAL LIVELIHOOD TRACK - Animation", 16 },
        { "TECHNICAL VOCATIONAL LIVELIHOOD TRACK - Electrical Installation and Maintenance", 17 },
        { "TECHNICAL VOCATIONAL LIVELIHOOD TRACK - Machining", 18 }
    };

    Dictionary<string, int> courseOptions = new Dictionary<string, int>
    {
        { "BACHELOR OF ARTS - Bachelor of Arts in English Language", 0 },
        { "BACHELOR OF ARTS - Bachelor of Arts in History (BA History)", 1 },
        { "BACHELOR OF ARTS - Bachelor of Arts in Political Science (BA PoS)", 2 },
        { "BACHELOR OF ARTS - Bachelor of Arts in Psychology (AB/BA Psychology)", 3 },
        { "BACHELOR OF ARTS - Bachelor of Performing Arts-Dance (BPeA)", 4 },
        { "BACHELOR OF SCIENCE - Bachelor of Science in Criminology", 5 },
        { "BACHELOR OF SCIENCE - Bachelor of Science in Medical Technology", 6 },
        { "BACHELOR OF SCIENCE - Bachelor of Science in Nursing", 7 },
        { "BACHELOR OF SCIENCE - Bachelor of Science in Pharmacy", 8 },
        { "BACHELOR OF SCIENCE - Bachelor of Science in Physical Therapy (BSPT)", 9 },
        { "BACHELOR OF SCIENCE - Bachelor of Science in Radiologic Technology", 10 },
        { "BACHELOR OF SCIENCE - Bachelor of Science in Computer Science", 11 },
        { "BACHELOR OF SCIENCE - Bachelor of Science in Information Technology", 12 },
        { "BACHELOR OF SCIENCE - Bachelor of Science in Information Systems", 13 },
        { "BACHELOR OF SCIENCE - Bachelor of Science in Accountancy", 14 },
        { "BACHELOR OF SCIENCE - Bachelor of Science in Business Administration Major in Marketing Management", 15 },
        { "BACHELOR OF SCIENCE - Bachelor of Science in Business Administration Major in Financial Management", 16 },
        { "BACHELOR OF SCIENCE - Bachelor of Science in Business Administration Major in Operations Management", 17 },
        { "BACHELOR OF SCIENCE - Bachelor of Science in Business Administration Major in Human Resource Development Management", 18 },
        { "BACHELOR OF SCIENCE - Bachelor of Science in Accounting Information System (BSAIS)", 19 },
        { "BACHELOR OF SCIENCE - BSBA – Major in Human Resource Management (BSBA – HRM)", 20 },
        { "BACHELOR OF SCIENCE - BSBA – Major in Marketing Management (BSBA – MM)", 21 },
        { "BACHELOR OF SCIENCE - BSBA – Major in Financial Management (BSBA – FM)", 22 },
        { "BACHELOR OF SCIENCE - BSBA – Major in Business Management (BSBA – BM)", 23 },
        { "BACHELOR OF SCIENCE - BSBA – Major in Management Accounting (BSBA – MA)", 24 },
        { "BACHELOR OF SCIENCE - Bachelor of Science in Hospitality Management (BSHM)", 25 },
        { "BACHELOR OF SCIENCE - Bachelor of Science in Tourism Management (BSTM)", 26 },
        { "BACHELOR OF SCIENCE - Bachelor of Science in Midwifery", 27 },
        { "BACHELOR OF SCIENCE - Bachelor of Science in Psychology", 28 },
        { "BACHELOR OF EDUCATION - Bachelor of Elementary Education - General Education", 29 },
        { "BACHELOR OF EDUCATION - Bachelor of Elementary - Pre-School Education", 30 },
        { "BACHELOR OF EDUCATION - Bachelor of Elementary Education - SPED", 31 },
        { "BACHELOR OF EDUCATION - Bachelor of Secondary Education Major in Science", 32 },
        { "BACHELOR OF EDUCATION - Bachelor of Secondary Education Major in English", 33 },
        { "BACHELOR OF EDUCATION - Bachelor of Secondary Education Major in Math", 34 },
        { "BACHELOR OF EDUCATION - Bachelor of Secondary Education Major in Filipino", 35 },
        { "BACHELOR OF EDUCATION - Bachelor of Secondary Education Major in Social Studies", 36 },
        { "BACHELOR OF EDUCATION - Bachelor of Secondary Education Major in Values Education", 37 },
        { "BACHELOR OF EDUCATION - Bachelor of Physical Education – Sports and Wellness Management (BPE-SWM)", 38 },
        { "BACHELOR OF EDUCATION - Bachelor of Physical Education (BPEd)", 39 },
        { "BACHELOR OF EDUCATION - Bachelor of Library and Information Science", 40 }
    };

    public frmAddstudent(Form frmStudents_load, string username)
    {
        InitializeComponent();
        this.username = username;
        this.Draggable(true);
        this.frmStudents_load = frmStudents_load;
    }

    private void validateForm()
    {
        errorProvider1.Clear();
        errorcount = 0;

        if (string.IsNullOrEmpty(txtstudentid.Text))
        {
            errorProvider1.SetError(txtstudentid, "Student ID is empty");
            errorcount++;
        }
        if (string.IsNullOrEmpty(txtstudentln.Text))
        {
            errorProvider1.SetError(txtstudentln, "Student last name is empty");
            errorcount++;
        }
        if (string.IsNullOrEmpty(txtstudentfn.Text))
        {
            errorProvider1.SetError(txtstudentfn, "Student first name is empty");
            errorcount++;
        }
        if (string.IsNullOrEmpty(txtstudentmn.Text))
        {
            txtstudentmn.Text = "N/A";
        }
        if (cmbyearlevel.SelectedIndex < 0)
        {
            errorProvider1.SetError(cmbyearlevel, "Select student year level");
            errorcount++;
        }

        if (cmbyearlevel.SelectedIndex == 2) // Senior High (Strands)
        {
            if (!strandsOptions.ContainsKey(cmbstudentcourse.Text))
            {
                errorProvider1.SetError(cmbstudentcourse, "Select a valid student strand");
                errorcount++;
            }
        }
        else if (cmbyearlevel.SelectedIndex == 3) // College (Courses)
        {
            if (!courseOptions.ContainsKey(cmbstudentcourse.Text))
            {
                errorProvider1.SetError(cmbstudentcourse, "Select a valid student course");
                errorcount++;
            }
        }
        else if (cmbyearlevel.SelectedIndex == 0 || cmbyearlevel.SelectedIndex == 1) // Junior High
        {
            cmbstudentcourse.Text = "N/A";
        }

        try
        {
            DataTable dt = addstudent.GetData($"SELECT * FROM tblstudents WHERE studentID = '{txtstudentid.Text}'");
            if (dt.Rows.Count > 0)
            {
                errorProvider1.SetError(txtstudentid, "Student ID already in use");
                errorcount++;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error on validating existing student", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void cmbyearlevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        cmbstudentcourse.Items.Clear();
        labelflexible.Text = "N/A";
        cmbstudentcourse.Text = "";

        if (cmbyearlevel.SelectedIndex == 2) // Senior High
        {
            foreach (var course in strandsOptions.Keys)
            {
                cmbstudentcourse.Items.Add(course);
            }
            labelflexible.Text = "STRANDS";
            labelcourse.Text = "Select:";
            cmbstudentcourse.Enabled = true;
        }
        else if (cmbyearlevel.SelectedIndex == 3) // College
        {
            foreach (var course in courseOptions.Keys)
            {
                cmbstudentcourse.Items.Add(course);
            }
            labelflexible.Text = "COURSES";
            labelcourse.Text = "Select:";
            cmbstudentcourse.Enabled = true;
        }
        else
        {
            cmbstudentcourse.Text = "N/A";
            cmbstudentcourse.Enabled = false;
        }
    }

    private void btnsave_Click(object sender, EventArgs e)
    {
        validateForm();
        if (errorcount == 0)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to add this account?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    addstudent.executeSQL(
                        "INSERT INTO tblstudents (studentID, studentLN, studentFN, studentMN, yearLevel, studentCourse, dateCreated, createdBy) " +
                        $"VALUES ('{txtstudentid.Text}', '{txtstudentln.Text}', '{txtstudentfn.Text}', '{txtstudentmn.Text}', '{cmbyearlevel.Text.ToUpper()}', '{cmbstudentcourse.Text}', '{DateTime.Now.ToShortDateString()}', '{username}')");

                    if (addstudent.rowAffected > 0)
                    {
                        addstudent.executeSQL(
                            "INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) VALUES (" +
                            $"'{DateTime.Now.ToShortDateString()}', '{DateTime.Now.ToShortTimeString()}', 'Add', 'Students Management', '{txtstudentid.Text}', '{username}')");

                        MessageBox.Show("New account added", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        var method = frmStudents_load.GetType().GetMethod("LoadStudents");
                        method?.Invoke(frmStudents_load, null);
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

    private void btnclear_Click(object sender, EventArgs e)
    {
        errorProvider1.Clear();
        txtstudentid.Clear();
        txtstudentln.Clear();
        txtstudentfn.Clear();
        txtstudentmn.Clear();
        cmbyearlevel.SelectedIndex = -1;
        cmbstudentcourse.Text = "";
        cmbstudentcourse.SelectedIndex = -1;
        txtstudentid.Focus();
    }

    // DESIGN related events

    private void btn_minimize_Click(object sender, EventArgs e)
    {
        this.WindowState = FormWindowState.Minimized;
    }

    private void btn_close_Click(object sender, EventArgs e)
    {
        this.Close();
    }

    private void btn_minimize_MouseHover(object sender, EventArgs e)
    {
        btn_minimize.BackColor = Color.Silver;
        btn_minimize.BorderStyle = BorderStyle.FixedSingle;
    }

    private void btn_minimize_MouseLeave(object sender, EventArgs e)
    {
        btn_minimize.BackColor = Color.Transparent;
        btn_minimize.BorderStyle = BorderStyle.None;
    }

    private void btn_close_MouseHover(object sender, EventArgs e)
    {
        btn_close.BackColor = Color.Salmon;
        btn_close.BorderStyle = BorderStyle.FixedSingle;
    }

    private void btn_close_MouseLeave(object sender, EventArgs e)
    {
        btn_close.BackColor = Color.Transparent;
        btn_close.BorderStyle = BorderStyle.None;
    }
}
