using System.Collections.Generic;
using System.Text;

namespace AppSpy.NET.Models
{
    public enum RuleCategories
    {
        HighCpu,
        Performance,
        Network,
        MemoryLeak,
        ResourceLeak,
        Exception,
        Crash,
        Miscellaneous
    }
    public enum OutcomeLevels
    {
        Critical,
        Success,
        Warning,
        Info,
        NoMatch
    }

    public class AnalysisCheckOutcome
    {
        public OutcomeLevels Level { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string SuggestedFix { get; set; }
        public StringBuilder MatchingCodeBlock { get; set; }
        public RuleCategories RuleCategory { get; set; }

        public long LineNumber { get; set; }

        public AnalysisCheckOutcome(OutcomeLevels outcomeLevel, string outcomeTitle, string outcomeDescription, string outcomeSuggestedFix)
        {
            this.Level = outcomeLevel;
            this.Title = outcomeTitle;
            this.Description = outcomeDescription;
            this.SuggestedFix = outcomeSuggestedFix;
            this.LineNumber = -1;
        }
        public AnalysisCheckOutcome(AnalysisCheckOutcome objToCopyFrom)
        {
            this.Level = objToCopyFrom.Level;
            this.Title = objToCopyFrom.Title;
            this.Description = objToCopyFrom.Description;
            this.SuggestedFix = objToCopyFrom.SuggestedFix;
            this.MatchingCodeBlock = objToCopyFrom.MatchingCodeBlock;
            this.RuleCategory = objToCopyFrom.RuleCategory;
            this.LineNumber = objToCopyFrom.LineNumber;
        }

        public static AnalysisCheckOutcome CreateInstance(AnalysisCheckOutcome objToCopyFrom)
        {
            return new AnalysisCheckOutcome(objToCopyFrom);
        }
    }

    public class AnalysisCheckOutcomeResponse
    {
        public string RuleName;
        public RuleCategories RuleCategory;
        public List<AnalysisCheckOutcome> AnalysisCheckResults;
    }
}