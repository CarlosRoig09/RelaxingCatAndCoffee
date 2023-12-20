namespace Interfaces
{
    public interface IModificableValue
    {
        int Value { get; set; }

        public void ModifyValue(int mod)
        {
            Value += mod;
        }

        public int GetValue() { return Value; }
    }
}
