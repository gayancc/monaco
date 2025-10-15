import * as monaco from 'monaco-editor/esm/vs/editor/editor.api';

const api = [
    { label: 'Payroll.GetAllEmployeesAsync', detail: 'Task<IReadOnlyList<Employee>>', insertText: 'Payroll.GetAllEmployeesAsync()' },
    { label: 'Payroll.GetSalaryByEmployeeIdAsync', detail: 'Task<decimal>(int employeeId)', insertText: 'Payroll.GetSalaryByEmployeeIdAsync(${1:employeeId})' },
    { label: 'Payroll.GetPaystubByIdAsync', detail: 'Task<Paystub?>(Guid id)', insertText: 'Payroll.GetPaystubByIdAsync(${1:id})' }
];

export function registerCSharpLanguageFeatures(model: monaco.editor.ITextModel) {
    monaco.languages.registerCompletionItemProvider('csharp', {
        triggerCharacters: ['.', '('],
        provideCompletionItems: (m, position) => {
            const word = m.getWordUntilPosition(position);
            const range = new monaco.Range(
                position.lineNumber,
                word.startColumn,
                position.lineNumber,
                word.endColumn
            );

            const suggestions: monaco.languages.CompletionItem[] = api.map(x => ({
                label: x.label,
                kind: monaco.languages.CompletionItemKind.Function,
                insertText: x.insertText,
                insertTextRules: monaco.languages.CompletionItemInsertTextRule.InsertAsSnippet,
                detail: x.detail,
                range
            }));

            return { suggestions };
        }
    });

    monaco.languages.registerHoverProvider('csharp', {
        provideHover: () => ({
            contents: api.map(x => ({ value: `**${x.label}**\n\n${x.detail}` }))
        })
    });
}
