#!/usr/bin/sh

pandoc -s -M lang:hr -o tin_svagelj.docx dokumentacija.tex
pdflatex --shell-escape dokumentacija.tex
