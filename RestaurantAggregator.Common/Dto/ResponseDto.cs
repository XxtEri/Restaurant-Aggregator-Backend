using System.Diagnostics.CodeAnalysis;

namespace RestaurantAggregator.CommonFiles.Dto;

public class ResponseDto
{
    [MaybeNull] 
    public string Status { get; set; }

    [MaybeNull]
    public string Message { get; set; }
}