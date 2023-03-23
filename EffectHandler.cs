using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGame
{
    internal class EffectHandler
    {
        private Effect _effect;
        private List<Vector3> lightpos;

        public EffectHandler(Effect effect)
        {
            _effect = effect;
            lightpos = new List<Vector3>();
        }

        public void AddLight(Vector3 vector3)
        {
            lightpos.Add(vector3);
        }
        public List<Vector3> GetLights()
        {
            return lightpos;
        }


        public void BasicDraw(Model model, Matrix world, Matrix view, Matrix projection, Texture2D texture2D)
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
                    _effect.Parameters["AmbientColor"].SetValue(Color.White.ToVector4());
                    _effect.Parameters["AmbientIntensity"].SetValue(1f);

                    _effect.Parameters["DiffuseIntensity"].SetValue(3f);
                    _effect.Parameters["ModelTexture"].SetValue(texture2D);
                    _effect.Parameters["Attenuation"].SetValue(new Vector3(0.1f, 0.1f, 0.1f));
                    _effect.Parameters["LightRange"].SetValue(20.0f);
                    _effect.Parameters["LightPosition"].SetValue(lightpos[0] + new Vector3(10f, 1f, 0f));

                }
                mesh.Draw();
            }
        }



    }
}
