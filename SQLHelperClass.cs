//add first this library
//using MySql.Data.MySqlClient;
using System.Data.SqlClient; // SQL Server
using System.Data;

class SQLHelperClass
{
    private string sqlConString;
    public int rowAffected = 0;

    public SQLHelperClass(string server_address,string database, string username, string password)
    {
        //Server = server name(xampp) Uid = username Pwd = password
        sqlConString = "Server = " + server_address + "; Database = " + database + "; User Id = "
        + username + "; Password = " + password;
        //XAMPP
        //sqlConString = "Server = " + server_address + "; Database = " + database + "; UId = "
        //+ username + "; Pwd = " + password + "; CharSet = utf8;";
    }

    //select
    public DataTable GetData(string sql)
    {
        SqlConnection Sqlcon = new SqlConnection(sqlConString);
        if (Sqlcon.State == ConnectionState.Closed) Sqlcon.Open();
        SqlCommand SQLcom = new SqlCommand(sql, Sqlcon);
        SqlDataAdapter SQLadap = new SqlDataAdapter(SQLcom);
        DataSet ds = new DataSet();
        SQLadap.Fill(ds);
        return ds.Tables[0];
    }

    //insert, update, delete
    public void executeSQL(string sql)
    {
        SqlConnection Sqlcon = new SqlConnection(sqlConString);
        if (Sqlcon.State == ConnectionState.Closed) Sqlcon.Open();
        SqlCommand SQLcom = new SqlCommand(sql, Sqlcon);
        rowAffected = SQLcom.ExecuteNonQuery();
    }

    public string SqlConString { get; set; }
}
