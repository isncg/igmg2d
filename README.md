## 2D Imdiate Graphics for MonoGame

### Usage

- 1 Initialize

```csharp
protected override void LoadContent()
{
    _canvas = new Canvas2D(GraphicsDevice);
    _texture = Content.Load<Texture2D>("avatar");
    GraphicsDevice.BlendState = BlendState.AlphaBlend;
}
```

- 2 Drawing data

```csharp
protected override void Update(GameTime gameTime)
{
    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        Exit();

    // TODO: Add your update logic here
    var now = System.DateTime.Now;
    float thetaSec = (float)(now.Second * Math.PI / 30);
    float thetaMin = (float)(now.Minute * Math.PI / 30);
    float thetaHour = (float)(now.Hour * Math.PI / 6) + thetaMin / 12;
    _canvas.Clear();

    for (int i = 0; i < 12; i++)
    {
        float theta = (float)(i * Math.PI / 6);
        Vector2 dir = new Vector2(System.MathF.Sin(theta), MathF.Cos(theta));
        _canvas.ThickLineBegin(dir, Color.Black, 0.05f);
        _canvas.ThickLineTo(dir * 0.9f, 0.02f);
    }

    _canvas.ThickLineBegin(Vector2.Zero, Color.Gray, 0.12f);
    _canvas.ThickLineTo(new Vector2(System.MathF.Sin(thetaHour), System.MathF.Cos(thetaHour)) * 0.5f, 0.0001f);
    _canvas.Outline(0.01f, Color.CornflowerBlue);
    _canvas.ThickLineBegin(Vector2.Zero, Color.Black, 0.1f);
    _canvas.ThickLineTo(new Vector2(System.MathF.Sin(thetaMin), System.MathF.Cos(thetaMin)) * 0.7f, 0.0001f);
    _canvas.Outline(0.01f, Color.CornflowerBlue);
    _canvas.LineBegin(Vector2.Zero, Color.Red);
    _canvas.LineTo(new Vector2(System.MathF.Sin(thetaSec), System.MathF.Cos(thetaSec)) * 0.8f);
    _canvas.Rectangle(Vector2.Zero, new Vector2(0.2f, 0.2f), _texture);
    _canvas.Outline(0.01f, Color.CornflowerBlue);

    base.Update(gameTime);
}
```

- 3 Draw

```csharp
protected override void Draw(GameTime gameTime)
{
    GraphicsDevice.Clear(Color.White);

    _canvas.Draw();

    base.Draw(gameTime);
}
```
