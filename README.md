# Gun Turret

A application that simulates a gun turret, allowing for rotation and firing projectiles.

![002](https://github.com/user-attachments/assets/c54120b8-8c66-443f-981d-b5687e75f405)




# Gun Turret Simulation Code Walkthrough

Welcome to the detailed walkthrough of the Gun Turret simulation code! This lesson will guide you through the code structure, explaining each part line by line. Whether you are a beginner or looking to refresh your understanding, this lesson aims to make the concepts clear and accessible.

## Table of Contents
1. [Imports](#imports)
2. [DeltaTimeStructure](#deltatimestructure)
3. [Turret Structure](#turret-structure)
4. [ProjectileManager Structure](#projectilemanager-structure)
5. [AudioPlayer Structure](#audioplayer-structure)
6. [Form1 Class](#form1-class)
7. [Conclusion](#conclusion)

---

## Imports

```vb
Imports System.IO
```
- **Imports System.IO**: This line imports the `System.IO` namespace, which contains classes for working with files and data streams. It allows us to read from and write to files.

```vb
Imports System.Math
```
- **Imports System.Math**: This imports mathematical functions such as trigonometric functions (like sine and cosine) that we will use to calculate angles and distances in our application.

```vb
Imports System.Runtime.InteropServices
```
- **Imports System.Runtime.InteropServices**: This enables interaction with unmanaged code, which is necessary for playing audio in our application using Windows multimedia functions.

```vb
Imports System.Text
```
- **Imports System.Text**: This namespace is used for manipulating strings, particularly useful when working with commands for the audio playback.

## DeltaTimeStructure

```vb
Public Structure DeltaTimeStructure
```
- **Public Structure DeltaTimeStructure**: This line defines a new structure called `DeltaTimeStructure`. Structures are used to group related data together.

```vb
    Public CurrentFrame As DateTime
```
- **Public CurrentFrame As DateTime**: This declares a public variable `CurrentFrame` of type `DateTime`. It will store the current time during each frame update.

```vb
    Public LastFrame As DateTime
```
- **Public LastFrame As DateTime**: This variable `LastFrame` also of type `DateTime`, will keep track of the time of the last frame update.

```vb
    Public ElapsedTime As TimeSpan
```
- **Public ElapsedTime As TimeSpan**: This variable `ElapsedTime` of type `TimeSpan` will store the duration between the current frame and the last frame.

### Constructor

```vb
Public Sub New(currentFrame As Date, lastFrame As Date, elapsedTime As TimeSpan)
```
- **Public Sub New(...)**: This is the constructor method for the `DeltaTimeStructure`. It initializes the structure with specific values.

```vb
    Me.CurrentFrame = currentFrame
```
- **Me.CurrentFrame = currentFrame**: This assigns the value of the `currentFrame` parameter to the `CurrentFrame` variable of the structure.

```vb
    Me.LastFrame = lastFrame
```
- **Me.LastFrame = lastFrame**: This assigns the value of the `lastFrame` parameter to the `LastFrame` variable of the structure.

```vb
    Me.ElapsedTime = elapsedTime
```
- **Me.ElapsedTime = elapsedTime**: This assigns the value of the `elapsedTime` parameter to the `ElapsedTime` variable of the structure.

### Update Method

```vb
Public Sub Update()
```
- **Public Sub Update()**: This method will update the time values for the current frame and calculate the elapsed time.

```vb
    CurrentFrame = Now
```
- **CurrentFrame = Now**: This sets `CurrentFrame` to the current system time using the `Now` function.

```vb
    ElapsedTime = CurrentFrame - LastFrame
```
- **ElapsedTime = CurrentFrame - LastFrame**: This calculates the time difference between the current frame and the last frame, storing it in `ElapsedTime`.

```vb
    LastFrame = CurrentFrame
```
- **LastFrame = CurrentFrame**: This updates `LastFrame` to the current frame time, so it can be used in the next update.

### LastFrameNow Method

```vb
Public Sub LastFrameNow()
```
- **Public Sub LastFrameNow()**: This method simply updates the `LastFrame` to the current time.

```vb
    LastFrame = Now
```
- **LastFrame = Now**: This sets `LastFrame` to the current system time.

## Turret Structure

```vb
Public Structure Turret
```
- **Public Structure Turret**: This line defines a new structure called `Turret`, which will hold the properties and methods related to the turret.

```vb
    Public Pen As Pen
```
- **Public Pen As Pen**: This variable `Pen` of type `Pen` is used for drawing the turret on the graphics surface.

```vb
    Public Center As PointF
```
- **Public Center As PointF**: This variable `Center` of type `PointF` represents the center point of the turret, which is where it will rotate.

```vb
    Public Length As Integer
```
- **Public Length As Integer**: This variable `Length` will define the length of the turret's barrel.

```vb
    Public AngleInDegrees As Single
```
- **Public AngleInDegrees As Single**: This variable `AngleInDegrees` stores the angle at which the turret is pointing, measured in degrees.

### Constructor

```vb
Public Sub New(pen As Pen, center As PointF, length As Integer, angleInDegrees As Single)
```
- **Public Sub New(...)**: This constructor initializes a new instance of the `Turret` structure with specific values.

```vb
    Me.Pen = pen
```
- **Me.Pen = pen**: This assigns the `pen` parameter to the `Pen` variable of the turret.

```vb
    Me.Center = center
```
- **Me.Center = center**: This assigns the `center` parameter to the `Center` variable of the turret.

```vb
    Me.Length = length
```
- **Me.Length = length**: This assigns the `length` parameter to the `Length` variable of the turret.

```vb
    Me.AngleInDegrees = angleInDegrees
```
- **Me.AngleInDegrees = angleInDegrees**: This assigns the `angleInDegrees` parameter to the `AngleInDegrees` variable of the turret.

### Draw Method

```vb
Public Sub Draw(g As Graphics)
```
- **Public Sub Draw(g As Graphics)**: This method is responsible for drawing the turret on the graphics surface.

```vb
    g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
```
- **g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias**: This sets the smoothing mode for the graphics object, which improves the visual quality of the drawn shapes.

```vb
    Dim Diameter As Integer = 75
```
- **Dim Diameter As Integer = 75**: This declares a variable `Diameter` and sets it to 75. This will be used to define the size of the turret's base.

```vb
    g.FillEllipse(Brushes.Gray, New Rectangle(Center.X - Diameter / 2, Center.Y - Diameter / 2, Diameter, Diameter))
```
- **g.FillEllipse(...)**: This draws a filled gray ellipse (the base of the turret) at the center point. The dimensions are calculated to center the ellipse around the turret's center.

```vb
    DrawLineFromCenterGivenLenghtAndAngle(g, Pen, Center, Length, AngleInDegrees)
```
- **DrawLineFromCenterGivenLenghtAndAngle(...)**: This calls another method to draw the turret's barrel based on its center, length, and angle.

### DrawLineFromCenterGivenLenghtAndAngle Method

```vb
Private Sub DrawLineFromCenterGivenLenghtAndAngle(g As Graphics, pen As Pen, center As PointF, length As Integer, angleInDegrees As Single)
```
- **Private Sub DrawLineFromCenterGivenLenghtAndAngle(...)**: This private method draws a line (the turret's barrel) from the center point at a specified angle and length.

```vb
    Dim angleInRadians As Single = angleInDegrees * (Math.PI / 180) ' Convert to radians
```
- **Dim angleInRadians As Single = angleInDegrees * (Math.PI / 180)**: This converts the angle from degrees to radians, which is necessary for trigonometric calculations.

```vb
    Dim EndPoint As PointF
```
- **Dim EndPoint As PointF**: This declares a variable `EndPoint` of type `PointF`, which will store the endpoint of the line to be drawn.

```vb
    EndPoint = New PointF(center.X + length * Cos(angleInRadians), center.Y + length * Sin(angleInRadians))
```
- **EndPoint = New PointF(...)**: This calculates the endpoint of the line using trigonometric functions (`Cos` and `Sin`) to determine the x and y coordinates based on the angle and length.

```vb
    g.DrawLine(pen, center, EndPoint)
```
- **g.DrawLine(pen, center, EndPoint)**: This draws the line from the center point to the calculated endpoint using the specified pen.

## ProjectileManager Structure

```vb
Public Structure ProjectileManager
```
- **Public Structure ProjectileManager**: This defines a new structure called `ProjectileManager`, which will manage all projectiles fired from the turret.

```vb
    Public Structure Projectile
```
- **Public Structure Projectile**: This defines a nested structure called `Projectile`, which represents individual projectiles with various properties.

```vb
        Public X, Y, Width, Height As Double
```
- **Public X, Y, Width, Height As Double**: These variables represent the position (X, Y) and size (Width, Height) of the projectile.

```vb
        Public Velocity As PointF
```
- **Public Velocity As PointF**: This variable stores the velocity of the projectile, including its direction and speed.

```vb
        Public Brush As Brush
```
- **Public Brush As Brush**: This variable is used to define the color/appearance of the projectile when it is drawn.

```vb
        Public Center As PointF
```
- **Public Center As PointF**: This variable represents the center point of the projectile.

```vb
        Public Length As Integer
```
- **Public Length As Integer**: This variable defines the length of the projectile.

```vb
        Public AngleInDegrees As Single
```
- **Public AngleInDegrees As Single**: This variable stores the angle at which the projectile is fired.

```vb
        Public Creation As DateTime
```
- **Public Creation As DateTime**: This variable records the time when the projectile was created.

### Projectile Constructor

```vb
Public Sub New(brush As Brush, width As Double, height As Double, velocity As Single, center As PointF, length As Integer, angleInDegrees As Single)
```
- **Public Sub New(...)**: This constructor initializes a new instance of the `Projectile` structure with specific values.

```vb
    Me.Brush = brush
```
- **Me.Brush = brush**: This assigns the `brush` parameter to the `Brush` variable of the projectile.

```vb
    Me.Width = width
```
- **Me.Width = width**: This assigns the `width` parameter to the `Width` variable of the projectile.

```vb
    Me.Height = height
```
- **Me.Height = height**: This assigns the `height` parameter to the `Height` variable of the projectile.

```vb
    Me.Center = center
```
- **Me.Center = center**: This assigns the `center` parameter to the `Center` variable of the projectile.

```vb
    Me.Length = length
```
- **Me.Length = length**: This assigns the `length` parameter to the `Length` variable of the projectile.

```vb
    Me.AngleInDegrees = angleInDegrees
```
- **Me.AngleInDegrees = angleInDegrees**: This assigns the `angleInDegrees` parameter to the `AngleInDegrees` variable of the projectile.

```vb
    Dim AngleInRadians As Single = angleInDegrees * (Math.PI / 180)
```
- **Dim AngleInRadians As Single = angleInDegrees * (Math.PI / 180)**: This converts the angle from degrees to radians for calculations.

```vb
    X = center.X + length * Cos(AngleInRadians)
```
- **X = center.X + length * Cos(AngleInRadians)**: This calculates the initial x-coordinate of the projectile based on its center, length, and angle.

```vb
    Y = center.Y + length * Sin(AngleInRadians)
```
- **Y = center.Y + length * Sin(AngleInRadians)**: This calculates the initial y-coordinate of the projectile based on its center, length, and angle.

```vb
    Select Case angleInDegrees
```
- **Select Case angleInDegrees**: This begins a case statement that will set the velocity of the projectile based on its firing angle.

```vb
        Case 0
```
- **Case 0**: This case handles when the angle is 0 degrees (firing directly to the right).

```vb
            Me.Velocity.X = velocity
            Me.Velocity.Y = 0
            Y -= Me.Height / 2
```
- **Me.Velocity.X = velocity**: Sets the x-velocity to the specified speed.
- **Me.Velocity.Y = 0**: Sets the y-velocity to 0 (no vertical movement).
- **Y -= Me.Height / 2**: Adjusts the y-position to center the projectile vertically.

```vb
        Case 360
```
- **Case 360**: This case is similar to the 0-degree case, as 360 degrees points directly to the right.

```vb
            Me.Velocity.X = velocity
            Me.Velocity.Y = 0
            Y -= Me.Height / 2
```
- Same as above, setting velocity and adjusting position.

```vb
        Case 45
```
- **Case 45**: This case handles when the angle is 45 degrees (firing diagonally up-right).

```vb
            Me.Velocity.X = velocity
            Me.Velocity.Y = velocity
```
- **Me.Velocity.X = velocity**: Sets the x-velocity to the specified speed.
- **Me.Velocity.Y = velocity**: Sets the y-velocity to the same speed, moving diagonally.

```vb
        Case 90
```
- **Case 90**: This case handles when the angle is 90 degrees (firing directly upwards).

```vb
            Me.Velocity.X = 0
            Me.Velocity.Y = velocity
            X -= Me.Width / 2 - 1
```
- **Me.Velocity.X = 0**: Sets the x-velocity to 0 (no horizontal movement).
- **Me.Velocity.Y = velocity**: Sets the y-velocity to the specified speed.
- **X -= Me.Width / 2 - 1**: Adjusts the x-position to center the projectile horizontally.

```vb
        Case 135
```
- **Case 135**: This case handles when the angle is 135 degrees (firing diagonally up-left).

```vb
            Me.Velocity.X = -velocity
            Me.Velocity.Y = velocity
            X -= Me.Width / 2
            Y -= Me.Height / 4
```
- **Me.Velocity.X = -velocity**: Sets the x-velocity to negative, moving left.
- **Me.Velocity.Y = velocity**: Sets the y-velocity to the specified speed, moving upwards.
- **X -= Me.Width / 2**: Adjusts the x-position to center the projectile horizontally.
- **Y -= Me.Height / 4**: Adjusts the y-position slightly upwards.

```vb
        Case 180
```
- **Case 180**: This case handles when the angle is 180 degrees (firing directly to the left).

```vb
            Me.Velocity.X = -velocity
            Me.Velocity.Y = 0
            Y -= Me.Height / 2
```
- **Me.Velocity.X = -velocity**: Sets the x-velocity to negative, moving left.
- **Me.Velocity.Y = 0**: Sets the y-velocity to 0 (no vertical movement).
- **Y -= Me.Height / 2**: Adjusts the y-position to center the projectile vertically.

```vb
        Case 225
```
- **Case 225**: This case handles when the angle is 225 degrees (firing diagonally down-left).

```vb
            Me.Velocity.X = -velocity
            Me.Velocity.Y = -velocity
```
- **Me.Velocity.X = -velocity**: Sets the x-velocity to negative, moving left.
- **Me.Velocity.Y = -velocity**: Sets the y-velocity to negative, moving downwards.

```vb
        Case 270
```
- **Case 270**: This case handles when the angle is 270 degrees (firing directly downwards).

```vb
            Me.Velocity.X = 0
            Me.Velocity.Y = -velocity
            X -= Me.Width / 2 - 1
```
- **Me.Velocity.X = 0**: Sets the x-velocity to 0 (no horizontal movement).
- **Me.Velocity.Y = -velocity**: Sets the y-velocity to negative, moving downwards.
- **X -= Me.Width / 2 - 1**: Adjusts the x-position to center the projectile horizontally.

```vb
        Case 315
```
- **Case 315**: This case handles when the angle is 315 degrees (firing diagonally down-right).

```vb
            Me.Velocity.X = velocity
            Me.Velocity.Y = -velocity
            X -= Me.Width / 2
            Y -= Me.Height / 4
```
- **Me.Velocity.X = velocity**: Sets the x-velocity to the specified speed, moving right.
- **Me.Velocity.Y = -velocity**: Sets the y-velocity to negative, moving downwards.
- **X -= Me.Width / 2**: Adjusts the x-position to center the projectile horizontally.
- **Y -= Me.Height / 4**: Adjusts the y-position to center the projectile vertically.



```vb
        Case Else
```
- **Case Else**: This handles any angles that do not match the specified cases (0, 45, 90, etc.).

```vb
            Debug.Print("Projectile was not set to an angle of fire in 45° increments.")
```
- **Debug.Print(...)**: This outputs a message to the debug console, indicating that the projectile's angle was not set correctly. This is useful for debugging purposes.

```vb
    Creation = Now()
```
- **Creation = Now()**: This sets the `Creation` variable to the current time, marking when the projectile was created.

### UpdateMovement Method

```vb
Public Sub UpdateMovement(ByVal deltaTime As TimeSpan)
```
- **Public Sub UpdateMovement(ByVal deltaTime As TimeSpan)**: This method updates the position of the projectile based on its velocity and the elapsed time.

```vb
    X += Velocity.X * deltaTime.TotalSeconds 'Δs = V * Δt
```
- **X += Velocity.X * deltaTime.TotalSeconds**: This updates the x-coordinate of the projectile by adding the product of its x-velocity and the total elapsed time in seconds. This follows the formula for displacement (Δs = V * Δt).

```vb
    Y += Velocity.Y * deltaTime.TotalSeconds 'Δs = V * Δt
```
- **Y += Velocity.Y * deltaTime.TotalSeconds**: This updates the y-coordinate of the projectile similarly, using its y-velocity.

### NearestX, NearestY, NearestWidth, NearestHeight Methods

```vb
Public Function NearestX() As Integer
```
- **Public Function NearestX() As Integer**: This method rounds the x-coordinate of the projectile to the nearest integer.

```vb
    Return RoundToNearest(X)
```
- **Return RoundToNearest(X)**: This calls the `RoundToNearest` function to round the x-coordinate.

```vb
Public Function NearestY() As Integer
```
- **Public Function NearestY() As Integer**: Similar to `NearestX`, this method rounds the y-coordinate.

```vb
    Return RoundToNearest(Y)
```
- **Return RoundToNearest(Y)**: This rounds the y-coordinate to the nearest integer.

```vb
Public Function NearestWidth() As Integer
```
- **Public Function NearestWidth() As Integer**: This method rounds the width of the projectile to the nearest integer.

```vb
    Return RoundToNearest(Width)
```
- **Return RoundToNearest(Width)**: This rounds the width.

```vb
Public Function NearestHeight() As Integer
```
- **Public Function NearestHeight() As Integer**: This method rounds the height of the projectile to the nearest integer.

```vb
    Return RoundToNearest(Height)
```
- **Return RoundToNearest(Height)**: This rounds the height.

### RoundToNearest Function

```vb
Private Function RoundToNearest(ByVal value As Double) As Integer
```
- **Private Function RoundToNearest(ByVal value As Double) As Integer**: This private function rounds a given double value to the nearest integer.

```vb
    Return CInt(Math.Round(value))
```
- **Return CInt(Math.Round(value))**: This uses `Math.Round` to round the value and then converts it to an integer using `CInt`.

### ProjectileManager Variables

```vb
Private Projectiles() As Projectile
```
- **Private Projectiles() As Projectile**: This declares an array of `Projectile` structures to hold all the projectiles.

```vb
Public Brush As Brush
```
- **Public Brush As Brush**: This variable defines the brush used to draw the projectiles.

```vb
Public BarrelLength As Integer
```
- **Public BarrelLength As Integer**: This variable stores the length of the turret's barrel.

```vb
Public MuzzleVelocity As Single
```
- **Public MuzzleVelocity As Single**: This variable defines the initial speed of the projectiles when fired.

```vb
Public Size As Size
```
- **Public Size As Size**: This variable stores the size of the projectiles.

```vb
Public LifeTimeInSeconds As Integer
```
- **Public LifeTimeInSeconds As Integer**: This variable defines how long the projectiles will remain active before being removed.

### ProjectileManager Constructor

```vb
Public Sub New(brush As Brush, size As Size, muzzleVelocity As Single, barrelLength As Integer, lifeTimeInSeconds As Integer)
```
- **Public Sub New(...)**: This constructor initializes a new instance of the `ProjectileManager` structure with specific values.

```vb
    Me.Brush = brush
```
- **Me.Brush = brush**: This assigns the `brush` parameter to the `Brush` variable.

```vb
    Me.BarrelLength = barrelLength
```
- **Me.BarrelLength = barrelLength**: This assigns the `barrelLength` parameter to the `BarrelLength` variable.

```vb
    Me.MuzzleVelocity = muzzleVelocity
```
- **Me.MuzzleVelocity = muzzleVelocity**: This assigns the `muzzleVelocity` parameter to the `MuzzleVelocity` variable.

```vb
    Me.Size = size
```
- **Me.Size = size**: This assigns the `size` parameter to the `Size` variable.

```vb
    Me.LifeTimeInSeconds = lifeTimeInSeconds
```
- **Me.LifeTimeInSeconds = lifeTimeInSeconds**: This assigns the `lifeTimeInSeconds` parameter to the `LifeTimeInSeconds` variable.

### DrawProjectiles Method

```vb
Public Sub DrawProjectiles(g As Graphics)
```
- **Public Sub DrawProjectiles(g As Graphics)**: This method draws all active projectiles on the graphics surface.

```vb
    If Projectiles IsNot Nothing Then
```
- **If Projectiles IsNot Nothing Then**: This checks if there are any projectiles in the array.

```vb
        For Each Projectile In Projectiles
```
- **For Each Projectile In Projectiles**: This starts a loop to iterate through each projectile in the `Projectiles` array.

```vb
            g.FillRectangle(Projectile.Brush, Projectile.NearestX, Projectile.NearestY, Projectile.NearestWidth, Projectile.NearestHeight)
```
- **g.FillRectangle(...)**: This draws each projectile as a filled rectangle using its brush and rounded dimensions.

```vb
        Next
```
- **Next**: This ends the loop.

```vb
    End If
```
- **End If**: This ends the conditional check for projectiles.

### FireProjectile Method

```vb
Public Sub FireProjectile(CenterOfFire As PointF, AngleInDegrees As Single)
```
- **Public Sub FireProjectile(CenterOfFire As PointF, AngleInDegrees As Single)**: This method is called to fire a projectile from a specified center point at a given angle.

```vb
    AddProjectile(CenterOfFire, AngleInDegrees)
```
- **AddProjectile(CenterOfFire, AngleInDegrees)**: This calls another method to add a new projectile to the array.

### AddProjectile Method

```vb
Private Sub AddProjectile(CenterOfFire As PointF, AngleInDegrees As Single)
```
- **Private Sub AddProjectile(CenterOfFire As PointF, AngleInDegrees As Single)**: This method adds a new projectile to the array.

```vb
    If Projectiles IsNot Nothing Then
```
- **If Projectiles IsNot Nothing Then**: This checks if there are already projectiles in the array.

```vb
        Array.Resize(Projectiles, Projectiles.Length + 1)
```
- **Array.Resize(Projectiles, Projectiles.Length + 1)**: This increases the size of the `Projectiles` array by one to accommodate the new projectile.

```vb
    Else
```
- **Else**: This begins the alternative case if there are no existing projectiles.

```vb
        ReDim Projectiles(0)
```
- **ReDim Projectiles(0)**: This initializes the `Projectiles` array with one element.

```vb
    End If
```
- **End If**: This ends the conditional check for existing projectiles.

```vb
    Dim Index As Integer = Projectiles.Length - 1
```
- **Dim Index As Integer = Projectiles.Length - 1**: This sets the `Index` variable to the last position in the array, where the new projectile will be stored.

```vb
    Projectiles(Index) = New Projectile(Brush, Size.Width, Size.Height, MuzzleVelocity, CenterOfFire, BarrelLength, AngleInDegrees)
```
- **Projectiles(Index) = New Projectile(...)**: This creates a new instance of the `Projectile` structure and stores it in the array at the `Index` position.

### UpdateProjectiles Method

```vb
Public Sub UpdateProjectiles(deltaTime As TimeSpan)
```
- **Public Sub UpdateProjectiles(deltaTime As TimeSpan)**: This method updates the positions of all active projectiles based on the elapsed time.

```vb
    Dim ElapsedTime As TimeSpan
```
- **Dim ElapsedTime As TimeSpan**: This declares a variable to track the elapsed time since each projectile was created.

```vb
    If Projectiles IsNot Nothing Then
```
- **If Projectiles IsNot Nothing Then**: This checks if there are any projectiles in the array.

```vb
        For Each Projectile In Projectiles
```
- **For Each Projectile In Projectiles**: This starts a loop to iterate through each projectile in the `Projectiles` array.

```vb
            Dim Index As Integer = Array.IndexOf(Projectiles, Projectile)
```
- **Dim Index As Integer = Array.IndexOf(Projectiles, Projectile)**: This retrieves the index of the current projectile in the array.

```vb
            ElapsedTime = Now - Projectile.Creation
```
- **ElapsedTime = Now - Projectile.Creation**: This calculates the time elapsed since the projectile was created.

```vb
            If ElapsedTime < TimeSpan.FromSeconds(LifeTimeInSeconds) Then
```
- **If ElapsedTime < TimeSpan.FromSeconds(LifeTimeInSeconds) Then**: This checks if the projectile's lifetime has not yet expired.

```vb
                Projectiles(Index).UpdateMovement(deltaTime)
```
- **Projectiles(Index).UpdateMovement(deltaTime)**: If the projectile is still active, this calls the `UpdateMovement` method to update its position.

```vb
            Else
```
- **Else**: This begins the alternative case if the projectile's lifetime has expired.

```vb
                RemoveProjectile(Index)
```
- **RemoveProjectile(Index)**: This calls a method to remove the expired projectile from the array.

```vb
            End If
```
- **End If**: This ends the conditional check for the projectile's lifetime.

```vb
        Next
```
- **Next**: This ends the loop.

```vb
    End If
```
- **End If**: This ends the conditional check for existing projectiles.

### RemoveProjectile Method

```vb
Private Sub RemoveProjectile(Index As Integer)
```
- **Private Sub RemoveProjectile(Index As Integer)**: This method removes a projectile from the array at the specified index.

```vb
    Projectiles = Projectiles.Where(Function(e, i) i <> Index).ToArray()
```
- **Projectiles = Projectiles.Where(Function(e, i) i <> Index).ToArray()**: This uses LINQ to filter the `Projectiles` array, keeping only those projectiles whose index does not match the specified index. The result is converted back to an array.

## AudioPlayer Structure

```vb
Public Structure AudioPlayer
```
- **Public Structure AudioPlayer**: This defines a new structure called `AudioPlayer`, which manages sound playback.

```vb
    <DllImport("winmm.dll", EntryPoint:="mciSendStringW")>
```
- **<DllImport(...)>**: This attribute allows the use of the Windows API function `mciSendStringW`, enabling us to send commands to the multimedia control interface for audio playback.

```vb
    Private Shared Function mciSendStringW(<MarshalAs(UnmanagedType.LPWStr)> ByVal lpszCommand As String, <MarshalAs(UnmanagedType.LPWStr)> ByVal lpszReturnString As StringBuilder, ByVal cchReturn As UInteger, ByVal hwndCallback As IntPtr) As Integer
```
- **Private Shared Function mciSendStringW(...)**: This declares the `mciSendStringW` function, which sends commands to the multimedia control interface. The parameters allow us to specify commands, receive return strings, and handle callbacks.

```vb
    End Function
```
- **End Function**: This ends the declaration of the `mciSendStringW` function.

### AddSound Method

```vb
Public Function AddSound(SoundName As String, FilePath As String) As Boolean
```
- **Public Function AddSound(SoundName As String, FilePath As String) As Boolean**: This method adds a sound to the audio player.

```vb
    If Not String.IsNullOrWhiteSpace(SoundName) AndAlso IO.File.Exists(FilePath) Then
```
- **If Not String.IsNullOrWhiteSpace(SoundName) AndAlso IO.File.Exists(FilePath) Then**: This checks if the sound name is not empty and if the specified file exists.

```vb
        Dim CommandOpen As String = $"open ""{FilePath}"" alias {SoundName}"
```
- **Dim CommandOpen As String = $"open ""{FilePath}"" alias {SoundName}"**: This constructs a command string to open the sound file and assign it an alias.

```vb
        If Sounds Is Nothing Then
```
- **If Sounds Is Nothing Then**: This checks if the `Sounds` array is empty.

```vb
            If SendMciCommand(CommandOpen, IntPtr.Zero) Then
```
- **If SendMciCommand(CommandOpen, IntPtr.Zero) Then**: This sends the command to open the sound file. If successful, it proceeds to add the sound.

```vb
                ReDim Sounds(0)
```
- **ReDim Sounds(0)**: This initializes the `Sounds` array with one element.

```vb
                Sounds(0) = SoundName
```
- **Sounds(0) = SoundName**: This stores the sound name in the first position of the array.

```vb
                Return True ' The sound was added.
```
- **Return True**: This returns `True`, indicating that the sound was successfully added.

```vb
            End If
```
- **End If**: This ends the conditional check for opening the sound file.

```vb
        ElseIf Not Sounds.Contains(SoundName) Then
```
- **ElseIf Not Sounds.Contains(SoundName) Then**: This checks if the sound is not already in the array.

```vb
            If SendMciCommand(CommandOpen, IntPtr.Zero) Then
```
- **If SendMciCommand(CommandOpen, IntPtr.Zero) Then**: This attempts to open the sound file again.

```vb
                Array.Resize(Sounds, Sounds.Length + 1)
```
- **Array.Resize(Sounds, Sounds.Length + 1)**: This increases the size of the `Sounds` array by one to accommodate the new sound.

```vb
                Sounds(Sounds.Length - 1) = SoundName
```
- **Sounds(Sounds.Length - 1) = SoundName**: This adds the new sound name to the last position in the array.

```vb
                Return True ' The sound was added.
```
- **Return True**: This returns `True`, indicating that the sound was successfully added.

```vb
            End If
```
- **End If**: This ends the conditional check for opening the sound file.

```vb
    End If
```
- **End If**: This ends the initial conditional check for sound name and file existence.

```vb
    Debug.Print($"{SoundName} not added to sounds.")
```
- **Debug.Print(...)**: This outputs a message to the debug console, indicating that the sound could not be added.

```vb
    Return False ' The sound was not added.
```
- **Return False**: This returns `False`, indicating that the sound was not added.

### SetVolume Method

```vb
Public Function SetVolume(SoundName As String, Level As Integer) As Boolean
```
- **Public Function SetVolume(SoundName As String, Level As Integer) As Boolean**: This method sets the volume level for a specified sound.

```vb
    If Sounds IsNot Nothing AndAlso Sounds.Contains(SoundName) AndAlso Level >= 0 AndAlso Level <= 1000 Then
```
- **If Sounds IsNot Nothing AndAlso Sounds.Contains(SoundName) AndAlso Level >= 0 AndAlso Level <= 1000 Then**: This checks if the sounds array is not empty, if the sound exists, and if the volume level is within a valid range.

```vb
        Dim CommandVolume As String = $"setaudio {SoundName} volume to {Level}"
```
- **Dim CommandVolume As String = $"setaudio {SoundName} volume to {Level}"**: This constructs a command string to set the volume for the specified sound.

```vb
        Return SendMciCommand(CommandVolume, IntPtr.Zero) ' The volume was set.
```
- **Return SendMciCommand(CommandVolume, IntPtr.Zero)**: This sends the command to set the volume and returns the result.

```vb
    End If
```
- **End If**: This ends the conditional check for valid parameters.

```vb
    Debug.Print($"{SoundName} volume not set.")
```
- **Debug.Print(...)**: This outputs a message indicating that the volume could not be set.

```vb
    Return False ' The volume was not set.
```
- **Return False**: This returns `False`, indicating that the volume was not set.

### LoopSound Method

```vb
Public Function LoopSound(SoundName As String) As Boolean
```
- **Public Function LoopSound(SoundName As String) As Boolean**: This method sets a specified sound to loop.

```vb
    If Sounds IsNot Nothing AndAlso Sounds.Contains(SoundName) Then
```
- **If Sounds IsNot Nothing AndAlso Sounds.Contains(SoundName) Then**: This checks if the sounds array is not empty and if the sound exists.

```vb
        Dim CommandSeekToStart As String = $"seek {SoundName}


