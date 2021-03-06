using System;
using System.IO;
using System.Runtime.InteropServices;
using Android.Graphics;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics;

namespace TwistedLogik.Ultraviolet.Android.Graphics
{
    /// <summary>
    /// Represents an implementation of the <see cref="SurfaceSource"/> class for the Android platform.
    /// </summary>
    public sealed unsafe class AndroidSurfaceSource : SurfaceSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidSurfaceSource"/> class.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> that contains the surface data.</param>
        public AndroidSurfaceSource(Stream stream)
        {
            Contract.Require(stream, "stream");

            using (var bmp = BitmapFactory.DecodeStream(stream))
            {
                this.width  = bmp.Width;
                this.height = bmp.Height;
                this.stride = bmp.RowBytes;

                this.bmpData       = new Byte[stride * height];
                this.bmpDataHandle = GCHandle.Alloc(bmpData);

                var pixels = bmp.LockPixels();

                Marshal.Copy(pixels, bmpData, 0, bmpData.Length);

                bmp.UnlockPixels();
            }
        }

        /// <inheritdoc/>
        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        public override Color this[int x, int y]
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                fixed (Byte* pixel = &bmpData[y * stride + (x * sizeof(UInt32))])
                {
                    return Color.FromArgb(*(UInt32*)pixel);
                }
            }
        }

        /// <inheritdoc/>
        public override IntPtr Data
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                return bmpDataHandle.AddrOfPinnedObject();
            }
        }

        /// <inheritdoc/>
        public override Int32 Stride
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                return stride;
            }
        }

        /// <inheritdoc/>
        public override Int32 Width
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                return width;
            }
        }

        /// <inheritdoc/>
        public override Int32 Height
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                return height;
            }
        }

        /// <summary>
        /// Releases resources associated with the object.
        /// </summary>
        /// <param name="disposing"><c>true</c> if the object is being disposed; <c>false</c> if the object is being finalized.</param>
        private void Dispose(Boolean disposing)
        {
            if (disposed)
                return;

            bmpDataHandle.Free();

            disposed = true;
        }

        // State values.
        private readonly Byte[] bmpData;
        private readonly GCHandle bmpDataHandle;
        private readonly Int32 width;
        private readonly Int32 height;
        private readonly Int32 stride;
        private Boolean disposed;
    }
}
