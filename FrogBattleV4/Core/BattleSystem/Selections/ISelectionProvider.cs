using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace FrogBattleV4.Core.BattleSystem.Selections;

public interface ISelectionProvider
{
    [Pure] Task<ISelectionResult<TResult>> GetSelectionAsync<TResult>(ISelectionRequest<TResult> request);
}