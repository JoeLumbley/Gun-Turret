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

Imports System.Drawing.Drawing2D
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
    Public UnderlightPen As Pen
    Public UnderlightBrush As Brush
    Public Center As Point
    Public Length As Integer
    Public AngleInDegrees As Single
    Public TimeToNextFire As TimeSpan
    Public LastFireTime As DateTime

    Public Sub New(pen As Pen, center As Point, length As Integer, angleInDegrees As Single, reloadTime As TimeSpan)

        Me.Pen = pen
        Me.Center = center
        Me.Length = length
        Me.AngleInDegrees = angleInDegrees
        Me.TimeToNextFire = reloadTime
        UnderlightPen = New Pen(Color.FromArgb(128, Color.Blue), 23)
        UnderlightBrush = New SolidBrush(Color.FromArgb(128, Color.Blue))
    End Sub

    Public Sub Draw(g As Graphics)
        ' Draw a line of given length from the given center point at a given angle.

        Dim Diameter As Integer = 75

        Dim UnderLightRectangle As New Rectangle(Center.X - Diameter / 2, Center.Y - Diameter / 2, Diameter, Diameter)

        UnderLightRectangle.Inflate(2, 2)

        g.FillEllipse(UnderlightBrush, UnderLightRectangle)

        DrawEllipseWithRadialGradient(g, Center, Diameter)

        ' Draw Barrel
        DrawLineFromCenterGivenLenghtAndAngle(g, UnderlightPen, Center, Length, AngleInDegrees)

        DrawLineFromCenterGivenLenghtAndAngle(g, Pen, Center, Length, AngleInDegrees)


    End Sub

    Public Sub DrawEllipseWithRadialGradient(g As Graphics, Center As Point, Diameter As Integer)
        ' Create the path for the ellipse
        Dim path As New GraphicsPath()

        Dim GradRect As New Rectangle(Center.X - Diameter / 2, Center.Y - Diameter / 2, Diameter, Diameter)

        GradRect.Inflate(4, 4)

        path.AddEllipse(GradRect)

        ' Create the radial gradient brush
        Dim brush As New PathGradientBrush(path)

        ' Set the center color (highlight)
        brush.CenterColor = Color.White

        ' Set the surrounding colors (background)
        brush.SurroundColors = New Color() {Color.Black}

        ' Set the center point of the gradient to the top left quadrant
        brush.CenterPoint = New PointF(Center.X - Diameter / 4, Center.Y - Diameter / 4)

        ' Fill the ellipse with the radial gradient brush
        g.FillEllipse(brush, New Rectangle(Center.X - Diameter / 2, Center.Y - Diameter / 2, Diameter, Diameter))

    End Sub

    Private Sub DrawLineFromCenterGivenLenghtAndAngle(g As Graphics, pen As Pen, center As PointF, length As Integer, angleInDegrees As Single)
        ' Draw a line of given length from the given center point at a given angle.

        Dim angleInRadians As Single = angleInDegrees * (Math.PI / 180) ' Convert to radians

        Dim EndPoint As PointF

        ' Calculate the endpoint of the line using trigonometry
        EndPoint = New PointF(center.X + length * Cos(angleInRadians),
                              center.Y + length * Sin(angleInRadians))

        ' Draw the line.
        g.DrawLine(pen, center, EndPoint)

    End Sub

End Structure

