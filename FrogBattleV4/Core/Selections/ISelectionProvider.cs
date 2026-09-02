using System.Threading.Tasks;

namespace FrogBattleV4.Core.Selections;

public interface ISelectionProvider
{
    Task<ISelectionResult<TResult>> GetSelectionAsync<TResult>(ISelectionRequest<TResult> request);
}