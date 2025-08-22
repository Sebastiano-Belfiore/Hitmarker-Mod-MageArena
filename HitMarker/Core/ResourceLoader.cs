using HitMarker.Utils;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace HitMarker.Core
{
    public static class ResourceLoader
    {
        public static Texture2D LoadTextureFromResources(string resourcePath)
        {
            LoggerUtils.LogDebug("ResourceLoader", $"Loading texture: {resourcePath}");

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
                        LoggerUtils.LogError("ResourceLoader", $"Incomplete stream read. Expected: {buffer.Length}, Read: {bytesRead}");
                        return null;
                    }

                    Texture2D texture = new Texture2D(2, 2);

                    if (ImageConversion.LoadImage(texture, buffer))
                    {
                        LoggerUtils.LogInfo("ResourceLoader", $"Texture loaded successfully ({texture.width}x{texture.height})");
                        return texture;
                    }
                    else
                    {
                        LoggerUtils.LogError("ResourceLoader", "Failed to convert bytes to image");
                        GameObject.Destroy(texture);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerUtils.LogCriticalError("ResourceLoader", $"Exception loading resource {resourcePath}", ex);
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

            try
            {
                Sprite sprite = Sprite.Create(
                    texture,
                    new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f)
                );

                LoggerUtils.LogDebug("ResourceLoader", $"Sprite created ({texture.width}x{texture.height})");
                return sprite;
            }
            catch (Exception ex)
            {
                LoggerUtils.LogError("ResourceLoader", $"Failed to create sprite: {ex.Message}");
                return null;
            }
        }

        private static void LogAvailableResources()
        {
            if (LoggerUtils.MinLogLevel > LogLevel.Debug) return;

            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                string[] resourceNames = assembly.GetManifestResourceNames();

                LoggerUtils.LogDebug("ResourceLoader", $"Available embedded resources ({resourceNames.Length}):");
                foreach (string resourceName in resourceNames)
                {
                    LoggerUtils.LogDebug("ResourceLoader", $"  - {resourceName}");
                }
            }
            catch (Exception ex)
            {
                LoggerUtils.LogError("ResourceLoader", $"Failed to list resources: {ex.Message}");
            }
        }
    }
}