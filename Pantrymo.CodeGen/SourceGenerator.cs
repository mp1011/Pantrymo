using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Diagnostics;
using System.Linq;

namespace SourceGenerator
{
    [Generator]
    public class SourceGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            var dataModelTrees = context.Compilation
                .SyntaxTrees
                .Where(s => s.FilePath.Contains("DataModels\\I"))
                .ToArray();

            foreach (var tree in dataModelTrees)
            {
                var name = tree.FilePath.Split('\\').Last()
                    .TrimStart('I')
                    .Replace(".cs", ".g.cs");

                var rewriter = new Rewriter();
                var transformed = rewriter.Visit(tree.GetRoot());
                context.AddSource(name, transformed.ToFullString());
            }
            
        }


        public void Initialize(GeneratorInitializationContext context)
        {
            //uncomment to enable debugging of the generator
            //Debugger.Launch();
        }
    }

    public class Rewriter : CSharpSyntaxRewriter
    {
        public override SyntaxNode VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
        {
            if (!(node.Parent is NamespaceDeclarationSyntax))
                return node;

            var interfaceName = node.ChildTokens().First(p => p.IsKind(SyntaxKind.IdentifierToken)).Text;
            var className = interfaceName.Substring(1) + "Gen";

            var classDec = SyntaxFactory.ClassDeclaration(
                attributeLists: new SyntaxList<AttributeListSyntax>(),
                modifiers: new SyntaxTokenList(),
                identifier: SyntaxFactory.Identifier(className).AddSpaceAround(),
                baseList: CreateBaseList(interfaceName),
                typeParameterList: null,
                constraintClauses: new SyntaxList<TypeParameterConstraintClauseSyntax>(),
                members: TransformPropertyDeclarations(node.Members));

            return classDec;
        }

        private SyntaxList<MemberDeclarationSyntax> TransformPropertyDeclarations(SyntaxList<MemberDeclarationSyntax> interfaceMembers)
        {
            return new SyntaxList<MemberDeclarationSyntax>(interfaceMembers.Select(m =>
            {
                return m.WithoutTrivia()
                     .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword).AddSpaceAfter())
                     .AddNewlineAndTabsBefore();          
            }));


        }

        private BaseListSyntax CreateBaseList(string interfaceName)
        {
            var type = SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName(interfaceName));
            var list = SyntaxFactory.SingletonSeparatedList(type as BaseTypeSyntax);
            var baseList = SyntaxFactory.BaseList(list);
            return baseList;
        }
    }
}