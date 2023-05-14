using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = Microsoft.Xna.Framework.Color;

namespace TheGame
{
    struct PointLight
    {
        Vector3 position;
        Vector4 color;
        Vector3 attenuation;
        float lightRange;
        bool isActive;

        public PointLight(Vector3 pos, Vector4 col, Vector3 att, float range)
        {
            position = pos;
            color = col;
            attenuation = att;
            lightRange = range;
            isActive = true;
        }

        public Vector3 Position { get { return position; } }
        public Vector4 Color { get { return color; } }
        public Vector3 Attenuation { get { return attenuation; } }
        public float Range { get { return lightRange; } }
    }

    internal class EffectHandler
    {
        private Effect _effect;
        private PointLight[] Lights;
        //private List<Vector3> lightpos;

        public EffectHandler(Effect effect)
        {
            _effect = effect;
            Vector3 pos1 = new Vector3(8, 1, 0);
            Vector3 pos2 = new Vector3(0, 1, 0);

            Lights = new PointLight[2];

            Vector3 att = new Vector3(0.1f, 0.1f, 0.1f);
            Lights[0] = new PointLight(pos1, Color.White.ToVector4(), att, 20);
            Lights[1] = new PointLight(pos2, Color.White.ToVector4(), att, 20);
            //lightpos = new List<Vector3>();
        }

        private Vector3[] getPointLightPositions(PointLight[] lights)
        {
            Vector3[] positions = new Vector3[lights.Length];

            for (int i = 0; i < lights.Length; i++)
            {
                positions[i] = lights[i].Position;
            }
            return positions;
        }

        private Vector4[] getPointLightColor(PointLight[] lights)
        {
            Vector4[] colors = new Vector4[lights.Length];

            for (int i = 0; i < lights.Length; i++)
            {
                colors[i] = lights[i].Color;
            }
            return colors;
        }

        private Vector3[] getPointLightAttenuation(PointLight[] lights)
        {
            Vector3[] att = new Vector3[lights.Length];

            for (int i = 0; i < lights.Length; i++)
            {
                att[i] = lights[i].Attenuation;
            }
            return att;
        }

        private float[] getPointLightRange(PointLight[] lights)
        {
            float[] ranges = new float[lights.Length];

            for (int i = 0; i < lights.Length; i++)
            {
                ranges[i] = lights[i].Range;
            }
            return ranges;
        }

        /*public void AddLight(Vector3 vector3)
        {
            lightpos.Add(vector3);
        }
        public List<Vector3> GetLights()
        {
            return lightpos;
        }*/


        public void BasicDraw(Model model, Matrix world, Matrix view, Matrix projection, Texture2D texture2D, Color color)
        {
            // point light (fire light?)
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = _effect;
                    _effect.Parameters["World"].SetValue(world * mesh.ParentBone.Transform);
                    _effect.Parameters["View"].SetValue(view);
                    _effect.Parameters["Projection"].SetValue(projection);
                   
                    _effect.Parameters["DiffuseColor"].SetValue((Color.DarkOrange.ToVector4() + Color.Yellow.ToVector4()) / 2);
                    _effect.Parameters["DiffuseLightDirection"].SetValue(new Vector3(1, 0, 1));
                    _effect.Parameters["DiffLightCol"].SetValue((Color.Gray.ToVector4()));
                    _effect.Parameters["LightPosition"].SetValue(getPointLightPositions(Lights));
                    _effect.Parameters["Attenuation"].SetValue(getPointLightAttenuation(Lights));
                    _effect.Parameters["LightRange"].SetValue(getPointLightRange(Lights));
                    _effect.Parameters["ModelTexture"].SetValue(texture2D);
                    _effect.Parameters["AmbientColor"].SetValue(color.ToVector4());
                    _effect.Parameters["AmbientIntensity"].SetValue(0.3f);
                    _effect.Parameters["DiffuseIntensity"].SetValue(0.5f);
                    _effect.Parameters["PointLightIntensity"].SetValue(3f);
                    Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(mesh.ParentBone.Transform * world));
                    _effect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);
                }
                mesh.Draw();
            }
        }

        public void PrzemyslawDraw(Model model, Matrix world, Matrix view, Matrix projection, Texture2D texture2D)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = _effect;
                    _effect.Parameters["World"].SetValue(world * mesh.ParentBone.Transform);
                    _effect.Parameters["View"].SetValue(view);
                    _effect.Parameters["Projection"].SetValue(projection);
                    _effect.Parameters["DiffuseColor"].SetValue((Color.White.ToVector4()));
                    _effect.Parameters["AmbientColor"].SetValue((Color.White.ToVector4()));
                    _effect.Parameters["AmbientIntensity"].SetValue(1f);
                    _effect.Parameters["ModelTexture"].SetValue(texture2D);
                    _effect.Parameters["DiffuseLightDirection"].SetValue(new Vector3(1, 0, 0));
                    _effect.Parameters["DiffuseIntensity"].SetValue(0.5f); 
                    Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(mesh.ParentBone.Transform * world));
                    _effect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);
                }
                mesh.Draw();
            }
        }

        public void WiktorDraw(Model model, Matrix world, Matrix view, Matrix projection, Texture2D texture2D)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = _effect;
                    _effect.Parameters["World"].SetValue(world * mesh.ParentBone.Transform);
                    _effect.Parameters["View"].SetValue(view);
                    _effect.Parameters["Projection"].SetValue(projection);
                    _effect.Parameters["AmbientIntensity"].SetValue(0.1f);
                    _effect.Parameters["ModelTexture"].SetValue(texture2D);
                    _effect.Parameters["DiffuseLightDirection"].SetValue(new Vector3(1, 0, 0));
                    _effect.Parameters["DiffuseIntensity"].SetValue(0.5f);
                    _effect.Parameters["Transparency"].SetValue(0.35f);

                    Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(mesh.ParentBone.Transform * world));
                    _effect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);
                }
                mesh.Draw();
            }
        }

        public void AnimationDraw(Animations animation,Model model, Matrix world, Matrix view, Matrix projection, Texture2D texture2D)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    effect.View = view;
                    effect.Projection = projection;
                    effect.World = world;
                    effect.SetBoneTransforms(animation.boneTransforms);
                    effect.Texture = texture2D;
                }
                mesh.Draw();
            }
        }


    }
}
