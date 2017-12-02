using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace GRaff.Graphics
{
    class SamplerBuffer : IDisposable
    {
        private int _bufferId, _textureId;

        public SamplerBuffer(byte[] data)
        {
            GL.ActiveTexture(TextureUnit.Texture1);

            this._bufferId = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.TextureBuffer, _bufferId);
            GL.BufferData(BufferTarget.TextureBuffer, new IntPtr(data.Length), data, BufferUsageHint.StaticRead);

            this._textureId = GL.GenTexture();
            GL.BindTexture(TextureTarget.TextureBuffer, _textureId);
            GL.TexBuffer(TextureBufferTarget.TextureBuffer, SizedInternalFormat.Rg8, _bufferId);

            GL.ActiveTexture(TextureUnit.Texture0);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~SamplerBuffer() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
