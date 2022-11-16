﻿Public Class Form1

    Public Bitmap_Histogram As Bitmap
    Public Graphics_Histogram As Graphics
    Public r As New Random

    Dim MinX As Double = 0
    Dim MinY As Double = 0
    Public MaxX As Double = 100
    Public MaxY As Double = 100
    'Sub InitializeGraphics()
    '    Me.Bitmap_Histogram = New Bitmap(Me.PictureBox1.Width, Me.PictureBox1.Height)
    '    Me.Graphics_Histogram = Graphics.FromImage(Bitmap_Histogram)
    'End Sub

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

    Sub PrintNumbers(ExperimentCount As Integer, Numbers As SortedDictionary(Of Integer, Integer))
        Me.RichTextBox1.AppendText("Experiment " & ExperimentCount & vbCrLf & "---------------------------" & vbCrLf)
        For Each kvp As KeyValuePair(Of Integer, Integer) In Numbers
            Me.RichTextBox1.AppendText(kvp.Key & ": " & kvp.Value & vbCrLf)
        Next
        Me.RichTextBox1.AppendText(vbCrLf)
    End Sub

    Function CalculateMeanOfIntegers(Numbers As SortedDictionary(Of Integer, Integer)) As Double
        Dim CurrentMean As Double = 0
        Dim CurrentMeanCount As Integer = 0

        For Each kvp As KeyValuePair(Of Integer, Integer) In Numbers
            CurrentMeanCount += kvp.Value
            CurrentMean += kvp.Key * kvp.Value
        Next

        Return CurrentMean / CurrentMeanCount
    End Function

    Function CalculateVarianceOfIntegers(Numbers As SortedDictionary(Of Integer, Integer), Mean As Double) As Double
        Dim CurrentVariance As Double = 0
        Dim CurrentVarianceCount As Integer = 0

        For Each kvp As KeyValuePair(Of Integer, Integer) In Numbers
            CurrentVarianceCount += kvp.Value
            Dim Square As Double = (kvp.Key - Mean) ^ 2
            CurrentVariance += Square * kvp.Value
        Next

        Return CurrentVariance / CurrentVarianceCount
    End Function

    Function CalculateMeanOfDoubles(Numbers As SortedDictionary(Of Double, Integer)) As Double
        Dim CurrentMean As Double = 0
        Dim CurrentMeanCount As Integer = 0

        For Each kvp As KeyValuePair(Of Double, Integer) In Numbers
            CurrentMeanCount += kvp.Value
            CurrentMean += kvp.Key * kvp.Value
        Next

        Return CurrentMean / CurrentMeanCount
    End Function

    Function CalculateVarianceOfDoubles(Numbers As SortedDictionary(Of Double, Integer), Mean As Double) As Double
        Dim CurrentVariance As Double = 0
        Dim CurrentVarianceCount As Integer = 0

        For Each kvp As KeyValuePair(Of Double, Integer) In Numbers
            CurrentVarianceCount += kvp.Value
            Dim Square As Double = (kvp.Key - Mean) ^ 2
            CurrentVariance += Square * kvp.Value
        Next

        Return CurrentVariance / CurrentVarianceCount
    End Function

    Sub PrintMeans(ListOfMeans As SortedDictionary(Of Double, Integer))
        Me.RichTextBox2.Clear()
        Me.RichTextBox2.AppendText("List of means" & vbCrLf)
        For Each kvp As KeyValuePair(Of Double, Integer) In ListOfMeans
            Me.RichTextBox2.AppendText(kvp.Key & ": " & kvp.Value & vbCrLf)
        Next
        Me.RichTextBox2.AppendText(vbCrLf)
    End Sub

    Sub PrintVariances(ListOfVariances As SortedDictionary(Of Double, Integer))
        Me.RichTextBox5.Clear()
        Me.RichTextBox5.AppendText("List of variances" & vbCrLf)
        For Each kvp As KeyValuePair(Of Double, Integer) In ListOfVariances
            Me.RichTextBox5.AppendText(kvp.Key & ": " & kvp.Value & vbCrLf)
        Next
        Me.RichTextBox5.AppendText(vbCrLf)
    End Sub

    Sub PrintMeanOfMeans(MeanOfMeans As Double)
        Me.RichTextBox3.Clear()
        Me.RichTextBox3.AppendText("Mean of means: " & MeanOfMeans & vbCrLf)
    End Sub

    Sub PrintMeanOfVariances(MeanOfVariances As Double)
        Me.RichTextBox6.Clear()
        Me.RichTextBox6.AppendText("Mean of variances: " & MeanOfVariances & vbCrLf)
    End Sub

    Sub PrintVarianceOfMeans(VarianceOfMeans As Double)
        Me.RichTextBox4.Clear()
        Me.RichTextBox4.AppendText("Variance of means: " & VarianceOfMeans & vbCrLf)
    End Sub

    Sub PrintVarianceOfVariances(VarianceOfVariances As Double)
        Me.RichTextBox7.Clear()
        Me.RichTextBox7.AppendText("Variance of variances: " & VarianceOfVariances & vbCrLf)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'Me.InitializeGraphics()

        'Dim VirtualWindow_Histogram As New Rectangle(25, 25, Me.Bitmap_Histogram.Width - 50, Me.Bitmap_Histogram.Height - 50)
        'Graphics_Histogram.DrawRectangle(Pens.Black, VirtualWindow_Histogram)

        Me.RichTextBox1.Clear()

        Dim Quantity As Integer = Me.TrackBar1.Value
        Dim Min As Integer = 1
        Dim Max As Integer = Me.TrackBar2.Value
        Dim ExperimentsCount As Integer = Me.TrackBar3.Value

        Dim ListOfMeans As New SortedDictionary(Of Double, Integer)
        Dim ListOfVariances As New SortedDictionary(Of Double, Integer)

        For Experiment As Integer = 1 To ExperimentsCount
            Dim RandomNumbers As SortedDictionary(Of Integer, Integer) = Me.CreateRandomNumbers(Quantity, Min, Max + 1)
            If Me.CheckBox1.Checked Then
                Me.PrintNumbers(Experiment, RandomNumbers)
            End If

            Dim Mean As Double = Me.CalculateMeanOfIntegers(RandomNumbers)
            Dim Variance As Double = Me.CalculateVarianceOfIntegers(RandomNumbers, Mean)

            If ListOfMeans.ContainsKey(Mean) Then
                ListOfMeans(Mean) += 1
            Else
                ListOfMeans.Add(Mean, 1)
            End If

            If ListOfVariances.ContainsKey(Variance) Then
                ListOfVariances(Variance) += 1
            Else
                ListOfVariances.Add(Variance, 1)
            End If

        Next

        Me.PrintMeans(ListOfMeans)
        Me.PrintVariances(ListOfVariances)

        Dim MeanOfMeans As Double = Me.CalculateMeanOfDoubles(ListOfMeans)
        Dim MeanOfVariances As Double = Me.CalculateMeanOfDoubles(ListOfVariances)
        Dim VarianceOfMeans As Double = Me.CalculateVarianceOfDoubles(ListOfMeans, MeanOfMeans)
        Dim VarianceOfVariances As Double = Me.CalculateVarianceOfDoubles(ListOfVariances, MeanOfVariances)
        Me.PrintMeanOfMeans(MeanOfMeans)
        Me.PrintMeanOfVariances(MeanOfVariances)
        Me.PrintVarianceOfMeans(VarianceOfMeans)
        Me.PrintVarianceOfVariances(VarianceOfVariances)

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

    Private Sub TrackBar2_Scroll(sender As Object, e As EventArgs) Handles TrackBar2.Scroll
        Me.TextBox2.Text = "Interval: [1 ; " & Me.TrackBar2.Value & "]"
    End Sub

    Private Sub TrackBar3_Scroll(sender As Object, e As EventArgs) Handles TrackBar3.Scroll
        Me.TextBox3.Text = "ExperimentsCount: " & Me.TrackBar3.Value
    End Sub

End Class
