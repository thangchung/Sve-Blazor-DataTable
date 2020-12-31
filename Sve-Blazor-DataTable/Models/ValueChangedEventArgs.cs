namespace Sve.Blazor.DataTable.Models
{
    public class ValueChangedEventArgs
    {
        public object Value { get; private set; }

        public ValueChangedEventArgs(object value) => this.Value = value;
    }
}
