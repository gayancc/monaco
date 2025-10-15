import { Component } from '@angular/core';
import { ScriptEditorComponent } from '../script-editor/script-editor.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [ScriptEditorComponent],
  templateUrl: './app.component.html'
})
export class AppComponent {}
