using System;
using System.Reflection;
using System.IO;
using Microsoft.Xna.Framework.Content;

namespace MRT
{
    public class EmbeddedResourceContentManager : ContentManager
    {
        public EmbeddedResourceContentManager(IServiceProvider serviceProvider)
            : base(serviceProvider) { }

        protected override Stream OpenStream(string assetName)
        {
            assetName = assetName.Replace('\\', '.');
            assetName = assetName.Replace('/', '.');

            var asm = Assembly.GetExecutingAssembly();
            return asm.GetManifestResourceStream(assetName + ".xnb");
        }
    }
}
