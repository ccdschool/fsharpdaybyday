module Datamodel

type Locations = string list
type SourceLines = int * string list
type Result = {numberOfFiles:int; totalLinesOfCode:int}