namespace Adapters

module Filesystem =
    let internal find_source_files (locations:Datamodel.Locations) : string list =
        let crawl_for_files =
            let files_in_folder foldername =
                System.IO.Directory.GetFiles(foldername, "*.*", System.IO.SearchOption.AllDirectories) |> Array.toList

            let (|File|Folder|Invalid|) location =
                if System.IO.File.Exists(location) then File location
                else if System.IO.Directory.Exists(location) then Folder location 
                else Invalid

            List.map (function
            | File filename -> [filename]
            | Folder foldername -> files_in_folder foldername
            | Invalid -> []) 
            >> List.concat

        let filter_files =
            let is_relevant_file filename =
                System.IO.Path.GetExtension(filename) = ".cs"

            List.filter is_relevant_file

        locations |> crawl_for_files 
                  |> filter_files


    let internal compile_source_lines (filenames:string List) : Datamodel.SourceLines =
        let lines = 
            let to_lines filename =
                System.IO.File.ReadAllLines(filename) |> Array.toList

            filenames |> List.map to_lines |> List.concat

        (filenames.Length, lines)


    let acquire_source_lines =
        find_source_files >> compile_source_lines