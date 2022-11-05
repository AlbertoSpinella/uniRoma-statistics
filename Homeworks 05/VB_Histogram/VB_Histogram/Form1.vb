Public Class Form1

    Public Bitmap_Histogram As Bitmap
    Public Graphics_Histogram As Graphics
    Public r As New Random

    Public MinX As Integer = 0
    Public MaxX As Integer = 1

    Public MinY As Integer = 0
    Public MaxY As Integer = 11

    Sub InitializeGraphics()
        Me.Bitmap_Histogram = New Bitmap(Me.PictureBox1.Width, Me.PictureBox1.Height)
        Me.Graphics_Histogram = Graphics.FromImage(Bitmap_Histogram)
    End Sub

    Function CreateRandomNumbers(Quantity As Integer, Min As Integer, Max As Integer) As SortedDictionary(Of Integer, Integer)
        Dim RandomNumbers As New SortedDictionary(Of Integer, Integer)
        For i As Integer = 1 To Quantity
            Dim Number As Integer = r.Next(Min, Max)

            If RandomNumbers.ContainsKey(Number) Then
                RandomNumbers(Number) += 1
            Else
                RandomNumbers.Add(Number, 1)
            End If
        Next
        Return RandomNumbers
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.InitializeGraphics()
        Me.RichTextBox1.Clear()

        Dim VirtualWindow_Histogram As New Rectangle(25, 25, Me.Bitmap_Histogram.Width - 50, Me.Bitmap_Histogram.Height - 50)
        Graphics_Histogram.DrawRectangle(Pens.Black, VirtualWindow_Histogram)

        Dim Quantity As Integer = Me.TrackBar1.Value
        Dim Min As Integer = 1
        Dim Max As Integer = 10
        Dim RandomNumbers As SortedDictionary(Of Integer, Integer) = CreateRandomNumbers(Quantity, Min, Max + 1)

        For Each kvp As KeyValuePair(Of Integer, Integer) In RandomNumbers
            Me.RichTextBox1.AppendText(kvp.Key & ": " & kvp.Value & vbCrLf)

            If Me.RadioButton1.Checked Then
                Dim CountWidth As Integer = kvp.Value * 10
                Dim HeightPosition As Integer = FromYWorldToYVirtual(kvp.Key, VirtualWindow_Histogram.Top, VirtualWindow_Histogram.Height, Max)

                Graphics_Histogram.FillRectangle(Brushes.Red, New Rectangle(50, HeightPosition, CountWidth, 2))
            ElseIf Me.RadioButton2.Checked Then
                Dim CountHeight As Integer = kvp.Value * 10
                Dim WidthPosition As Integer = FromXWorldToXVirtual(kvp.Key, VirtualWindow_Histogram.Left, VirtualWindow_Histogram.Width, Max)

                Graphics_Histogram.FillRectangle(Brushes.Blue, New Rectangle(WidthPosition, VirtualWindow_Histogram.Height + 5 - CountHeight, 2, CountHeight))
            Else
                Me.RichTextBox1.BackColor = Color.Red
                Me.RichTextBox1.Text = "Unexpected error"
            End If
        Next

        Me.PictureBox1.Image = Bitmap_Histogram

    End Sub


    Function FromXWorldToXVirtual(X As Double, Left As Integer, W As Integer, Max As Integer) As Integer

        Return ((Left + W) / Max) * X - 10

    End Function

    Function FromYWorldToYVirtual(Y As Double, Top As Integer, H As Integer, Max As Integer) As Integer

        Return ((Top + H) / Max) * Y - 5

    End Function

    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll
        Me.TextBox1.Text = "Quantity: " & Me.TrackBar1.Value
    End Sub

End Class
