Imports System.Diagnostics.Eventing.Reader

Public Class Form1

    Public RandomNumber As New Random
    Public Occurencies1 As Integer = 0
    Public Occurencies2 As Integer = 0
    Public Occurencies3 As Integer = 0
    Public Occurencies4 As Integer = 0
    Public Occurencies5 As Integer = 0
    Public Occurencies6 As Integer = 0
    Public TotalOccurencies As Integer = 0

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Timer1.Interval = 1000
        Me.Timer1.Start()
        Me.Button3.Visible = False
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Timer1.Stop()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Me.Timer1.Interval = 10
        Me.Timer1.Start()
        Me.Button1.Visible = False
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If Me.Timer1.Interval = 10 And TotalOccurencies > 999 Then
            Me.Timer1.Stop()
        Else
            Dim RandomInRange As Integer = RandomNumber.Next(1, 7)
            Me.RichTextBox2.AppendText(RandomInRange & Environment.NewLine)

            TotalOccurencies += 1
            Me.RichTextBox9.Text = TotalOccurencies

            Select Case RandomInRange
                Case 1
                    Occurencies1 += 1
                    Me.RichTextBox3.Text = Occurencies1
                Case 2
                    Occurencies2 += 1
                    Me.RichTextBox4.Text = Occurencies2
                Case 3
                    Occurencies3 += 1
                    Me.RichTextBox5.Text = Occurencies3
                Case 4
                    Occurencies4 += 1
                    Me.RichTextBox6.Text = Occurencies4
                Case 5
                    Occurencies5 += 1
                    Me.RichTextBox7.Text = Occurencies5
                Case 6
                    Occurencies6 += 1
                    Me.RichTextBox8.Text = Occurencies6
                Case Else
                    Me.Timer1.Stop()
                    Me.RichTextBox1.Text = "Something went wrong!"
            End Select
        End If
    End Sub
End Class
