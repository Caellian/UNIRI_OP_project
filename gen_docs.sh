#!/usr/bin/sh

pandoc -s -M lang:hr -o tin_svagelj.docx doc/dokumentacija.tex

cd doc
pdflatex --shell-escape dokumentacija.tex
cd ..

mv doc/dokumentacija.pdf tin_svagelj.pdf
