using System.Threading.Tasks;

namespace FrogBattleV4.Core.Combat.Selections;

public interface ISelectionProvider
{
    Task<ISelectionResult<TResult>> GetSelectionAsync<TResult>(ISelectionRequest<TResult> request);
}