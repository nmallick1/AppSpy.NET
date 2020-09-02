using AppSpy.NET.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;


namespace AppSpy.NET.Analyzer
{
    public class SemanticAnalyzer : ICodeAnalyzer
    {

        public RuleCategories Category { get; set; }
        public string RuleName { get; set; }

        public bool isMatch(List<AnalysisCheckOutcome> checkOutcomes)
        {
            if (checkOutcomes == null)
            {
                throw new ArgumentNullException("checkOutcomes", "Failed to check match on NULL list of outcomes.");
            }
            return checkOutcomes.Where(o => o.Level != OutcomeLevels.NoMatch).Count() > 0;
        }

        public List<AnalysisCheckOutcome> GetAssertedOutcomes(List<AnalysisCheckOutcome> checkOutcomes)
        {
            if (checkOutcomes == null)
            {
                throw new ArgumentNullException("checkOutcomes", "Failed to check asserted outcomes on NULL list of outcomes.");
            }
            return checkOutcomes.Where(o => o.Level != OutcomeLevels.NoMatch).ToList<AnalysisCheckOutcome>();
        }

        List<AnalysisCheckOutcome> ICodeAnalyzer.RunCheck(StringBuilder codeToCheckAgainst)
        {
            SyntaxTree tree =  CSharpSyntaxTree.ParseText(codeToCheckAgainst.ToString(), CSharpParseOptions.Default);
            if(tree.TryGetRoot(out SyntaxNode rootNode))
            {
                //rootNode.DescendantNodes().OfType<WhileStatementSyntax>().FirstOrDefault()
            }

            throw new NotImplementedException();
        }
    }
}