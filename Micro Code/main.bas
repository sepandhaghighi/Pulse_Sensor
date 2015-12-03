$regfile = "m8def.dat"
$crystal = 8000000
$baud = 9600
Enable Interrupts

Config Serialout = Buffered , Size = 100
Config Adc = Single , Prescaler = Auto
Start Adc

Dim A As Integer
Dim B As String * 7
Do


    A = Getadc(1)
    B = "sep"
    B = B + Str(a)

    Print B







Loop