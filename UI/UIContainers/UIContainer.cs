using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAndLadders.UI.UIContainers
{
    public abstract class UIContainer : UIElement
    {
        protected readonly List<UIElement> _uiChildElements;

        public List<UIElement> Children { 
            get
            {
                return _uiChildElements;
            }
        }

        public UIContainer(GraphicsContext graphicsMetaData) : base(graphicsMetaData)
        {
            _uiChildElements = new List<UIElement>();
        }

        public abstract int GetWidth();
        public abstract int GetHeight();
    }
}
