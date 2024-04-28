using System;
using System.Collections.Generic;
using System.Linq;
using Apos.Shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Fractal
{
    public class GameRoot : Game
    {

        private List<Vector2> verts = new List<Vector2>();
        private GraphicsDeviceManager _graphics;
        private ShapeBatch _ShapeBatch;

        public GameRoot()
        {

            _graphics = new GraphicsDeviceManager(this);
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.HardwareModeSwitch = false;
            _graphics.IsFullScreen = true;

        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.ApplyChanges();
            base.Initialize();
            Vector2 center = new Vector2(800, 400);
            Vector2 unitVector = new Vector2(400, 0);
            Vector2 vector1 = RotateVector(unitVector, 2f * MathF.PI / 3f);
            Vector2 vector2 = RotateVector(vector1, 2f * MathF.PI / 3f);

            verts.Add(unitVector);
            verts.Add(vector1);
            verts.Add(vector2);
            //   Vector2 vector3 = vector2 + center;


            //  RotateVector(unitVector, MathF.PI / 3);
            // verts.Add(RotateVector(Vector2.UnitX, MathF.PI / 3));



        }
        public static Vector2 RotateVector(Vector2 vector, float radians)
        {
            float cos = MathF.Cos(radians);
            float sin = MathF.Sin(radians);

            float x = vector.X * cos - vector.Y * sin;
            float y = vector.X * sin + vector.Y * cos;
            return new Vector2(x, y);
        }

        protected override void LoadContent()
        {
            _ShapeBatch = new ShapeBatch(GraphicsDevice, Content);
        }

        KeyboardState pks;
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Space) && !pks.IsKeyDown(Keys.Space))
            {
                Fracktalize();
            }

            pks = ks;
            base.Update(gameTime);
        }


        private void Fracktalize()
        {
            List<Vector2> cpy = verts.ToArray().ToList();
            verts = new List<Vector2>();
            for (int i = 0; i < cpy.Count; i++)
            {
                FracktalizeSide(i, cpy);
            }
        }

        private void FracktalizeSide(int index, List<Vector2> getFrom)
        {
            Vector2 first = getFrom[index];
            Vector2 second = getFrom[(index + 1) % getFrom.Count];
            Vector2 closerToFirst = (first * 2 + second) / 3;
            Vector2 closerToSecond = (second * 2 + first) / 3;
            Vector2 diff = first - closerToFirst;
            diff = RotateVector(diff, 2 * MathF.PI / 3) + closerToFirst;


            verts.Add(first);
            verts.Add(closerToFirst);
            verts.Add(diff);
            verts.Add(closerToSecond);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _ShapeBatch.Begin();
            Vector2 off = Mouse.GetState().Position.ToVector2();
            for (int i = 0; i < verts.Count; i++)
            {
                _ShapeBatch.FillLine(verts[i] + off, verts[(i + 1) % verts.Count] + off, 3, Color.White);
            }
            _ShapeBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }


    }
}