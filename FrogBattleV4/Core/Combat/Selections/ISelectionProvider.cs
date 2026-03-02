using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace FrogBattleV4.Core.Combat.Selections;

public interface ISelectionProvider
{
    [Pure] Task<ISelectionResult<TResult>> GetSelectionAsync<TResult>(ISelectionRequest<TResult> request);
}