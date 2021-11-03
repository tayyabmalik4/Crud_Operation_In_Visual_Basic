Imports MySql.Data.MySqlClient
Imports Microsoft.VisualBasic.ApplicationServices
Imports Microsoft.Win32


Public Class Form1

    Dim sqlConn As New MySqlConnection
    Dim sqlCmd As New MySqlCommand
    Dim sqlRd As MySqlDataReader
    Dim sqlDt As New DataTable
    Dim DTA As New MySqlDataAdapter
    Dim sqlQuery As String

    Dim server As String = "localhost"
    Dim username As String = "root"
    Dim password As String = ""
    Dim database As String = "myconnector"

    Private bitmap As Bitmap
    'for getting the data from database
    Private Sub updateTable()
        sqlConn.ConnectionString = "server =" + server + ";" + "user id =" + username + ";" _
            + "password = " + password + ";" + "database =" + database

        sqlConn.Open()
        sqlCmd.Connection = sqlConn
        sqlCmd.CommandText = "SELECT * FROM myconnector "

        sqlRd = sqlCmd.ExecuteReader
        sqlDt.Load(sqlRd)
        sqlRd.Close()
        sqlConn.Close()

        DataGridView1.DataSource = sqlDt

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        updateTable()
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Dim iExit As DialogResult

        iExit = MessageBox.Show("Confirm if you want to exit", "MySQLConnector", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If iExit = DialogResult.Yes Then
            Application.Exit()
        End If
    End Sub

    Private Sub btnAddNew_Click(sender As Object, e As EventArgs) Handles btnAddNew.Click
        sqlConn.ConnectionString = "server =" + server + ";" + "user id =" + username + ";" _
            + "password = " + password + ";" + "database =" + database
        Try
            sqlConn.Open()
            sqlQuery = "Insert"
        Catch ex As Exception
            MessageBox.Show(ex.Message, "MySQL Connector", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try


    End Sub

End Class
