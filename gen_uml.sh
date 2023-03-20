#!/usr/bin/sh

rm -rf ./PlantUML

Global="@startuml"

function process() {
    echo Generating UML for $1...
    puml-gen ./$1 ../../PlantUML/$1 -dir -createAssociation
    Global=$(echo "$Global\n!include ./$1/include.puml")
}

cd Wordel/Wordel
process Model
process Components
process ViewModels
process Views
cd ../..

Global=$(echo "$Global\n@enduml\n")
echo -en "$Global" > ./PlantUML/Global.puml

plantuml -tsvg ./PlantUML
plantuml -tsvg ./PlantUML/*/include.puml
