﻿using System;
using System.Drawing;
using TwistedLogik.Ultraviolet.Content;

namespace TwistedLogik.Ultraviolet.Testing
{
    /// <summary>
    /// Represents an Ultraviolet application used in unit tests.
    /// </summary>
    public interface IUltravioletTestApplication
    {
        /// <summary>
        /// Specifies the application's audio subsystem assembly.
        /// </summary>
        /// <param name="audioSubsystem">The fully-qualified name of the application's audio subsystem assembly.</param>
        /// <returns>The Ultraviolet test application.</returns>
        IUltravioletTestApplication WithAudioSubsystem(String audioSubsystem);

        /// <summary>
        /// Specifies the application's initialization code.
        /// </summary>
        /// <param name="initializer">An action which will initialize the application.</param>
        /// <returns>The Ultraviolet test application.</returns>
        IUltravioletTestApplication WithInitialization(Action<UltravioletContext> initializer);

        /// <summary>
        /// Specifies the application's content loading code.
        /// </summary>
        /// <param name="loader">An action which will load the unit test's required content.</param>
        /// <returns>The Ultraviolet test application.</returns>
        IUltravioletTestApplication WithContent(Action<ContentManager> loader);

        /// <summary>
        /// Renders a scene and outputs the resulting image.
        /// </summary>
        /// <param name="renderer">An action which will render the desired scene.</param>
        /// <returns>A bitmap containing the result of rendering the specified scene.</returns>
        Bitmap Render(Action<UltravioletContext> renderer);

        /// <summary>
        /// Runs the application until the specified predicate is true.
        /// </summary>
        /// <param name="predicate">The predicate that evaluates when the application should exit.</param>
        void RunUntil(Func<Boolean> predicate);

        /// <summary>
        /// Runs the application until the specified period of time has elapsed.
        /// </summary>
        /// <param name="time">The amount of time for which to run the application.</param>
        void RunFor(TimeSpan time);

        /// <summary>
        /// Runs a single frame of the application.
        /// </summary>
        void RunForOneFrame();
    }
}