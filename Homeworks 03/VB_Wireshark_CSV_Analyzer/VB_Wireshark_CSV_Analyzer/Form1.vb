Imports System.Diagnostics.Metrics
Imports System.Reflection
Imports Microsoft.VisualBasic.FileIO

Public Class Form1

    Function ParseFile(Path As String) As List(Of Packet)
        Dim ListOfUnits As New List(Of Packet)

        Using R As New TextFieldParser(Path)
            R.Delimiters = New String() {","}
            R.HasFieldsEnclosedInQuotes = True

            Dim NamesOfVariables As String = R.ReadLine

            Do While Not R.EndOfData

                Dim Values() As String = R.ReadFields

                Dim Packet As New Packet

                Dim PacketType As Type = GetType(Packet)
                Dim PacketFieldInfo() As FieldInfo = PacketType.GetFields()

                Dim ValueIndex As Integer = 0
                For Each FieldInfo As FieldInfo In PacketFieldInfo
                    If Not String.IsNullOrWhiteSpace(Values(ValueIndex)) Then
                        Dim Value As Object = Convert.ChangeType(Values(ValueIndex), FieldInfo.FieldType)
                        FieldInfo.SetValue(Packet, Value)
                        ValueIndex += 1
                    End If
                Next

                ListOfUnits.Add(Packet)


            Loop
        End Using

        Return ListOfUnits
    End Function

    Function UnivariateDistribution_DiscreteVariable(ListOfObservations As List(Of BivariateDistribution)) As Dictionary(Of Tuple(Of String, String), FrequenciesForValue)

        Dim FrequencyDistribution As New Dictionary(Of Tuple(Of String, String), FrequenciesForValue)

        For Each Obs As BivariateDistribution In ListOfObservations

            Dim ObservedValues As New Tuple(Of String, String)(Obs.x, Obs.y)

            If FrequencyDistribution.ContainsKey(ObservedValues) Then
                FrequencyDistribution(ObservedValues).Count += 1
            Else
                FrequencyDistribution.Add(ObservedValues, New FrequenciesForValue)
            End If

            FrequencyDistribution(ObservedValues).RelativeFrequency = FrequencyDistribution(ObservedValues).Count / ListOfObservations.Count
            FrequencyDistribution(ObservedValues).Percentage = FrequencyDistribution(ObservedValues).RelativeFrequency * 100

        Next

        Return FrequencyDistribution

    End Function

    Sub PrintResults_DiscreteDistribution(FrequencyDistribution As Dictionary(Of Tuple(Of String, String), FrequenciesForValue))

        Me.RichTextBox2.AppendText("____________________________________________" & vbCrLf & vbCrLf)
        Me.RichTextBox2.AppendText("BIVARIATE DISTRIBUTION of X and Y" & vbCrLf & vbCrLf)

        'For Each kvp As KeyValuePair(Of Tuple(Of Double, Double), FrequenciesForValue) In SortedDistr
        '    Me.RichTextBox2.AppendText(kvp.Key.ToString.PadRight(7) &
        '                               kvp.Value.Count.ToString.PadRight(7) &
        '                               kvp.Value.RelativeFrequency.ToString("0.##").PadRight(7) &
        '                               kvp.Value.Percentage.ToString.PadRight(4) & " % " & vbCrLf)
        'Next

        Dim DistinctValue_FirstVariable As New Dictionary(Of String, Object)
        Dim DistinctValue_SecondVariable As New Dictionary(Of String, Object)

        For Each kvp As KeyValuePair(Of Tuple(Of String, String), FrequenciesForValue) In FrequencyDistribution

            Dim t As Tuple(Of String, String) = kvp.Key

            If Not DistinctValue_FirstVariable.ContainsKey(t.Item1) Then
                DistinctValue_FirstVariable.Add(t.Item1, Nothing)
            End If
            If Not DistinctValue_SecondVariable.ContainsKey(t.Item2) Then
                DistinctValue_SecondVariable.Add(t.Item2, Nothing)
            End If
        Next

        Dim s1 As New SortedDictionary(Of String, Object)(DistinctValue_FirstVariable)
        Dim s2 As New SortedDictionary(Of String, Object)(DistinctValue_SecondVariable)



        Me.RichTextBox2.AppendText("SRC\DST".ToString.PadRight(10) & "  |  ")
        For Each Y As KeyValuePair(Of String, Object) In s2
            Me.RichTextBox2.AppendText(Y.Key.ToString.PadRight(12))
        Next
        Me.RichTextBox2.AppendText(vbCrLf & "            ---------------------------------------------------" & vbCrLf)


        For Each X As KeyValuePair(Of String, Object) In s1

            Me.RichTextBox2.AppendText(X.Key.ToString.PadRight(5) & "  |  ")

            For Each Y As KeyValuePair(Of String, Object) In s2

                Dim t As New Tuple(Of String, String)(X.Key, Y.Key)

                Dim c As Integer
                If FrequencyDistribution.ContainsKey(t) Then
                    c = FrequencyDistribution(t).Count   'joint frequency of X and Y
                Else
                    c = 0                                'joint frequency of X and Y
                End If

                Me.RichTextBox2.AppendText(c.ToString.PadRight(12))
            Next
            Me.RichTextBox2.AppendText(vbCrLf)
        Next

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim Path As String = Me.TextBox1.Text
        Me.RichTextBox2.Clear()

        Dim ListOfUnits As List(Of Packet) = Me.ParseFile(Path)

        Dim ListOfBivariateDistribution As New List(Of BivariateDistribution)
        For Each Packet As Packet In ListOfUnits
            Dim BivariateDistribution As New BivariateDistribution
            BivariateDistribution.x = Packet.Source
            BivariateDistribution.y = Packet.Destination
            ListOfBivariateDistribution.Add(BivariateDistribution)
        Next

        Dim FrequencyDistribution As Dictionary(Of Tuple(Of String, String), FrequenciesForValue) = Me.UnivariateDistribution_DiscreteVariable(ListOfBivariateDistribution)

        Me.PrintResults_DiscreteDistribution(FrequencyDistribution)

    End Sub
End Class
