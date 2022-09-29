Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Me.RichTextBox1.BackColor = Color.Orange Then
            Me.RichTextBox1.BackColor = Color.Red
        Else
            Me.RichTextBox1.BackColor = Color.Orange
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If Me.RichTextBox1.ForeColor = Color.Green Then
            Me.RichTextBox1.ForeColor = Color.Blue
        Else
            Me.RichTextBox1.ForeColor = Color.Green
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Me.RichTextBox1.ForeColor = Color.Black
        Me.RichTextBox1.BackColor = Color.White
    End Sub
End Class
