#!/usr/bin/sh

rm -rf ./PlantUML

cd Wordel/Wordel
puml-gen ./Model ../../PlantUML -dir -createAssociation
cd ../..

plantuml -tsvg ./PlantUML
