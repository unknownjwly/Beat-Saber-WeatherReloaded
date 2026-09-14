using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace WeatherReloaded
{
    internal static class Utils
    {
        private static Texture2D? LoadTextureRaw(byte[]? file)
        {
            if (file == null || !file.Any()) return null;
            var tex2D = new Texture2D(2, 2);
            return tex2D.LoadImage(file) ? tex2D : null;
        }

        public static Sprite LoadSpriteFromResources(string resourcePath, float pixelsPerUnit = 100.0f)
        {
            var rawData = LoadFromResource(resourcePath);
            var sprite = LoadSpriteRaw(rawData, pixelsPerUnit);
            
            if (sprite == null)
            {
                var fallbackTex = new Texture2D(2, 2);
                return Sprite.Create(fallbackTex, new Rect(0, 0, 2, 2), new Vector2(0, 0), pixelsPerUnit);
            }
            return sprite;
        }

        private static Sprite LoadSpriteRaw(byte[]? image, float pixelsPerUnit = 100.0f)
        {
            return LoadSpriteFromTexture(LoadTextureRaw(image), pixelsPerUnit);
        }

        private static Sprite LoadSpriteFromTexture(Texture2D? spriteTexture, float pixelsPerUnit = 100.0f)
        {
            return (spriteTexture ? Sprite.Create(spriteTexture, new Rect(0, 0, spriteTexture!.width, spriteTexture.height), new Vector2(0, 0), pixelsPerUnit) : null)!;
        }

        private static byte[]? LoadFromResource(string resourcePath)
        {
            return GetResource(Assembly.GetCallingAssembly(), resourcePath);
        }

        private static byte[]? GetResource(Assembly assembly, string resourcePath)
        {
            using var stream = assembly.GetManifestResourceStream(resourcePath);
            if (stream == null) return null;

            var data = new byte[stream.Length];
            stream.Read(data, 0, (int)stream.Length);
            return data;
        }
    }
}