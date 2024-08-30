using System.Collections.Generic;

namespace FabulaUltimaGMTool.BeastiaryScenes
{
    internal interface IValidatable
    {
        string Name { get; }

        IEnumerable<TemplateValidation> Validate();
    }

    public struct TemplateValidation
    {
        public string Message { get; internal set; }
        public ValidationLevel Level { get; internal set; }
    }

    public enum ValidationLevel
    {
        WARNING,
        ERROR
    }
}
