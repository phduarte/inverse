namespace EngenhariaReversaDb.Domain.Model
{
    public interface IDraggableElement : IVisualElement
    {
        void MoveOffset(int x, int y);

        void MoveTo(int x, int y);

        bool IsHover(int x, int y);
    }
}
