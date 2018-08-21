Imports System.IO



Public Class Form1
    Dim MyBytes As String = "" ' System.Environment.CurrentDirectory & "\1.jpg"
    'Dim t As Thread '定义全局线程变量
    Dim cnStr As String = "Provider=microsoft.ace.oledb.12.0;Data Source=ff8.accdb;Jet OLEDB:Database Password=;Persist Security Info=False"  '筛选中定义重复
    Dim sql As String = "select * from 会员"      '筛选中定义重复
    'Dim con As New OleDb.OleDbConnection(cnStr)
    'Dim cmd As New OleDb.OleDbCommand(sql, con)
    Dim Navigator As BindingManagerBase
    Dim cn As OleDb.OleDbConnection
    Dim cm As OleDb.OleDbCommand
    Dim da As OleDb.OleDbDataAdapter
    Dim ds As DataSet
    Dim dt As DataTable
    Dim r As DataRow


    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
            PictureBox1.Image = Image.FromFile(OpenFileDialog1.FileName)
        End If

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        addpic()
    End Sub
    Private Sub addpic()
        Dim MyStream As New System.IO.MemoryStream
        '将图片框中的图片以BMP形式存入内存流中
        Me.PictureBox1.Image.Save(MyStream, System.Drawing.Imaging.ImageFormat.Bmp)
        Dim MyBytes(MyStream.Length) As Byte    '声明数组
        MyBytes = MyStream.ToArray()           '调用方法给数组赋值
        Dim options As Object() = {MyBytes}
        cn = New OleDb.OleDbConnection(cnStr)

        cn.Close()
        cn.Open() '插入前，必须连接
        Dim sql As String = "INSERT INTO gf (img)  VALUES(?)"
        cm = New OleDb.OleDbCommand(sql, cn)
        cm.Parameters.Add(New OleDb.OleDbParameter)
        cm.Parameters(0).Value = options(0)
        cm.ExecuteNonQuery()
        MsgBox("修改成功！", vbInformation, "ts")
        cn.Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        loadpic()
    End Sub
    Private Sub loadpic()
        Dim sql As String = "select * from gf"
        cn = New OleDb.OleDbConnection(cnStr)

        cn.Close()
        cn.Open()
        Dim cmd As New OleDb.OleDbCommand(sql, cn)
        Dim RS As OleDb.OleDbDataAdapter = New OleDb.OleDbDataAdapter(cmd)
        Dim DT As New DataSet
        RS.Fill(DT, "shouz")
        cn.Close()
        Dim ok As Integer
        ok = TextBox1.Text          '指定行的数据
        Dim Picturebyte = DT.Tables("shouz").Rows(ok - 1).Item(1)    '指定行的数据第2项的数据
        '用这个二进制数据创建一个流  
        Dim ioStream As IO.MemoryStream = New IO.MemoryStream(Picturebyte, True)
        '用流转化成BITMAP并设置到图片框中去  
        PictureBox1.Image = Bitmap.FromStream(ioStream, True)
    End Sub


End Class
