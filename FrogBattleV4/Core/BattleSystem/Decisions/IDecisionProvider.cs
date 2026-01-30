using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace FrogBattleV4.Core.BattleSystem.Decisions;

public interface IDecisionProvider
{
    [Pure] Task<DecisionResult<TResult>> GetSelectionAsync<TResult>(IDecisionRequest<TResult> request);
}