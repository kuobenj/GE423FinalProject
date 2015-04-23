Attribute VB_Name = "Module1"
Option Explicit

Type SAFEARRAYBOUND
    cElements As Long
    lLbound As Long
End Type

Type SAFEARRAY2D
    cDims As Integer
    fFeatures As Integer
    cbElements As Long
    cLocks As Long
    pvData As Long
    Bounds(0 To 1) As SAFEARRAYBOUND
End Type

Type BITMAP
    bmType As Long
    bmWidth As Long
    bmHeight As Long
    bmWidthBytes As Long
    bmPlanes As Integer
    bmBitsPixel As Integer
    bmBits As Long
End Type

Declare Function VarPtrArray Lib "msvbvm50.dll" Alias "VarPtr" (Ptr() As Any) As Long
Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As Any, pSrc As Any, ByVal ByteLen As Long)
Declare Function GetObjectAPI Lib "gdi32" Alias "GetObjectA" (ByVal hObject As Long, ByVal nCount As Long, lpObject As Any) As Long

Dim pic() As Byte

Dim sa As SAFEARRAY2D

Dim bmp As BITMAP


Dim r As Long, g As Long, b As Long

'Public mypicbuffred(360, 360) As Byte  ' Give myself extra memory,  because I can :)
'Public mypicbuffgreen(360, 360) As Byte  ' Give myself extra memory,  because I can :)
'Public mypicbuffblue(360, 360) As Byte  ' Give myself extra memory,  because I can :)


Public PictureData(76032) As Byte


Public Sub ShowPic()
    Dim i As Long
    Dim j As Long
    Dim k As Long
    Dim l As Long
    Dim x As Long
    Dim y As Long
    Dim multiplier As Long
    
    GetObjectAPI Form1.Image1.Picture, Len(bmp), bmp

    With sa
        .cbElements = 1
        .cDims = 2
        .Bounds(0).lLbound = 0
        .Bounds(0).cElements = bmp.bmHeight
        .Bounds(1).lLbound = 0
        .Bounds(1).cElements = bmp.bmWidthBytes
        .pvData = bmp.bmBits
    End With
        
    CopyMemory ByVal VarPtrArray(pic), VarPtr(sa), 4
        
    For i = 0 To 143
       For j = 0 To 175
                'pic((3 * j), i) = mypicbuffblue(71 - i, 87 - j) 'blue
                'pic(((3 * j) + 1), i) = mypicbuffgreen(71 - i, 87 - j) 'green
                'pic(((3 * j) + 2), i) = mypicbuffred(71 - i, 87 - j) 'red
                pic((3 * j), i) = PictureData(i * (176 * 3) + 3 * j) 'blue
                pic(((3 * j) + 1), i) = PictureData(i * (176 * 3) + 3 * j + 1) 'green
                pic(((3 * j) + 2), i) = PictureData(i * (176 * 3) + 3 * j + 2) 'red
        Next j
    Next i
        
    CopyMemory ByVal VarPtrArray(pic), 0&, 4
    
    Form1.Image1.Refresh

End Sub

