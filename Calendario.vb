Public Class Calendario
    Public PFecha As Date = Date.Now
    Private Sub Calendario_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
       
        If PFecha <> Now Then
            If Format(PFecha, "dd/MM/yyyy") <> "01/01/1800" And Format(PFecha, "dd/MM/yyyy") <> "01/01/2000" Then
                Calendar.ShowTodayCircle = False
                Calendar.SetDate(PFecha)
            End If
        End If

    End Sub
    Private Sub Calendar_DateSelected(ByVal sender As Object, ByVal e As System.Windows.Forms.DateRangeEventArgs) Handles Calendar.DateSelected

        PFecha = e.End
        Me.Close()

    End Sub
End Class