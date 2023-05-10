﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using TheGame.Core;
using Color = Microsoft.Xna.Framework.Color;

namespace TheGame
{
    [Serializable]
    internal class SceneObject
    {
        public Vector3 position;
        public Vector3 rotation = new Vector3(0.0f, 0.0f, 0.0f);
        public float scale = 1;
        protected Model model;
        protected Texture2D texture2D;
        public string _modelFileName;
        public string _textureFileName;
        public BoundingSphere boundingSphere;
        public BoundingBox boundingBox;
        public BoundingBox helper;
        public Vector3 boundingboxrotation = new Vector3(0.0f, 0.0f, 0.0f);
        public Color color = Color.White;

        private DateTime lastEventTime, actualTime;

        public SceneObject(Vector3 worldPosition, string modelFileName, string textureFileName)
        {
            position = worldPosition;
            _modelFileName = modelFileName;
            _textureFileName = textureFileName;

        }

        public void Update()    //przyda się do efektów podłoża (obszarowych)
        {
            /*actualTime = DateTime.Now;
            TimeSpan time = actualTime - lastEventTime;
            if (time.TotalSeconds > 1)
            {
                lastEventTime = actualTime;
                if (color != Color.White)
                {
                    color = Color.White;
                }
            }*/
        }

        public void LoadContent(ContentManager content)
        {
            LoadedModels models = LoadedModels.Instance;

            model = models.getModel(_modelFileName, content);
            texture2D = models.getTexture(_textureFileName, content);
            //texture2D = (content.Load<Texture2D>(_textureFileName));
            helper = CreateBoundingBox(this.model);

            if (_modelFileName == "tree1" || _modelFileName == "tree2" || _modelFileName == "tree3")
            {
                helper.Min = new Vector3(-1, 0, -1);
                helper.Max = new Vector3(1, 6, 1);
                boundingBox = new BoundingBox(helper.Min + this.position, helper.Max + this.position);
            } else if (_modelFileName == "mis")
            {
                helper.Min = new Vector3(-1f, 0, -1f);
                helper.Max = new Vector3(1f, 4, 1f);
                boundingBox = new BoundingBox(helper.Min + this.position, helper.Max + this.position);
                Debug.Write(helper.Min + "\n" + helper.Max);
            } else
            {
                boundingBox = new BoundingBox(helper.Min + this.position, helper.Max + this.position);
            }
            
            boundingSphere = BoundingSphere.CreateFromBoundingBox(boundingBox);
            boundingSphere.Center += new Vector3(0, 0, 3);

        }

        public void Draw(EffectHandler effectHandler, Matrix worldMatrix, Matrix viewMatrix, Matrix projectionMatrix, Color color)
        {
            effectHandler.BasicDraw(GetModel(), worldMatrix * Matrix.CreateScale(GetScale())
                        * Matrix.CreateRotationX(GetRotation().X) * Matrix.CreateRotationY(GetRotation().Y) *
                        Matrix.CreateRotationZ(GetRotation().Z)
                        * Matrix.CreateTranslation(GetPosition().X, GetPosition().Y, GetPosition().Z)
                         , viewMatrix, projectionMatrix, GetTexture2D(), color);
        }

        public void PrzemyslawDraw(EffectHandler effectHandler, Matrix worldMatrix, Matrix viewMatrix, Matrix projectionMatrix, Color color)
        {
            effectHandler.PrzemyslawDraw(GetModel(), worldMatrix * Matrix.CreateScale(GetScale())
                       * Matrix.CreateRotationX(GetRotation().X) * Matrix.CreateRotationY(GetRotation().Y) *
                       Matrix.CreateRotationZ(GetRotation().Z)
                       * Matrix.CreateTranslation(GetPosition().X, GetPosition().Y, GetPosition().Z)
                        , viewMatrix, projectionMatrix, GetTexture2D());
        }

        public double GetDistance(SceneObject entity)
        {
            double distance = (double)Vector3.Distance(this.position, entity.GetPosition());
            return distance;
        }

