using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRaff.Synchronization;
using OpenTK.Graphics.OpenGL4;

namespace GRaff.Graphics.Shaders
{
    public class SamplerBuffer : IDisposable
    {
        private int _bufferId, _textureId;

        public SamplerBuffer(byte[] data, UsageHint usageHint = UsageHint.StaticRead)
        {
            this._bufferId = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.TextureBuffer, _bufferId);
            GL.BufferData(BufferTarget.TextureBuffer, new IntPtr(data.Length), data, (BufferUsageHint)usageHint);

            this._textureId = GL.GenTexture();
        }

        public void BindToLocation(int location)
        {
            Contract.Requires<ObjectDisposedException>(!IsDisposed);

            GL.ActiveTexture(TextureUnit.Texture0 + location);

            GL.BindBuffer(BufferTarget.TextureBuffer, _bufferId);
            GL.BindTexture(TextureTarget.TextureBuffer, _textureId);
            GL.TexBuffer(TextureBufferTarget.TextureBuffer, SizedInternalFormat.R8, _bufferId);

            GL.ActiveTexture(TextureUnit.Texture0);
        }

        public void WriteData(byte[] data, UsageHint usageHint = UsageHint.DynamicRead)
        {
            Contract.Requires<ObjectDisposedException>(!IsDisposed);
            GL.BindBuffer(BufferTarget.TextureBuffer, _bufferId);
            GL.BufferData(BufferTarget.TextureBuffer, new IntPtr(data.Length), data, (BufferUsageHint)usageHint);
        }

        #region IDisposable implementation 

        public bool IsDisposed { get; private set; }

        ~SamplerBuffer()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                Async.Capture((bufferId: _bufferId, textureId: _textureId)).ThenQueue(cap =>
                {
                    if (_Graphics.IsContextActive)
                    {
                        GL.DeleteBuffer(cap.bufferId);
                        GL.DeleteTexture(cap.textureId);
                    }
                });
                IsDisposed = true;
            }
        }

        #endregion

    }
}
