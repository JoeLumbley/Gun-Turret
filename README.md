# Gun Turret

A application that simulates a gun turret, allowing for rotation and firing projectiles.

![002](https://github.com/user-attachments/assets/c54120b8-8c66-443f-981d-b5687e75f405)











---





# Code Walkthrough


## Turret Structure

The `Turret` structure defines how the turret behaves and is drawn on the screen.

### Code Breakdown

```vb
Public Structure Turret
```
This line declares a new structure named `Turret`. A structure is a value type that can encapsulate data and related functionality.

```vb
    Public Pen As Pen
    Public Center As PointF
    Public Length As Integer
    Public AngleInDegrees As Single
```
Here, we define four public variables:
- `Pen`: This is used to define the drawing style (color, width) of the turret.
- `Center`: A `PointF` structure that holds the coordinates of the turret's center.
- `Length`: An integer representing the length of the turret's barrel.
- `AngleInDegrees`: A single-precision floating-point number that specifies the angle of the turret in degrees.

### Constructor

```vb
    Public Sub New(pen As Pen, center As PointF, length As Integer, angleInDegrees As Single)
        Me.Pen = pen
        Me.Center = center
        Me.Length = length
        Me.AngleInDegrees = angleInDegrees
    End Sub
```
This is the constructor for the `Turret` structure. It initializes the turret's properties with the values passed when creating a new turret. The `Me` keyword refers to the current instance of the structure.

### Draw Method

```vb
    Public Sub Draw(g As Graphics)
        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
```
The `Draw` method is responsible for rendering the turret on the screen. It accepts a `Graphics` object, which is used for drawing. The smoothing mode is set to `AntiAlias` to make the edges of the shapes smoother.

```vb
        Dim Diameter As Integer = 75
        g.FillEllipse(Brushes.Gray, New Rectangle(Center.X - Diameter / 2, Center.Y - Diameter / 2, Diameter, Diameter))
```
Here, we define a diameter for the turret's base and draw a gray ellipse (circle) at the turret's center. The `Rectangle` is calculated to center the ellipse based on its diameter.

```vb
        DrawLineFromCenterGivenLenghtAndAngle(g, Pen, Center, Length, AngleInDegrees)
    End Sub
```
This line calls another method, `DrawLineFromCenterGivenLenghtAndAngle`, to draw the barrel of the turret based on its center, length, and angle.

### Line Drawing Method

```vb
    Private Sub DrawLineFromCenterGivenLenghtAndAngle(g As Graphics,
                                                      pen As Pen,
                                                      center As PointF,
                                                      length As Integer,
                                                      angleInDegrees As Single)

        Dim angleInRadians As Single = angleInDegrees * (Math.PI / 180)

```

This private method calculates the endpoint of the turret's barrel based on the angle provided. It converts degrees to radians since trigonometric functions in VB use radians.

```vb
        Dim EndPoint As PointF

        EndPoint = New PointF(center.X + length * Cos(angleInRadians),
                              center.Y + length * Sin(angleInRadians))

```

Using trigonometry, it calculates the endpoint of the barrel based on the center point and the length of the barrel.

```vb
        g.DrawLine(pen, center, EndPoint)

    End Sub

End Structure

```
Finally, it draws the line (the barrel) from the center to the calculated endpoint.

---

## ProjectileManager Structure

The `ProjectileManager` structure manages the projectiles fired from the turret.

### Code Breakdown

```vb
Public Structure ProjectileManager
```
This line declares a new structure named `ProjectileManager`.

### Projectile Structure

```vb
    Public Structure Projectile
```
Inside `ProjectileManager`, we define another structure called `Projectile`, which represents each projectile.

### Properties

```vb
        Public X, Y, Width, Height As Double
        Public Velocity As PointF
        Public Brush As Brush
        Public Center As PointF
        Public Length As Integer
        Public AngleInDegrees As Single
        Public Creation As DateTime
```
These properties define the characteristics of a projectile:
- `X` and `Y`: The position of the projectile.
- `Width` and `Height`: The dimensions of the projectile.
- `Velocity`: A `PointF` that represents the speed and direction of the projectile.
- `Brush`: Used to fill the projectile.
- `Center`: The center point of the projectile.
- `Length`: The length of the projectile.
- `AngleInDegrees`: The angle at which the projectile is fired.
- `Creation`: The time when the projectile was created.

### Constructor

```vb
        Public Sub New(brush As Brush, width As Double, height As Double, velocity As Single, center As PointF, length As Integer, angleInDegrees As Single)
```
This constructor initializes a new projectile with the provided parameters.

```vb
            Dim AngleInRadians As Single = angleInDegrees * (Math.PI / 180)
```
It converts the angle from degrees to radians for calculations.

```vb
            X = center.X + length * Cos(AngleInRadians)
            Y = center.Y + length * Sin(AngleInRadians)
```
The initial position of the projectile is set based on the center point and the angle.

### Velocity Calculation

```vb
            Select Case angleInDegrees
```
This `Select Case` statement determines the projectile's velocity based on the angle it is fired at. Each case sets the `X` and `Y` components of the velocity vector.

```vb
            Creation = Now()
```
This line records the current time when the projectile is created.

### UpdateMovement Method

```vb
        Public Sub UpdateMovement(ByVal deltaTime As TimeSpan)
            X += Velocity.X * deltaTime.TotalSeconds
            Y += Velocity.Y * deltaTime.TotalSeconds
        End Sub
```
The `UpdateMovement` method updates the position of the projectile based on its velocity and the elapsed time since the last update.

### Rounding Functions

```vb
        Public Function NearestX() As Integer
            Return RoundToNearest(X)
        End Function
```
These functions round the projectile's attributes to the nearest integer for rendering.

```vb
        Private Function RoundToNearest(ByVal value As Double) As Integer
            Return CInt(Math.Round(value))
        End Function
```
This private function handles the rounding logic.

### Projectile Array

```vb
    Private Projectiles() As Projectile
```
An array to hold all active projectiles.

### Constructor

```vb
    Public Sub New(brush As Brush, size As Size, muzzleVelocity As Single, barrelLength As Integer, lifeTimeInSeconds As Integer)
```
This constructor initializes the `ProjectileManager` with the provided parameters.

### Drawing Projectiles

```vb
    Public Sub DrawProjectiles(g As Graphics)
        If Projectiles IsNot Nothing Then
            For Each Projectile In Projectiles
                g.FillRectangle(Projectile.Brush, Projectile.NearestX, Projectile.NearestY, Projectile.NearestWidth, Projectile.NearestHeight)
            Next
        End If
    End Sub
```
This method iterates through all projectiles and draws them on the screen.

### Firing Projectiles

```vb
    Public Sub FireProjectile(CenterOfFire As PointF, AngleInDegrees As Single)
        AddProjectile(CenterOfFire, AngleInDegrees)
    End Sub
```
This method is called to fire a new projectile from a specified center point and angle.

### Updating Projectiles

```vb
    Public Sub UpdateProjectiles(deltaTime As TimeSpan)
        If Projectiles IsNot Nothing Then
            For Each Projectile In Projectiles
                Dim Index As Integer = Array.IndexOf(Projectiles, Projectile)
                Dim ElapsedTime As TimeSpan = Now - Projectile.Creation
                If ElapsedTime < TimeSpan.FromSeconds(LifeTimeInSeconds) Then
                    Projectiles(Index).UpdateMovement(deltaTime)
                Else
                    RemoveProjectile(Index)
                End If
            Next
        End If
    End Sub
```
This method updates each projectile's position and removes it if its lifetime has expired.

### Removing Projectiles

```vb
    Private Sub RemoveProjectile(Index As Integer)
        Projectiles = Projectiles.Where(Function(e, i) i <> Index).ToArray()
    End Sub
```
This method removes a projectile from the array using LINQ.

---

## Form1 Class

The `Form1` class is the main interface for our application.

### Code Breakdown

```vb
Public Class Form1
```
This line declares the main form class.

### Member Variables

```vb
    Private Player As AudioPlayer
    Private MyTurret As Turret
    Private Projectiles As New ProjectileManager(Brushes.Red, New Drawing.Size(10, 10), 100, 100, 9)
```
Here, we declare member variables:
- `Player`: An instance of an audio player for sound effects.
- `MyTurret`: An instance of the `Turret` structure.
- `Projectiles`: An instance of the `ProjectileManager`.

### Constructor

```vb
    Public Sub New()
        InitializeComponent()
```
This is the constructor for the form. It initializes the components of the form.

```vb
        ReloadTime = TimeSpan.FromMilliseconds(120)
```
Sets the reload time for firing projectiles.

```vb
        Dim FilePath As String = Path.Combine(Application.StartupPath, "gunshot.mp3")
        Player.AddOverlapping("gunshot", FilePath)
        Player.SetVolumeOverlapping("gunshot", 1000)
```
This block sets up the sound file for the gunshot effect.

```vb
        center = New PointF(ClientSize.Width / 2, ClientSize.Height / 2)
        Me.DoubleBuffered = True
```
The center point for the turret is calculated and double buffering is enabled to reduce flickering during drawing.

```vb
        MyTurret = New Turret(MyPen, center, 100, angle)
        Timer1.Interval = 15
        Timer1.Start()
        Text = "Gun Turret - Code with Joe"
    End Sub
```
A new turret is created, the timer is set, and the form's title is defined.

### Timer Tick Event

```vb
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
```
This method is called at each tick of the timer.

```vb
        DeltaTime.Update()
```
Updates the delta time for movement calculations.

```vb
        If ADown Then
            Dim ElapsedTime As TimeSpan = Now - LastFireTime
            If ElapsedTime > ReloadTime Then
                Player.PlayOverlapping("gunshot")
                Projectiles.FireProjectile(MyTurret.Center, MyTurret.AngleInDegrees)
                LastFireTime = Now
            End If
        End If
```
If the "A" key is pressed, it checks if enough time has passed to fire another projectile. If so, it plays the gunshot sound and fires the projectile.

```vb
        Projectiles.UpdateProjectiles(DeltaTime.ElapsedTime)
        Invalidate()
    End Sub
```
Updates the positions of all projectiles and triggers a redraw of the form.

### Paint Event

```vb
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
```
This method handles the painting of the form.

```vb
        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
        e.Graphics.DrawString("Use arrow keys to rotate turret. Press A to fire hold for automatic.", New Font("Segoe UI", 12), Brushes.Black, New PointF(0, 0))
        Projectiles.DrawProjectiles(e.Graphics)
        MyTurret.Draw(e.Graphics)
    End Sub
```
It draws instructions on the screen, the projectiles, and the turret.

### Key Events

```vb
    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
```
This method handles key presses to rotate the turret or fire projectiles.

```vb
        If e.KeyCode = Keys.Right Then
            If MyTurret.AngleInDegrees < 360 Then
                MyTurret.AngleInDegrees += 45 ' Rotate clockwise
            Else
                MyTurret.AngleInDegrees = 45
            End If
        End If
```
This block rotates the turret to the right by 45 degrees.

```vb
        If e.KeyCode = Keys.Left Then
            If MyTurret.AngleInDegrees > 0 Then
                MyTurret.AngleInDegrees -= 45 ' Rotate counter-clockwise
            Else
                MyTurret.AngleInDegrees = 315
            End If
        End If
```
This block rotates the turret to the left by 45 degrees.

```vb
        If e.KeyCode = Keys.A Then
            ADown = True
        End If
    End Sub
```
This line sets a flag indicating that the "A" key is being held down.

### Key Up Event

```vb
    Protected Overrides Sub OnKeyUp(e As KeyEventArgs)
        MyBase.OnKeyUp(e)
        If e.KeyCode = Keys.A Then
            ADown = False
        End If
    End Sub
```
This method resets the `ADown` flag when the "A" key is released.

### Resize Event

```vb
    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        MyTurret.Center = New PointF(ClientSize.Width / 2, ClientSize.Height / 2)
    End Sub
```
This method re-centers the turret when the form is resized.

---

## Conclusion

This code represents a simple yet effective simulation of a turret that can rotate and fire projectiles. Each structure and method is designed to encapsulate specific functionality, making the code modular and easier to understand. By breaking down the code line by line, we can see how each part contributes to the overall behavior of the turret and projectile manager.

Feel free to experiment with the code, change parameters, and see how it affects the behavior of the turret and its projectiles. Happy coding!




Welcome to this detailed walkthrough of a code structure that simulates a turret and projectile manager. This code is designed to create a simple graphical representation of a turret that can fire projectiles. We'll break down each part of the code, explaining what it does line by line.

## Table of Contents
1. [Turret Structure](#turret-structure)
2. [ProjectileManager Structure](#projectilemanager-structure)
3. [Form1 Class](#form1-class)
4. [Conclusion](#conclusion)















