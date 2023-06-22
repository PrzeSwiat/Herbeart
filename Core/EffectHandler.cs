using Assimp.Unmanaged;
using Liru3D.Animations;
using Liru3D.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

    public class EffectHandler
    {
        private Effect _effect;
        private PointLight[] Lights;
        //private List<Vector3> lightpos;

        public EffectHandler(Effect effect)
        {
            _effect = effect;

            Vector3 pos1 = new Vector3(30, 1, 20);
            Vector3 pos2 = new Vector3(0, 1, 0);

            Lights = new PointLight[2];

            Vector3 att = new Vector3(0.1f, 0.1f, 0.1f);
            Lights[0] = new PointLight(pos1, Color.White.ToVector4(), att, 20);
            Lights[1] = new PointLight(pos2, Color.White.ToVector4(), att, 20);
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



        public void PlayerDraw(Model model, Matrix world, Matrix view, Matrix projection, Texture2D texture2D)
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
                    Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(mesh.ParentBone.Transform * world));
                    _effect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);
                    _effect.Parameters["DiffuseLightDirection"].SetValue(new Vector3(0f, 0f, 0f));
                    _effect.Parameters["DiffuseColor"].SetValue(new Vector4(10f, 10F, 10f, 1));
                    _effect.Parameters["DiffuseIntensity"].SetValue(1f);
                    _effect.Parameters["LineColor"].SetValue(new Vector4(0, 0, 0, 1));
                    _effect.Parameters["LineThickness"].SetValue(0.045f);
                    //_effect.Parameters["AmbientColor"].SetValue(new Vector4(0.3f, 0.3f, 0.3f, 1f));  
                    _effect.Parameters["Texture"].SetValue(texture2D);
                }
                mesh.Draw();
            }
                
            
        }

        public void WroldDraw(Model model, Matrix world, Matrix view, Matrix projection, Texture2D texture2D,Vector3 Lightpos)
        {
            // point light (fire light?)
            float percent = Globals.HPpercent;
            if (percent > 0.4f) { percent = 0.6f; }
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = _effect;
                    _effect.Parameters["World"].SetValue(world * mesh.ParentBone.Transform);
                    _effect.Parameters["View"].SetValue(view);
                    _effect.Parameters["Projection"].SetValue(projection);
                    Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(mesh.ParentBone.Transform * world));
                    _effect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);
                    _effect.Parameters["DiffuseLightDirection"].SetValue(new Vector3(0f, 0f, 0f));
                    _effect.Parameters["DiffuseColor"].SetValue(new Vector4(10f, 10F, 10f, 1));
                    _effect.Parameters["DiffuseIntensity"].SetValue(1f);
                    _effect.Parameters["LineColor"].SetValue(new Vector4(0, 0, 0, 1));
                    _effect.Parameters["LineThickness"].SetValue(0.045f);
                    //_effect.Parameters["AmbientColor"].SetValue(new Vector4(0.3f, 0.3f, 0.3f, 1f));  
                    _effect.Parameters["Texture"].SetValue(texture2D);
                    _effect.Parameters["hp"].SetValue(percent);
                }
                mesh.Draw();
            }
        }
        public void WroldTransparencyDraw(Model model, Matrix world, Matrix view, Matrix projection, Texture2D texture2D, Vector3 Lightpos)
        {
            // point light (fire light?)
            float percent = Globals.HPpercent;
            if (percent > 0.4f) { percent = 0.6f; }
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = _effect;
                    _effect.Parameters["World"].SetValue(world * mesh.ParentBone.Transform);
                    _effect.Parameters["View"].SetValue(view);
                    _effect.Parameters["Projection"].SetValue(projection);
                    Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(mesh.ParentBone.Transform * world));
                    _effect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);
                    _effect.Parameters["DiffuseLightDirection"].SetValue(new Vector3(0f, 0f, 0f));
                    _effect.Parameters["DiffuseColor"].SetValue(new Vector4(10f, 10F, 10f, 0.99f));
                    _effect.Parameters["DiffuseIntensity"].SetValue(1f);
                    _effect.Parameters["LineColor"].SetValue(new Vector4(0, 0, 0, 0.5f));
                    _effect.Parameters["LineThickness"].SetValue(0.045f);
                    //_effect.Parameters["AmbientColor"].SetValue(new Vector4(0.3f, 0.3f, 0.3f, 1f));  
                    _effect.Parameters["Texture"].SetValue(texture2D);
                    _effect.Parameters["hp"].SetValue(percent);
                }
                mesh.Draw();
            }
        }

    }
}
