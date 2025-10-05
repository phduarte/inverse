using System.Collections.Generic;
using System.Windows.Forms;

namespace Inverse.Desktop.Extensions;

internal static class GridViewToJsonExtensions
{
    public static string AsJson(this DataGridView dataGridView)
    {
        var list = new List<Dictionary<string, object>>();
        foreach (DataGridViewRow row in dataGridView.Rows)
        {
            if (row.IsNewRow) continue;
            var dict = new Dictionary<string, object>();
            foreach (DataGridViewCell cell in row.Cells)
            {
                var columnName = cell.OwningColumn.Name;
                var cellValue = cell.Value ?? string.Empty;
                dict[columnName] = cellValue;
            }
            list.Add(dict);
        }
        return System.Text.Json.JsonSerializer.Serialize(list);
    }

    public static void FillWithJson(this DataGridView dataGridView, string json)
    {
        if (string.IsNullOrWhiteSpace(json)) return;

        try
        {
            var list = System.Text.Json.JsonSerializer.Deserialize<List<Dictionary<string, object>>>(json);
            if (list == null) return;
            dataGridView.Rows.Clear();
            foreach (var dict in list)
            {
                var rowIndex = dataGridView.Rows.Add();
                var row = dataGridView.Rows[rowIndex];
                foreach (var kvp in dict)
                {
                    if (dataGridView.Columns.Contains(kvp.Key))
                    {
                        row.Cells[kvp.Key].Value = kvp.Value;
                    }
                }
            }
        }
        catch
        {
            // Handle or log the error as needed
        }
    }
}
