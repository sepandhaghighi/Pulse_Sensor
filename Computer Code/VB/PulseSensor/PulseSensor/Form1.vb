Public Class Form1
    Public Shared input As String
    Public Shared array(3) As String
    Public Shared value As Double = 0
    Public Shared x_value As Double = 0
    Public Shared counter As Integer = 0
    Public Shared convert As Double = 100 / 1023
    Public Shared start_flag As Integer = 0
    Public Shared prev_value As Double = 1024
    Public Shared diff As Double = 0
    Public Shared prev_flag As Boolean = False
    Public Shared max_range As Double = 100
    Public Shared stop_thr As Double = 10
    Public Sub serial_set(ByVal index As Integer)
        If index = 1 Then
            Button2.Text = "Disconnect"
            serial_status(1)
            combo_set(1)
            Timer1.Enabled = True
            Button2.BackColor = Color.Aqua
        ElseIf index = 2 Then
            Button2.Text = "Connect"
            serial_status(2)
            combo_set(2)
            Timer1.Enabled = False
            Button2.BackColor = Color.SkyBlue
        End If
    End Sub
    Public Sub serial_status(ByVal index As Integer)
        If index = 1 Then
            Label13.Text = "Active"
            Label13.BackColor = Color.YellowGreen

        ElseIf index = 2 Then
            Label13.Text = "Inactive"
            Label13.BackColor = Color.Red
        End If
    End Sub
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
    Public Sub update_info(ByVal x As Double, ByVal v As Double)
        Label2.Text = Int(v)
        ProgressBar1.Value = v
        Chart1.Series(0).Points.AddXY(x, v)
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
    Public Sub sensor_status(ByVal index As Integer)
        If index = 1 Then
            Label9.Text = "Active"
            Label9.BackColor = Color.YellowGreen
        ElseIf index = 2 Then
            Label9.Text = "Inactive"
            Label9.BackColor = Color.Red
        ElseIf index = 3 Then
            Label9.Text = "Process"
            Label9.BackColor = Color.Orange
        End If
    End Sub
    Public Function chart_type(ByVal index As Integer) As DataVisualization.Charting.SeriesChartType
        If index = 0 Then
            Return DataVisualization.Charting.SeriesChartType.Line
        ElseIf index = 1 Then
            Return DataVisualization.Charting.SeriesChartType.Spline
        ElseIf index = 2 Then
            Return DataVisualization.Charting.SeriesChartType.StepLine
        ElseIf index = 3 Then
            Return DataVisualization.Charting.SeriesChartType.FastLine
        ElseIf index = 4 Then
            Return DataVisualization.Charting.SeriesChartType.Area
        End If
        Return DataVisualization.Charting.SeriesChartType.Line
    End Function
    Public Function chart_pallete(ByVal index As Integer) As DataVisualization.Charting.ChartColorPalette
        Select Case index
            Case 0 : Return DataVisualization.Charting.ChartColorPalette.None
            Case 1 : Return DataVisualization.Charting.ChartColorPalette.Bright
            Case 2 : Return DataVisualization.Charting.ChartColorPalette.Grayscale
            Case 3 : Return DataVisualization.Charting.ChartColorPalette.Excel
            Case 4 : Return DataVisualization.Charting.ChartColorPalette.Light
            Case 5 : Return DataVisualization.Charting.ChartColorPalette.Pastel
            Case 6 : Return DataVisualization.Charting.ChartColorPalette.EarthTones
            Case 7 : Return DataVisualization.Charting.ChartColorPalette.Berry
            Case 8 : Return DataVisualization.Charting.ChartColorPalette.Chocolate
            Case 9 : Return DataVisualization.Charting.ChartColorPalette.Fire
            Case Else
                Return DataVisualization.Charting.ChartColorPalette.None
        End Select
    End Function
    Public Sub init()
        ComboBox1.SelectedIndex = 0
        ComboBox2.SelectedIndex = 0
        ComboBox4.SelectedIndex = 0
        ComboBox3.SelectedIndex = 0
        ComboBox5.SelectedIndex = 0
        ListBox1.SelectedIndex = 0
        ListBox2.SelectedIndex = 0
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        End
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        init()
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
                serial_set(1)
            Else
                MsgBox("Port Is Open")
            End If
        Else
            SerialPort1.Close()
            serial_set(2)
        End If

        Exit Sub
error_label:
        MsgBox("Error In Serial Connection")
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        On Error GoTo timer_error_label
        input = SerialPort1.ReadExisting
        If input.Contains("sep") Then
            array = Split(input, "sep")
            If Val(array(1)) < 10 Then
                start_flag = start_flag + 1
            End If
        End If
        If start_flag > 20 Then
            sensor_status(3)
            prev_flag = False
            start_flag = 0
            Timer2.Enabled = True
            Timer1.Enabled = False
        End If
        Exit Sub
timer_error_label:
        serial_set(2)
        SerialPort1.Dispose()
        Timer1.Enabled = False
        Timer2.Enabled = False
        MsgBox("Serial Port Disconnected")

        





    End Sub

    Private Sub Chart1_Click(sender As Object, e As EventArgs) Handles Chart1.Click

    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        On Error GoTo timer2_error_link
        input = SerialPort1.ReadExisting
        If input.Contains("sep") Then
            array = Split(input, "sep")
            If Val(array(1)) > 10 Then
                value = value + Val(array(1))
                counter = counter + 1

            End If
            If counter = 10 Then
                value = value / 10
                counter = 0
                sensor_status(1)
                value = value * convert
                If value - prev_value > stop_thr And prev_flag = True Then
                    ProgressBar1.Value = 0
                    Label2.Text = 0
                    prev_flag = False
                    sensor_status(2)
                    Timer1.Enabled = True
                    Timer2.Enabled = False

                End If
                If value > 0 And value < max_range And prev_flag = True Then
                    update_info(x_value, value)
                    x_value = x_value + 1
                End If
                prev_value = value
                prev_flag = True
                value = 0
            End If

        End If
        Exit Sub
timer2_error_link:
        serial_set(2)
        SerialPort1.Dispose()
        Timer2.Enabled = False
        Timer1.Enabled = False
        MsgBox("Serial Port Disconnected")
        




    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs)
        Timer2.Enabled = True
    End Sub

    Private Sub Label9_Click(sender As Object, e As EventArgs) Handles Label9.Click

    End Sub

    Private Sub Timer3_Tick(sender As Object, e As EventArgs)

    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        Chart1.Series(0).ChartType=chart_type(ListBox1.SelectedIndex)
    End Sub

    Private Sub Label13_Click(sender As Object, e As EventArgs) Handles Label13.Click

    End Sub

    Private Sub ListBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox2.SelectedIndexChanged
        Chart1.Series(0).Palette = chart_pallete(ListBox2.SelectedIndex)
    End Sub

    Private Sub Label10_Click(sender As Object, e As EventArgs) Handles Label10.Click

    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        convert = 100 / 1024
        max_range = 100
        ProgressBar1.Maximum = max_range
        Label11.Text = max_range
        stop_thr = 40

    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton2.CheckedChanged
        convert = 1
        max_range = 1024
        ProgressBar1.Maximum = max_range
        Label11.Text = max_range
        stop_thr = 350
    End Sub
End Class
