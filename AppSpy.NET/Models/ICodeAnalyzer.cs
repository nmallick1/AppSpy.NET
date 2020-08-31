using System.Collections.Generic;
using System.Text;

namespace AppSpy.NET.Models
{
    interface ICodeAnalyzer
    {
        List<AnalysisCheckOutcome> RunCheck(StringBuilder codeToCheckAgainst);
        bool isMatch(List<AnalysisCheckOutcome> checkOutcomes);
        List<AnalysisCheckOutcome> GetAssertedOutcomes(List<AnalysisCheckOutcome> checkOutcomes);
    }            
}