import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { IDialogData } from '../shared/models/code-selector-container-model';
import { DecompilerService } from '../shared/service/decompiler.service';
import {ICodeDecompilationResult  } from "../shared/models/code-decompilation-result-model";
import { CodeAnalysisService } from '../shared/service/code-analysis.service';
import {ICodeAnalysisResponse, ICodeAnalysisResult, OutcomeLevels, RuleCategories, IAnalysisOutcome  } from "../shared/models/code-analysis-result-model";


@Component({
  selector: 'app-code-selector-container',
  templateUrl: './code-selector-container.component.html',
  styleUrls: ['./code-selector-container.component.scss']
})
export class CodeSelectorContainerComponent implements OnInit {
  public editorOptions = {
    theme: 'vs-dark',
    language: 'csharp',
    readOnly:true};
  //public code: string= "using System;\r\nusing System.Diagnostics;\r\nusing System.Threading;\r\n\r\nprivate void ForceCpuUsage(int percentage)\r\n{\r\n\tfor (int i = 0; i < Environment.ProcessorCount; i++)\r\n\t{\r\n\t\tnew Thread((ThreadStart)delegate\r\n\t\t{\r\n\t\t\tStopwatch stopwatch = new Stopwatch();\r\n\t\t\tstopwatch.Start();\r\n\t\t\twhile (true)\r\n\t\t\t{\r\n\t\t\t\tif (stopwatch.ElapsedMilliseconds > percentage)\r\n\t\t\t\t{\r\n\t\t\t\t\tThread.Sleep(100 - percentage);\r\n\t\t\t\t\tstopwatch.Reset();\r\n\t\t\t\t\tstopwatch.Start();\r\n\t\t\t\t}\r\n\t\t\t}\r\n\t\t}).Start();\r\n\t}\r\n}\r\n";
  public code: string= "Loading...";
  public lineNumberToHighlight_from:number = -1;
  public lineNumberToHighlight_to:number = -1;
  private _monacoEditor:any = null;
  public codeAnalysisResult:ICodeAnalysisResponse = null; 

  constructor(@Inject(MAT_DIALOG_DATA) public data: IDialogData, public dialogRef: MatDialogRef<CodeSelectorContainerComponent>,
    private _decompilerService:DecompilerService, private _codeAnalysisService:CodeAnalysisService) {   
   }

   public getStatusIcon(level:OutcomeLevels):string {
     let className = "fa ";
     switch (level) {
        case OutcomeLevels.Critical:
          className = `${className} fa-exclamation-circle critical-color`;
          break;
        case OutcomeLevels.Warning:
          className = `${className} fa-exclamation-triangle warning-color`;         
          break;    
        case OutcomeLevels.Success:
          className = `${className} fa-check-circle success-color`;
          break;
        default:
          className = `${className} fa-info-circle info-color`;
          break;
     }
     return className;
   }


   ngOnInit(): void {
     
    //this.lineNumberToHighlight_from = 13;
    //this.lineNumberToHighlight_to = 13;
    if(!!this.data && !!this.data.functionName && this.data.functionName.length > 0) {
      this._decompilerService.decompileMethod(this.data.functionName).subscribe((decompilationResult:ICodeDecompilationResult)=>{
        if(decompilationResult.successful) {
          this.code = decompilationResult.code;
          this._codeAnalysisService.analyzeCode(this.code).subscribe((codeAnalysisResponse:ICodeAnalysisResult)=>{
            if(codeAnalysisResponse.successful) {              
              this.codeAnalysisResult = codeAnalysisResponse.codeAnalysisResult;
            }
            else {
              this.codeAnalysisResult = {
                ruleName : 'No issues found.',
                ruleCategory : RuleCategories.Miscellaneous,
                analysisResults:[ {
                  level: OutcomeLevels.Success,
                  title: 'No issues found.',
                  description: 'No code level reliability issues found within this code block.',
                  suggestedFix: 'We\'re good here.',
                  lineNumber:-1,
                  matchingCodeBlock: '',
                  ruleCategory:RuleCategories.Miscellaneous                  
                } as IAnalysisOutcome,
               ]
              } as ICodeAnalysisResponse;
            }
            
          });
        }
        else {
          this.code = `${this.code}\n\tFAILED TO DECOMPILE METHOD ${this.data.functionName}`;
        }
      });
    } 
   }

   onInit(editor: any) {
     this._monacoEditor = editor;
     if (this.lineNumberToHighlight_from > -1 && this.lineNumberToHighlight_from > -1) {
      const t = this._monacoEditor.deltaDecorations([], [
        {
          range: new monaco.Range(this.lineNumberToHighlight_from, 1, this.lineNumberToHighlight_to, 1),
          options: {
            isWholeLine: true,
            className: 'myContentClass',
            glyphMarginClassName: 'myGlyphMarginClass'
          }
        }
      ]);
     }    
  }

  public highlightCodeLine(analysisResult:IAnalysisOutcome): void {
    if(!!this._monacoEditor && !!analysisResult && analysisResult.lineNumber > -1) {
      this._monacoEditor.deltaDecorations([], [
        {
          range: new monaco.Range(analysisResult.lineNumber, 1, analysisResult.lineNumber, 1),
          options: {
            isWholeLine: true,
            className: 'myContentClass',
            glyphMarginClassName: 'myGlyphMarginClass'
          }
        }
      ]);
    }
  }

  

  public closeDialog() {
    this.code = "Loading...";
    this.lineNumberToHighlight_from = -1;
    this.lineNumberToHighlight_to = -1;
    this._monacoEditor = null;
    this.codeAnalysisResult = null; 
    this.dialogRef.close('Done!');
  }

}
