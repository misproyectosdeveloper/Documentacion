<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Calendario
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Calendar = New System.Windows.Forms.MonthCalendar
        Me.SuspendLayout()
        '
        'Calendar
        '
        Me.Calendar.Location = New System.Drawing.Point(31, 18)
        Me.Calendar.Name = "Calendar"
        Me.Calendar.ShowTodayCircle = False
        Me.Calendar.TabIndex = 15
        Me.Calendar.TodayDate = New Date(2012, 7, 26, 0, 0, 0, 0)
        '
        'Calendario
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.ClientSize = New System.Drawing.Size(292, 198)
        Me.Controls.Add(Me.Calendar)
        Me.Name = "Calendario"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Calendario"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Calendar As System.Windows.Forms.MonthCalendar
End Class
