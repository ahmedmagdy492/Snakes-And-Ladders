﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAndLadders.UI.Screens
{
    public class ScreenNaviagor
    {
        private Stack<Screen> _screens;
        private static ScreenNaviagor _instance = null;

        public static ScreenNaviagor CreateInstance()
        {
            if(_instance == null)
            {
                _instance = new ScreenNaviagor();
            }

            return _instance;
        }

        private ScreenNaviagor()
        {
            _screens = new Stack<Screen>();
        }

        public void PushScreen(Screen screen)
        {
            _screens.Push(screen);
        }

        public void PopScreen()
        {
            _screens.Pop();
        }

        public void ClearScreens(Screen screen)
        {
            while(_screens.Count > 0)
            {
                _screens.Pop();
            }

            _screens.Push(screen);
        }

        public void Update(GameTime gameTime)
        {
            _screens.Peek().Update(gameTime);
        }

        public void Draw()
        {
            _screens.Peek().Draw();
        }
    }
}
