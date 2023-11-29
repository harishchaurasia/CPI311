using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPI311.GameEngine
{
    public static class InputManager
    {
        static KeyboardState PreviousKeyboardState { get; set; }
        static KeyboardState CurrentKeyboardState { get; set; }
        static MouseState PreviousMouseState { get; set; }
        static MouseState CurrentMouseState { get; set; }
        public static void Initialize()
        {
            PreviousKeyboardState = CurrentKeyboardState =
            Keyboard.GetState();
            PreviousMouseState = CurrentMouseState =
            Mouse.GetState();
        }
        public static void Update()
        {
            PreviousKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();
            PreviousMouseState = CurrentMouseState;
            CurrentMouseState = Mouse.GetState();
        }
        public static bool IsKeyDown(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key);
        }
        public static bool IsKeyUp(Keys key)
        {
            return CurrentKeyboardState.IsKeyUp(key);
        }
        public static bool IsKeyPressed(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key) &&
            PreviousKeyboardState.IsKeyUp(key);
        }
        public static bool IsKeyReleased(Keys key)
        {
            return CurrentKeyboardState.IsKeyUp(key) &&
            PreviousKeyboardState.IsKeyDown(key);
        }
        public static bool IsMouseHeld()
        {
            return CurrentMouseState.LeftButton == ButtonState.Pressed && PreviousMouseState.LeftButton == ButtonState.Pressed;
        }
        public static Point getMousePosition()
        {
            return CurrentMouseState.Position;
        }
        public static Vector2 GetMousePosition()
        {
            return new Vector2(CurrentMouseState.X, CurrentMouseState.Y);
        }

        public static MouseState GetMouseCurrent()
        {
            return CurrentMouseState;
        }

        public static MouseState GetMousePrevious()
        {
            return PreviousMouseState;
        }





        //Lab 11 Update ******************
        public static bool IsMouseReleased(int mouseButton)
        {
            switch (mouseButton) 
            {
                case 0:
                    return PreviousMouseState.LeftButton == ButtonState.Pressed && CurrentMouseState.LeftButton == ButtonState.Released;

                case 1:
                    return PreviousMouseState.RightButton == ButtonState.Pressed && CurrentMouseState.RightButton == ButtonState.Released;

                case 2:
                    return PreviousMouseState.MiddleButton == ButtonState.Pressed && CurrentMouseState.MiddleButton == ButtonState.Released;

                default: return false;
            }
        }
    }
}
