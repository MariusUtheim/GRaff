using System;
using System.Linq;

namespace GRaff.Effects
{
    public class AudioFilter
    {
        private float[] _filter;

        public AudioFilter(float[] filter)
        {
            this._filter = filter.ToArray();
        }

        
    }
}
