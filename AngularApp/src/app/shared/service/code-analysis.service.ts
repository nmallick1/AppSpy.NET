import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { ICodeAnalysisResponse, ICodeAnalysisResult, IAnalysisOutcome, RuleCategories, OutcomeLevels } from '../models/code-analysis-result-model';

@Injectable({
  providedIn: 'root'
})
export class CodeAnalysisService {

  private apiEndpoint:string = 'http://localhost:62008/api';
  private codeAnalysisEndpoint:string = `${this.apiEndpoint}/analyzeCode`;
  
  private _getAnalyzeCodeUri():string {
    return `${this.codeAnalysisEndpoint}`;
  }

  constructor(private _http:HttpClient) { }

  private _handleError(error: any):Observable<ICodeAnalysisResult> {
    console.error('Error in DecompilerService');
    console.error(error);
    let returnValue = {
      successful:false,
      codeAnalysisResult:null,
      isError:true,
      errorObj:error
    } as ICodeAnalysisResult;
    return of(returnValue);
}

private _getHeaders(): HttpHeaders {
  let headers = new HttpHeaders();
  headers = headers.set('Content-Type', 'application/json');
  headers = headers.set('Accept', 'application/json');
  return headers;
}

  public analyzeCode(codeString:string):Observable<ICodeAnalysisResult> {
    if(!!codeString && codeString.length > 0) {
      return this._http.post<ICodeAnalysisResponse>(this._getAnalyzeCodeUri(), JSON.stringify(codeString), {
        headers: this._getHeaders()
      }).pipe(
        map((response:any)=>{          
          if(!!response) {
            let analysisOutcomesArr:IAnalysisOutcome[] = [];
            if(!!response.AnalysisCheckResults && (response.AnalysisCheckResults instanceof Array) && response.AnalysisCheckResults.length > 0) {
              for(let i:number = 0; i < response.AnalysisCheckResults.length; i++) {
                let currOutcome = {
                  ruleCategory : response.AnalysisCheckResults[i].RuleCategory,
                  level : response.AnalysisCheckResults[i].Level,
                  title : response.AnalysisCheckResults[i].Title,
                  description : response.AnalysisCheckResults[i].Description,
                  suggestedFix : response.AnalysisCheckResults[i].SuggestedFix,
                  matchingCodeBlock : response.AnalysisCheckResults[i].MatchingCodeBlock,
                  lineNumber:response.AnalysisCheckResults[i].LineNumber 
                } as IAnalysisOutcome
                analysisOutcomesArr.push(currOutcome);
              }              
            }
            
            let analysisResult = {
              ruleName: response.RuleName,
              ruleCategory: response.RuleCategory,
              analysisResults:analysisOutcomesArr
            } as ICodeAnalysisResponse;
            return {
              successful:true,
              codeAnalysisResult:analysisResult,
              isError:false,
              errorObj:{}
            } as ICodeAnalysisResult;
          }
          else {
            return {
              successful:false,
              codeAnalysisResult:null,
              isError:false,
              errorObj:{}
            } as ICodeAnalysisResult;
          }
        }, this), 
        catchError(this._handleError)
      );
    }
    else {
      return of({
        successful:false,
        codeAnalysisResult:null,
        isError:true,
        errorObj:{
          erorDescription:`Cannot analyze null/empty code.`
        }
      } as ICodeAnalysisResult);
    }
    //return null;
  }
}
