using GRaff.Graphics.Text;


namespace GRaff.Graphics
{
    public interface IRenderDevice
    {
        //TODO// Interwoven rendering (v1, c1, v2, c2, ...)
        //TODO// Primitive rendering (v11, v12, v13, c1, v21, v22, v23, c2, ...)

        void Clear(Color color);

        void Draw(PrimitiveType type, GraphicsPoint[] vertices, Color[] colors);

        void Draw(PrimitiveType type, GraphicsPoint[] vertices, Color color);

        void Draw(PrimitiveType type, (GraphicsPoint vertex, Color color)[] primitive);

        void FillEllipse(Color color, GraphicsPoint center, double hRadius, double vRadius);

        void FillEllipse(Color innerColor, Color outerColor, GraphicsPoint center, double hRadius, double vRadius);

        void DrawTexture(Texture texture, double xOrigin, double yOrigin, Matrix transform, Color blend);

        void DrawTexture(TextureBuffer buffer, PrimitiveType type, GraphicsPoint[] vertices, Color blend, GraphicsPoint[] texCoords);
        void DrawTexture(TextureBuffer buffer, PrimitiveType type, GraphicsPoint[] vertices, Color[] colors, GraphicsPoint[] texCoords);
        void DrawTexture(TextureBuffer buffer, PrimitiveType type, (GraphicsPoint vertex, Color color, GraphicsPoint texCoord)[] primitive);

        void DrawText(TextRenderer renderer, Color color, string text, Matrix transform);

    }
}
