import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ProfilerComponent } from './profiler/profiler.component';
import { CodeSelectorContainerComponent } from './code-selector-container/code-selector-container.component';
import { MatDialogModule } from "@angular/material/dialog";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { FormsModule } from '@angular/forms';
import { MonacoEditorModule } from 'ngx-monaco-editor';
import { HttpClientModule } from '@angular/common/http';


@NgModule({
  declarations: [
    AppComponent,
    ProfilerComponent,
    CodeSelectorContainerComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    MatDialogModule,
    BrowserAnimationsModule, 
    FormsModule,
    MonacoEditorModule.forRoot(),
    HttpClientModule,
  ],
  entryComponents:[CodeSelectorContainerComponent],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
