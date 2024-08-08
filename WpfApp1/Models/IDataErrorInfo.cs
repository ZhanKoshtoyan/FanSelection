namespace WpfApp1.Models;

public interface IDataErrorInfo
{
    string Error { get; }
    string this[string columnName] { get; }
}
