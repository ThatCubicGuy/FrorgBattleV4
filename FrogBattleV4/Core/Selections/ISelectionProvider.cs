using System.Threading.Tasks;

namespace FrogBattleV4.Core.Selections;

/// <summary>
/// Represents an object that can resolve a selection request for any type of request.
/// </summary>
public interface ISelectionProvider
{
    Task<SelectionResult<TResult>> GetSelectionAsync<TResult>(ISelectionRequest<TResult> request);
}

/// <summary>
/// Represents an object that can resolve selection requests of a certain type.
/// </summary>
/// <typeparam name="TResult">The type of selection that this type can resolve.</typeparam>
public interface ISelectionProvider<TResult>
{
    Task<SelectionResult<TResult>> GetSelectionAsync(ISelectionRequest<TResult> request);
}