import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { MonacoEditorModule } from 'ngx-monaco-editor-v2';
import { HttpClientModule } from '@angular/common/http';
import { AppComponent } from './app.component';
import { ScriptEditorComponent } from './script-editor/script-editor.component';
@NgModule({
    declarations: [AppComponent, ScriptEditorComponent],
    imports: [BrowserModule, MonacoEditorModule.forRoot(), HttpClientModule],
    bootstrap: [AppComponent]
})
export class AppModule { }