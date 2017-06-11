using GRaff.Graphics.Text;


namespace GRaff.Graphics
{
    public interface IRenderDevice
    {
        void Clear(Color color);

        void Draw(GraphicsPoint[] vertices, Color[] colors, PrimitiveType type);

        void Draw(GraphicsPoint[] vertices, Color color, PrimitiveType type);

        void FillEllipse(Color color, GraphicsPoint center, double hRadius, double vRadius);

        void FillEllipse(Color innerColor, Color outerColor, GraphicsPoint center, double hRadius, double vRadius);

        void DrawTexture(Texture texture, double xOrigin, double yOrigin, Color blend, Matrix transform);

        void DrawTexture(TextureBuffer buffer, GraphicsPoint[] vertices, Color blend, GraphicsPoint[] texCoords, PrimitiveType type);
        void DrawTexture(TextureBuffer buffer, GraphicsPoint[] vertices, Color[] colors, GraphicsPoint[] texCoords, PrimitiveType type);

        void DrawText(TextRenderer renderer, Color color, string text, Matrix transform);

    }
}
