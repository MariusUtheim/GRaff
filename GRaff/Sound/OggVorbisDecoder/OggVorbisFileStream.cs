﻿/*
 * Copyright © 2008, Atachiants Roman (kelindar@gmail.com)
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without modification, 
 * are permitted provided that the following conditions are met:
 *
 *    - Redistributions of source code must retain the above copyright notice, 
 *      this list of conditions and the following disclaimer.
 * 
 *    - Redistributions in binary form must reproduce the above copyright notice, 
 *      this list of conditions and the following disclaimer in the documentation 
 *      and/or other materials provided with the distribution.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
 * IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
 * INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT 
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, 
 * OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
 * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY 
 * OF SUCH DAMAGE.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;

namespace OggVorbisDecoder
{
    /// <summary>
    /// This class acts like a read FileStream on a Ogg Vorbis file, will handle the file and stream the decoded contents
    /// </summary>
    public class OggVorbisFileStream : Stream
    {
        #region Properties
        private NativeOggVorbisFile file = null;
        private VorbisInfo info = null;
        private long pcmLength = 0;
        private long rawLength = 0;
        private double duration = 0;

        internal NativeOggVorbisFile FileHandle
        {
            get { return file; }
        }
        
        public VorbisInfo Info
        {
            get { return info; }
        }

        public override bool CanRead
        {
            get { return file != null; }
        }

        public override bool CanSeek
        {
            get { return (file.IsSekable == 1); }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override long Length
        {
            get { return pcmLength * 2 * info.Channels; }
        }

        public long RawLength
        {
            get { return rawLength; }
        }

        public double Duration
        {
            get { return duration; }
        }

        public override long Position
        {
            get { return file.PcmOffset; }
            set { OggVorbisFile.NativeSeek(file, value); }
        }

        public double Time
        {
            get { return OggVorbisFile.NativeGetTime(file); }
            set { OggVorbisFile.NativeSeekTime(file, value); }
        }


        #endregion

        #region Constructors

        public OggVorbisFileStream(FileInfo fileInfo) : this(fileInfo.FullName) { }
        public OggVorbisFileStream(string filename)
        {         
            //Open File
            file = new NativeOggVorbisFile();

            //if (ov_open(stream.SafeFileHandle, ref vf, IntPtr.Zero,0) == 0)
            // fopen for Win32!
            if (OggVorbisFile.NativeFOpen(filename, file) != 0)
            {
                throw new OggVorbisException("Error during fopen");
            }

            //Get info
            info = new VorbisInfo(OggVorbisFile.NativeGetInfo(file, -1));
            
            //Get lengths of the stream
            pcmLength = OggVorbisFile.NativeGetLength(file, -1); // -1 to entire bitstream
            rawLength = OggVorbisFile.NativeGetRawLength(file, -1); // -1 to entire bitstream
            duration = OggVorbisFile.NativeGetDuration(file, -1); // -1 to entire bitstream

            //Update info
            info.Duration = duration;
        }

        #endregion

        #region Methods

        public override int Read(byte[] buffer, int offset, int length)
        {
            unsafe
            {
                fixed (byte* bp = buffer)
                {
                    //init and set initial pointer position
                    int result = 0;
                    byte* bufferPos = bp + offset;

                    bufferPos = bp + offset; // going to the correct buffer position
                    result = OggVorbisFile.NativeRead(file, bufferPos, length, 0, 2, 1, IntPtr.Zero);

                    if (result < 0)
                    {
                        if (result == -1) throw new OggVorbisException("The initial file headers couldn't be read or are corrupt, or that the initial open call for vf failed.");
                        if (result == -2) throw new OggVorbisException("An invalid stream section was supplied to libvorbisfile, or the requested link is corrupt.");
                        if (result == -3) throw new OggVorbisException("There was an interruption in the data. (one of: garbage between pages, loss of sync followed by recapture, or a corrupt page)");
                        else throw new OggVorbisException("Unknown OggVorbis error: " + result);
                    }

                    return result;
                }
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            // Using this because it's the fastest seek method possible (with crosslap)
            OggVorbisFile.NativeSeek(file, offset);
            return file.PcmOffset;
        }

        public virtual int SeekTime(double time)
        {
            return OggVorbisFile.NativeSeekTime(file, time);
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override void Flush() 
        {
            //no need to flush
        }

        public override void Close()
        {
            base.Close();
            if (file != null)
            {
                OggVorbisFile.NativeClear(file);
            }
        }
        #endregion



    }
}