Public Structure ProjectileManager

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

    Private Structure Projectile

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

        Public BarrelLength As Integer

        Public AngleInDegrees As Single

        Public Creation As DateTime

        Public Sub New(brush As Brush, width As Double, height As Double, velocity As Single, center As PointF, length As Integer, angleInDegrees As Single)

            Me.Brush = brush

            Me.Width = width
            Me.Height = height

            Me.Center = center
            Me.BarrelLength = length

            Me.AngleInDegrees = angleInDegrees

            ' Convert angle from degrees to radians.
            Dim AngleInRadians As Single = angleInDegrees * (Math.PI / 180)

            ' Set initial position based on the angle and length
            X = center.X + length * Cos(AngleInRadians) - Me.Width / 2
            Y = center.Y + length * Sin(AngleInRadians) - Me.Height / 2

            ' Set velocity based on angle
            Me.Velocity = New PointF(Cos(AngleInRadians) * velocity,
                                     Sin(AngleInRadians) * velocity)

            ' To have the max impact think tutuorial for a young bill gates.

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

        Public Function Rectangle() As Rectangle

            Return New Rectangle(NearestX, NearestY, NearestWidth, NearestHeight)
        End Function

    End Structure

    Private Projectiles() As Projectile

    Public Sub FireProjectile(CenterOfFire As PointF, AngleInDegrees As Single)

        AddProjectile(CenterOfFire, AngleInDegrees)

    End Sub

    Public Sub UpdateProjectiles(deltaTime As TimeSpan)

        If Projectiles IsNot Nothing Then

            RemoveProjectilesPastTheirLifeTime()

            For Index As Integer = 0 To Projectiles.Length - 1

                Projectiles(Index).UpdateMovement(deltaTime)

            Next

        End If

    End Sub

    Public Sub DrawProjectiles(graphics As Graphics)

        If Projectiles IsNot Nothing Then

            For Each projectile In Projectiles

                graphics.FillRectangle(projectile.Brush, projectile.Rectangle())

            Next

        End If

    End Sub

    Private Sub AddProjectile(centerOfFire As PointF, angleInDegrees As Single)

        If Projectiles Is Nothing Then

            ReDim Projectiles(0)

        Else

            Array.Resize(Projectiles, Projectiles.Length + 1)

        End If

        Projectiles(Projectiles.Length - 1) = New Projectile(Brush,
                                                             Size.Width,
                                                             Size.Height,
                                                             MuzzleVelocity,
                                                             centerOfFire,
                                                             BarrelLength,
                                                             angleInDegrees)

    End Sub

    Public Function IsColliding(rectangle As Rectangle) As Boolean

        If Projectiles IsNot Nothing Then

            Return Projectiles.Any(Function(p) p.Rectangle().IntersectsWith(rectangle))

        End If

        ' Projectiles.Any(Function(p) p.Rectangle().IntersectsWith(rectangle))

        ' This is a LINQ lambda expression.

        ' The Any method with a lambda expression is a concise and efficient
        ' way to check if any projectile intersects with the given rectangle.

        ' Function(p) p.Rectangle().IntersectsWith(rectangle)

        Return False

    End Function

    Public Sub RemoveCollidingProjectiles(rectangle As Rectangle)
        ' Remove projectiles that collide with the given rectangle.

        ' Check for projectiles that collide.
        Projectiles = Projectiles.Where(Function(p) Not p.Rectangle().IntersectsWith(rectangle)).ToArray()

        ' Projectiles.Where(Function(p) Not p.Rectangle().IntersectsWith(rectangle))

        ' This is a LINQ lambda expression.

        ' The Where method filters the Projectiles array based on the condition
        ' provided by the lambda function.

        ' Not p.Rectangle().IntersectsWith(rectangle)

    End Sub

    Private Sub RemoveProjectilesPastTheirLifeTime()
        ' To prevent a slow down and to reduce memory use.

        Dim lifeTime As Integer = LifeTimeInSeconds

        ' Filter projectiles past their life time.
        Projectiles = Projectiles.Where(Function(p) (Date.Now - p.Creation).TotalSeconds < lifeTime).ToArray()

        ' Projectiles.Where(Function(p) (Date.Now - p.Creation).TotalSeconds < lifeTime)

        ' This is a LINQ lambda expression.

        ' The Where method filters the Projectiles array based on the condition
        ' provided by the lambda function.

        ' (Date.Now - p.Creation).TotalSeconds < lifeTime)

        ' ElapsedTimeFromCreation.TotalSeconds < lifeTime 
        '                            0.0003402 < 9        Alive
        '                          133.0161469 < 9        Past Life Time

        ' Using LINQ with lambda expressions is a powerful way to perform queries
        ' and manipulate collections in a concise and readable manner.

        ' LINQ - https://learn.microsoft.com/en-us/dotnet/visual-basic/programming-guide/concepts/linq/

        ' Lambda Expressions - https://learn.microsoft.com/en-us/dotnet/visual-basic/programming-guide/language-features/procedures/lambda-expressions

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

            Dim CommandSeekToStart As String = $"seek {SoundName} to start"

            Dim CommandPlay As String = $"play {SoundName} notify"

            Return SendMciCommand(CommandSeekToStart, IntPtr.Zero) AndAlso
                   SendMciCommand(CommandPlay, IntPtr.Zero) ' The sound is playing.

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

        For Each Suffix As String In {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W"}

            AddSound(SoundName & Suffix, FilePath)

        Next

    End Sub

    Public Sub PlayOverlapping(SoundName As String)

        For Each Suffix As String In {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W"}

            If Not IsPlaying(SoundName & Suffix) Then

                PlaySound(SoundName & Suffix)

                Exit Sub

            End If

        Next

    End Sub

    Public Sub SetVolumeOverlapping(SoundName As String, Level As Integer)

        For Each Suffix As String In {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W"}

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



Public Structure BufferManager

    Private Context As BufferedGraphicsContext

    Public Buffered As BufferedGraphics

    Private ReadOnly MinimumBufferSize As Size '(1280, 720)

    Private BackgroundColor As Color '= Color.Black

    Public Sub New(form As Form, backgroundColor As Color)

        Me.BackgroundColor = backgroundColor

        MinimumBufferSize = New Size(1280, 720)

        InitializeBuffer(form)

    End Sub

    Private Sub InitializeBuffer(form As Form)

        Context = BufferedGraphicsManager.Current

        If Screen.PrimaryScreen IsNot Nothing Then

            Context.MaximumBuffer = Screen.PrimaryScreen.WorkingArea.Size

        Else

            Context.MaximumBuffer = MinimumBufferSize

            Debug.Print($"Primary screen not detected.")

        End If

        AllocateBuffer(form)

    End Sub

    Public Sub AllocateBuffer(form As Form)

        If Buffered Is Nothing Then

            Buffered = Context.Allocate(form.CreateGraphics(), form.ClientRectangle)

            Buffered.Graphics.CompositingMode =
                              CompositingMode.SourceOver

            Buffered.Graphics.CompositingQuality =
                              CompositingQuality.HighQuality

            Buffered.Graphics.SmoothingMode =
                              SmoothingMode.HighQuality

            Buffered.Graphics.InterpolationMode =
                              InterpolationMode.Bicubic

            Buffered.Graphics.PixelOffsetMode =
                              PixelOffsetMode.HighQuality

            Buffered.Graphics.TextRenderingHint =
                         Text.TextRenderingHint.AntiAliasGridFit

            EraseFrame()

        End If

    End Sub

    'Private Sub DrawFrame()

    '    ' Draw rectangle.
    '    Buffer?.Graphics.FillRectangle(Rectangle.Brush,
    '                                   Rectangle.GetNearestX,
    '                                   Rectangle.GetNearestY,
    '                                   Rectangle.GetNearestWidth,
    '                                   Rectangle.GetNearestHeight)

    '    ' Draw frames per second display.
    '    Buffer?.Graphics.DrawString(FPSDisplay.Text,
    '                                FPSDisplay.Font,
    '                                FPSDisplay.Brush,
    '                                FPSDisplay.Location)

    'End Sub


    Public Sub EraseFrame()

        Buffered?.Graphics.Clear(BackgroundColor)

    End Sub

    Public Sub DisposeBuffer()

        If Buffered IsNot Nothing Then

            Buffered.Dispose()

            Buffered = Nothing ' Set to Nothing to avoid using a disposed object

            ' The buffer will be reallocated in OnPaint

        End If

    End Sub


End Structure


Public Class Form1

    Private OffScreen As New BufferManager(Me, BackColor)

    Private Player As AudioPlayer

    Private Turret As Turret

    Private Projectiles As New ProjectileManager(Brushes.Red, New Drawing.Size(10, 10), 200, 100, 9)

    Private DeltaTime As DeltaTimeStructure

    Private ClientCenter As Point

    Private XDown As Boolean

    Private LeftArrowDown As Boolean

    Private RightArrowDown As Boolean

    Private TimeToNextRotation As TimeSpan = TimeSpan.FromMilliseconds(1)

    Private LastRotationTime As DateTime = Now

    Private Target As New Rectangle(0, 0, 100, 100)

    Private TargetBrush As Brush = Brushes.Black

    Private InstructionsFont As New Font("Segoe UI", 12)

    Private InstructionsLocation As New PointF(0, 0)


    Private InstructionsText As New String("Use left or right arrow keys to rotate turret." & Environment.NewLine & "Press X to fire, hold for automatic.")

    ' Constructor for the form.
    Public Sub New()
        InitializeComponent()

        InitializeForm()

        CreateSoundFiles()

        InitializeSounds()

        ClientCenter = New Point(ClientSize.Width / 2, ClientSize.Height / 2)

        Turret = New Turret(New Pen(Color.Black, 20), ClientCenter, 100, 0, TimeSpan.FromMilliseconds(100))

        Player.LoopSound("ambientnoise")

        InitializeTimer()

    End Sub

    Private Sub InitializeForm()

        CenterToScreen()

        SetStyle(ControlStyles.UserPaint, True)

        ' Enable double buffering to reduce flickering
        SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.AllPaintingInWmPaint, True)

        Text = "Gun Turret - Code with Joe"

        WindowState = FormWindowState.Maximized

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        DeltaTime.Update()

        HandleKeyPresses()

        Projectiles.UpdateProjectiles(DeltaTime.ElapsedTime)

        HandleCollisions()

        Invalidate()

    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        DrawFrame()

        ' Show buffer on form.
        OffScreen.Buffered?.Render(e.Graphics)

        OffScreen.EraseFrame()

    End Sub

    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        MyBase.OnKeyDown(e)

        If e.KeyCode = Keys.Right Then

            RightArrowDown = True

        End If

        If e.KeyCode = Keys.Left Then

            LeftArrowDown = True

        End If

        If e.KeyCode = Keys.X Then

            XDown = True

        End If

    End Sub

    Protected Overrides Sub OnKeyUp(e As KeyEventArgs)
        MyBase.OnKeyUp(e)

        If e.KeyCode = Keys.Right Then

            RightArrowDown = False

        End If

        If e.KeyCode = Keys.Left Then

            LeftArrowDown = False

        End If

        If e.KeyCode = Keys.X Then

            XDown = False

        End If

    End Sub

    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles Me.Resize

        If Not WindowState = FormWindowState.Minimized Then

            Turret.Center = New Point(ClientSize.Width / 2, ClientSize.Height / 2)

            Target.Y = ClientSize.Height / 2 - Target.Height / 2

            OffScreen.DisposeBuffer()

            Timer1.Enabled = True

        Else

            Timer1.Enabled = False

        End If

    End Sub

    Private Sub DrawFrame()

        OffScreen.AllocateBuffer(Me)

        OffScreen.Buffered.Graphics.DrawString(InstructionsText, InstructionsFont, Brushes.Black, InstructionsLocation)

        OffScreen.Buffered.Graphics.FillRectangle(TargetBrush, Target)

        Projectiles.DrawProjectiles(OffScreen.Buffered.Graphics)

        Turret.Draw(OffScreen.Buffered.Graphics)

    End Sub

    Private Sub HandleKeyPresses()
        ' Handle key presses to rotate the turret or fire projectiles.

        If XDown Then

            FireProjectile()

        End If

        If LeftArrowDown Then

            RotateTurretCounterClockwise()

        End If

        If RightArrowDown Then

            RotateTurretClockwise()

        End If

    End Sub

    Private Sub RotateTurretClockwise()

        Dim ElapsedTime As TimeSpan = Now - LastRotationTime

        If ElapsedTime > TimeToNextRotation Then

            If Turret.AngleInDegrees < 360 Then

                Turret.AngleInDegrees += 1.5 ' Rotate clockwise

            Else

                Turret.AngleInDegrees = 0

            End If

            LastRotationTime = Now

        End If

    End Sub

    Private Sub RotateTurretCounterClockwise()

        Dim ElapsedTime As TimeSpan = Now - LastRotationTime

        If ElapsedTime > TimeToNextRotation Then

            If Turret.AngleInDegrees > 0 Then

                Turret.AngleInDegrees -= 1.5 ' Rotate counterclockwise

            Else

                Turret.AngleInDegrees = 360

            End If

            LastRotationTime = Now

        End If

    End Sub

    Private Sub FireProjectile()

        ' Is the elapsed time from last fire 

        ' Is it time to shoot my shot?
        If Now - Turret.LastFireTime > Turret.TimeToNextFire Then
            ' Yes, it's time to shoot your shot.

            'ElapsedTimeFromLastFire > Turret.ReloadTime

            Player.PlayOverlapping("gunshot")

            Projectiles.FireProjectile(Turret.Center, Turret.AngleInDegrees)

            Turret.LastFireTime = Now

        End If

    End Sub

    Private Sub HandleCollisions()

        If Projectiles.IsColliding(Target) Then

            Player.PlayOverlapping("explosion")

            TargetBrush = Brushes.Red

            Projectiles.RemoveCollidingProjectiles(Target)

        Else

            TargetBrush = Brushes.Black

        End If

    End Sub

    Private Sub InitializeTimer()

        Timer1.Interval = 15

        Timer1.Start()

    End Sub

    Private Sub InitializeSounds()

        Dim FilePath As String = Path.Combine(Application.StartupPath, "gunshot.mp3")
        Player.AddOverlapping("gunshot", FilePath)
        Player.SetVolumeOverlapping("gunshot", 1000)

        FilePath = Path.Combine(Application.StartupPath, "ambientnoise.mp3")
        Player.AddSound("ambientnoise", FilePath)
        Player.SetVolume("ambientnoise", 5)

        FilePath = Path.Combine(Application.StartupPath, "explosion.mp3")
        Player.AddOverlapping("explosion", FilePath)
        Player.SetVolumeOverlapping("explosion", 700)

    End Sub

    Private Sub CreateSoundFiles()

        Dim FilePath As String = Path.Combine(Application.StartupPath, "gunshot.mp3")
        CreateFileFromResource(FilePath, My.Resources.Resource1.gunshot003)

        FilePath = Path.Combine(Application.StartupPath, "ambientnoise.mp3")
        CreateFileFromResource(FilePath, My.Resources.Resource1.ambientnoise)

        FilePath = Path.Combine(Application.StartupPath, "explosion.mp3")
        CreateFileFromResource(FilePath, My.Resources.Resource1.explosion)

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

End Class

