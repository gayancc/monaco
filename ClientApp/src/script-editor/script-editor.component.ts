import { Component, ElementRef, ViewChild, AfterViewInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import * as monaco from 'monaco-editor/esm/vs/editor/editor.api';
import { ApiService } from '../app/api.service';
import { registerCSharpLanguageFeatures } from './monaco-init';

@Component({
    selector: 'app-script-editor',
    standalone: true,
    imports: [CommonModule],
    templateUrl: './script-editor.component.html'
})
export class ScriptEditorComponent implements AfterViewInit, OnDestroy {
    @ViewChild('editorHost', { static: true }) editorHost!: ElementRef<HTMLDivElement>;
    editor?: monaco.editor.IStandaloneCodeEditor;
    code = 'using System;\nusing System.Linq;\nusing System.Collections.Generic;\nusing Harmony.ScriptSdk;\npublic async Task<object> Main(){ var all = await Payroll.GetAllEmployeesAsync(); return all.Select(x=>new{ x.Id,x.Name,x.Salary}).ToList(); }';
    status = ''; statusColor = 'gray'; result: any;

    constructor(private api: ApiService) { }

    ngAfterViewInit() {
        (self as any).MonacoEnvironment = {
            getWorkerUrl: () => '/assets/monaco/vs/base/worker/workerMain.js'
        };
        this.editor = monaco.editor.create(this.editorHost.nativeElement, {
            language: 'csharp', value: this.code, automaticLayout: true, minimap: { enabled: false }, fontSize: 14
        });
        const model = this.editor.getModel();
        if (model) registerCSharpLanguageFeatures(model);
        let t: any;
        this.editor.onDidChangeModelContent(() => {
            this.code = this.editor!.getValue();
            clearTimeout(t); t = setTimeout(() => this.pushDiagnostics(), 600);
        });
        this.pushDiagnostics();
    }
    ngOnDestroy() { this.editor?.dispose(); }

    pushDiagnostics() {
        this.api.diagnostics(this.code).subscribe(r => {
            const m = this.editor?.getModel(); if (!m) return;
            const markers = (r.diagnostics as any[]).map((d: any) => ({
                startLineNumber: 1, startColumn: 1, endLineNumber: 1, endColumn: 1,
                message: d.msg, severity: monaco.MarkerSeverity.Error
            }));
            monaco.editor.setModelMarkers(m, 'owner', markers);
            this.status = r.ok ? 'OK' : 'Errors'; this.statusColor = r.ok ? 'green' : 'crimson';
        });
    }
    validate() { this.pushDiagnostics(); }
    run() { this.api.run(this.code).subscribe(r => this.result = r.result); }
}
