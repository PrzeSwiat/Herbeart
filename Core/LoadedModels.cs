using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;


namespace TheGame.Core
{
    internal class LoadedModels
    {
        private static LoadedModels instance = null;
        Dictionary<string, Model> loadedModels;
        Dictionary<string, Texture2D> loadedTextures;

        private LoadedModels() 
        {
            loadedModels = new Dictionary<string, Model>();
            loadedTextures = new Dictionary<string, Texture2D>();
        }

        public static LoadedModels Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new LoadedModels();
                }
                return instance;
            }
        }

        public Model getModel(String name, ContentManager content)
        {
            if (loadedModels.ContainsKey(name))
            {
                return loadedModels[name];
            } else
            {
                loadedModels.Add(name, content.Load<Model>(name));
                return loadedModels[name];
            }
        }

        public Texture2D getTexture(String name, ContentManager content)
        {
            if (loadedTextures.ContainsKey(name))
            {
                return loadedTextures[name];
            }
            else
            {
                loadedTextures.Add(name, content.Load<Texture2D>(name));
                return loadedTextures[name];
            }
        }

    }
}
