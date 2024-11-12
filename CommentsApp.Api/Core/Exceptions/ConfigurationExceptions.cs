namespace CommentsApp.Api.Core.Exceptions;

public class SectionNotFoundException(string path)
    : Exception($"Section with path `{path}` is not found in configuration!");
public class SectionValueNotFoundException(string path) 
    : Exception($"Section value with path `{path}` is not found in configuration!");