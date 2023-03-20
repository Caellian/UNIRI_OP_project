#!/usr/bin/sh

pandoc --embed-resources --standalone -s -M lang:hr -o tin_svagelj.pdf README.md
pandoc --embed-resources --standalone -s -M lang:hr -o tin_svagelj.docx README.md
