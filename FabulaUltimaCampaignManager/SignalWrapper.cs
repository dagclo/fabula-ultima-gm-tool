using Godot;

namespace FirstProject.Beastiary
{
    public partial class SignalWrapper<T> : GodotObject
    {

        public SignalWrapper(T package) 
        { 
            Value = package;
        }

        public T Value { get; }
    }
}
