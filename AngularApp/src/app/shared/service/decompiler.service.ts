import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { ICodeDecompilationResult } from '../models/code-decompilation-result-model';

@Injectable({
  providedIn: 'root'
})
export class DecompilerService {

  private apiEndpoint:string = 'http://localhost:62008/api';
  private decompileMethodEndpoint:string = `${this.apiEndpoint}/decompileMethod`;
  private decompileMethodqueryParam:string = `fullyQualifiedFunctionName`;

  private _getDecompileMethodUri(methodNameToDecompile:string):string {
    return `${this.decompileMethodEndpoint}?${this.decompileMethodqueryParam}=${methodNameToDecompile}`;
  }
  constructor(private _http:HttpClient) { }

  private _handleError(error: any):Observable<ICodeDecompilationResult> {
    console.error('Error in DecompilerService');
    console.error(error);
    let returnValue = {
      successful:false,
      code:'',
      isError:true,
      errorObj:error
    } as ICodeDecompilationResult;
    return of(returnValue);
}

  public decompileMethod(methodNameToDecompile:string) : Observable<ICodeDecompilationResult> {
    if(!!methodNameToDecompile && methodNameToDecompile.length > 0) {
      return this._http.get<string>(`${this._getDecompileMethodUri(methodNameToDecompile)}`).pipe(
        map((response:string)=>{
          if(!!response && response.length > 0) {
            return {
              successful:true,
              code:response,
              isError:false,
              errorObj:{}
            } as ICodeDecompilationResult;
          }
          else {
            return {
              successful:false,
              code:'',
              isError:false,
              errorObj:{}
            } as ICodeDecompilationResult;
          }
          
        },this),
        catchError(this._handleError)
      );
    }
    else {
      return of({
        successful:false,
        code:'',
        isError:true,
        errorObj:{
          erorDescription:`Cannot decompile null/empty method name.`
        }
      } as ICodeDecompilationResult);
    }
    
  }


}
