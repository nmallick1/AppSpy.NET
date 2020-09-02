export interface ICodeAnalysisResult {
  successful:boolean,
  codeAnalysisResult:ICodeAnalysisResponse,
  isError:boolean,
  errorObj:any
  }

  export enum RuleCategories {
    HighCpu = 0,
    Performance = 1,
    Network = 2,
    MemoryLeak = 3,
    ResourceLeak = 4,
    Exception = 5,
    Crash = 6,
    Miscellaneous = 7
  }

  export enum OutcomeLevels {
    Critical = 0,
    Success = 1,
    Warning = 2,
    Info = 3,
    NoMatch = 4
  }


  export interface IAnalysisOutcome {
    ruleCategory:RuleCategories,
    level:OutcomeLevels,
    title:string,
    description:string,
    suggestedFix:string,
    matchingCodeBlock:string,
    lineNumber:number    
  }



  export interface ICodeAnalysisResponse {
    ruleName:string,
    ruleCategory:RuleCategories,
    analysisResults:IAnalysisOutcome[]
  }