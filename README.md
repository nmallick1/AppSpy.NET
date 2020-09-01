
# AppSpy.NET
Given a function name , inspect .NET code against a set of known bad coding practices that cause production perf and stability impact.
1. Call to decompile a method

	Request : GET http://localhost:62008/api/decompileMethod?fullyQualifiedFunctionName=mawshol!demomvp.HighCPUPage1.ForceCpuUsage(int32)
    Response : Source Code

2. Call to run a check against source code

	Request : POST http://localhost:62008/api/analyzeCode
    Request Content-Type : application.json
    Request Body : Source code from above call.
  Response
```
    {
        "RuleName": "Infinite Loop Check",
        "RuleCategory": 0,
        "AnalysisCheckResults": [
            {
                "Level": 2,
                "Title": "Potential infinite while loop",
                "Description": "Infinite while loops can cause CPU spike if the condition to break the loop fails.",
                "SuggestedFix": "Consider assigning a max iteration to the while loop or an alternative bounding condition and deterministically terminate the loop.",
                "MatchingCodeBlock": {
                    "m_MaxCapacity": 2147483647,
                    "Capacity": 16,
                    "m_StringValue": "while (true)",
                    "m_currentThread": 0
                },
                "RuleCategory": 0,
                "LineNumber": 13
            }
        ]
    }
```
