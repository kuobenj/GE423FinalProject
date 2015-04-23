VERSION 5.00
Object = "{248DD890-BB45-11CF-9ABC-0080C7E7B78D}#1.0#0"; "MSWINSCK.OCX"
Begin VB.Form Form1 
   Caption         =   "Form1"
   ClientHeight    =   4875
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   6645
   LinkTopic       =   "Form1"
   ScaleHeight     =   4875
   ScaleWidth      =   6645
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton Command2 
      Caption         =   "Stop Camera"
      Enabled         =   0   'False
      Height          =   732
      Left            =   1440
      TabIndex        =   2
      Top             =   960
      Width           =   2292
   End
   Begin VB.Timer Timer1 
      Enabled         =   0   'False
      Interval        =   500
      Left            =   360
      Top             =   2160
   End
   Begin VB.CommandButton Command1 
      Caption         =   "Connect Camera"
      Height          =   612
      Left            =   1440
      TabIndex        =   0
      Top             =   360
      Width           =   2292
   End
   Begin MSWinsockLib.Winsock Winsock1 
      Left            =   360
      Top             =   2880
      _ExtentX        =   741
      _ExtentY        =   741
      _Version        =   393216
      RemoteHost      =   "192.168.1.71"
      RemotePort      =   10001
   End
   Begin VB.Label Label4 
      Height          =   495
      Left            =   4440
      TabIndex        =   1
      Top             =   2760
      Width           =   2175
   End
   Begin VB.Image Image1 
      Height          =   2160
      Left            =   1560
      Picture         =   "vbwinsock.frx":0000
      Top             =   2040
      Width           =   2640
   End
End
Attribute VB_Name = "Form1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
Dim datatosend As String
Dim RecieveData As String
Dim tmp As Byte
Dim commandstate As Integer
Dim stopvideo As Integer
Dim strNextString As String
Dim timeinitial As Variant
Dim timercount As Long


Private Sub Command1_Click()
    Winsock1.Connect
    
    Command1.Caption = "Small Delay"
    Command1.Enabled = False
    
    timeinitial = Time
    While Time - timeinitial < 0.000025
    Wend
    Command1.Caption = "Connect Camera"
    
    Command2.Enabled = True
    
    commandstate = 0
    stopvideo = 0
    Timer1.Interval = 1
    Timer1.Enabled = True
    
End Sub

Private Sub Command2_Click()
    stopvideo = 1
    Command1.Enabled = True
    Command2.Enabled = False
    
End Sub





Private Sub Form_Load()
  stopvideo = 0
  Timer1.Enabled = False

  timeinitial = Time

  timercount = 0
End Sub





Private Sub Form_Unload(Cancel As Integer)
    Winsock1.Close
    
End Sub



Private Sub Timer1_Timer()
    Dim index As Long
    
    timercount = timercount + 1
    If commandstate = 0 Then
        Label4.Caption = timercount
        datatosend = "I1"
        Winsock1.SendData (datatosend)
        commandstate = 1
    ElseIf commandstate = 1 Then
        If Winsock1.BytesReceived = 10000 Then

            Winsock1.GetData RecieveData, vbString

            For index = 0 To 9999
                PictureData(index) = Asc(Mid(RecieveData, index + 1, 1))
            Next index
            commandstate = 2
        End If
    ElseIf commandstate = 2 Then
        datatosend = "I2"
        Winsock1.SendData (datatosend)
        commandstate = 3
    ElseIf commandstate = 3 Then
        If Winsock1.BytesReceived = 10000 Then

            Winsock1.GetData RecieveData, vbString

            For index = 0 To 9999
                PictureData(index + 10000) = Asc(Mid(RecieveData, index + 1, 1))
            Next index
            commandstate = 4
        End If
    ElseIf commandstate = 4 Then
        datatosend = "I3"
        Winsock1.SendData (datatosend)
        commandstate = 5
    ElseIf commandstate = 5 Then
        If Winsock1.BytesReceived = 10000 Then

            Winsock1.GetData RecieveData, vbString

            For index = 0 To 9999
                PictureData(index + 20000) = Asc(Mid(RecieveData, index + 1, 1))
            Next index
            commandstate = 6
        End If
    ElseIf commandstate = 6 Then
        datatosend = "I4"
        Winsock1.SendData (datatosend)
        commandstate = 7
    ElseIf commandstate = 7 Then
        If Winsock1.BytesReceived = 10000 Then

            Winsock1.GetData RecieveData, vbString

            For index = 0 To 9999
                PictureData(index + 30000) = Asc(Mid(RecieveData, index + 1, 1))
            Next index
            commandstate = 8
        End If
    ElseIf commandstate = 8 Then
        datatosend = "I5"
        Winsock1.SendData (datatosend)
        commandstate = 9
    ElseIf commandstate = 9 Then
        If Winsock1.BytesReceived = 10000 Then

            Winsock1.GetData RecieveData, vbString

            For index = 0 To 9999
                PictureData(index + 40000) = Asc(Mid(RecieveData, index + 1, 1))
            Next index
            commandstate = 10
        End If
    ElseIf commandstate = 10 Then
        datatosend = "I6"
        Winsock1.SendData (datatosend)
        commandstate = 11
    ElseIf commandstate = 11 Then
        If Winsock1.BytesReceived = 10000 Then

            Winsock1.GetData RecieveData, vbString

            For index = 0 To 9999
                PictureData(index + 50000) = Asc(Mid(RecieveData, index + 1, 1))
            Next index
            commandstate = 12
        End If
    ElseIf commandstate = 12 Then
        datatosend = "I7"
        Winsock1.SendData (datatosend)
        commandstate = 13
    ElseIf commandstate = 13 Then
        If Winsock1.BytesReceived = 10000 Then

            Winsock1.GetData RecieveData, vbString

            For index = 0 To 9999
                PictureData(index + 60000) = Asc(Mid(RecieveData, index + 1, 1))
            Next index
            commandstate = 14
        End If
    ElseIf commandstate = 14 Then
        datatosend = "I8"
        Winsock1.SendData (datatosend)
        commandstate = 15
    ElseIf commandstate = 15 Then
        If Winsock1.BytesReceived = 6032 Then

            Winsock1.GetData RecieveData, vbString

            For index = 0 To 6031
                PictureData(index + 70000) = Asc(Mid(RecieveData, index + 1, 1))
            Next index

            Module1.ShowPic
            commandstate = 0
            If stopvideo = 1 Then
                stopvideo = 0
                Timer1.Enabled = False
                Winsock1.Close
            End If
        End If
    End If
    
End Sub


