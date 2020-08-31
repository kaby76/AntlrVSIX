"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const vscode = require("vscode");
const vscodelc = require("vscode-languageclient");
/**
 * Method to get workspace configuration option
 * @param option name of the option (e.g. for antlr.path should be path)
 * @param defaultValue default value to return if option is not set
 */
function getConfig(option, defaultValue) {
    const config = vscode.workspace.getConfiguration('antlr');
    return config.get(option, defaultValue);
}
var SwitchSourceHeaderRequest;
(function (SwitchSourceHeaderRequest) {
    SwitchSourceHeaderRequest.type = new vscodelc.RequestType('textDocument/switchSourceHeader');
})(SwitchSourceHeaderRequest || (SwitchSourceHeaderRequest = {}));
class FileStatus {
    constructor() {
        this.statuses = new Map();
        this.statusBarItem = vscode.window.createStatusBarItem(vscode.StatusBarAlignment.Left, 10);
    }
    onFileUpdated(fileStatus) {
        const filePath = vscode.Uri.parse(fileStatus.uri);
        this.statuses.set(filePath.fsPath, fileStatus);
        this.updateStatus();
    }
    updateStatus() {
        const path = vscode.window.activeTextEditor.document.fileName;
        const status = this.statuses.get(path);
        if (!status) {
            this.statusBarItem.hide();
            return;
        }
        this.statusBarItem.text = `antlr: ` + status.state;
        this.statusBarItem.show();
    }
    clear() {
        this.statuses.clear();
        this.statusBarItem.hide();
    }
    dispose() {
        this.statusBarItem.dispose();
    }
}
let client;
function activate(context) {
    const server = {
        command: `C:/Users/kenne/Documents/AntlrVSIX2/Server/bin/Debug/netcoreapp3.1/Server.exe`,
        args: [],
        options: { shell: false, detached: false }
    };
    const serverOptions = server;
    let clientOptions = {
        // Register the server for plain text documents
        documentSelector: [
            { scheme: 'file', language: 'antlr2' },
            { scheme: 'file', language: 'antlr3' },
            { scheme: 'file', language: 'antlr4' },
            { scheme: 'file', language: 'bison' },
            { scheme: 'file', language: 'ebnf' },
        ]
    };
    client = new vscodelc.LanguageClient('Antlr Language Server', serverOptions, clientOptions);
    console.log('Antlr Language Server is now active!');
    client.start();
}
exports.activate = activate;
function deactivate() {
    if (!client) {
        return undefined;
    }
    return client.stop();
}
exports.deactivate = deactivate;
//# sourceMappingURL=extension.js.map