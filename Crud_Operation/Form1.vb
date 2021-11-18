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
            sqlQuery = "Insert into myconnector (StudentID,FirstName, LastName, Adress, PostCode, Phone_No) 
values('" & txtStudentID.Text & "','" & txtFirstName.Text & "','" & txtLastName.Text & "','" & txtAddress.Text & "','" & txtPostCode.Text & "','" & txtPhoneNo.Text & "')"
            sqlCmd = New MySqlCommand(sqlQuery, sqlConn)
            sqlRd = sqlCmd.ExecuteReader
            sqlConn.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "MySQL Connector", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            sqlConn.Dispose()
        End Try

        updateTable()


    End Sub

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        Try
            For Each txt In Panel6.Controls
                If TypeOf txt Is TextBox Then
                    txt.text = ""
                End If
            Next
            txtSearch.Text = ""
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        sqlConn.ConnectionString = "server =" + server + ";" + "user id =" + username + ";" _
            + "password = " + password + ";" + "database =" + database
        sqlConn.Open()

        sqlCmd.Connection = sqlConn

        With sqlCmd
            .CommandText = "Update myconnector set StudentID = @Studentid,FirstName = @Firstname, LastName= @lastname, 
            Adress= @Address, PostCode= @PostCode, Phone_No= @phoneNo where StudentID=@Studentid"

            .CommandType = CommandType.Text
            .Parameters.AddWithValue("@Studentid", txtStudentID.Text)
            .Parameters.AddWithValue("@Firstname", txtFirstName.Text)
            .Parameters.AddWithValue("@lastname", txtLastName.Text)
            .Parameters.AddWithValue("@Address", txtAddress.Text)
            .Parameters.AddWithValue("@PostCode", txtPostCode.Text)
            .Parameters.AddWithValue("@phoneNo", txtPhoneNo.Text)


        End With


        sqlCmd.ExecuteNonQuery()
        sqlConn.Close()
        updateTable()
        sqlCmd.Parameters.Clear()
    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        Try
            txtStudentID.Text = DataGridView1.SelectedRows(0).Cells(1).Value.ToString
            txtFirstName.Text = DataGridView1.SelectedRows(0).Cells(2).Value.ToString
            txtLastName.Text = DataGridView1.SelectedRows(0).Cells(3).Value.ToString
            txtAddress.Text = DataGridView1.SelectedRows(0).Cells(4).Value.ToString
            txtPostCode.Text = DataGridView1.SelectedRows(0).Cells(5).Value.ToString
            txtPhoneNo.Text = DataGridView1.SelectedRows(0).Cells(6).Value.ToString
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        For Each row As DataGridViewRow In DataGridView1.SelectedRows
            DataGridView1.Rows.Remove(row)
        Next
        updateTable()
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        Dim height As Integer = DataGridView1.Height
        DataGridView1.Height = DataGridView1.RowCount * DataGridView1.RowTemplate.Height
        bitmap = New Bitmap(Me.DataGridView1.Width, Me.DataGridView1.Height)
        DataGridView1.DrawToBitmap(bitmap, New Rectangle(0, 0, Me.DataGridView1.Width, Me.DataGridView1.Height))
        PrintPreviewDialog1.Document = PrintDocument1
        PrintPreviewDialog1.PrintPreviewControl.Zoom = 1
        PrintPreviewDialog1.ShowDialog()
        DataGridView1.Height = height
    End Sub

    Private Sub PrintDocument1_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        e.Graphics.DrawImage(bitmap, 0, 0)
        Dim recP As RectangleF = e.PageSettings.PrintableArea

        If Me.DataGridView1.Height - recP.Height > 0 Then e.HasMorePages = True
    End Sub

    Private Sub txtSearch_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtSearch.KeyPress
        Try
            If Asc(e.KeyChar) = 13 Then
                Dim dv As DataView
                dv = sqlDt.DefaultView
                dv.RowFilter = String.Format("Firstname like '%{0}%'", txtSearch.Text)
                DataGridView1.DataSource = dv.ToTable()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
End Class
