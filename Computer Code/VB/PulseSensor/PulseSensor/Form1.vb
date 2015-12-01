Public Class Form1
    Public Shared input As String
    Public Shared array(3) As String
    Public Shared value As Integer = 0
    Public Shared x_value As Double = 0
    Public Shared counter As Integer = 0
    Public Sub combo_set(ByVal index As Integer)
        If index = 1 Then
            ComboBox1.Enabled = False
            ComboBox2.Enabled = False
            ComboBox4.Enabled = False
            ComboBox3.Enabled = False
            ComboBox5.Enabled = False
        Else
            ComboBox1.Enabled = True
            ComboBox2.Enabled = True
            ComboBox4.Enabled = True
            ComboBox3.Enabled = True
            ComboBox5.Enabled = True
        End If
    End Sub
    Public Function stop_select(ByVal index As String) As IO.Ports.StopBits
        If index = "One" Then
            Return IO.Ports.StopBits.One
        ElseIf index = "Two" Then
            Return IO.Ports.StopBits.Two
        ElseIf index = "OnePointFive" Then
            Return IO.Ports.StopBits.OnePointFive


        End If
        Return IO.Ports.StopBits.One

    End Function
    Public Function parity_select(ByVal index As String) As IO.Ports.Parity
        If index = "None" Then
            Return IO.Ports.Parity.None
        ElseIf index = "Mark" Then
            Return IO.Ports.Parity.Mark
        ElseIf index = "Space" Then
            Return IO.Ports.Parity.Space
        ElseIf index = "Odd" Then
            Return IO.Ports.Parity.Odd
        ElseIf index = "Even" Then
            Return IO.Ports.Parity.Even
        End If
        Return IO.Ports.Parity.None
    End Function
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        End
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.SelectedIndex = 0
        ComboBox2.SelectedIndex = 0
        ComboBox4.SelectedIndex = 0
        ComboBox3.SelectedIndex = 0
        ComboBox5.SelectedIndex = 0
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        On Error GoTo error_label
        If Button2.Text = "Connect" Then
            SerialPort1.PortName = ComboBox1.SelectedItem.ToString
            SerialPort1.BaudRate = ComboBox2.SelectedItem
            SerialPort1.DataBits = ComboBox4.SelectedItem
            SerialPort1.Parity = parity_select(ComboBox3.SelectedItem.ToString)
            SerialPort1.StopBits=stop_select(ComboBox4.SelectedItem.ToString)
            If Not SerialPort1.IsOpen Then
                SerialPort1.Open()
                Button2.Text = "Disconnect"
                combo_set(1)
                Timer1.Enabled = True
                Button2.BackColor = Color.Aqua
            Else
                MsgBox("Port Is Open")
            End If
        Else
            SerialPort1.Close()
            Button2.Text = "Connect"
            combo_set(2)
            Timer1.Enabled = False
            Button2.BackColor = Color.SkyBlue
        End If

        Exit Sub
error_label:
        MsgBox("Error In Serial Connection")
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        input = SerialPort1.ReadExisting
        If input.Contains("sep") Then
            array = Split(input, "sep")
            value = value + Val(array(1))
            counter = counter + 1
            If counter = 10 Then
                value = value / 10
                counter = 0
                ProgressBar1.Value = value
                Chart1.Series(0).Points.AddXY(x_value, value)
            End If
        End If

    End Sub

    Private Sub Chart1_Click(sender As Object, e As EventArgs) Handles Chart1.Click

    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        Randomize()
        x_value = Rnd()
        x_value = x_value * 120
        counter = counter + 1
        Chart1.Series(0).Points.AddXY(counter, Int(x_value))
        ProgressBar1.Value = Int(x_value)
        Label2.Text = Int(x_value)
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Timer2.Enabled = True
    End Sub
End Class
