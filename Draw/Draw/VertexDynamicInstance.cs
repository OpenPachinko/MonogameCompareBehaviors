
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Draw
{
    public struct VertexDynamicInstance : IVertexType
    {
        public Matrix World;

        public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration(
            new VertexElement( 0, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 0),
            new VertexElement(16, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 1),
            new VertexElement(32, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 2),
            new VertexElement(48, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 3));

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return VertexDynamicInstance.VertexDeclaration; }
        }
    }
}
