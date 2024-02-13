namespace Rewriter.Configuration;

public class FileInputOptions : IEquatable<FileInputOptions>
{
    public const string Key = "FileInput";
    
    public IEnumerable<FileInputOption> FileInputList { get; set; }

    public bool Equals(FileInputOptions? other)
    {
        return other is not null 
               && FileInputList.SequenceEqual(other.FileInputList);
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        return Equals((FileInputOptions) obj);
    }

    public override int GetHashCode()
    {
        return FileInputList.GetHashCode();
    }
}

public class FileInputOption : IEquatable<FileInputOption>
{
    public IEnumerable<string> FolderPaths { get; set; }
    public IEnumerable<string> Extensions { get; set; }

    public bool Equals(FileInputOption? other)
    {
        return other is not null 
               && FolderPaths.SequenceEqual(other.FolderPaths)
               && Extensions.SequenceEqual(other.Extensions);
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        return Equals((FileInputOption) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(FolderPaths, Extensions);
    }
}