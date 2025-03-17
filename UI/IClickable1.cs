using System;

namespace SnakeAndLadders.UI
{
    public interface IClickable1
    {
        event Action<UIElement, UIEvent> OnClick;
    }
}