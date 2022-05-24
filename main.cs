using IWshRuntimeLibrary;
using Newtonsoft.Json;
using System.Diagnostics;

namespace main
{
    public class logic {
        public Dictionary<string, string[]> Unpack(string[] data) {
            return new Dictionary<string, string[]> {
                { "event", new string[] { data[0] } }, //event name/id
                { "event", new string[] { data[1] } }, //event handle
                { "arguments", new string[] { data[2] } }, //event array for event, such as text
                { "functions", new string[] { data[3] } } //event actions, call functions before/during/or after event.
            }; 
        } //data.Split(":")
        public void Save() { }
        public void Load(string data, Dictionary<string, string[]> database) { 
            for (int i = 0; i < database["events"].Length; i++) { 
                if (database["events"][i] == data) {
                    Unpack(database["events"][i].Split(":"));
                    break;
                } 
            } 
        } //load specific name/id from data.json
    }
    public class main { 
        static void Main(string[] args) {
            string path = AppDomain.CurrentDomain.BaseDirectory; List<string> icons = new List<string>();
            Dictionary<string, string[]> data = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(System.IO.File.ReadAllText(path + "/bin/data.json")); //Add system to create files if failed to find any
            Dictionary<string, List<string>> App = new Dictionary<string, List<string>>(); 
                App.Add("name", data["names"].ToList() ); //add random name system
                App.Add("attributes", new List<string> {  });
            if (args.Length > 0) {
                for (int i = 0; i < args.Length; i++) {
                    switch (args[i]) {
                        case "boot":
                            App["attributes"].Add("running");
                            break;
                        case "modded":
                            App["attributes"].Add("modded");
                            break;
                        default:
                            if (args[i] == "debug" && App["attributes"].ToArray().Contains("running")){
                                App["attributes"].Add("debug");
                            }
                            break;
                    }
                    if (args.Length == i+1) { //move to default
                        if (App["attributes"].Contains("running")) { //not necessary, fix later
                            for (int a = 0; a < data["boot"].Length; a++) {
                                Console.WriteLine(data["boot"][a]); //add coloring
                                Thread.Sleep(int.Parse(data["settings"][0].Split(":")[1]) / data["boot"].Length); //fix to finder function instead of relying that boot_time is in the first position
                            }
                            
                            if (App["attributes"].Contains("debug")) { } //open debugging here
                        }
                    }
                }
            } else {
                foreach (FileInfo file in new DirectoryInfo(path + "/bin/icos").GetFiles("*.ico")) { //Add system to create files if failed to find any
                    icons.Add(file.Name); }
                
                    WshShell shell = new WshShell();
                    IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut((string)shell.SpecialFolders.Item("Desktop") + "/ Pluto.lnk");
                        shortcut.Description = "trolled and seethed";
                        shortcut.IconLocation = @Directory.GetCurrentDirectory() + "/bin/icos/" + icons.ToArray()[new Random().Next(0, icons.Count)];
                        shortcut.TargetPath = path + "/Pluto.exe";
                        shortcut.Arguments = "boot";
                    shortcut.Save(); try { Microsoft.VisualBasic.FileIO.FileSystem.RenameFile((string)shell.SpecialFolders.Item("Desktop") + "/ Pluto.lnk", " " + App["name"].ToArray()[new Random().Next(0, App["name"].Count)] + ".lnk"); } catch (IOException e) { Microsoft.VisualBasic.FileIO.FileSystem.RenameFile((string)shell.SpecialFolders.Item("Desktop") + "/ Pluto.lnk", " " + new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 5).Select(s => s[new Random().Next(s.Length)]).ToArray()) + ".lnk"); };
            }
        }
    }
}