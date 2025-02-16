' Gun Turret

' MIT License
' Copyright(c) 2025 Joseph W. Lumbley

' Permission is hereby granted, free of charge, to any person obtaining a copy
' of this software and associated documentation files (the "Software"), to deal
' in the Software without restriction, including without limitation the rights
' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
' copies of the Software, and to permit persons to whom the Software is
' furnished to do so, subject to the following conditions:

' The above copyright notice and this permission notice shall be included in all
' copies or substantial portions of the Software.

' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
' IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
' SOFTWARE.


' https://github.com/JoeLumbley/Gun-Turret

Imports System.IO
Imports System.Math
Imports System.Runtime.InteropServices
Imports System.Text

Public Structure DeltaTimeStructure

    Public CurrentFrame As DateTime
    Public LastFrame As DateTime
    Public ElapsedTime As TimeSpan

    Public Sub New(currentFrame As Date, lastFrame As Date,
                       elapsedTime As TimeSpan)

        Me.CurrentFrame = currentFrame
        Me.LastFrame = lastFrame
        Me.ElapsedTime = elapsedTime
    End Sub

    Public Sub Update()

        ' Set the current frame's time to the current system time.
        CurrentFrame = Now

        ' Calculates the elapsed time ( delta time Δt ) between the
        ' current frame and the last frame.
        ElapsedTime = CurrentFrame - LastFrame

        ' Updates the last frame's time to the current frame's time for
        ' use in the next update.
        LastFrame = CurrentFrame

    End Sub

    Public Sub LastFrameNow()

        ' Set the current frame's time to the current system time.
        LastFrame = Now

    End Sub

End Structure

Public Structure Turret

    Public Pen As Pen
    Public Center As PointF
    Public Length As Integer
    Public AngleInDegrees As Single

    Public Sub New(pen As Pen, center As PointF, length As Integer, angleInDegrees As Single)

        Me.Pen = pen
        Me.Center = center
        Me.Length = length
        Me.AngleInDegrees = angleInDegrees

    End Sub

    Public Sub Draw(g As Graphics)
        ' Draw a line of given length from the given center point at a given angle.
        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

        Dim Diameter As Integer = 75


        g.FillEllipse(Brushes.Gray, New Rectangle(Center.X - Diameter / 2, Center.Y - Diameter / 2, Diameter, Diameter))


        ' Draw Barrel
        DrawLineFromCenterGivenLenghtAndAngle(g, Pen, Center, Length, AngleInDegrees)

    End Sub

    Private Sub DrawLineFromCenterGivenLenghtAndAngle(g As Graphics, pen As Pen, center As PointF, length As Integer, angleInDegrees As Single)
        ' Draw a line of given length from the given center point at a given angle.

        Dim angleInRadians As Single = angleInDegrees * (Math.PI / 180) ' Convert to radians

        Dim EndPoint As PointF

        ' Calculate the endpoint of the line using trigonometry
        EndPoint = New PointF(center.X + length * Cos(angleInRadians),
                              center.Y + length * Sin(angleInRadians))

        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

        ' Draw the line.
        g.DrawLine(pen, center, EndPoint)

    End Sub

End Structure

