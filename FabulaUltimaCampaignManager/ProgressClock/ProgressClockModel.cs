using Godot;
using Godot.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FabulaUltimaGMTool.Model.ProgressClock
{
    public partial class ProgressClockModel : Resource
    {

        private Godot.Collections.Array<bool> _sectionStates = [];
        public Godot.Collections.Array<bool> SectionStates
        {
            get => _sectionStates;
            set
            {
                _sectionStates = value;
                EmitChanged();
            }
        }

        public void PushStates(IEnumerable<bool> states, bool replace = false)
        {
            if (replace) SectionStates.Clear();
            SectionStates.AddRange(states);
            EmitChanged();
        }

        public void ReduceStates(int remainingStates)
        {
            SectionStates = new Array<bool>(SectionStates.Take(remainingStates));
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
