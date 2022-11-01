Imports System.Net.Sockets
Imports System.Security.Cryptography

Public Class Form1

    Public Bitmap_Chart As Bitmap
    Public Bitmap_Histogram As Bitmap
    Public Graphics_Chart As Graphics
    Public Graphics_Histogram As Graphics
    Public r As New Random

    Dim MinX As Double = 0
    Dim MinY As Double = 0
    Public MaxX As Double = 100
    Public MaxY As Double = 100

    Sub InitializeGraphics()
        Me.Bitmap_Chart = New Bitmap(Me.PictureBox1.Width, Me.PictureBox1.Height)
        Me.Bitmap_Histogram = New Bitmap(Me.PictureBox2.Width, Me.PictureBox2.Height)
        Me.Graphics_Chart = Graphics.FromImage(Bitmap_Chart)
        Me.Graphics_Histogram = Graphics.FromImage(Bitmap_Histogram)
    End Sub

    Function GenerateListOfPoints(VirtualWindow As Rectangle) As Tuple(Of List(Of Point), List(Of Point), Integer)

        Dim TrialsCount As Integer = Me.TrackBar1.Value
        Dim SuccessChance As Double = Me.TrackBar3.Value / 100

        MaxX = TrialsCount
        MaxY = TrialsCount

        Dim PointsOfAbsoluteFrequency As New List(Of Point)
        Dim PointsOfRelativeFrequency As New List(Of Point)

        Dim LastY As Integer
        Dim Y As Double = 0
        For X As Integer = 1 To TrialsCount
            Dim RandomNumber As Double = r.NextDouble
            If RandomNumber < SuccessChance Then
                Y = Y + 1
            End If
            Dim X_Device_Absolute As Integer = FromXWorldToXVirtual(X, MinX, MaxX, VirtualWindow.Left, VirtualWindow.Width)
            Dim Y_Device_Absolute As Integer = FromYWorldToYVirtual(Y, MinY, MaxY, VirtualWindow.Top, VirtualWindow.Height)
            PointsOfAbsoluteFrequency.Add(New Point(X_Device_Absolute, Y_Device_Absolute))

            Dim X_Device_Relative As Integer = FromXWorldToXVirtual(X, MinX, MaxX, VirtualWindow.Left, VirtualWindow.Width)
            Dim Y_Device_Relative As Integer = FromYWorldToYVirtual_Relative(Y / X, VirtualWindow.Top, VirtualWindow.Height)
            PointsOfRelativeFrequency.Add(New Point(X_Device_Relative, Y_Device_Relative))

            If X = TrialsCount Then
                LastY = Y
            End If
        Next
        Return New Tuple(Of List(Of Point), List(Of Point), Integer)(PointsOfAbsoluteFrequency, PointsOfRelativeFrequency, LastY)
    End Function

    Sub Print_Chart_Absolute()
        Me.PictureBox1.Image = Bitmap_Chart
    End Sub

    Sub Print_Chart_Relative()
        Me.PictureBox2.Image = Bitmap_Histogram
    End Sub

    Sub Print_Histogram()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.InitializeGraphics()

        Dim VirtualWindow_Chart As New Rectangle(25, 25, Me.Bitmap_Chart.Width - 50, Me.Bitmap_Chart.Height - 50)
        Dim VirtualWindow_Histogram As New Rectangle(25, 25, Me.Bitmap_Histogram.Width - 50, Me.Bitmap_Histogram.Height - 50)

        Graphics_Chart.DrawRectangle(Pens.Black, VirtualWindow_Chart)
        Graphics_Histogram.DrawRectangle(Pens.Black, VirtualWindow_Histogram)

        Dim ExperimentsCount As Integer = Me.TrackBar2.Value

        Dim DictionaryOfLastY As New SortedDictionary(Of Integer, Integer)

        Dim BrushColor As Brush = Brushes.Red

        For Experiment As Integer = 1 To ExperimentsCount
            Dim AllPoints As Tuple(Of List(Of Point), List(Of Point), Integer) = Me.GenerateListOfPoints(VirtualWindow_Chart)

            If Me.CheckBox1.Checked Then
                Graphics_Chart.DrawLines(Pens.Red, AllPoints.Item1.ToArray)
            Else
                BrushColor = Brushes.Green
            End If
            If Me.CheckBox2.Checked Then
                Graphics_Chart.DrawLines(Pens.Green, AllPoints.Item2.ToArray)
            End If



            Dim LastY As Integer = AllPoints.Item3


            If DictionaryOfLastY.ContainsKey(LastY) Then
                'Me.RichTextBox2.AppendText("YES" & vbCrLf)
                DictionaryOfLastY(LastY) += 1
            Else
                'Me.RichTextBox2.AppendText("NO" & vbCrLf)
                DictionaryOfLastY.Add(LastY, 1)
            End If
        Next

        Me.RichTextBox1.Clear()
        For Each kvp As KeyValuePair(Of Integer, Integer) In DictionaryOfLastY
            Me.RichTextBox1.AppendText(kvp.Key & ": " & kvp.Value & vbCrLf)

            Dim CountWidth As Integer = kvp.Value * 5
            Dim HeightPosition As Integer = FromYWorldToYVirtual(kvp.Key, MinY, MaxY, VirtualWindow_Histogram.Top, VirtualWindow_Histogram.Height)

            Graphics_Histogram.FillRectangle(BrushColor, New Rectangle(50, HeightPosition, CountWidth, 2))
        Next

        Me.Print_Chart_Absolute()
        Me.Print_Chart_Relative()

    End Sub

    Function FromXWorldToXVirtual(X As Double, MinX As Double, MaxX As Double, Left As Integer, W As Integer) As Integer

        If (MaxX - MinX) = 0 Then Return 0
        Return Left + W * (X - MinX) / (MaxX - MinX)

    End Function

    Function FromYWorldToYVirtual(Y As Double, MinY As Double, MaxY As Double, Top As Integer, H As Integer) As Integer

        If (MaxY - MinY) = 0 Then Return 0
        Return Top + H - H * (Y - MinY) / (MaxY - MinY)

    End Function

    Function FromYWorldToYVirtual_Relative(Y As Double, Top As Integer, H As Integer) As Integer

        Return Top + H - H * Y

    End Function

    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll
        Me.TextBox1.Text = "TrialsCount: " & Me.TrackBar1.Value
    End Sub

    Private Sub TrackBar2_Scroll(sender As Object, e As EventArgs) Handles TrackBar2.Scroll
        Me.TextBox2.Text = "ExperimentsCount: " & Me.TrackBar2.Value
    End Sub

    Private Sub TrackBar3_Scroll(sender As Object, e As EventArgs) Handles TrackBar3.Scroll
        Me.TextBox3.Text = "SuccessChance: " & Me.TrackBar3.Value / 100
    End Sub

End Class
