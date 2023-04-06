using Microsoft.Xna.Framework;
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

namespace TheGame
{
    [Serializable]
    internal class SceneObject
    {
        public Vector3 position;
        public Vector3 rotation = new Vector3(0.0f,0.0f,0.0f);
        public float scale = 1;
        protected Model model;
        protected Texture2D texture2D;
        public string _modelFileName;
        public string _textureFileName;
        public BoundingBox boundingBox;
        public BoundingBox helper;
        public RectangleF rect;
        public SceneObject(Vector3 worldPosition, string modelFileName , string textureFileName)
        {
            position = worldPosition;
            _modelFileName = modelFileName;
            _textureFileName = textureFileName;
            
        }

        public void LoadContent(ContentManager content)
        {
            model = (content.Load<Model>(_modelFileName));
            texture2D = (content.Load<Texture2D>(_textureFileName));
            helper = CreateBoundingBox(this.model);
            boundingBox = new BoundingBox(helper.Min + this.position, helper.Max + this.position);
            rect = CreateRectangle(this.model);
        }

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

    }
}
