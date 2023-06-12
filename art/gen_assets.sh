!/usr/bin/sh

inkscape -w 16 -h 16 -o icon-16.png icon.svg
inkscape -w 32 -h 32 -o icon-32.png icon.svg
inkscape -w 48 -h 48 -o icon-48.png icon.svg
inkscape -w 64 -h 64 -o icon-64.png icon.svg
inkscape -w 128 -h 128 -o icon-128.png icon.svg
inkscape -w 512 -h 512 -o icon-512.png icon.svg

convert icon-16.png icon-32.png icon-48.png icon-64.png icon.ico

