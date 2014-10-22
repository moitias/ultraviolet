﻿using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Contains methods for rendering formatted text.
    /// </summary>
    public class TextRenderer
    {
        /// <summary>
        /// Registers a style with the specified name.
        /// </summary>
        /// <param name="name">The name of the style to register.</param>
        /// <param name="style">The style to register.</param>
        public void RegisterStyle(String name, TextStyle style)
        {
            layoutEngine.RegisterStyle(name, style);
        }

        /// <summary>
        /// Unregisters the style with the specified name.
        /// </summary>
        /// <param name="name">The name of the style to unregister.</param>
        /// <returns><c>true</c> if the style was unregistered; otherwise, <c>false</c>.</returns>
        public bool UnregisterStyle(String name)
        {
            return layoutEngine.UnregisterStyle(name);
        }

        /// <summary>
        /// Registers the font with the specified name.
        /// </summary>
        /// <param name="name">The name of the font to register.</param>
        /// <param name="font">The font to register.</param>
        public void RegisterFont(String name, SpriteFont font)
        {
            layoutEngine.RegisterFont(name, font);
        }

        /// <summary>
        /// Unregisters the font with the specified name.
        /// </summary>
        /// <param name="name">The name of the font to unregister.</param>
        /// <returns><c>true</c> if the font was unregistered; otherwise, <c>false</c>.</returns>
        public bool UnregisterFont(String name)
        {
            return layoutEngine.UnregisterFont(name);
        }

        /// <summary>
        /// Registers the icon with the specified name.
        /// </summary>
        /// <param name="name">The name of the icon to register.</param>
        /// <param name="icon">The icon to register.</param>
        public void RegisterIcon(String name, SpriteAnimation icon)
        {
            layoutEngine.RegisterIcon(name, icon);
        }

        /// <summary>
        /// Unregisters the icon with the specified name.
        /// </summary>
        /// <param name="name">The name of the icon to unregister.</param>
        /// <returns><c>true</c> if the icon was unregistered; otherwise, <c>false</c>.</returns>
        public bool UnregisterIcon(String name)
        {
            return layoutEngine.UnregisterIcon(name);
        }

        /// <summary>
        /// Lexes the specified string.
        /// </summary>
        /// <param name="input">The string to lex.</param>
        /// <param name="output">The lexed token stream.</param>
        public void Lex(String input, TextLexerResult output)
        {
            Contract.Require(input, "input");
            Contract.Require(output, "output");

            lexer.Lex(input, output);
        }

        /// <summary>
        /// Parses the specified string.
        /// </summary>
        /// <param name="input">The token stream to parse.</param>
        /// <param name="output">The parsed token stream.</param>
        public void Parse(String input, TextParserResult output)
        {
            Contract.Require(input, "input");
            Contract.Require(output, "output");

            lexer.Lex(input, lexerResult);
            parser.Parse(lexerResult, output);
        }

        /// <summary>
        /// Parses the specified token stream.
        /// </summary>
        /// <param name="input">The token stream to parse.</param>
        /// <param name="output">The parsed token stream.</param>
        public void Parse(TextLexerResult input, TextParserResult output)
        {
            Contract.Require(input, "input");
            Contract.Require(output, "output");

            parser.Parse(lexerResult, output);
        }

        /// <summary>
        /// Calculates a layout for the specified text.
        /// </summary>
        /// <param name="input">The string of text to lay out.</param>
        /// <param name="output">The formatted text with layout information.</param>
        /// <param name="settings">The layout settings.</param>
        public void CalculateLayout(String input, TextLayoutResult output, TextLayoutSettings settings)
        {
            Contract.Require(input, "input");
            Contract.Require(output, "output");

            lexer.Lex(input, lexerResult);
            parser.Parse(lexerResult, parserResult);
            layoutEngine.CalculateLayout(parserResult, output, settings);
        }

        /// <summary>
        /// Calculates a layout for the specified text.
        /// </summary>
        /// <param name="input">The lexed text to lay out.</param>
        /// <param name="output">The formatted text with layout information.</param>
        /// <param name="settings">The layout settings.</param>
        public void CalculateLayout(TextLexerResult input, TextLayoutResult output, TextLayoutSettings settings)
        {
            Contract.Require(input, "input");
            Contract.Require(output, "output");

            parser.Parse(input, parserResult);
            layoutEngine.CalculateLayout(parserResult, output, settings);
        }

        /// <summary>
        /// Calculates a layout for the specified text.
        /// </summary>
        /// <param name="input">The parsed text to lay out.</param>
        /// <param name="output">The formatted text with layout information.</param>
        /// <param name="settings">The layout settings.</param>
        public void CalculateLayout(TextParserResult input, TextLayoutResult output, TextLayoutSettings settings)
        {
            Contract.Require(input, "input");
            Contract.Require(output, "output");

            layoutEngine.CalculateLayout(input, output, settings);
        }

        /// <summary>
        /// Draws a string of formatted text.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch with which to draw the text.</param>
        /// <param name="input">The string to draw.</param>
        /// <param name="position">The position in screen coordinates at which to draw.</param>
        /// <param name="color">The color with which to draw the text.</param>
        /// <param name="settings">The layout settings.</param>
        public void Draw(SpriteBatch spriteBatch, String input, Vector2 position, Color color, TextLayoutSettings settings)
        {
            Contract.Require(spriteBatch, "spriteBatch");
            Contract.Require(input, "input");

            lexer.Lex(input, lexerResult);
            parser.Parse(lexerResult, parserResult);
            layoutEngine.CalculateLayout(parserResult, layoutResult, settings);

            Draw(spriteBatch, layoutResult, position, color);
        }

        /// <summary>
        /// Draws a string of formatted text.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch with which to draw the text.</param>
        /// <param name="input">The token stream to draw.</param>
        /// <param name="position">The position in screen coordinates at which to draw.</param>
        /// <param name="color">The color with which to draw the text.</param>
        /// <param name="settings">The layout settings.</param>
        public void Draw(SpriteBatch spriteBatch, TextLexerResult input, Vector2 position, Color color, TextLayoutSettings settings)
        {
            Contract.Require(spriteBatch, "spriteBatch");
            Contract.Require(input, "input");

            parser.Parse(input, parserResult);
            layoutEngine.CalculateLayout(parserResult, layoutResult, settings);

            Draw(spriteBatch, layoutResult, position, color);
        }

        /// <summary>
        /// Draws a string of formatted text.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch with which to draw the text.</param>
        /// <param name="input">The token stream to draw.</param>
        /// <param name="position">The position in screen coordinates at which to draw.</param>
        /// <param name="color">The color with which to draw the text.</param>
        /// <param name="settings">The layout settings.</param>
        public void Draw(SpriteBatch spriteBatch, TextParserResult input, Vector2 position, Color color, TextLayoutSettings settings)
        {
            Contract.Require(spriteBatch, "spriteBatch");
            Contract.Require(input, "input");

            layoutEngine.CalculateLayout(input, layoutResult, settings);

            Draw(spriteBatch, layoutResult, position, color);
        }

        /// <summary>
        /// Draws a string of formatted text.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch with which to draw the text.</param>
        /// <param name="input">The token stream to draw.</param>
        /// <param name="position">The position in screen coordinates at which to draw.</param>
        /// <param name="color">The color with which to draw the text.</param>
        public void Draw(SpriteBatch spriteBatch, TextLayoutResult input, Vector2 position, Color color)
        {
            Contract.Require(spriteBatch, "spriteBatch");
            Contract.Require(input, "input");

            var alpha = color.A / (float)Byte.MaxValue;

            foreach (var token in input)
            {
                var tokenBounds = token.Bounds;
                if (token.Icon != null)
                {
                    var tokenOrigin = token.Icon.Controller.GetFrame().Origin;
                    var tokenPosition = new Vector2(
                        position.X + tokenBounds.X + tokenOrigin.X, 
                        position.Y + tokenBounds.Y + tokenOrigin.Y);
                    spriteBatch.DrawSprite(token.Icon.Controller, tokenPosition, null, null, Color.White * alpha, 0f);
                }
                else
                {
                    var tokenPosition = new Vector2(
                        position.X + tokenBounds.X, 
                        position.Y + tokenBounds.Y);
                    spriteBatch.DrawString(token.FontFace, token.Text, tokenPosition, (token.Color * alpha) ?? color);
                }
            }
        }

        // The lexer and parser used to process input text.
        private readonly TextLexer lexer = new TextLexer();
        private readonly TextLexerResult lexerResult = new TextLexerResult();
        private readonly TextParser parser = new TextParser();
        private readonly TextParserResult parserResult = new TextParserResult();
        private readonly TextLayoutEngine layoutEngine = new TextLayoutEngine();
        private readonly TextLayoutResult layoutResult = new TextLayoutResult();
    }
}