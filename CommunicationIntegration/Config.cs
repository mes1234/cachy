using PowerArgs;

namespace Cachy.CommunicationIntegration
{
    public class Config
    {
        [HelpHook, ArgShortcut("-h"), ArgDescription("Shows this help")]
        public bool Help { get; set; }
        [ArgRequired(PromptIfMissing = true), ArgDescription("Defines in which mode run app: ping, add, get")]
        public string Mode { get; set; }
    }
}