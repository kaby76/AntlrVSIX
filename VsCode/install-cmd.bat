
call npm i vscode-jsonrpc@6.0.0-next.5
call npm i vscode-languageclient@7.0.0-next.9
call npm i vscode-languageserver@7.0.0-next.7
call npm i vscode-languageserver-protocol@3.16.0-next.7
call npm i vscode-languageserver-types@3.16.0-next.3
call npm install -g vsce
echo d | xcopy /E /H ..\Server\bin\Debug\netcoreapp3.1 server\netcoreapp3.1
echo d | xcopy /E /H ..\Trash\bin\Debug\netcoreapp3.1 Trash
call npm install
call npm run compile
call vsce package
