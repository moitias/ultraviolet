﻿using System;
using System.Linq;
using TwistedLogik.Ultraviolet.Tooling;

namespace UvFont
{
    /// <summary>
    /// Represents the user-specified parameters which are used to determine how the font is generated.
    /// </summary>
    public class FontGenerationParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FontGenerationParameters"/> class.
        /// </summary>
        /// <param name="args">The application's command line arguments.</param>
        public FontGenerationParameters(String[] args)
        {
            if (args == null || !args.Any())
                throw new InvalidCommandLineException();

            var parser = new CommandLineParser(args.Skip(1));
            if (parser.IsParameter(args.First()))
            {
                throw new InvalidCommandLineException();
            }

            NoBold   = parser.HasArgument("nobold");
            NoItalic = parser.HasArgument("noitalic");

            FontName = args.First();
            FontSize = parser.GetArgumentOrDefault<Single>("fontsize", 16f);
            Overhang = parser.GetArgumentOrDefault<Int32>("overhang", 0);

            SourceText    = parser.GetArgumentOrDefault<String>("sourcetext");
            SourceFile    = parser.GetArgumentOrDefault<String>("sourcefile");
            SourceCulture = parser.GetArgumentOrDefault<String>("sourceculture");

            if (SourceText != null && SourceFile != null)
                throw new InvalidCommandLineException("Both a source text and a source file were specified. Pick one!");

            SubstitutionCharacter = parser.GetArgumentOrDefault<Char>("sub", '?');
        }

        /// <summary>
        /// Gets or sets a value indicating whether the bold font faces are exluded.
        /// </summary>
        public Boolean NoBold { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the italic font faces are excluded.
        /// </summary>
        public Boolean NoItalic { get; set; }

        /// <summary>
        /// Gets or sets the name of the font family for which a font definition is being generated.
        /// </summary>
        public String FontName { get; set; }

        /// <summary>
        /// Gets or sets the point size of the font being generated.
        /// </summary>
        public Single FontSize { get; set; }

        /// <summary>
        /// Gets or sets the amount of additional overhang space to include when generating glyphs.
        /// </summary>
        public Int32 Overhang { get; set; }

        /// <summary>
        /// Gets or sets the source text to use when determining which glyphs to include in the font.
        /// </summary>
        public String SourceText { get; set; }

        /// <summary>
        /// Gets or sets the source file to use when determining which glyphs to include in the font.
        /// </summary>
        public String SourceFile { get; set; }

        /// <summary>
        /// Gets or sets the culture to use when extracting source text from a localization database.
        /// </summary>
        public String SourceCulture { get; set; }

        /// <summary>
        /// Gets or sets the font's substitution character.
        /// </summary>
        public Char SubstitutionCharacter { get; set; }
    }
}
