using Godot;

namespace FabulaUltimaGMTool.Model.ProgressClock
{
    public partial class ProgressClockModel : Resource
    {
        private int _sections;
        public int Sections
        {
            get => _sections;
            set
            {
                _sections = value;
                EmitChanged();
            }
        }

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                EmitChanged();
            }
        }
    }
}
