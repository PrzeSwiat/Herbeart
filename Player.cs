using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Timers;
using System.Xml;

namespace TheGame
{
    [Serializable]
    internal class Player : Creature
    {
        public List<Leaf> inventory;
        private GamePadState beffore;

        private PlayerEffectHandler playerEffects;
        public event EventHandler OnAttackPressed;

        public Player(Vector3 Position, string modelFileName, string textureFileName) : base(Position, modelFileName, textureFileName)
        {
            SetScale(1.7f);

            AssignParameters(200, 20, 5);

            // Dołączenie metody, która będzie wykonywana przy każdym ticku timera
            playerEffects = new PlayerEffectHandler(this);
            // Uruchomienie timera
            playerEffects.Start();
        }

        public void Update(World world, float deltaTime) //Logic player here
        {
            Update();
            PlayerMovement(world, deltaTime);
            GamePadClick();
        }

        public void AddHealth(int amount)
        {
            if (this.Health + amount > this.MaxHealth) 
            { 
                this.Health = this.MaxHealth;
            } else { this.Health += amount; }
        }

        public void SubstractHealth(int amount)
        {
            if (this.Health - amount < 0)
            {
                this.Health = 0;
            } else { this.Health -= amount; }
        }


        #region Getters
        //GET'ERS

        //.................
        #endregion

        #region Setters
        //SET'ERS

        #endregion
        //.................

        #region MovementAndColisionRegion
        public void PlayerMovement(World world, float deltaTime)
        {
            float rotation = 0;
            //GameControler 
            GamePadCapabilities capabilities = GamePad.GetCapabilities(PlayerIndex.One);
            if (capabilities.IsConnected)
            {

                GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
                if (capabilities.HasLeftXThumbStick)
                {
                    double thumbLeftX = Math.Round(gamePadState.ThumbSticks.Left.X);
                    double thumbLeftY = Math.Round(gamePadState.ThumbSticks.Left.Y);
                    float LeftjoystickX = gamePadState.ThumbSticks.Left.X;
                    float LeftjoystickY = gamePadState.ThumbSticks.Left.Y;
                    float RightjoystickX = gamePadState.ThumbSticks.Right.X;
                    float RightjoystickY = gamePadState.ThumbSticks.Right.Y;

                    Vector2 w1 = new Vector2(0, -1);    // wektor wyjsciowy od ktorego obliczam kat czyli ten do dolu
                    Vector2 w2 = new Vector2(LeftjoystickX, LeftjoystickY);

                    rotation = angle(w1, w2);
                    Direction = new Vector2(LeftjoystickX, LeftjoystickY);

                    // JUZ WCALE NIE UPOŚLEDZONY RUCH KAMERĄ LEFT FUKIN THUMBSTICK
                    if (thumbLeftX == 0 && thumbLeftY == 0)
                    {
                        Direction = new Vector2(0, 0);
                        rotation = this.GetRotation().Y;
                    }
                    
                    ///Right THUMBSTICK
                    if (RightjoystickX != 0 || RightjoystickY != 0)
                    {
                        w2 = new Vector2(RightjoystickX, RightjoystickY);
                        rotation = angle(w1, w2);
                    }
                }
            }
            else
            {
                KeyboardState state = Keyboard.GetState();

                Direction = new Vector2(0, 0);
                if (state.IsKeyDown(Keys.A))
                {
                    setDirectionX(1.0f);
                }
                if (state.IsKeyDown(Keys.D))
                {
                    setDirectionX(-1.0f);
                }
                if (state.IsKeyDown(Keys.W))
                {
                    setDirectionY(1.0f);
                }
                if (state.IsKeyDown(Keys.S))
                {
                    setDirectionY(-1.0f);
                }

                Vector2 w3 = new Vector2(0, -1);    // wektor wyjsciowy od ktorego obliczam kat czyli ten do dolu
                Vector2 w4 = new Vector2(-Direction.X, Direction.Y);

                rotation = angle(w3, w4);
            }
            float Sphereang = rotation - this.GetRotation().Y;
            rotateSphere(Sphereang);
            this.UpdateBB(0, world, new Vector3(Direction.X * deltaTime * Speed, 0, 0));
            this.UpdateBB(0, world, new Vector3(0, 0, Direction.Y * deltaTime * Speed));
            SetRotation(0, rotation, 0);
        }


