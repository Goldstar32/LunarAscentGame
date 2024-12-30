using System;
using Godot;

public partial class CSVWriter : Node
{
    private string runNumberFilePath = "user://test_data/run_number.txt";
    private string filePath;

    // Get the current run number, add one, and save it
    private int GetAndIncrementRunNumber()
    {
        int runNumber = 1;

        // Check if the run number file exists
        if (FileAccess.FileExists(runNumberFilePath))
        {
            try
            {
                using var file = FileAccess.Open(runNumberFilePath, FileAccess.ModeFlags.Read); // Open file
                string runNumberStr = file.GetAsText(); // Get text in file
                if (int.TryParse(runNumberStr.Trim(), out int parsedRunNumber)) // Parse text
                {
                    runNumber = parsedRunNumber + 1;
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"Failed to read run number file: {ex.Message}");
            }
        }
		else // Create new test_data folder if needed
		{
			DirAccess dir = DirAccess.Open(runNumberFilePath);
            if (dir == null)
            {
                dir = DirAccess.Open("user://");
                if (dir != null)
                {
                    dir.MakeDirRecursive("test_data");
                    GD.Print($"Directory created: {runNumberFilePath}");
                }
            }
		}

        // Save the new run number to the file
        try
        {
            using var file = FileAccess.Open(runNumberFilePath, FileAccess.ModeFlags.Write);
            file.StoreString(runNumber.ToString());
        }
        catch (Exception ex)
        {
            GD.PrintErr($"Failed to write run number file: {ex.Message}");
        }

        return runNumber;
    }

    // Initialize the CSV file
    public void Start(string filePathBase, string[] initialValues, string[] headers)
    {
        try
        {
            // Update and get the run number
            int runNumber = GetAndIncrementRunNumber();

            // Generate the full file path
            string directoryPath = $"user://{filePathBase}";
            filePath = $"{directoryPath}/run{runNumber}.csv";

            // Ensure the directory exists
            DirAccess dir = DirAccess.Open(directoryPath);
            if (dir == null)
            {
                dir = DirAccess.Open("user://");
                if (dir != null)
                {
                    dir.MakeDirRecursive(filePathBase);
                    GD.Print($"Directory created: {directoryPath}");
                }
            }

            // Open the file for writing
            using var file = FileAccess.Open(filePath, FileAccess.ModeFlags.Write);

            // Write initial values and headers
            file.StoreString(string.Join(";", initialValues) + "\n");
            file.StoreString(string.Join(";", headers) + "\n");
        }
        catch (Exception ex)
        {
            GD.PrintErr($"Failed to initialize CSVWriter: {ex.Message}");
        }
    }

    // Append a new row to file
    public void WriteRow(string[] row)
    {
        try
        {
            using var file = FileAccess.Open(filePath, FileAccess.ModeFlags.ReadWrite); // Open file
            file.SeekEnd(); // Move to the end of the file
            file.StoreString(string.Join(";", row) + "\n"); // Write the new row
        }
        catch (Exception ex)
        {
            GD.PrintErr($"Failed to write row: {ex.Message}");
        }
    }
}