        public void DrawBB(GraphicsDevice graphicsDevice)
        {
            Vector3[] corners = boundingBox.GetCorners();

            var bottom = new[]
            {
                new VertexPositionColor(corners[2], Color.White),
                new VertexPositionColor(corners[3], Color.White),
                new VertexPositionColor(corners[7], Color.White),
                new VertexPositionColor(corners[6], Color.White),
                new VertexPositionColor(corners[2], Color.White),
            };

            var top = new[]
            {
                new VertexPositionColor(corners[4], Color.White),
                new VertexPositionColor(corners[5], Color.White),
                new VertexPositionColor(corners[1], Color.White),
                new VertexPositionColor(corners[0], Color.White),
                new VertexPositionColor(corners[4], Color.White),
            };

            var rf = new[]
            {
                new VertexPositionColor(corners[1], Color.White),
                new VertexPositionColor(corners[2], Color.White)
            };
            var lf = new[]
            {
                new VertexPositionColor(corners[3], Color.White),
                new VertexPositionColor(corners[0], Color.White)
            };
            var rb = new[]
            {
                new VertexPositionColor(corners[5], Color.White),
                new VertexPositionColor(corners[6], Color.White)
            };
            var lb = new[]
            {
                new VertexPositionColor(corners[4], Color.White),
                new VertexPositionColor(corners[7], Color.White)
            };

            //Sciany dolna i górna
            graphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, bottom, 0, 4);
            graphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, top, 0, 4);
            //boki
            graphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, rf, 0, 1);
            graphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, lf, 0, 1);
            graphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, rb, 0, 1);
            graphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, lb, 0, 1);
        }

        #region Getters
        //GET'ERS
        public Vector3 GetPosition()
        {
            return position;
        }
        public Vector3 GetRotation()
        {
            return rotation;
        }
        public float GetScale()
        {
            return scale;
        }
        public Model GetModel()
        {
            return model;
        }
        public Texture2D GetTexture2D()
        {
            return texture2D;
        }
        public string GetModelFileName()
        {
            return _modelFileName;
        }
        public string GetTextureFileName()
        {
            return _textureFileName;
        }
        //.................
        #endregion

        #region Setters
        //SET'ERS
        public void SetPosition(float x, float y, float z)
        {
            position = new Vector3(x,y,z);
        }
        public void SetPosition(Vector3 pos)
        {
            position = pos;
        }
        public void SetRotation(Vector3 rot)
        {
            rotation = rot;
        }
        public void SetRotation(float x, float y, float z)
        {
            rotation = new Vector3(x, y, z);
        }
        public void SetScale(float scl)
        {
            scale = scl;
        }
        public void SetModel(Model mod)
        {
            model = mod;
        }
        public void SetTexture(Texture2D tex)
        {
            texture2D = tex;
        }
        //.................
        #endregion

        #region BoundingBoxes
        public BoundingBox CreateBoundingBox(Model model)
        {
            Vector3 min = new Vector3(float.MaxValue);
            Vector3 max = new Vector3(float.MinValue);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    VertexBuffer vertexBuffer = part.VertexBuffer;
                    int vertexStride = vertexBuffer.VertexDeclaration.VertexStride;
                    int numVertices = vertexBuffer.VertexCount;
                    byte[] vertexData = new byte[vertexStride * numVertices];
                    vertexBuffer.GetData(vertexData);

                    for (int i = 0; i < numVertices; i++)
                    {
                        Vector3 vertex = Vector3.Transform(
                            new Vector3(
                                BitConverter.ToSingle(vertexData, i * vertexStride),
                                BitConverter.ToSingle(vertexData, i * vertexStride + 4),
                                BitConverter.ToSingle(vertexData, i * vertexStride + 8)),
                            mesh.ParentBone.Transform);

                        min = Vector3.Min(min, vertex);
                        max = Vector3.Max(max, vertex);
                    }
                }
            }

            return new BoundingBox(min, max);
        }

        public List<Vector3> GetBoundingVertices()
        {
            float x = boundingBox.Min.X;
            float y = boundingBox.Min.Y;
            float z = boundingBox.Min.Z;

            float a = boundingBox.Max.X;
            float b = boundingBox.Max.Y;
            float c = boundingBox.Max.Z;


            List<Vector3> vertices = new List<Vector3>();
            vertices.Add(new Vector3(x, y, z));
            vertices.Add(new Vector3(a, y, z));
            vertices.Add(new Vector3(a, y, c));
            vertices.Add(new Vector3(x, y, c));
            vertices.Add(new Vector3(x, b, z));
            vertices.Add(new Vector3(a, b, z));
            vertices.Add(new Vector3(a, b, c));
            vertices.Add(new Vector3(x, b, c));
            return vertices;
        }
        private RectangleF CreateRectangle(Model model)
        {
            Vector3 min = new Vector3(float.MaxValue);
            Vector3 max = new Vector3(float.MinValue);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    VertexBuffer vertexBuffer = part.VertexBuffer;
                    int vertexStride = vertexBuffer.VertexDeclaration.VertexStride;
                    int numVertices = vertexBuffer.VertexCount;
                    byte[] vertexData = new byte[vertexStride * numVertices];
                    vertexBuffer.GetData(vertexData);

                    for (int i = 0; i < numVertices; i++)
                    {
                        Vector3 vertex = Vector3.Transform(
                            new Vector3(
                                BitConverter.ToSingle(vertexData, i * vertexStride),
                                BitConverter.ToSingle(vertexData, i * vertexStride + 4),
                                BitConverter.ToSingle(vertexData, i * vertexStride + 8)),
                            mesh.ParentBone.Transform);

                        min = Vector3.Min(min, vertex);
                        max = Vector3.Max(max, vertex);
                    }
                }
            }

            return new RectangleF(min.X,min.Z,max.X-min.X, max.Z - min.Z);
        }
        #endregion
    }
}
