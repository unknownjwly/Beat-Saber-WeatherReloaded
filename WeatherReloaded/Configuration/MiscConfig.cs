using System.IO;
using System.Linq;
using IPA.Utilities;

namespace WeatherReloaded.Configuration
{
    public class MiscConfigObject
    {
        public string Name { get; set; }
        public bool ShowInMenu { get; set; }
        public bool ShowInGame { get; set; }

        public MiscConfigObject(string name, bool showInMenu, bool showInGame)
        {
            Name = name;
            ShowInMenu = showInMenu;
            ShowInGame = showInGame;
        }
    }

    public static class MiscConfig
    {
        private static string GetConfigPath()
        {
            return Path.Combine(UnityGame.UserDataPath, "WeatherMisConfig.txt");
        }

        public static void Read()
        {
            var path = GetConfigPath();
            if (!File.Exists(path))
            {
                File.WriteAllText(path, string.Empty);
            }
        }

        public static bool HasObject(string name)
        {
            var path = GetConfigPath();
            if (!File.Exists(path))
            {
                File.WriteAllText(path, string.Empty);
                return false;
            }

            var lines = File.ReadAllLines(path);
            return lines.Any(line => line.Contains(name));
        }

        public static MiscConfigObject ReadObject(string name)
        {
            var path = GetConfigPath();
            if (!File.Exists(path)) return new MiscConfigObject(name, true, true);

            var lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                if (!line.Contains(name)) continue;
                var parts = line.Split(',');
                if (parts.Length >= 3 && 
                    bool.TryParse(parts[1], out var menu) && 
                    bool.TryParse(parts[2], out var game))
                {
                    return new MiscConfigObject(parts[0], menu, game);
                }
            }

            return new MiscConfigObject(name, true, true);
        }

        public static void Add(MiscConfigObject configObject)
        {
            var path = GetConfigPath();
            var entry = $"{configObject.Name},{configObject.ShowInMenu},{configObject.ShowInGame}";
            File.AppendAllLines(path, new[] { entry });
        }

        public static void WriteToObject(MiscConfigObject configObject)
        {
            var path = GetConfigPath();
            if (!File.Exists(path))
            {
                Add(configObject);
                return;
            }

            var lines = File.ReadAllLines(path).ToList();
            bool found = false;

            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].StartsWith(configObject.Name + ","))
                {
                    lines[i] = $"{configObject.Name},{configObject.ShowInMenu},{configObject.ShowInGame}";
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                lines.Add($"{configObject.Name},{configObject.ShowInMenu},{configObject.ShowInGame}");
            }

            File.WriteAllLines(path, lines);
        }

        public static void Write()
        {
            // Placeholder for saving state changes if handled dynamically
        }
    }
}