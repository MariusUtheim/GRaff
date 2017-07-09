using System;
using System.Linq;
using GRaff.Synchronization;
using OpenTK.Graphics.OpenGL4;
using GLPrimitiveType = OpenTK.Graphics.OpenGL4.PrimitiveType;


namespace GRaff.Graphics
{
    public class InterleavedRenderSystem : IDisposable
    {
        private readonly int _array;
        private readonly int _arrayBuffer;
        private int _vertexCount;

        private static Quadrilateral defaultQuadCoords = new Quadrilateral(0.0f, 0.0f, 1.0f, 0.0f, 1.0f, 1.0f, 0.0f, 1.0f);
        private static Quadrilateral defaultTriangleStripCoords = new Quadrilateral(0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f);
        private static int _vertexSize = GraphicsPoint.Size + 4;

        public InterleavedRenderSystem()
        {
            _array = GL.GenVertexArray();
            GL.BindVertexArray(_array);

            _arrayBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _arrayBuffer);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 2, GraphicsPoint.PointerType, false, _vertexSize, 0);

            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.UnsignedByte, true, _vertexSize, GraphicsPoint.Size);


			//            GL.DisableVertexAttribArray(1);
			//            GL.VertexAttrib4N(1, 255, 255, 255, 255);
		}

        public bool IsDisposed { get; private set; }

        ~InterleavedRenderSystem()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            Contract.Requires<ObjectDisposedException>(!IsDisposed, nameof(InterleavedRenderSystem));
            Async.Run(() =>
            {
                if (Giraffe.IsRunning)
                {
                    GL.DeleteVertexArray(_array);
                    GL.DeleteBuffer(_arrayBuffer);
                }
            });
            IsDisposed = true;
            GC.SuppressFinalize(this);
        }


        public void SetPrimitive(params (GraphicsPoint vertex, Color color)[] primitive)
        {
            Contract.Requires<ObjectDisposedException>(!IsDisposed, nameof(InterleavedRenderSystem));
            Contract.Requires<ArgumentNullException>(primitive != null);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _arrayBuffer);
            //GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(_vertexSize * primitive.Length), primitive.Select(p => p.vertex).ToArray(), BufferUsageHint.StreamDraw);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(_vertexSize * primitive.Length), primitive, BufferUsageHint.StreamDraw);
            _vertexCount = primitive.Length;
        }

		public void Render(PrimitiveType type)
		{
            Contract.Requires<ObjectDisposedException>(!IsDisposed, nameof(InterleavedRenderSystem));

			var loc = GL.GetUniformLocation(ShaderProgram.Current.Id, "GRaff_IsTextured");
			if (loc >= 0)
				GL.Uniform1(loc, 0);
			_Graphics.ErrorCheck();

			GL.BindVertexArray(_array);
			GL.DrawArrays((GLPrimitiveType)type, 0, _vertexCount);
			_Graphics.ErrorCheck();
		}

	}
}