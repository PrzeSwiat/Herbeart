using Assimp;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assimp.Configs;
using System.Diagnostics;
using Quaternion = Assimp.Quaternion;

namespace TheGame
{
    internal class Animations
    {
        public bool HasAnimation;
        public AssimpContext importer;
        public Scene scene;
        public int numModelBones;   // 3 więcej niż Nodów
        public int numSceneBones;
        public int numChanelNodes;
        public Matrix[] boneTransforms;     //też 3 więcej niż Nodów
        public float animationTimeInTicks;
        public float animationTimeInSec;
        public double animationTime;
        public double actualTick;
        public Dictionary<string, int> modelBones;   //W boneMapping szukamy tylko po nazwie (kości modelu jest o 3 więcej niż powinno)
        public Dictionary<string, int> sceneBones;
        public Dictionary<string, int> sceneChanelNodes;
        //public Dictionary<string, int> positions;
        //public int numBones;
        public Matrix[] modelTransforms;
        List<List<QuaternionKey>> Rotations;
        List<List<VectorKey>> Positions;
        List<List<VectorKey>> Scaling;
        List<Matrix> AllTransforms;
        QuaternionKey lastQuad;
        VectorKey lastPos;
        VectorKey lastScale;
        List<QuaternionKey> rot;
        Model model;
        Matrix lastRotation = Matrix.Identity;

        public Animations(string filename, Model _model)
        {
            model = _model;
            importer = new AssimpContext();
            importer.SetConfig(new NormalSmoothingAngleConfig(66.0f));
            modelBones = new Dictionary<string, int>();
            sceneBones = new Dictionary<string, int>();
            sceneChanelNodes = new Dictionary<string, int>();
            try
            {
                scene = importer.ImportFile("../../../Content/" + filename + ".fbx");
                HasAnimation = true;
            }
            catch(Exception)
            {
                scene = importer.ImportFile("../../../Content/nasze.fbx");
                HasAnimation = false;
            }
            Debug.WriteLine(scene.ToString());
            animationTimeInTicks = (float)scene.Animations[0].DurationInTicks;
            animationTimeInSec = (float)(scene.Animations[0].DurationInTicks / scene.Animations[0].TicksPerSecond);
            numModelBones = model.Bones.Count;
            numSceneBones = scene.Meshes[0].BoneCount;
            numChanelNodes = scene.Animations[0].NodeAnimationChannels.Count;
            boneTransforms = new Matrix[numModelBones];

            Rotations = new List<List<QuaternionKey>>();
            Positions = new List<List<VectorKey>>();
            Scaling = new List<List<VectorKey>>();
            AllTransforms = new List<Matrix>();

            for (int i = 0; i < numModelBones; i++)
            {
                modelBones[model.Bones[i].Name] = i;
                boneTransforms[i] = Matrix.Identity;
            }
            for (int i = 0; i < numSceneBones; i++)
            {
                sceneBones[scene.Meshes[0].Bones[i].Name] = i;
            }
            for (int i = 0; i < numChanelNodes; i++)
            {
                sceneChanelNodes[scene.Animations[0].NodeAnimationChannels[i].NodeName] = i;
                AllTransforms.Add(Matrix.Identity);
            }


            //Cofnięcie optymalizacji QuaternionKey biblioteki Assimp 
            int o = 0;
            foreach (var chanel in scene.Animations[0].NodeAnimationChannels)       //DZIAŁA ROTA
            {
                Rotations.Add(new List<QuaternionKey>());
                for (int j = 0; j <= animationTimeInTicks; j++)
                {
                    bool hasKeyAtTime = chanel.RotationKeys.Exists(key => key.Time == j);
                    if (hasKeyAtTime)
                    {
                        var szukanyindex = chanel.RotationKeys.Where(key => key.Time == j).First();

                        Rotations[o].Add(szukanyindex);

                        lastQuad = new QuaternionKey(j, szukanyindex.Value);
                    }
                    else
                    {
                        QuaternionKey pakowany = new QuaternionKey(j, lastQuad.Value);
                        Rotations[o].Add(pakowany);
                    }
                }
                o++;
            }

            //Cofnięcie optymalizacji QuaternionKey biblioteki Assimp 
            int z = 0;
            foreach (var chanel in scene.Animations[0].NodeAnimationChannels)       //DZIAŁA
            {
                Positions.Add(new List<VectorKey>());
                for (int j = 0; j <= animationTimeInTicks; j++)
                {
                    bool hasKeyAtTime = chanel.PositionKeys.Exists(key => key.Time == j);
                    if (hasKeyAtTime)
                    {
                        var szukanyindex = chanel.PositionKeys.Where(key => key.Time == j).First();

                        Positions[z].Add(szukanyindex);

                        lastPos = new VectorKey(j, szukanyindex.Value);
                    }
                    else
                    {
                        VectorKey pakowany = new VectorKey(j, lastPos.Value);
                        Positions[z].Add(pakowany);
                    }
                }
                z++;
            }
            //Cofnięcie optymalizacji QuaternionKey biblioteki Assimp 
            int x = 0;
            foreach (var chanel in scene.Animations[0].NodeAnimationChannels)       //DZIAŁA
            {
                Scaling.Add(new List<VectorKey>());
                for (int j = 0; j <= animationTimeInTicks; j++)
                {
                    bool hasKeyAtTime = chanel.ScalingKeys.Exists(key => key.Time == j);
                    if (hasKeyAtTime)
                    {
                        var szukanyindex = chanel.ScalingKeys.Where(key => key.Time == j).First();

                        Scaling[x].Add(szukanyindex);

                        lastScale = new VectorKey(j, szukanyindex.Value);
                    }
                    else
                    {
                        VectorKey pakowany = new VectorKey(j, lastScale.Value);
                        Scaling[x].Add(pakowany);
                    }
                }
                x++;
            }
           //Debug.WriteLine(numModelBones);
            foreach (var kurwa in modelBones)
            {
               // Debug.WriteLine(kurwa);
            }

            foreach (var chanel in Rotations)
            {
                //Debug.WriteLine("node nr : " + Rotations.FindIndex(key => key == chanel));
                foreach (var rot in chanel)
                {
                  //  Debug.WriteLine(rot.Time + " : " + Matrix.CreateFromQuaternion(QuatToQuat(rot.Value)));
                }
            }
        }