Public Structure ProjectileManager

    Public Structure Projectile

        ' Array of projectiles
        '   Fire
        '       Add
        '   Update
        '       Move
        '       Remove
        '   Draw
        '   Collide

        Public X, Y, Width, Height As Double

        Public Velocity As PointF

        Public Brush As Brush

        Public Center As PointF

        Public Length As Integer

        Public AngleInDegrees As Single

        Public Creation As DateTime

        Public Sub New(brush As Brush, width As Double, height As Double, velocity As Single, center As PointF, length As Integer, angleInDegrees As Single)

            Me.Brush = brush

            Me.Width = width
            Me.Height = height

            Me.Center = center
            Me.Length = length

            Me.AngleInDegrees = angleInDegrees

            ' Convert angle from degrees to radians.
            Dim AngleInRadians As Single = angleInDegrees * (Math.PI / 180)

            ' Set initial position based on the angle and length
            X = center.X + length * Cos(AngleInRadians)
            Y = center.Y + length * Sin(AngleInRadians)

            ' Set direction vector based on angle and velocity.
            ' Adjust the initial position based on the angle.
            Select Case angleInDegrees
                Case 0
                    Me.Velocity.X = velocity
                    Me.Velocity.Y = 0
                    Y -= Me.Height / 2
                Case 360
                    Me.Velocity.X = velocity
                    Me.Velocity.Y = 0
                    Y -= Me.Height / 2
                Case 45
                    Me.Velocity.X = velocity
                    Me.Velocity.Y = velocity
                Case 90
                    Me.Velocity.X = 0
                    Me.Velocity.Y = velocity
                    X -= Me.Width / 2 - 1
                Case 135
                    Me.Velocity.X = -velocity
                    Me.Velocity.Y = velocity
                    X -= Me.Width / 2
                    Y -= Me.Height / 4
                Case 180
                    Me.Velocity.X = -velocity
                    Me.Velocity.Y = 0
                    Y -= Me.Height / 2
                Case 225
                    Me.Velocity.X = -velocity
                    Me.Velocity.Y = -velocity
                Case 270
                    Me.Velocity.X = 0
                    Me.Velocity.Y = -velocity
                    X -= Me.Width / 2 - 1
                Case 315
                    Me.Velocity.X = velocity
                    Me.Velocity.Y = -velocity
                    X -= Me.Width / 2
                    Y -= Me.Height / 4
                Case Else
                    Debug.Print("Projectile was not set to an angle of fire in 45° increments.")
            End Select

            Creation = Now()

        End Sub

        Public Sub UpdateMovement(ByVal deltaTime As TimeSpan)

            'Move our projectile horizontally.
            X += Velocity.X * deltaTime.TotalSeconds 'Δs = V * Δt
            'Displacement = Velocity x Delta Time

            'Move our projectile vertically.
            Y += Velocity.Y * deltaTime.TotalSeconds 'Δs = V * Δt
            'Displacement = Velocity x Delta Time

        End Sub

        ' Methods to round attributes to
        ' the nearest integer values.
        Public Function NearestX() As Integer

            Return RoundToNearest(X)
        End Function
        Public Function NearestY() As Integer

            Return RoundToNearest(Y)
        End Function
        Public Function NearestWidth() As Integer

            Return RoundToNearest(Width)
        End Function
        Public Function NearestHeight() As Integer

            Return RoundToNearest(Height)
        End Function

        ' Generic method to round attributes to the nearest integer values.
        Private Function RoundToNearest(ByVal value As Double) As Integer

            Return CInt(Math.Round(value))
        End Function

    End Structure

    Private Projectiles() As Projectile

    Public Brush As Brush
    Public BarrelLength As Integer
    Public MuzzleVelocity As Single
    Public Size As Size
    Public LifeTimeInSeconds As Integer


    Public Sub New(brush As Brush, size As Size, muzzleVelocity As Single, barrelLength As Integer, lifeTimeInSeconds As Integer)

        Me.Brush = brush

        Me.BarrelLength = barrelLength

        Me.MuzzleVelocity = muzzleVelocity

        Me.Size = size

        Me.LifeTimeInSeconds = lifeTimeInSeconds

    End Sub

    Public Function IsColliding(rect As Rectangle) As Boolean

        Dim Colliding As Boolean

        If Projectiles IsNot Nothing Then

            For Each Projectile In Projectiles

                If rect.IntersectsWith(New Rectangle(Projectile.X, Projectile.Y, Projectile.Width, Projectile.Height)) Then

                    Colliding = True

                End If

            Next

        End If

        Return Colliding

    End Function

    Public Sub DrawProjectiles(g As Graphics)

        If Projectiles IsNot Nothing Then

            For Each Projectile In Projectiles

                g.FillRectangle(Projectile.Brush,
                                Projectile.NearestX,
                                Projectile.NearestY,
                                Projectile.NearestWidth,
                                Projectile.NearestHeight)

            Next

        End If

    End Sub


    Public Sub FireProjectile(CenterOfFire As PointF, AngleInDegrees As Single)

        AddProjectile(CenterOfFire, AngleInDegrees)

    End Sub

    Private Sub AddProjectile(CenterOfFire As PointF, AngleInDegrees As Single)

        If Projectiles IsNot Nothing Then

            Array.Resize(Projectiles, Projectiles.Length + 1)

        Else

            ReDim Projectiles(0)

        End If

        Dim Index As Integer = Projectiles.Length - 1

        Projectiles(Index) = New Projectile(Brush, Size.Width, Size.Height, MuzzleVelocity, CenterOfFire, BarrelLength, AngleInDegrees)

    End Sub

    Public Sub UpdateProjectiles(deltaTime As TimeSpan)

        Dim ElapsedTime As TimeSpan

        If Projectiles IsNot Nothing Then

            For Each Projectile In Projectiles

                Dim Index As Integer = Array.IndexOf(Projectiles, Projectile)

                ElapsedTime = Now - Projectile.Creation

                If ElapsedTime < TimeSpan.FromSeconds(LifeTimeInSeconds) Then

                    Projectiles(Index).UpdateMovement(deltaTime)

                Else

                    RemoveProjectile(Index)

                End If

            Next

        End If

    End Sub

    Private Sub RemoveProjectile(Index As Integer)

        Projectiles = Projectiles.Where(Function(e, i) i <> Index).ToArray()

    End Sub

