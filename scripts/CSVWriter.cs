using System;
using System.Linq;
using Godot;

public partial class CSVWriter : Node
{
    private string simVarsFilePath = "user://lunar_ascent_game/";
    private string filePath;

    // Initialize the CSV file. Creates new directory and file if provided path does not already exist
    public void Start(
        string filePathBase,
        string fileName,
        string[] initialValues,
        string[] headers
    )
    {
        try
        {
            // Generate the full file path
            string directoryPath = $"user://{filePathBase}";
            filePath = $"{directoryPath}/{fileName}.csv";

            // Ensure the directory exists
            DirAccess dir = DirAccess.Open(directoryPath);
            if (dir == null)
            {
                dir = DirAccess.Open("user://");
                if (dir != null)
                {
                    // Create directory
                    dir.MakeDirRecursive(filePathBase);
                    GD.Print($"Directory created: {directoryPath}");

                    // Open the file for writing
                    using var file = FileAccess.Open(filePath, FileAccess.ModeFlags.Write);

                    // Write initial values and headers
                    file.StoreString(string.Join(";", initialValues) + "\n");
                    file.StoreString(string.Join(";", headers) + "\n");
                }
            }
        }
        catch (Exception ex)
        {
            GD.PrintErr($"Failed to initialize CSVWriter: {ex.Message}");
        }
    }

    // Append a new row to file
    public void WriteRow(FileAccess file, string[] row)
    {
        try
        {
            file.SeekEnd(); // Move to the end of the file
            file.StoreString(string.Join(";", row) + "\n"); // Write the new row
        }
        catch (Exception ex)
        {
            GD.PrintErr($"Failed to write row: {ex.Message}");
        }
    }

    // Gets values of first row where value in first column matches "searchRow"
    public string[] GetRow(string searchRow)
    {
        try
        {
            using var file = FileAccess.Open(filePath, FileAccess.ModeFlags.ReadWrite); // Open file

            // Split csv values to array with rows
            string[] rows = file.GetAsText().Split("\b");
            foreach (string row in rows)
            {
                string[] csvRow = row.Split(";");
                if (csvRow.First() == searchRow)
                {
                    return csvRow;
                }
            }
        }
        catch (Exception ex)
        {
            GD.PrintErr($"Failed to write row: {ex.Message}");
        }
        return Array.Empty<string>();
    }

    // Sets values of each row where value in first column matches "searchRow"
    public void SetRow(string searchRow, string[] newRow)
    {
        try
        {
            using var file = FileAccess.Open(filePath, FileAccess.ModeFlags.ReadWrite); // Open file

            string[] rows = file.GetAsText().Split("\n");
            foreach (string row in rows)
            {
                string[] csvRow = row.Split(";");

                if (csvRow[0] == searchRow)
                {
                    WriteRow(file, newRow);
                }
                else
                {
                    WriteRow(file, csvRow);
                }
            }
        }
        catch (Exception ex)
        {
            GD.PrintErr($"Failed to write row: {ex.Message}");
        }
    }
}
