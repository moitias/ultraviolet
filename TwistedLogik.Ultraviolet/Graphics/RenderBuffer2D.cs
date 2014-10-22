﻿using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a factory method which constructs instances of the RenderBuffer2D class.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <param name="format">The render buffer's format.</param>
    /// <param name="width">The render buffer's width in pixels.</param>
    /// <param name="height">The render buffer's height in pixels.</param>
    /// <returns>The instance of RenderBuffer2D that was created.</returns>
    public delegate RenderBuffer2D RenderBuffer2DFactory(UltravioletContext uv, RenderBufferFormat format, Int32 width, Int32 height);

    /// <summary>
    /// Represents a two-dimensional render buffer containing color, depth, or stencil data.
    /// </summary>
    public abstract class RenderBuffer2D : Texture2D
    {
        /// <summary>
        /// Initializes a new instance of the RenderBuffer2D class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public RenderBuffer2D(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <summary>
        /// Creates a new instance of the RenderBuffer2D class.
        /// </summary>
        /// <param name="format">The render buffer's format.</param>
        /// <param name="width">The render buffer's width in pixels.</param>
        /// <param name="height">The render buffer's height in pixels.</param>
        /// <returns>The instance of RenderBuffer2D that was created.</returns>
        public static RenderBuffer2D Create(RenderBufferFormat format, Int32 width, Int32 height)
        {
            Contract.EnsureRange(width > 0, "width");
            Contract.EnsureRange(height > 0, "height");

            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<RenderBuffer2DFactory>()(uv, format, width, height);
        }

        /// <summary>
        /// Gets the render buffer's format.
        /// </summary>
        public abstract RenderBufferFormat Format
        {
            get;
        }
    }
}