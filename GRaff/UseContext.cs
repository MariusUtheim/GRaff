using GRaff.Synchronization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff
{
    public sealed class UseContext : IDisposable
    {
        private string _source;
        private Action _callback;
        private bool _isDisposed;

        private UseContext(string source, Action callback)
        {
            this._source = source;
            this._callback = callback;
        }

        public static UseContext CreateAt<T>(string location, T captureData, Action onCreated, Action<T> onDestroyed)
        {
            onCreated();
            return new UseContext($"A context returned from {location} was garbage collected before Dispose was called.", () => onDestroyed(captureData));
        }

        ~UseContext()
        {
            Async.Throw(new ObjectDisposedIncorrectlyException(_source));
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                GC.SuppressFinalize(this);
                _isDisposed = true;

                _callback();
            }
            else
                throw new ObjectDisposedException(nameof(UseContext));
        }
    }
}
