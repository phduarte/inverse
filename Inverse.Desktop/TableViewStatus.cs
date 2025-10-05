namespace Inverse.Desktop;

internal class TableViewStatus
{
    public string Table { get; set; }
    public int Left { get; set; }
    public int Top { get; set; }
    public bool Visible { get; set; }

    public override string ToString()
    {
        return $"{Table}, Left={Left},Top={Top}";
    }
}