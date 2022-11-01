Imports System.Net.Sockets

Public Class Form1

    Public b As Bitmap
    Public g As Graphics
    Public r As New Random

    Sub InitializeGraphics()
        Me.b = New Bitmap(Me.PictureBox1.Width, Me.PictureBox1.Height)
        Me.g = Graphics.FromImage(b)
        Me.g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        Me.g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
    End Sub

    Function GenerateListOfPoints(VirtualWindow As Rectangle) As Tuple(Of List(Of Point), List(Of Point))

        Dim TrialsCount As Integer = Me.TrackBar1.Value
        Dim SuccessChance As Double = Me.TrackBar3.Value / 100

        Dim MinX As Double = 0
        Dim MaxX As Double = TrialsCount
        Dim MinY As Double = 0
        Dim MaxY As Double = TrialsCount

        Dim PointsOfAbsoluteFrequency As New List(Of Point)
        Dim PointsOfRelativeFrequency As New List(Of Point)

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

        Next
        Return New Tuple(Of List(Of Point), List(Of Point))(PointsOfAbsoluteFrequency, PointsOfRelativeFrequency)
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.InitializeGraphics()

        Dim VirtualWindow As New Rectangle(25, 25, Me.b.Width - 50, Me.b.Height - 50)

        g.DrawRectangle(Pens.Black, VirtualWindow)

        Dim ExperimentsCount As Integer = Me.TrackBar2.Value

        For Experiment As Integer = 1 To ExperimentsCount
            Dim AllPoints As Tuple(Of List(Of Point), List(Of Point)) = Me.GenerateListOfPoints(VirtualWindow)


            g.DrawLines(Pens.Red, AllPoints.Item1.ToArray)
            g.DrawLines(Pens.Green, AllPoints.Item2.ToArray)
        Next

        Me.PictureBox1.Image = b
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
