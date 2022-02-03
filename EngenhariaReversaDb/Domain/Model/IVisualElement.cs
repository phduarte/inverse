namespace EngenhariaReversaDb.Domain.Model
{
    public interface IVisualElement
    {
        int Width { get; }
        int Height { get; }
        int Left { get; }
        int Right { get; }
        int Top { get; }
        int Bottom { get; }
        int Center { get; }
        int Middle { get; }
    }
}
