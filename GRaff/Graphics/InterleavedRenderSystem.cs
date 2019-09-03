using System;
using GRaff.Synchronization;
using GRaff.Graphics.Shaders;
using OpenTK.Graphics.OpenGL4;
using GLPrimitiveType = OpenTK.Graphics.OpenGL4.PrimitiveType;

namespace GRaff.Graphics
{
    public class InterleavedRenderSystem : IDisposable
    {

        private readonly int _array;
        private readonly int _arrayBuffer;
        private int _vertexCount;

        public InterleavedRenderSystem()
        {
            _array = GL.GenVertexArray();
            GL.BindVertexArray(_array);

            _arrayBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _arrayBuffer);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 2, GraphicsPoint.PointerType, false, GraphicsVertex.Size, 0);

            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.UnsignedByte, true, GraphicsVertex.Size, GraphicsPoint.Size);

		}

        public void SetPrimitive(params GraphicsVertex[] primitive)
        {
            Contract.Requires<ObjectDisposedException>(!IsDisposed);
            Contract.Requires<ArgumentNullException>(primitive != null);
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, _arrayBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(GraphicsVertex.Size * primitive.Length), primitive, BufferUsageHint.StreamDraw);
            _vertexCount = primitive.Length;
        }

		public void Render(PrimitiveType type)
		{
            Contract.Requires<ObjectDisposedException>(!IsDisposed);

			var loc = GL.GetUniformLocation(ShaderProgram.Current.Id, "GRaff_IsTextured");
			if (loc >= 0)
				GL.Uniform1(loc, 0);
			_Graphics.ErrorCheck();

			GL.BindVertexArray(_array);
			GL.DrawArrays((GLPrimitiveType)type, 0, _vertexCount);
			_Graphics.ErrorCheck();
		}


        #region IDisposable implementation

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
                if (_Graphics.IsContextActive)
                {
                    GL.DeleteVertexArray(_array);
                    GL.DeleteBuffer(_arrayBuffer);
                }
            });
            IsDisposed = true;
            GC.SuppressFinalize(this);
        }

        #endregion

    }
}