using CommandLine;

namespace FabulaUltimaDataImporter.Processor
{
    internal class Options
    {
        [Option("create", Required = false, HelpText = "create database for first time")]
        public bool Create { get; set; } = false;

        [Option("filename", Required = true, HelpText = "target db")]
        public string Filename { get; set; } = string.Empty;


        [Option("reloadKnownSkills", Required = false, Default = false, HelpText = "reload known skills")]
        public bool ReloadKnownSkills { get; set; }

        [Option("inputTape", Required = false, Default = "", HelpText = "Run inputs in order")]
        public string InputTape { get; set; } = string.Empty;
    }
}