        public bool collision(List<SceneObject> objects)
        {
            foreach (SceneObject obj in objects)
            {
                if (this.boundingBox.Intersects(obj.boundingBox))
                {
                    return true;
                }
            }
            return false;
        }

        public void UpdateBB(float ang, World world, Vector3 moveVec)
        {
            Vector3 center = ((boundingBox.Min + boundingBox.Max) / 2);
            // Przenieś punkt środka boxa do punktu (0, 0, 0)
            Matrix translation1 = Matrix.CreateTranslation(-center);
            Matrix rotation = Matrix.CreateRotationY(ang);
            Matrix translation2 = Matrix.CreateTranslation(center);
            Matrix transform = translation1 * rotation * translation2;
            Vector3[] vertices = boundingBox.GetCorners();

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = Vector3.Transform(vertices[i], transform);
            }
            Vector3 min = vertices[0];
            Vector3 max = vertices[0];
            for (int i = 1; i < vertices.Length; i++)
            {
                min = Vector3.Min(min, vertices[i]);
                max = Vector3.Max(max, vertices[i]);
            }

            boundingBox = new BoundingBox(min, max);

            //boundingBox =  BoundingBox.CreateFromPoints(vertices);

            BoundingBox bb = new BoundingBox(boundingBox.Min - moveVec, boundingBox.Max - moveVec);
            // Wyznacz punkt środka boxa

            BoundingBox boksik = boundingBox;
            boundingBox = bb;

            if (!(this.collision(world.GetWorldList())))
            {

                SetPosition(GetPosition() - moveVec);
                this.boundingSphere.Center -= moveVec;

            }
            else
            {
                boundingBox = boksik;

            }

        }

        public void rotateSphere(float Angle)
        {
            Matrix rotationMatrix = Matrix.CreateRotationY(Angle);
            boundingSphere.Center -= this.GetPosition();
            boundingSphere.Center = Vector3.Transform(boundingSphere.Center, rotationMatrix);
            boundingSphere.Center += this.GetPosition();
        }


        #endregion


        public void AddLeaf(Leaf leaf)
        {
            inventory.Add(leaf);
        }

        public void RemoveLeaf(Leaf leaf)
        {
            inventory.Remove(leaf);
        }


        public void GamePadClick()
        {
            GamePadCapabilities capabilities = GamePad.GetCapabilities(PlayerIndex.One);
            if (capabilities.IsConnected)
            {

                GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
                if (capabilities.HasAButton)
                {
                    if (gamePadState.IsButtonDown(Buttons.A) && beffore != gamePadState)
                    {
                        OnAttackPressed?.Invoke(this, EventArgs.Empty);  //?.Invoke() ==  if(ojb != null)
                        beffore = gamePadState;
                    }
                    if (gamePadState.IsButtonUp(Buttons.A))
                    {
                        beffore = gamePadState;
                    }
                }



            }
        }


        // MOZNA GDZIES PRZERZUCIC DO JAKIEJS KLASY CZY CO IDK
        public static float angle(Vector2 a, Vector2 b)
        {
            float dot = dotProduct(a, b);
            float det = a.X * b.Y - a.Y * b.X;
            return (float)Math.Atan2(det, dot);
        }

        public static float dotProduct(Vector2 vector, Vector2 vector1)
        {
            float result = vector.X * vector1.X + vector.Y * vector1.Y;

            return result;
        }

        // MATEMATYCZNE RZECZY
    }
}
