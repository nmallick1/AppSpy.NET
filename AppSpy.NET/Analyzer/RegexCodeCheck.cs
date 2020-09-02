using AppSpy.NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AppSpy.NET.Analyzer
{

    public class RegExCheck
    {
        public string CheckName { get; set; }
        public Regex RegexToCheck { get; set; }
        public AnalysisCheckOutcome ResponseIfCheckPassed { get; set; }

        public RegExCheck(Regex regexTocheck, AnalysisCheckOutcome outcomeOnMatch)
        {
            this.RegexToCheck = regexTocheck ?? throw new ArgumentNullException("regexTocheck", "Failed to create RegExCheck due to NULL parameter");
            this.ResponseIfCheckPassed = outcomeOnMatch ?? throw new ArgumentNullException("outcomeOnMatch", "Failed to create RegExCheck due to NULL parameter");
        }

    }

    public class RegexCodeCheck : ICodeAnalyzer
    {
        public int LineFromPos(string input, int indexPosition)
        {
            int lineNumber = 1;
            for (int i = 0; i < indexPosition; i++)
            {
                if (input[i] == '\n') lineNumber++;
            }
            return lineNumber;
        }

        public RuleCategories Category { get; set; }
        public string RuleName { get; set; }
        public List<RegExCheck> checkRegExs { get; set; }

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


        public RegexCodeCheck(RuleCategories category, string ruleName, List<RegExCheck> regExsToCheck)
        {
            if (regExsToCheck == null)
            {
                throw new ArgumentNullException("regExToCheck", "Failed to create check due to null regex list.");
            }

            if (regExsToCheck.Count<1)
            {
                throw new ArgumentException("Failed to create check due to empty regex list.", "regExToCheck");
            }

            this.checkRegExs = regExsToCheck;
            this.Category = category;
            this.RuleName = ruleName;
        }

        public List<AnalysisCheckOutcome> RunCheck(StringBuilder codeToCheckAgainst)
        {
            List<AnalysisCheckOutcome> finalResult = new List<AnalysisCheckOutcome>();
            foreach (RegExCheck currCheck in this.checkRegExs)
            {
                var currMatch = currCheck.RegexToCheck.Match(codeToCheckAgainst.ToString());
                if (currMatch.Success)
                {
                    AnalysisCheckOutcome currOutcome = AnalysisCheckOutcome.CreateInstance(currCheck.ResponseIfCheckPassed);
                    currOutcome.MatchingCodeBlock = currMatch.Value;
                    currOutcome.RuleCategory = this.Category;
                    currOutcome.LineNumber = this.LineFromPos(codeToCheckAgainst.ToString(), currMatch.Index) + 1;
                    finalResult.Add(currOutcome);
                }
                else
                {
                    AnalysisCheckOutcome currOutcome = AnalysisCheckOutcome.CreateInstance(currCheck.ResponseIfCheckPassed);
                    currOutcome.MatchingCodeBlock = string.Empty;
                    currOutcome.Level = OutcomeLevels.NoMatch;
                    currOutcome.RuleCategory = this.Category;
                    finalResult.Add(currOutcome);
                }
            }
            return finalResult;
        }
    }
}