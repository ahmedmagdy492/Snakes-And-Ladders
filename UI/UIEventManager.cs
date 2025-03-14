using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAndLadders.UI
{
    public class UIEventManager
    {
        private readonly Queue<UIEvent> _events;

        public UIEventManager()
        {
            _events = new Queue<UIEvent>();
        }

        public void PushEvent(UIEvent e)
        {
            _events.Enqueue(e);
        }

        public void ProcessEvents(List<UIElement> uIElements)
        {
            while(_events.Count > 0)
            {
                UIEvent e = _events.Dequeue();

                foreach(var item in uIElements)
                {
                    item.HandleEvent(e);
                }
            }
        }
    }
}
