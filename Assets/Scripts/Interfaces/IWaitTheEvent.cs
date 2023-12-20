
using personalLibrary;

namespace Interfaces
{
    public interface IWaitTheEvent 
    {
        public EnumLibrary.TypeOfEvent Type { get; }
        public void MethodForEvent(object value);
    }
}
