﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text;
using TwistedLogik.Ultraviolet.Testing;

namespace TwistedLogik.Ultraviolet.Tests.Graphics.Graphics2D.Text
{
    [TestClass]
    public class TextTableTests : UltravioletApplicationTestFramework
    {
        private class TextTableViewModel
        {
            public String Name
            { get; set; }
            
            public String Description
            { get; set; }

            public String Type
            { get; set; }

            public String Subtype
            { get; set; }
        }

        [TestMethod]
        [TestCategory("Rendering")]
        public void TextTable_RendersFromViewModel()
        {
            const Int32 TablePadding = 4;
            const Int32 TableOffset  = 32;

            var spriteBatch  = default(SpriteBatch);
            var spriteFont   = default(SpriteFont);
            var textRenderer = default(TextRenderer);
            var table        = default(TextTable<TextTableViewModel>);
            var tableLayout  = default(TextTableLayout);
            var tableTexture = default(Texture2D);

            var result = GivenAnUltravioletApplication()
                .WithContent(content =>
                {
                    spriteBatch  = SpriteBatch.Create();
                    spriteFont   = content.Load<SpriteFont>("Fonts/SegoeUI");

                    textRenderer = new TextRenderer();
                    textRenderer.RegisterFont("header", content.Load<SpriteFont>("Fonts/Garamond"));
                    textRenderer.RegisterFont("text", content.Load<SpriteFont>("Fonts/SegoeUI"));

                    tableTexture = Texture2D.Create(1, 1);
                    tableTexture.SetData(new[] { Color.White });

                    tableLayout     = content.Load<TextTableLayout>("Tables/TestTextTable");
                    table           = tableLayout.Create<TextTableViewModel>(textRenderer, spriteFont);
                    table.ViewModel = new TextTableViewModel()
                    {
                        Name        = "Legendary Item of Power",
                        Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean feugiat dui nec dui interdum, a bibendum odio pellentesque. Ut cursus neque eros, nec luctus ligula euismod a.",
                        Type        = "Weapon",
                        Subtype     = "Ranged"
                    };
                })
                .Render(uv =>
                {
                    uv.GetGraphics().Clear(Color.Magenta);

                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

                    table.PerformLayout();

                    spriteBatch.Draw(tableTexture, new RectangleF(TableOffset, TableOffset, 
                        table.ActualWidth + (TablePadding * 2), 
                        table.ActualHeight + (TablePadding * 2)), Color.Black * 0.75f);

                    table.Draw(spriteBatch, Vector2.One * (TableOffset + TablePadding));

                    spriteBatch.End();
                });

            TheResultingImage(result).ShouldMatch(@"Resources\Expected\Graphics\Graphics2D\Text\TextTable_RendersFromViewModel.png");
        }
    }
}
