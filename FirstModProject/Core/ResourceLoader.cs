using FirstModProject.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FirstModProject.Core
{
    public static class ResourceLoader
    {
        public static Texture2D LoadTextureFromResources(string resourcePath)
        {
            LoggerUtils.LogResource($"Attempting to load texture from: {resourcePath}");

            Assembly assembly = Assembly.GetExecutingAssembly();

            try
            {
                using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
                {
                    if (stream == null)
                    {
                        LoggerUtils.LogError("ResourceLoader", $"Resource not found: {resourcePath}");
                        LogAvailableResources();
                        return null;
                    }

                    byte[] buffer = new byte[stream.Length];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);

                    if (bytesRead != buffer.Length)
                    {
                        LoggerUtils.LogError("ResourceLoader", $"Failed to read complete stream. Expected: {buffer.Length}, Read: {bytesRead}");
                        return null;
                    }

                    Texture2D texture = new Texture2D(2, 2);

                    if (ImageConversion.LoadImage(texture, buffer))
                    {
                        LoggerUtils.LogResource($"Successfully loaded texture: {resourcePath} ({texture.width}x{texture.height})");
                        return texture;
                    }
                    else
                    {
                        LoggerUtils.LogError("ResourceLoader", $"Failed to convert bytes to image for resource: {resourcePath}");
                        GameObject.Destroy(texture);
                        return null;
                    }
                }
            }
            catch (System.Exception ex)
            {
                LoggerUtils.LogError("ResourceLoader", $"Exception loading resource {resourcePath}: {ex.Message}");
                return null;
            }
        }
        public static Sprite CreateSpriteFromTexture(Texture2D texture)
        {
            if (texture == null)
            {
                LoggerUtils.LogError("ResourceLoader", "Cannot create sprite from null texture");
                return null;
            }

            Sprite sprite = Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f)
            );

            LoggerUtils.LogResource($"Created sprite from texture ({texture.width}x{texture.height})");
            return sprite;
        }
        private static void LogAvailableResources()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string[] resourceNames = assembly.GetManifestResourceNames();

            LoggerUtils.LogResource($"Available embedded resources ({resourceNames.Length}):");
            foreach (string resourceName in resourceNames)
            {
                LoggerUtils.LogResource($"  - {resourceName}");
            }
        }
    }
}
