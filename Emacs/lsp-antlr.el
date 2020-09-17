;;; lsp-antlr.el --- major mode for ANTLR grammar files  -*- lexical-binding: t -*-

;; Copyright (C) 2020 emacs-lsp maintainers

;; Author: emacs-lsp maintainers
;; Keywords: lsp, cmake

;; This program is free software; you can redistribute it and/or modify
;; it under the terms of the GNU General Public License as published by
;; the Free Software Foundation, either version 3 of the License, or
;; (at your option) any later version.

;; This program is distributed in the hope that it will be useful,
;; but WITHOUT ANY WARRANTY; without even the implied warranty of
;; MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
;; GNU General Public License for more details.

;; You should have received a copy of the GNU General Public License
;; along with this program.  If not, see <https://www.gnu.org/licenses/>.

;;; Commentary:

;; LSP Clients for the CMake build tool.

;;; Code:

(require 'lsp-mode)

(defgroup lsp-antlr nil
  "LSP support for Antlr, Bison, and other grammars, using Antlrvsix."
  :group 'lsp-mode
  :tag "Language Server"
  :package-version '(lsp-mode . "6.2"))

(defcustom lsp-clients-antlr-executable "c:/users/kenne/documents/antlrvsix2/server/bin/debug/netcoreapp3.1/server.exe"
  "The antlrvsix executable to use.
Leave as just the executable name to use the default behavior of
finding the executable with `exec-path'."
  :group 'lsp-antlr
  :risky t
  :type 'file)

(defcustom lsp-clients-antlr-args '()
  "Extra arguments for the antlrvsix executable"
  :group 'lsp-antlr
  :risky t
  :type '(repeat string))

(defun lsp-clients--antlr-command ()
  "Generate the language server startup command."
  `(,lsp-clients-antlr-executable,@lsp-clients-antlr-args))

(lsp-register-client
 (make-lsp-client :new-connection (lsp-stdio-connection 'lsp-clients--antlr-command)
                  :major-modes '(antlr-mode)
                  :priority -1
                  :server-id 'antlrvsix))

(provide 'lsp-antlr)
;;; lsp-antlr.el ends here
