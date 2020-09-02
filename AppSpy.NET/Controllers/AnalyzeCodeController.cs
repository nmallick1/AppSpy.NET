using AppSpy.NET.Analyzer;
using AppSpy.NET.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Cors;

namespace AppSpy.NET.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AnalyzeCodeController : ApiController
    {
        // GET: api/AnalyzeCode
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/AnalyzeCode/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/analyzeCode

        [Route("api/analyzeCode")]
        public AnalysisCheckOutcomeResponse Post([FromBody]string codeString)
        {
            if (string.IsNullOrWhiteSpace(codeString))
            {
                throw new ArgumentNullException("codeString", "Unable to run problem detection analysis on empty code block");
            }
            StringBuilder codeSb = new StringBuilder(string.Empty);
            string[] codeLines = codeString.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            foreach (string codeLine in codeLines)
            {
                codeSb.AppendLine(codeLine);
            }

            Regex infiniteWhileLoopRegEx = new Regex(@"[\{|\}]*\s*[while]+\s*[\(]\s*[true|1|\!false|\!0]+\s*[\)]", RegexOptions.IgnoreCase);
            AnalysisCheckOutcome outcomeForInfiniteWhileLoop = new AnalysisCheckOutcome(OutcomeLevels.Warning, "Potential infinite while loop", "An infinite while loop can cause CPU spike if the condition to break the loop fails.", "Consider assigning a max iteration to the while loop or an alternative bounding condition and deterministically terminate the loop.");
            RegExCheck infiniteWhileLoop = new RegExCheck(infiniteWhileLoopRegEx, outcomeForInfiniteWhileLoop);


            RegexCodeCheck infiniteLoopCheck = new RegexCodeCheck(RuleCategories.HighCpu, "Infinite Loop Check", new List<RegExCheck>() { infiniteWhileLoop });
            List<AnalysisCheckOutcome> infiniteLooEvalResults = infiniteLoopCheck.RunCheck(codeSb);
            if (infiniteLoopCheck.isMatch(infiniteLooEvalResults))
            {
                AnalysisCheckOutcomeResponse response = new AnalysisCheckOutcomeResponse();
                response.RuleName = infiniteLoopCheck.RuleName;
                response.RuleCategory = infiniteLoopCheck.Category;
                response.AnalysisCheckResults = new List<AnalysisCheckOutcome>();
                foreach (AnalysisCheckOutcome evalResult in infiniteLoopCheck.GetAssertedOutcomes(infiniteLooEvalResults))
                {
                    evalResult.MatchingCodeBlock = evalResult.MatchingCodeBlock.ToString().Trim().Replace(@"\t", string.Empty).Replace(@"\n", string.Empty);
                    response.AnalysisCheckResults.Add(evalResult);
                }
                return response;
            }
            else
            {
                return null;
            }

            
        }

        // PUT: api/AnalyzeCode/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/AnalyzeCode/5
        public void Delete(int id)
        {
        }
    }
}
