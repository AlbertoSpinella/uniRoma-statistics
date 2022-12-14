Public Class Form1

    Public b As Bitmap
    Public g As Graphics

    Public Viewport As New Rectangle(25, 25, 250, 250)
    Public MinX_Window As Double = -250
    Public MaxX_Window As Double = 250
    Public MinY_Window As Double = -250
    Public MaxY_Window As Double = 250
    Public RangeX As Double = MaxX_Window - MinX_Window
    Public RangeY As Double = MaxY_Window - MinY_Window

    Sub InitializeGraphics()
        Me.b = New Bitmap(Me.PictureBox1.Width, Me.PictureBox1.Height)
        Me.g = Graphics.FromImage(b)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.InitializeGraphics()

        g.Clear(Color.White)

        Me.g.DrawRectangle(Pens.Red, Viewport)

        Me.PictureBox1.Image = b
    End Sub

    Sub DrawScene()

        g.Clear(Color.White)
        Me.g.DrawRectangle(Pens.Red, Viewport)
        Me.PictureBox1.Image = b

    End Sub

    Function X_Viewport(X_World As Double, Viewport As Rectangle, MinX As Double, RangeX As Double) As Integer
        Return CInt(Viewport.Left + Viewport.Width * (X_World - MinX) / RangeX)
    End Function

    Function Y_Viewport(Y_World As Double, Viewport As Rectangle, MinY As Double, RangeY As Double) As Integer
        Return CInt(Viewport.Top + Viewport.Height - Viewport.Height * (Y_World - MinY) / RangeY)
    End Function

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Application.CurrentCulture = Globalization.CultureInfo.InvariantCulture
    End Sub

    Private Viewport_At_Mouse_Down As Rectangle
    Private MouseLocation_At_MouseDown As Point

    Private Dragging_Started As Boolean
    Private Resizing_Started As Boolean

    Public MinX_Window_At_Mouse_Down As Double
    Public MaxX_Window_At_Mouse_Down As Double
    Public MinY_Window_At_Mouse_Down As Double
    Public MaxY_Window_At_Mouse_Down As Double
    Public RangeX_At_Mouse_Down As Double
    Public RangeY_At_Mouse_Down As Double

    Private Sub PictureBox1_MouseDown(sender As Object, e As MouseEventArgs) Handles PictureBox1.MouseDown

        If Me.Viewport.Contains(e.X, e.Y) Then

            If e.Button = Windows.Forms.MouseButtons.Left Then

                Me.Viewport_At_Mouse_Down = Me.Viewport
                Me.MouseLocation_At_MouseDown = New Point(e.X, e.Y)

                Me.MinX_Window_At_Mouse_Down = Me.MinX_Window
                Me.MaxX_Window_At_Mouse_Down = Me.MaxX_Window
                Me.MinY_Window_At_Mouse_Down = Me.MinY_Window
                Me.MaxY_Window_At_Mouse_Down = Me.MaxY_Window
                Me.RangeX_At_Mouse_Down = Me.RangeX
                Me.RangeY_At_Mouse_Down = Me.RangeY

                Me.Dragging_Started = True
            ElseIf e.Button = Windows.Forms.MouseButtons.Right Then
                Me.Resizing_Started = True
            End If

        End If

    End Sub

    Private Sub PictureBox1_MouseEnter(sender As Object, e As EventArgs) Handles PictureBox1.MouseEnter
        Me.PictureBox1.Focus()
    End Sub

    Private Sub PictureBox1_MouseMove(sender As Object, e As MouseEventArgs) Handles PictureBox1.MouseMove

        If Me.Dragging_Started Then
            Dim Delta_X As Integer = e.X - Me.MouseLocation_At_MouseDown.X
            Dim Delta_Y As Integer = e.Y - Me.MouseLocation_At_MouseDown.Y

            Me.Viewport.X = Me.Viewport_At_Mouse_Down.X + Delta_X
            Me.Viewport.Y = Me.Viewport_At_Mouse_Down.Y + Delta_Y

            Me.DrawScene()

        ElseIf Me.Resizing_Started Then
            Dim Delta_X As Integer = e.X - Me.MouseLocation_At_MouseDown.X
            Dim Delta_Y As Integer = e.Y - Me.MouseLocation_At_MouseDown.Y

            Me.Viewport.Width = Me.Viewport_At_Mouse_Down.Width + Delta_X
            Me.Viewport.Height = Me.Viewport_At_Mouse_Down.Height + Delta_Y

            Me.DrawScene()
        End If

    End Sub

    Private Sub PictureBox1_MouseUp(sender As Object, e As MouseEventArgs) Handles PictureBox1.MouseUp
        Me.Dragging_Started = False
        Me.Resizing_Started = False
    End Sub

    Private Sub PictureBox1_MouseWheel(sender As Object, e As MouseEventArgs) Handles PictureBox1.MouseWheel

        Dim Change_X As Integer = CInt(Me.Viewport.Width / 10)
        Dim Change_Y As Integer = CInt(Me.Viewport.Height * Change_X / Me.Viewport.Width)

        If e.Delta > 0 Then

            Me.Viewport.X -= Change_X
            Me.Viewport.Width += 2 * Change_X

            Me.Viewport.Y -= Change_Y
            Me.Viewport.Height += 2 * Change_Y

            Me.DrawScene()

        ElseIf e.Delta < 0 Then

            Me.Viewport.X += Change_X
            Me.Viewport.Width -= 2 * Change_X

            Me.Viewport.Y += Change_Y
            Me.Viewport.Height -= 2 * Change_Y

            Me.DrawScene()

        End If

    End Sub

End Class
