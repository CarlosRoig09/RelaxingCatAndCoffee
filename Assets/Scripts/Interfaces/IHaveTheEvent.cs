using personalLibrary;
namespace Interfaces
{
    public interface IHaveTheEvent
    {
        public EnumLibrary.TypeOfEvent Type { get; set; }

        public delegate void IHaveTheEvent(object value);
        public event IHaveTheEvent IHTEvent;
    }
}
