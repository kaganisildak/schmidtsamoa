Imports System.Numerics
Public Module SchmidtSamoa
    Dim randoma As New Random(DateTime.Now.Millisecond)

    Public Function KeyGenerate(KeyBit As Integer, Optional ByVal PuFPr As Boolean = False, Optional ByVal privatekey As String = "")
        Dim p, q, pkey, skey As BigInteger
        Dim askey, apkey As String
        If PuFPr = False Then
            Dim digit = Math.Round(KeyBit * 3 / 10) + 3
            p = Prime(digit)
            q = Prime(digit)
            pkey = BigInteger.Multiply(BigInteger.Pow(p, 2), q)
            skey = inversemod(pkey, LCM(p - 1, q - 1))
            askey = "###SSPRIVATEKEY###" & vbCrLf & t2b("###SSPRIVATEKEY###" & t2b(skey.ToString) & "###SSPRIVATEKEY######SSP###" & t2b(p.ToString) & "###SSP######SSQ###" & t2b(q.ToString) & "###SSQ###") & vbCrLf & "###SSPRIVATEKEY###"
            apkey = "###SSPUBLICKEY###" & vbCrLf & t2b(pkey.ToString) & vbCrLf & "###SSPUBLICKEY###"
            Return askey & vbCrLf & vbCrLf & apkey
        Else
            Try
                Dim tempkey As String
                tempkey = b2t(Microsoft.VisualBasic.Split(privatekey, "###SSPRIVATEKEY###")(1))
                p = BigInteger.Parse(b2t(Microsoft.VisualBasic.Split(tempkey, "###SSP###")(1)))
                q = BigInteger.Parse(b2t(Microsoft.VisualBasic.Split(tempkey, "###SSQ###")(1)))
                pkey = BigInteger.Multiply(BigInteger.Pow(p, 2), q)
                apkey = "###SSPUBLICKEY###" & vbCrLf & t2b(pkey.ToString) & vbCrLf & "###SSPUBLICKEY###"
                Return apkey
            Catch ex As Exception
                Throw New Exception("PrivateKey Hatalı")
            End Try
        End If

    End Function


    Public Function Encrypt(Data As String, PublicKey As String)
        Try
            Dim pkey As BigInteger
            Dim sdata
            pkey = BigInteger.Parse(b2t(Microsoft.VisualBasic.Split(PublicKey, "###SSPUBLICKEY###")(1)))
            For i = 0 To Data.Length - 1
                If i = Data.Length - 1 Then
                    sdata += BigInteger.ModPow(BigInteger.Parse(Asc(Data.Chars(i))), pkey, pkey).ToString
                Else
                    sdata += BigInteger.ModPow(BigInteger.Parse(Asc(Data.Chars(i))), pkey, pkey).ToString & " "
                End If
            Next
            Return t2b(sdata)

        Catch ex As Exception
            Throw New Exception("PublicKey Hatalı")
        End Try
    End Function
    Public Function Decrypt(Data As String, PrivateKey As String)
        Try
            Dim skey, p, q As BigInteger
            Dim tdata, tempkey, out, adata() As String
            tempkey = b2t(Microsoft.VisualBasic.Split(PrivateKey, "###SSPRIVATEKEY###")(1))
            skey = BigInteger.Parse(b2t(Microsoft.VisualBasic.Split(tempkey, "###SSPRIVATEKEY###")(1)))
            p = BigInteger.Parse(b2t(Microsoft.VisualBasic.Split(tempkey, "###SSP###")(1)))
            q = BigInteger.Parse(b2t(Microsoft.VisualBasic.Split(tempkey, "###SSQ###")(1)))
            tdata = b2t(Data)
            adata = Microsoft.VisualBasic.Split(tdata, " ")
            For i = 0 To CountCharacter(tdata, " ")
                Dim number As BigInteger = BigInteger.Parse(adata(i))
                out = out & Chr(BigInteger.ModPow(number, skey, BigInteger.Multiply(p, q)).ToString)
            Next
            Return out
        Catch ex As Exception
            Throw New Exception("PrivateKey Hatalı")
        End Try

    End Function

    Function inversemod(a As BigInteger, n As BigInteger) As BigInteger
        Dim i As BigInteger = n, v As BigInteger = 0, d As BigInteger = 1
        While a > 0
            Dim t As BigInteger = i / a, x As BigInteger = a
            a = i Mod x
            i = x
            x = d
            d = v - t * x
            v = x
        End While
        v = v Mod n
        If v < 0 Then
            v = (v + n) Mod n
        End If
        Return v
    End Function
    Private Function CountCharacter(ByVal value As String, ByVal ch As Char) As Integer
        Dim say As Integer = 0
        For Each c As Char In value
            If c = ch Then
                say += 1
            End If
        Next
        Return say
    End Function
    Private Function LCM(a1 As BigInteger, a2 As BigInteger) As BigInteger
        Try
            Dim a As BigInteger = BigInteger.Abs(a1)
            Dim b As BigInteger = BigInteger.Abs(a2)
            a = BigInteger.Divide(a, BigInteger.GreatestCommonDivisor(a, b))
            Return BigInteger.Multiply(a, b)
        Catch
            Throw
        End Try
    End Function
    Private Function t2b(text As String) As String
        Return (System.Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(text)))
    End Function
    Private Function b2t(text As String) As String
        Return System.Text.ASCIIEncoding.ASCII.GetString(System.Convert.FromBase64String(text))
    End Function
    Private Function Prime(length As Integer) As BigInteger
        Dim numbers As String = ""
        For i As Integer = 0 To length - 1
            numbers += randoma.[Next](0, 10).ToString
        Next
        Dim number As BigInteger = BigInteger.Parse(numbers)
        If IsPrimeMillerRabin(number) Then
            Return number
        Else
            Return Prime(length)
        End If
    End Function
    Private Enum NumberType
        Composite
        Prime
    End Enum
    Private Function IsPrimeMillerRabin([integer] As BigInteger) As Boolean
        Dim type As NumberType = MillerRabin([integer], 400)
        Return type = NumberType.Prime
    End Function
    Private Function MillerRabin(n As BigInteger, s As Integer) As NumberType
        Dim nMinusOne As BigInteger = BigInteger.Subtract(n, 1)
        For j As Integer = 1 To s
            Dim a As BigInteger = Random(1, nMinusOne)
            If Witness(a, n) Then
                Return NumberType.Composite
            End If
        Next
        Return NumberType.Prime
    End Function
    Private Function Random(min As BigInteger, max As BigInteger) As BigInteger
        Dim maxBytes As Byte() = max.ToByteArray()
        Dim maxBits As New BitArray(maxBytes)
        Dim random__1 As New Random(DateTime.Now.Millisecond)
        For i As Integer = 0 To maxBits.Length - 1
            Dim randomInt As Integer = random__1.[Next]()
            If (randomInt Mod 2) = 0 Then
                maxBits(i) = Not maxBits(i)
            End If
        Next

        Dim result As New BigInteger()
        For k As Integer = (maxBits.Count - 1) To 0 Step -1
            Dim bitValue As BigInteger = 0

            If maxBits(k) Then
                bitValue = BigInteger.Pow(2, k)
            End If

            result = BigInteger.Add(result, bitValue)
        Next
        Dim randomBigInt As BigInteger = BigInteger.ModPow(result, 1, BigInteger.Add(max, min))
        Return randomBigInt
    End Function
    Private Function Witness(a As BigInteger, n As BigInteger) As Boolean
        Dim tAndU As KeyValuePair(Of Integer, BigInteger) = GetTAndU(BigInteger.Subtract(n, 1))
        Dim t As Integer = tAndU.Key
        Dim u As BigInteger = tAndU.Value
        Dim x As BigInteger() = New BigInteger(t) {}
        x(0) = ModularExponentiation(a, u, n)
        For i As Integer = 1 To t
            x(i) = BigInteger.ModPow(BigInteger.Multiply(x(i - 1), x(i - 1)), 1, n)
            Dim minus As BigInteger = BigInteger.Subtract(x(i - 1), BigInteger.Subtract(n, 1))
            If x(i) = 1 AndAlso x(i - 1) <> 1 AndAlso Not minus.IsZero Then
                Return True
            End If
        Next
        If Not x(t).IsOne Then
            Return True
        End If

        Return False
    End Function
    Private Function GetTAndU(nMinusOne As BigInteger) As KeyValuePair(Of Integer, BigInteger)
        Dim nBytes As Byte() = nMinusOne.ToByteArray()
        Dim bits As New BitArray(nBytes)
        Dim t As Integer = 0
        Dim u As New BigInteger()
        Dim n As Integer = bits.Count - 1
        Dim lastBit As Boolean = bits(n)
        While Not lastBit
            t += 1
            n -= 1
            lastBit = bits(n)
        End While
        For k As Integer = ((bits.Count - 1) - t) To 0 Step -1
            Dim bitValue As BigInteger = 0
            If bits(k) Then
                bitValue = BigInteger.Pow(2, k)
            End If
            u = BigInteger.Add(u, bitValue)
        Next
        Dim tAndU As New KeyValuePair(Of Integer, BigInteger)(t, u)
        Return tAndU
    End Function
    Private Function ModularExponentiation(a As BigInteger, b As BigInteger, n As BigInteger) As BigInteger
        Return BigInteger.ModPow(a, b, n)
    End Function
End Module