        public void Update(double gameTime)
        {
            animationTime += gameTime;
            if (animationTime >= animationTimeInSec)
            {
                animationTime = 0;
            }
            actualTick = (int)(animationTime * scene.Animations[0].TicksPerSecond);

            myFunk(scene.RootNode, Matrix.Identity, actualTick);
            for (int licznik = 0; licznik < AllTransforms.Count; licznik++)
            {
                AllTransforms[licznik] = Matrix.Identity;
            }
        }

        public void myFunk(Node node, Matrix parentTransform, double tick)
        {
            string nodename = node.Name;
            int boneIndex = modelBones[nodename];
            Matrix nodeTransformation = AssimpToXnaMatrix(node.Transform);
            //Debug.WriteLine(node.Transform);
            Matrix globalTransformation = parentTransform;
            Vector3D pos;
            Quaternion rot;
            Vector3D scale;
            Matrix rotation = Matrix.Identity;
            Matrix position = Matrix.Identity;
            Matrix scaling = Matrix.Identity;

            if (modelBones.ContainsKey(nodename))
            {
                int o = 0;
                foreach (var chanel in scene.Animations[0].NodeAnimationChannels)
                {

                    if (chanel.NodeName == nodename)
                    {
                        foreach (VectorKey vec in Positions[o])
                        {
                            if (vec.Time == tick)
                            {
                                pos = vec.Value;
                                position = Matrix.CreateTranslation(pos.X, pos.Y, pos.Z);

                            }
                        }
                        foreach (QuaternionKey vec in Rotations[o])
                        {
                            if (vec.Time == tick)
                            {
                                rot = vec.Value;
                                rotation = Matrix.CreateFromQuaternion(QuatToQuat(rot));
                                lastRotation = rotation;
                            }

                        }
                        foreach (VectorKey vec in Scaling[0])
                        {
                            if (vec.Time == tick)
                            {
                                scale = vec.Value;

                            }
                        }
                        parentTransform = rotation;


                        if (sceneBones.ContainsKey(nodename))
                        {
                            Matrix offsetMatrix = AssimpToXnaMatrix(scene.Meshes[0].Bones[sceneBones[nodename]].OffsetMatrix);

                        }

                    }
                    o++;
                }

                if (boneIndex > 2)
                {
                    boneTransforms[boneIndex - 2] = parentTransform;
                    // Debug.WriteLine(" index; " + (boneIndex -2) + " tick: " + tick + " : " + rotation + " nodename; " + nodename);
                    //Debug.WriteLine(" index; " + (boneIndex) + " tick: " + tick + " : " + parentTransform + " nodename; " + nodename);
                }
                else
                {
                    boneTransforms[boneIndex] = parentTransform;
                }
            }
            foreach (var child in node.Children)
            {
                myFunk(child, parentTransform, tick);
            }
        }

        public static Matrix AssimpToXnaMatrix(Matrix4x4 assimpMatrix)
        {
            Matrix xnaMatrix = new Matrix();

            xnaMatrix.M11 = assimpMatrix.A1;
            xnaMatrix.M12 = assimpMatrix.B1;
            xnaMatrix.M13 = assimpMatrix.C1;
            xnaMatrix.M14 = assimpMatrix.D1;

            xnaMatrix.M21 = assimpMatrix.A2;
            xnaMatrix.M22 = assimpMatrix.B2;
            xnaMatrix.M23 = assimpMatrix.C2;
            xnaMatrix.M24 = assimpMatrix.D2;

            xnaMatrix.M31 = assimpMatrix.A3;
            xnaMatrix.M32 = assimpMatrix.B3;
            xnaMatrix.M33 = assimpMatrix.C3;
            xnaMatrix.M34 = assimpMatrix.D3;

            xnaMatrix.M41 = assimpMatrix.A4;
            xnaMatrix.M42 = assimpMatrix.B4;
            xnaMatrix.M43 = assimpMatrix.C4;
            xnaMatrix.M44 = assimpMatrix.D4;

            return xnaMatrix;
        }

        public Microsoft.Xna.Framework.Quaternion QuatToQuat(Quaternion quad)
        {
            Microsoft.Xna.Framework.Quaternion quaternion = new Microsoft.Xna.Framework.Quaternion();
            quaternion.X = quad.X;
            quaternion.Y = -quad.Z;
            quaternion.Z = quad.Y;
            quaternion.W = quad.W;
            return quaternion;
        }

    }

}
