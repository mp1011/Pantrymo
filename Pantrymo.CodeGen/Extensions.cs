using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Linq;

namespace SourceGenerator
{
    public static class Extensions
    {
        public static SyntaxToken AddSpaceAfter(this SyntaxToken token)
            => token.WithTrailingTrivia(SyntaxFactory.Whitespace(" "));
        

        public static SyntaxToken AddSpaceBefore(this SyntaxToken token)
            => token.WithLeadingTrivia(SyntaxFactory.Whitespace(" "));

        public static SyntaxToken AddSpaceAround(this SyntaxToken token)
            => token.AddSpaceBefore().AddSpaceAfter();

        public static TNode AddNewlineAndTabsBefore<TNode>(this TNode node) where TNode : SyntaxNode
           => node.WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed, SyntaxFactory.Tab, SyntaxFactory.Tab);

    }
}
