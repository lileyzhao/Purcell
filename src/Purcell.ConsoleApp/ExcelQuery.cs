using MiniExcelLibs;
using Sylvan.Data.Excel;

namespace PurcellLibs.ConsoleApp;

public class ExcelQuery
{
    private const string FileName = @"Resources/10_0000x11.xlsx";

    public void Purcell_Query_Dynamic()
    {
        string path = Path.Combine(AppContext.BaseDirectory, FileName);
        PurTable tableConfig = new() { HasHeader = false, DataStart = "A1" };
        foreach (IDictionary<string, object?> unused in Purcell.CreateQuerier(path).Query(tableConfig))
        {
        }
    }

    public void Sylvan_Query_Dynamic()
    {
        string path = Path.Combine(AppContext.BaseDirectory, FileName);
        using ExcelDataReader edr = ExcelDataReader.Create(path);
        do
        {
            while (edr.Read())
            {
                for (int i = 0; i < edr.FieldCount; i++)
                {
                    string unused = edr.GetString(i);
                }
            }
        } while (edr.NextResult());
    }

    public void MiniExcel_Query_Dynamic()
    {
        string path = Path.Combine(AppContext.BaseDirectory, FileName);
        foreach (dynamic? unused in MiniExcel.Query(path))
        {
        }
    }
}