using System;

namespace SnakeAndLadders.UI
{
    public interface IClickable2
    {
        event Action<UIElement, UIEvent> OnClick;
    }
}