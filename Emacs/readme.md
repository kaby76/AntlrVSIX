# Emacs client

This code uses the [lsp-mode](https://github.com/emacs-lsp/lsp-mode) client. Make sure you update
to the latest version.

1) Use GNU Emacs.
2) Install lsp-mode (instructions [here](https://emacs-lsp.github.io/lsp-mode/page/installation/))
3) Make sure Antlr package installed (instructions [here](https://sourceforge.net/projects/antlr-mode/))
4) Open a .g4 file.
5) M-x antlr-mode
6) M-x lsp-mode
7) M-x load-file
  * lsp-antlr.el (in this directory)
8) M-x lsp

The executable "Server.exe" should start up once you provide information on how to open this file (in a project).
You may have to tidle with the full path name of the .exe file to where it is actually located.

So far I have only been able to start the server, then click on a symbol and get "hover" info in the
message area of GNU Emacs.
