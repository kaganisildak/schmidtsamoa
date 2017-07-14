Imports SchmidtSamoa.SchmidtSamoa
Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        TextBox1.Text = KeyGenerate(50, True, "###SSPRIVATEKEY###
IyMjU1NQUklWQVRFS0VZIyMjTVRrM016UXpPRGc1TnprMk5EazFNalUwTlRRME9USXdNRGN4TlRBeE56Y3dNelV4IyMjU1NQUklWQVRFS0VZIyMjIyMjU1NQIyMjTmpNeE5UTTBNRFkzT0RZeE16YzNNek01IyMjU1NQIyMjIyMjU1NRIyMjT1RVek9ETTBORGs1Tmpnd05EUTRPRE01IyMjU1NRIyMj
###SSPRIVATEKEY###")
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        TextBox3.Text = Encrypt(TextBox2.Text, TextBox1.Text)
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        TextBox3.Text = Decrypt(TextBox2.Text, TextBox1.Text)
    End Sub
End Class
