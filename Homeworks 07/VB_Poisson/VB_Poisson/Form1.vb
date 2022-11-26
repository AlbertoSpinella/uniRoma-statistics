Imports System.Configuration
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

    Sub AddCountArrivals(CountArrivals As Integer, IntervalSize As Integer, ListOfCountArrivals As SortedDictionary(Of Interval, Integer))
        'Me.RichTextBox3.AppendText(CountArrivals & vbCrLf)

        Dim StartingEndPoint As Double = 0

        Dim Interval_0 As New Interval
        Interval_0.LowerEnd = StartingEndPoint
        Interval_0.UpperEnd = Interval_0.LowerEnd + IntervalSize

        Dim ListOfIntervals As New List(Of Interval)
        ListOfIntervals.Add(Interval_0)

        Dim IntervalWhereTheValueFalls As Interval = Me.FindIntervalForValue(CountArrivals, IntervalSize, ListOfIntervals)

        If ListOfCountArrivals.ContainsKey(IntervalWhereTheValueFalls) Then
            ListOfCountArrivals(IntervalWhereTheValueFalls) += CountArrivals
        Else
            ListOfCountArrivals.Add(IntervalWhereTheValueFalls, CountArrivals)
        End If
    End Sub

    Function FindIntervalForValue(v As Double, IntervalSize As Double, ByRef ListOfIntervals As List(Of Interval)) As Interval

        For Each Interval In ListOfIntervals

            If Interval.ContainsValue(v) Then Return Interval
        Next

        If v <= ListOfIntervals(0).LowerEnd Then
            Do
                Dim NewLeftInterval As New Interval
                NewLeftInterval.UpperEnd = ListOfIntervals(0).LowerEnd
                NewLeftInterval.LowerEnd = NewLeftInterval.UpperEnd - IntervalSize

                ListOfIntervals.Insert(0, NewLeftInterval)

                If NewLeftInterval.ContainsValue(v) Then Return NewLeftInterval
            Loop

        ElseIf v > ListOfIntervals(ListOfIntervals.Count - 1).UpperEnd Then
            Do
                Dim NewRightInterval As New Interval
                NewRightInterval.LowerEnd = ListOfIntervals(ListOfIntervals.Count - 1).UpperEnd
                NewRightInterval.UpperEnd = NewRightInterval.LowerEnd + IntervalSize

                ListOfIntervals.Add(NewRightInterval)

                If NewRightInterval.ContainsValue(v) Then Return NewRightInterval
            Loop
        Else
            Throw New Exception("Not expected occurence")
        End If
    End Function

    Function GenerateListOfPoints(VirtualWindow As Rectangle, ListOfCountArrivals As SortedDictionary(Of Interval, Integer), IntervalSize As Integer) As Tuple(Of List(Of Point))

        Dim TrialsCount As Integer = Me.TrackBar1.Value
        Dim SuccessChance As Double = Me.TrackBar3.Value / Me.TrackBar1.Value

        MaxX = TrialsCount
        MaxY = TrialsCount

        Dim PointsOfAbsoluteFrequency As New List(Of Point)

        Dim CountArrivals As Integer = 0

        Dim Y As Double = 0
        For X As Integer = 1 To TrialsCount
            Dim RandomNumber As Double = r.NextDouble
            If RandomNumber < SuccessChance Then
                Y = Y + 1
                Me.AddCountArrivals(CountArrivals, IntervalSize, ListOfCountArrivals)
                CountArrivals = 0
            Else
                CountArrivals += 1
            End If
            Dim X_Device_Absolute As Integer = FromXWorldToXVirtual(X, MinX, MaxX, VirtualWindow.Left, VirtualWindow.Width)
            Dim Y_Device_Absolute As Integer = FromYWorldToYVirtual(Y, MinY, MaxY, VirtualWindow.Top, VirtualWindow.Height)
            PointsOfAbsoluteFrequency.Add(New Point(X_Device_Absolute, Y_Device_Absolute))

        Next
        Return New Tuple(Of List(Of Point))(PointsOfAbsoluteFrequency)
    End Function



    Sub PlotHistogram(ListOfCountArrivals As SortedDictionary(Of Interval, Integer), VirtualWindow_Histogram As Rectangle, ArrivalsCount As Integer, IntervalSize As Integer)

        If ListOfCountArrivals.Count = 0 Then
            Exit Sub
        End If

        Dim First As Double = ListOfCountArrivals.Keys.First().LowerEnd
        Dim Last As Double = ListOfCountArrivals.Keys.Last().UpperEnd

        Dim MaxElement As Double = 0
        Dim FirstLowerEnd As Double = ListOfCountArrivals.Keys.First().LowerEnd
        Dim LastUpperEnd As Double = ListOfCountArrivals.Keys.Last().UpperEnd

        For Each kvp As KeyValuePair(Of Interval, Integer) In ListOfCountArrivals
            If kvp.Value > MaxElement Then
                MaxElement = kvp.Value
            End If
        Next


        For Each kvp As KeyValuePair(Of Interval, Integer) In ListOfCountArrivals

            Dim X As Double = VirtualWindow_Histogram.Location.X + VirtualWindow_Histogram.Width * ((kvp.Key.LowerEnd - FirstLowerEnd) / (LastUpperEnd - FirstLowerEnd))
            Dim Y As Double = VirtualWindow_Histogram.Location.Y + VirtualWindow_Histogram.Height * (1 - (kvp.Value / MaxElement))
            Dim W As Double = VirtualWindow_Histogram.Width * (1 / ArrivalsCount)
            Dim H As Double = VirtualWindow_Histogram.Height * (kvp.Value / MaxElement)

            If W < 2 Then
                W = 2
            End If

            Graphics_Histogram.DrawRectangle(Pens.Black, New Rectangle(X, Y, W, H))
            Graphics_Histogram.FillRectangle(Brushes.Cyan, New Rectangle(X + 1, Y + 1, W - 1, H - 1))
        Next
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.InitializeGraphics()
        Me.RichTextBox1.Clear()

        Dim VirtualWindow_Chart As New Rectangle(25, 25, Me.Bitmap_Chart.Width - 50, Me.Bitmap_Chart.Height - 50)
        Dim VirtualWindow_Histogram As New Rectangle(25, 25, Me.Bitmap_Histogram.Width - 50, Me.Bitmap_Histogram.Height - 50)

        Graphics_Chart.DrawRectangle(Pens.Black, VirtualWindow_Chart)
        Graphics_Histogram.DrawRectangle(Pens.Black, VirtualWindow_Histogram)

        Dim ExperimentsCount As Integer = Me.TrackBar2.Value
        Dim IntervalSize As Integer = Me.TrackBar4.Value * 2

        Dim ListOfCountArrivals As New SortedDictionary(Of Interval, Integer)

        Dim DictionaryOfLastY As New SortedDictionary(Of Integer, Integer)

        Dim BrushColor As Brush = Brushes.Red

        For Experiment As Integer = 1 To ExperimentsCount
            Dim AllPoints As Tuple(Of List(Of Point)) = Me.GenerateListOfPoints(VirtualWindow_Chart, ListOfCountArrivals, IntervalSize)

            Graphics_Chart.DrawLines(Pens.Red, AllPoints.Item1.ToArray)
        Next

        For Each kvp As KeyValuePair(Of Interval, Integer) In ListOfCountArrivals
            Me.RichTextBox1.AppendText(kvp.Key.ToString() & ": " & kvp.Value & vbCrLf)
        Next

        Me.PlotHistogram(ListOfCountArrivals, VirtualWindow_Histogram, ListOfCountArrivals.Count, IntervalSize)

        Me.PictureBox1.Image = Bitmap_Chart
        Me.PictureBox2.Image = Bitmap_Histogram

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
        Me.TrackBar3.Maximum = Me.TrackBar1.Value
        If Me.TrackBar3.Value = Me.TrackBar3.Maximum Then
            Me.TextBox3.Text = "Lambda: " & Me.TrackBar3.Value
        End If
        Me.TrackBar4.Maximum = Math.Floor(Me.TrackBar1.Value / 10)
        If Me.TrackBar1.Value / (Me.TrackBar4.Value * 2) < 5 Then
            Me.TrackBar4.Value = Me.TrackBar4.Maximum
        End If
        Me.TextBox4.Text = "IntervalSize: " & Me.TrackBar4.Value * 2
    End Sub

    Private Sub TrackBar2_Scroll(sender As Object, e As EventArgs) Handles TrackBar2.Scroll
        Me.TextBox2.Text = "ExperimentsCount: " & Me.TrackBar2.Value
    End Sub

    Private Sub TrackBar3_Scroll(sender As Object, e As EventArgs) Handles TrackBar3.Scroll
        Me.TextBox3.Text = "Lambda: " & Me.TrackBar3.Value
    End Sub

    Private Sub TrackBar4_Scroll(sender As Object, e As EventArgs) Handles TrackBar4.Scroll
        Me.TextBox4.Text = "IntervalSize: " & Me.TrackBar4.Value * 2
    End Sub

End Class
