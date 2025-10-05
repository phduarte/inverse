namespace Inverse.Domain;

public interface IDraggableElement : IVisualElement
{
    void MoveOffset(int x, int y);

    void MoveTo(int x, int y);
}