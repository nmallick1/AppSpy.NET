import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { CodeSelectorContainerComponent } from '../code-selector-container/code-selector-container.component';
import {IDialogData} from '../shared/models/code-selector-container-model'
@Component({
  selector: 'app-profiler',
  templateUrl: './profiler.component.html',
  styleUrls: ['./profiler.component.scss']
})
export class ProfilerComponent implements OnInit {

  constructor(public dialog: MatDialog) { }

  ngOnInit(): void {
  }

  openDialog(functionNameToDecompile:string)
  {
    
    let dialogData = {
      functionName:functionNameToDecompile
    } as IDialogData;
    
    this.dialog.open(CodeSelectorContainerComponent, {
      width:"95%",
      height:"90%",
      data:dialogData
    } );
  }

}
