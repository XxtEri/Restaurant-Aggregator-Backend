using System.Diagnostics.CodeAnalysis;

namespace RestaurantAggregatorService.Models;

public class ResponseModel
{
    [MaybeNull] public string Status { get; set; }

    [MaybeNull]
    public string Message { get; set; }

}