End Structure

Public Structure AudioPlayer

    <DllImport("winmm.dll", EntryPoint:="mciSendStringW")>
    Private Shared Function mciSendStringW(<MarshalAs(UnmanagedType.LPWStr)> ByVal lpszCommand As String,
                                           <MarshalAs(UnmanagedType.LPWStr)> ByVal lpszReturnString As StringBuilder,
                                           ByVal cchReturn As UInteger, ByVal hwndCallback As IntPtr) As Integer
    End Function

    Private Sounds() As String

    Public Function AddSound(SoundName As String, FilePath As String) As Boolean

        ' Do we have a name and does the file exist?
        If Not String.IsNullOrWhiteSpace(SoundName) AndAlso IO.File.Exists(FilePath) Then
            ' Yes, we have a name and the file exists.

            Dim CommandOpen As String = $"open ""{FilePath}"" alias {SoundName}"

            ' Do we have sounds?
            If Sounds Is Nothing Then
                ' No we do not have sounds.

                ' Did the sound file open?
                If SendMciCommand(CommandOpen, IntPtr.Zero) Then
                    ' Yes, the sound file did open.

                    ' Start the Sounds array with the sound.
                    ReDim Sounds(0)

                    Sounds(0) = SoundName

                    Return True ' The sound was added.

                End If

                ' Is the sound in the array already?
            ElseIf Not Sounds.Contains(SoundName) Then
                ' Yes we have sounds and no the sound is not in the array.

                ' Did the sound file open?
                If SendMciCommand(CommandOpen, IntPtr.Zero) Then
                    ' Yes, the sound file did open.

                    ' Add the sound to the Sounds array.
                    Array.Resize(Sounds, Sounds.Length + 1)

                    Sounds(Sounds.Length - 1) = SoundName

                    Return True ' The sound was added.

                End If

            End If

        End If

        Debug.Print($"{SoundName} not added to sounds.")

        Return False ' The sound was not added.

    End Function

    Public Function SetVolume(SoundName As String, Level As Integer) As Boolean

        ' Do we have sounds and is the sound in the array and is the level in the valid range?
        If Sounds IsNot Nothing AndAlso Sounds.Contains(SoundName) AndAlso Level >= 0 AndAlso Level <= 1000 Then
            ' We have sounds and the sound is in the array and the level is in range.

            Dim CommandVolume As String = $"setaudio {SoundName} volume to {Level}"

            Return SendMciCommand(CommandVolume, IntPtr.Zero) ' The volume was set.

        End If

        Debug.Print($"{SoundName} volume not set.")

        Return False ' The volume was not set.

    End Function

    Public Function LoopSound(SoundName As String) As Boolean

        ' Do we have sounds and is the sound in the array?
        If Sounds IsNot Nothing AndAlso Sounds.Contains(SoundName) Then
            ' We have sounds and the sound is in the array.

            Dim CommandSeekToStart As String = $"seek {SoundName} to start"

            Dim CommandPlayRepeat As String = $"play {SoundName} repeat"

            Return SendMciCommand(CommandSeekToStart, IntPtr.Zero) AndAlso
                   SendMciCommand(CommandPlayRepeat, IntPtr.Zero) ' The sound is looping.

        End If

        Debug.Print($"{SoundName} not looping.")

        Return False ' The sound is not looping.

    End Function

    Public Function PlaySound(SoundName As String) As Boolean

        ' Do we have sounds and is the sound in the array?
        If Sounds IsNot Nothing AndAlso Sounds.Contains(SoundName) Then
            ' We have sounds and the sound is in the array.

            'Dim CommandSeekToStart As String = $"seek {SoundName} to start"

            'Dim CommandPlay As String = $"play {SoundName} notify"

            Dim CommandPlay As String = $"play {SoundName} from 0"

            'Return SendMciCommand(CommandSeekToStart, IntPtr.Zero) AndAlso
            '       SendMciCommand(CommandPlay, IntPtr.Zero) ' The sound is playing.

            Return SendMciCommand(CommandPlay, IntPtr.Zero) ' The sound is playing.


        End If

        Debug.Print($"{SoundName} not playing.")

        Return False ' The sound is not playing.

    End Function

    Public Function PauseSound(SoundName As String) As Boolean

        ' Do we have sounds and is the sound in the array?
        If Sounds IsNot Nothing AndAlso Sounds.Contains(SoundName) Then
            ' We have sounds and the sound is in the array.

            Dim CommandPause As String = $"pause {SoundName} notify"

            Return SendMciCommand(CommandPause, IntPtr.Zero) ' The sound is paused.

        End If

        Debug.Print($"{SoundName} not paused.")

        Return False ' The sound is not paused.

    End Function

    Public Function IsPlaying(SoundName As String) As Boolean

        Return GetStatus(SoundName, "mode") = "playing"

    End Function

    Public Sub AddOverlapping(SoundName As String, FilePath As String)

        For Each Suffix As String In {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L"}

            AddSound(SoundName & Suffix, FilePath)

        Next

    End Sub

    Public Sub PlayOverlapping(SoundName As String)

        For Each Suffix As String In {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L"}

            If Not IsPlaying(SoundName & Suffix) Then

                PlaySound(SoundName & Suffix)

                Exit Sub

            End If

        Next

    End Sub

    Public Sub SetVolumeOverlapping(SoundName As String, Level As Integer)

        For Each Suffix As String In {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L"}

            SetVolume(SoundName & Suffix, Level)

        Next

    End Sub

    Private Function SendMciCommand(command As String, hwndCallback As IntPtr) As Boolean

        Dim ReturnString As New StringBuilder(128)

        Try

            Return mciSendStringW(command, ReturnString, 0, hwndCallback) = 0

        Catch ex As Exception

            Debug.Print($"Error sending MCI command: {command} | {ex.Message}")

            Return False

        End Try

    End Function

    Private Function GetStatus(SoundName As String, StatusType As String) As String

        Try

            ' Do we have sounds and is the sound in the array?
            If Sounds IsNot Nothing AndAlso Sounds.Contains(SoundName) Then
                ' We have sounds and the sound is in the array.

                Dim CommandStatus As String = $"status {SoundName} {StatusType}"

                Dim StatusReturn As New StringBuilder(128)

                mciSendStringW(CommandStatus, StatusReturn, 128, IntPtr.Zero)

                Return StatusReturn.ToString.Trim.ToLower

            End If

        Catch ex As Exception

            Debug.Print($"Error getting status: {SoundName} | {ex.Message}")

        End Try

        Return String.Empty

    End Function

    Public Sub CloseSounds()

        If Sounds IsNot Nothing Then

            For Each Sound In Sounds

                Dim CommandClose As String = $"close {Sound}"

                SendMciCommand(CommandClose, IntPtr.Zero)

            Next

            Sounds = Nothing

        End If

    End Sub

End Structure


Public Class Form1

    Private Player As AudioPlayer

    Private MyTurret As Turret

    Private Projectiles As New ProjectileManager(Brushes.Red, New Drawing.Size(10, 10), 100, 100, 9)

    Private DeltaTime As DeltaTimeStructure

    ' Angle for the rotating line
    Private angle As Single = 0

    ' Center point for rotation
    Private center As PointF

    Private MyPen As New Pen(Color.Black, 20)


    Private ADown As Boolean

    Private ReloadTime As TimeSpan

    Private LastFireTime As DateTime = Now

    Private Target As New Rectangle(0, 0, 100, 100)

    Private TargetHit As Boolean

    ' Constructor for the form.
    Public Sub New()
        InitializeComponent()

        CreateSoundFiles()

        ReloadTime = TimeSpan.FromMilliseconds(100)

        Dim FilePath As String = Path.Combine(Application.StartupPath, "gunshot.mp3")

        Player.AddOverlapping("gunshot", FilePath)

        Player.SetVolumeOverlapping("gunshot", 1000)

        FilePath = Path.Combine(Application.StartupPath, "ambientnoise.mp3")

        Player.AddSound("ambientnoise", FilePath)

        Player.SetVolume("ambientnoise", 200)

        ' Set the center point to the middle of the form
        center = New PointF(ClientSize.Width / 2, ClientSize.Height / 2)

        ' Enable double buffering to reduce flickering
        Me.DoubleBuffered = True

        MyTurret = New Turret(MyPen, center, 100, angle)

        Timer1.Interval = 15

        Timer1.Start()

        Text = "Gun Turret - Code with Joe"

        Player.LoopSound("ambientnoise")

    End Sub

    Private Sub CreateSoundFiles()

        Dim FilePath As String = Path.Combine(Application.StartupPath, "gunshot.mp3")

        CreateFileFromResource(FilePath, My.Resources.Resource1.gunshot003)

        FilePath = Path.Combine(Application.StartupPath, "ambientnoise.mp3")

        CreateFileFromResource(FilePath, My.Resources.Resource1.ambientnoise)

    End Sub

    Private Sub CreateFileFromResource(filepath As String, resource As Byte())

        Try

            If Not IO.File.Exists(filepath) Then

                IO.File.WriteAllBytes(filepath, resource)

            End If

        Catch ex As Exception

            Debug.Print($"Error creating file: {ex.Message}")

        End Try

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        DeltaTime.Update()

        If ADown Then

            Dim ElapsedTime As TimeSpan = Now - LastFireTime

            If ElapsedTime > ReloadTime Then

                Player.PlayOverlapping("gunshot")

                Projectiles.FireProjectile(MyTurret.Center, MyTurret.AngleInDegrees)

                LastFireTime = Now

            End If

        End If

        Projectiles.UpdateProjectiles(DeltaTime.ElapsedTime)

        If Projectiles.IsColliding(Target) Then

            TargetHit = True

        Else

            TargetHit = False

        End If

        Invalidate()

    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        e.Graphics.DrawString("Use arrow keys to rotate turret. Press A to fire hold for automatic.", New Font("Segoe UI", 12), Brushes.Black, New PointF(0, 0))

        Projectiles.DrawProjectiles(e.Graphics)

        MyTurret.Draw(e.Graphics)



        If TargetHit Then

            e.Graphics.FillRectangle(Brushes.Red, Target)

        Else

            e.Graphics.FillRectangle(Brushes.Black, Target)

        End If

    End Sub

    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        MyBase.OnKeyDown(e)
        ' Handle key presses to rotate the turret or fire projectiles.

        ' Rotate turret based on key press.
        If e.KeyCode = Keys.Right Then

            If MyTurret.AngleInDegrees < 360 Then

                MyTurret.AngleInDegrees += 45 ' Rotate clockwise

            Else

                MyTurret.AngleInDegrees = 45

            End If

        End If

        If e.KeyCode = Keys.Left Then

            If MyTurret.AngleInDegrees > 0 Then

                MyTurret.AngleInDegrees -= 45 ' Rotate clockwise

            Else

                MyTurret.AngleInDegrees = 315

            End If

        End If

        If e.KeyCode = Keys.A Then

            ADown = True

        End If

    End Sub

    Protected Overrides Sub OnKeyUp(e As KeyEventArgs)
        MyBase.OnKeyUp(e)

        If e.KeyCode = Keys.Right Then

        End If

        If e.KeyCode = Keys.Left Then

        End If

        If e.KeyCode = Keys.A Then

            ADown = False

        End If

    End Sub

    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles Me.Resize

        MyTurret.Center = New PointF(ClientSize.Width / 2, ClientSize.Height / 2)

        Target.Y = ClientSize.Height / 2 - Target.Height / 2

    End Sub

End Class

