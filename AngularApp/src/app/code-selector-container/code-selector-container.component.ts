import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DialogData } from '../shared/code-selector-container-model';


@Component({
  selector: 'app-code-selector-container',
  templateUrl: './code-selector-container.component.html',
  styleUrls: ['./code-selector-container.component.scss']
})
export class CodeSelectorContainerComponent implements OnInit {
  public editorOptions = {
    theme: 'vs-dark',
    language: 'c#',
    readOnly:true};
  public code: string= "using System;\r\nusing System.Diagnostics;\r\nusing System.Threading;\r\n\r\nprivate void ForceCpuUsage(int percentage)\r\n{\r\n\tfor (int i = 0; i < Environment.ProcessorCount; i++)\r\n\t{\r\n\t\tnew Thread((ThreadStart)delegate\r\n\t\t{\r\n\t\t\tStopwatch stopwatch = new Stopwatch();\r\n\t\t\tstopwatch.Start();\r\n\t\t\twhile (true)\r\n\t\t\t{\r\n\t\t\t\tif (stopwatch.ElapsedMilliseconds > percentage)\r\n\t\t\t\t{\r\n\t\t\t\t\tThread.Sleep(100 - percentage);\r\n\t\t\t\t\tstopwatch.Reset();\r\n\t\t\t\t\tstopwatch.Start();\r\n\t\t\t\t}\r\n\t\t\t}\r\n\t\t}).Start();\r\n\t}\r\n}\r\n";
  public lineNumberToHighlight_from:number = -1;
  public lineNumberToHighlight_to:number = -1;

  constructor(@Inject(MAT_DIALOG_DATA) public data: DialogData, public dialogRef: MatDialogRef<CodeSelectorContainerComponent>) {
    this.lineNumberToHighlight_from = 13;
    this.lineNumberToHighlight_to = 13;
   }

   onInit(editor: any) {
    const t = editor.deltaDecorations([], [
      {
        range: new monaco.Range(this.lineNumberToHighlight_from, 1, this.lineNumberToHighlight_to, 1),
        options: {
          isWholeLine: true,
          className: 'myContentClass',
          glyphMarginClassName: 'myGlyphMarginClass'
        }
      }
    ]);
    console.log(t);
  }

  ngOnInit(): void {  
    
  }

  public closeDialog() {
    this.dialogRef.close('Done!');
  }

}
