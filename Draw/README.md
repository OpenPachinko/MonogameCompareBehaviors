I tried saving RenderTarget2D to a file on Android.
It was drawn properly.
Problems with spriteBatch.Draw?

```
if (Game1.Activity.CheckSelfPermission(Android.Manifest.Permission.WriteExternalStorage) == Android.Content.PM.Permission.Denied)
{
    Game1.Activity.RequestPermissions(new[] { Android.Manifest.Permission.WriteExternalStorage }, 1);
}
else
{
    var path = Path.Combine( Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "colorRT.jpg");
    colorRT.SaveAsJpeg(File.OpenWrite(path), colorRT.Width, colorRT.Height);
}
```
